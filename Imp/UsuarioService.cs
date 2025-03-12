using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi_SGI_T.Imp.Authentication;
using WebApi_SGI_T.Imp.FileStorage;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Helpers;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;
using BC = BCrypt.Net.BCrypt;

namespace WebApi_SGI_T.Imp
{
    public class UsuarioService
    {
        private readonly SgiSacramentosContext _context;
        private readonly IConfiguration _configuration;
        private readonly ImageService _imageService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioService(SgiSacramentosContext context, IConfiguration configuration, ImageService imageService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _imageService = imageService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> GenerateToken(TokenRequestDto token)
        {
            var response = new BaseResponse<string>();
            var account = await GetUsuarioByUserName(token.Username!);

            if (account.Data is not null && !string.IsNullOrEmpty(token.Password) && !string.IsNullOrEmpty(account.Data.UsPass))
            {
                if (BC.Verify(token.Password, account.Data.UsPass))
                {
                    await UpdateExistingRolesPermissionsAsync();

                    response.IsSuccess = true;
                    response.Data = await GenerateToken(account.Data);
                    response.Message = ReplyMessage.MESSAGE_TOKEN;
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
            }

            return response;
        }

        public async Task<BaseResponse<BaseEntityResponse<UsuarioResponse>>> ListUsuarios(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<UsuarioResponse>>()
            {
                Data = new BaseEntityResponse<UsuarioResponse>()
            };
            try
            {
                // Crear la consulta base
                var query = _context.TblUsuarios
                                    .Include(x => x.TblUserRols)
                                    .ThenInclude(ur => ur.UrIdRolNavigation)
                                    .AsNoTracking()
                                    .Where(x => x.UsEstado == 1);

                // Aplicar filtros
                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            query = query.Where(x => x.UsNombre.Contains(filters.TextFilter));
                            break;
                        case 2:
                            query = query.Where(x => x.UsUserName.Contains(filters.TextFilter));
                            break;
                        case 3:
                            query = query.Where(x => x.UsDireccion.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    query = query.Where(x => x.UsEstado == filters.StateFilter);
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    query = query.Where(x => x.UsCreateDate >= Convert.ToDateTime(filters.StartDate) &&
                                             x.UsCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }

                var totalRecords = await query.CountAsync();

                // Mapear datos
                var mappedData = await query.Select(x => new UsuarioResponse
                {
                    UsIdUsuario = x.UsIdusuario,
                    UsUserName = x.UsUserName,
                    //UsPass = x.UsPass,
                    UsImage = x.UsImage,
                    UsNombre = x.UsNombre,
                    UsFechaNacimiento = x.UsFechaNacimiento,
                    UsIdGenero = x.UsSexoId,
                    UsDireccion = x.UsDireccion!,
                    UsIdTipoDocumento = x.UsIdTipoDocumento,
                    UsNumerodocumento = x.UsNumerodocumento,
                    UsCreateDate = x.UsCreateDate,
                    UsEstado = x.UsEstado,
                    EstadoDescripcion = x.UsEstado == 1 ? "Activo" : "Inactivo",
                    // Obtener el nombre del rol
                    UserIdRole = x.TblUserRols.FirstOrDefault(ur => ur.UrEstado == 1) != null ? x.TblUserRols.FirstOrDefault(ur => ur.UrEstado == 1)!.UrIdRolNavigation.RoIdRol : 0!,
                    UserRole = x.TblUserRols.FirstOrDefault(ur => ur.UrEstado == 1) != null ? x.TblUserRols.FirstOrDefault(ur => ur.UrEstado == 1)!.UrIdRolNavigation.RoNombre : null!
                }).ToListAsync();

                // Aplicar ordenamiento
                if (filters.Sort is null) filters.Sort = "UsIdUsuario";
                var orderedList = OrderingHelper.Ordering(filters, mappedData.AsQueryable(), !(bool)filters.Download!);

                response.Data.Items = orderedList.ToList();
                response.Data.TotalRecords = totalRecords;
                response.IsSuccess = response.Data.Items.Any();
                response.Message = response.IsSuccess ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<BaseResponse<UsuarioResponse>> GetUsuarioByUserName(string userName)
        {
            var response = new BaseResponse<UsuarioResponse>
            {
                Data = new UsuarioResponse()
            };

            SqlConnection con = new SqlConnection();
            SqlCommand cmd;
            SqlParameter param = new SqlParameter();
            DataSet ds = new DataSet();
            SqlDataReader dr;

            try
            {
                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
                cmd = new SqlCommand();
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM tbl_usuarios WHERE us_userName = @UserName";

                param.ParameterName = "@UserName";
                param.Value = userName;
                cmd.Parameters.Add(param);

                await con.OpenAsync();

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        response.Data.UsIdUsuario = dr.GetInt32(0);
                        response.Data.UsUserName = dr.GetString(1);
                        response.Data.UsPass = dr.GetString(2);
                        response.Data.UsImage = dr.IsDBNull(3) ? null : dr.GetString(3);
                        response.Data.UsNombre = dr.GetString(4);
                        response.Data.UsFechaNacimiento = dr.GetDateTime(5);
                        response.Data.UsIdTipoDocumento = dr.GetByte(6);
                        response.Data.UsNumerodocumento = dr.GetString(7);
                        response.Data.UsIdGenero = dr.IsDBNull(8) ? (byte)0 : dr.GetByte(8);
                        response.Data.UsDireccion = dr.GetString(9);
                        response.Data.UsCreateDate = dr.GetDateTime(12);
                        response.Data.UsEstado = dr.GetByte(10);
                        response.Data.EstadoDescripcion = dr.GetByte(10) == 1 ? "Activo" : "Inactivo";
                    }
                    response.IsSuccess = true;
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }

            }
            catch (SqlException ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RegisterUser(UserRequestDto userRequest)
        {
            var response = new BaseResponse<bool>();

            SqlConnection con = new SqlConnection();
            SqlCommand cmd;
            SqlParameter param = new SqlParameter();
            DataSet ds = new DataSet();
            SqlDataReader dr;

            try
            {
                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;

                var existingUser = await _context.TblUsuarios
                    .FirstOrDefaultAsync(x => x.UsUserName == userRequest.UsUserName);
                if (existingUser != null)
                {
                    response.IsSuccess = false;
                    response.Message = "El nombre de usuario ya está en uso.";
                    return response;
                }

                if (!IsValidPassword(userRequest.UsPass))
                {
                    response.IsSuccess = false;
                    response.Message = "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula y un carácter especial.";
                    return response;
                }

                var createUser = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var createDate = System.DateTime.Now;

                string imageUrl = null!;
                if (userRequest.UsImage != null)
                {
                    var container = "user-images";
                    imageUrl = await _imageService.SaveImage(userRequest.UsImage, container);
                }

                
                cmd = new SqlCommand();
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_control_usuario";

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "RNU"));
                cmd.Parameters.Add(new SqlParameter("@i_userName", userRequest.UsUserName));
                cmd.Parameters.Add(new SqlParameter("@i_pass", BC.HashPassword(userRequest.UsPass)));
                cmd.Parameters.Add(new SqlParameter("@i_image", imageUrl));
                cmd.Parameters.Add(new SqlParameter("@i_nombre", userRequest.UsNombre));
                cmd.Parameters.Add(new SqlParameter("@i_fechaNacimiento", userRequest.UsFechaNacimiento));
                cmd.Parameters.Add(new SqlParameter("@i_tipoDocumento", userRequest.UsIdTipoDocumento));
                cmd.Parameters.Add(new SqlParameter("@i_numDocto", userRequest.UsNumerodocumento));
                cmd.Parameters.Add(new SqlParameter("@i_genero", userRequest.UsIdGenero));
                cmd.Parameters.Add(new SqlParameter("@i_direccion", userRequest.UsDireccion));
                cmd.Parameters.Add(new SqlParameter("@i_User", createUser));
                cmd.Parameters.Add(new SqlParameter("@i_Date", createDate));

                con.Open();

                var rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    response.IsSuccess = true;
                    response.Data = rowsAffected > 0;
                    response.Message = ReplyMessage.MESSAGE_SAVE;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_FAILED;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        private async Task<string> GenerateToken(UsuarioResponse user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _context.TblUserRols
                    .Where(ur => ur.UrIdUsuario == user.UsIdUsuario && ur.UrEstado == 1)
                    .Select(ur => ur.UrIdRolNavigation.RoNombre)
                    .ToListAsync();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UsIdUsuario.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UsIdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.UsUserName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.UsNombre),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UsUserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UsIdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, Guid.NewGuid().ToString(), ClaimValueTypes.Integer64)
            };

            roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:Expires"])),
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<BaseResponse<bool>> UpdateUser(int userId, UserRequestDto request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Verificar si el nombre de usuario ya existe, excepto para el usuario actual
                var existingUser = await _context.TblUsuarios
                    .FirstOrDefaultAsync(x => x.UsUserName == request.UsUserName && x.UsIdusuario != userId);
                if (existingUser != null)
                {
                    response.IsSuccess = false;
                    response.Message = "El nombre de usuario ya está en uso.";
                    return response;
                }

                // Obtener el usuario actual
                var usuario = await _context.TblUsuarios
                    .FirstOrDefaultAsync(x => x.UsIdusuario == userId);

                if (usuario != null)
                {
                    // Validar la contraseña solo si no es nula o vacía
                    if (!string.IsNullOrWhiteSpace(request.UsPass))
                    {
                        if (!IsValidPassword(request.UsPass))
                        {
                            response.IsSuccess = false;
                            response.Message = "La contraseña debe tener al menos 8 caracteres, una mayúscula, una minúscula y un carácter especial.";
                            return response;
                        }

                        // Si la contraseña es válida y se proporcionó, se actualiza
                        usuario.UsPass = BC.HashPassword(request.UsPass);
                    }

                    string imageUrl = null!;
                    if (request.UsImage != null)
                    {
                        var container = "user-images";
                        imageUrl = await _imageService.SaveImage(request.UsImage, container);
                    }

                    // Actualizar otros campos
                    usuario.UsUserName = request.UsUserName;
                    usuario.UsImage = imageUrl;
                    usuario.UsNombre = request.UsNombre;
                    usuario.UsFechaNacimiento = request.UsFechaNacimiento;
                    usuario.UsIdTipoDocumento = request.UsIdTipoDocumento;
                    usuario.UsNumerodocumento = request.UsNumerodocumento;
                    usuario.UsSexoId = request.UsIdGenero;
                    usuario.UsDireccion = request.UsDireccion;
                    usuario.UsUpdateUser = currentUserId;
                    usuario.UsUpdateDate = DateTime.Now;

                    await _context.SaveChangesAsync();

                    response.Data = true;
                    response.Message = ReplyMessage.MESSAGE_UPDATE;
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }


        private bool IsValidPassword(string password)
        {
            if (password.Length < 8) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!password.Any(char.IsLower)) return false;
            if (!password.Any(char.IsDigit)) return false;
            if (!password.Any(ch => !char.IsLetterOrDigit(ch))) return false;
            return true;
        }

        public async Task<BaseResponse<bool>> DeleteUser(int id)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                if (currentUserId == id)
                {
                    response.IsSuccess = false;
                    response.Message = "No puedes eliminar tu propia cuenta.";
                    return response;
                }

                var usuario = await _context.TblUsuarios
                    .FirstOrDefaultAsync(x => x.UsIdusuario == id);

                if (usuario != null)
                {
                    usuario.UsEstado = 0;
                    usuario.UsDeleteUser = currentUserId;
                    usuario.UsDeleteDate = System.DateTime.Now;

                    await _context.SaveChangesAsync();

                    response.Data = true;
                    response.Message = ReplyMessage.MESSAGE_DELETE;
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }

            return response;
        }

        public async Task<BaseResponse<string>> resetPassword(int userId)
        {
            var response = new BaseResponse<string>();

            try
            {
                var user = await _context.TblUsuarios.FindAsync(userId);

                if (user != null)
                {
                    string newPassword = GenerateRandomPassword();

                    user.UsPass = BC.HashPassword(newPassword);

                    await _context.SaveChangesAsync();

                    response.Data = newPassword;
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                    response.IsSuccess = true;

                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }

            return response;
        }

        public async Task UpdateExistingRolesPermissionsAsync()
        {
            var roles = await _context.TblRols.ToListAsync();
            foreach (var role in roles)
            {
                var newPermissions = GetPermissionsForRole(role.RoNombre);

                if (role.Permissions != PermissionHelper.ConvertPermissionsToString(newPermissions))
                {
                    role.Permissions = PermissionHelper.ConvertPermissionsToString(newPermissions);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private List<Permission> GetPermissionsForRole(string roleName)
        {
            return roleName switch
            {
                "Administrador" => RolePermissions.AdminPermissions,
                "Secretario" => RolePermissions.SecretaryPermissions,
                _ => new List<Permission>()
            };
        }

        public string GenerateRandomPassword(int length = 12)
        {
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()_+[]{}|;:,.<>?";

            Random random = new Random();

            // Asegúrate de incluir al menos un carácter de cada tipo
            char[] password = new char[length];
            password[0] = lowerChars[random.Next(lowerChars.Length)];
            password[1] = upperChars[random.Next(upperChars.Length)];
            password[2] = digits[random.Next(digits.Length)];
            password[3] = specialChars[random.Next(specialChars.Length)];

            for (int i = 4; i < length; i++)
            {
                string allChars = lowerChars + upperChars + digits + specialChars;
                password[i] = allChars[random.Next(allChars.Length)];
            }

            // Mezcla la contraseña
            return new string(password.OrderBy(c => random.Next()).ToArray());
        }

    }
}
