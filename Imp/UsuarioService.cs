using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi_SGI_T.Imp.FileStorage;
using WebApi_SGI_T.Models;
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

        public UsuarioService(SgiSacramentosContext context, IConfiguration configuration, ImageService imageService)
        {
            _context = context;
            _configuration = configuration;
            _imageService = imageService;
        }

        public async Task<BaseResponse<string>> GenerateToken(TokenRequestDto token)
        {
            var response = new BaseResponse<string>();
            var account = await GetUsuarioByUserName(token.Username!);

            if (account.Data is not null && !string.IsNullOrEmpty(token.Password) && !string.IsNullOrEmpty(account.Data.UsPass))
            {
                if(BC.Verify(token.Password, account.Data.UsPass))
                {
                    response.IsSuccess = true;
                    response.Data = GenerateToken(account.Data);
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
                        response.Data.UsDireccion = dr.GetString(8);
                        response.Data.UsCreateDate = dr.GetDateTime(11);
                        response.Data.UsEstado = dr.GetByte(9);
                        response.Data.EstadoDescripcion = dr.GetByte(9) == 1 ? "Activo" : "Inactivo";
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
                var createUser = 1;
                var createDate = System.DateTime.Now;

                string imageUrl = null;
                if (userRequest.UsImage != null)
                {
                    var container = "user-images";
                    imageUrl = await _imageService.SaveImage(userRequest.UsImage, container);
                }

                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
                cmd = new SqlCommand();
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_control_usuario";

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "RNU"));
                cmd.Parameters.Add(new SqlParameter("@i_userName", userRequest.UsUserName));
                cmd.Parameters.Add(new SqlParameter("@i_pass", BC.HashPassword(userRequest.UsPass)));
                cmd.Parameters.Add(new SqlParameter("@i_image", imageUrl ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@i_nombre", userRequest.UsNombre));
                cmd.Parameters.Add(new SqlParameter("@i_fechaNacimiento", userRequest.UsFechaNacimiento));
                cmd.Parameters.Add(new SqlParameter("@i_tipoDocumento", userRequest.UsIdTipoDocumento));
                cmd.Parameters.Add(new SqlParameter("@i_numDocto", userRequest.UsNumerodocumento));
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

        private string GenerateToken(UsuarioResponse user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UsUserName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.UsNombre),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UsUserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UsIdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, Guid.NewGuid().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:Expires"])),
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
