using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp.Sacramentos
{
    public class EliminarSacramentoService
    {
        private readonly SgiSacramentosContext _context;
        private readonly SacramentoService _sacramentoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EliminarSacramentoService(SgiSacramentosContext context, SacramentoService sacramentoService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _sacramentoService = sacramentoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<bool>> DeleteSacramento(int sacramentoId, SacramentoRequestDto request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;

                response = await DeleteSacramento(sacramentoId, request, con);
                con.Close();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }

            return response;
        }

        private async Task<BaseResponse<bool>> DeleteSacramento(int sacramentoId, SacramentoRequestDto request, SqlConnection con)
        {
            var response = new BaseResponse<bool>();

            SqlCommand cmd;
            SqlParameter param = new SqlParameter();
            DataSet ds = new DataSet();
            SqlDataReader dr;

            try
            {
                var deleteUser = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var deleteDate = DateTime.Now;

                cmd = new SqlCommand();
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_control_sacramento";

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "ESE"));
                cmd.Parameters.Add(new SqlParameter("@i_id", sacramentoId));
                cmd.Parameters.Add(new SqlParameter("@i_id_sacramento", request.ScIdTipoSacramento));
                cmd.Parameters.Add(new SqlParameter("@i_numDocto", request.PeNumeroDocumento));
                cmd.Parameters.Add(new SqlParameter("@i_fechaSacramento", request.ScFechaSacramento));
                cmd.Parameters.Add(new SqlParameter("@i_numpartida", request.ScNumeroPartida));
                cmd.Parameters.Add(new SqlParameter("@i_padre", request.ScPadre));
                cmd.Parameters.Add(new SqlParameter("@i_madre", request.ScMadre));
                cmd.Parameters.Add(new SqlParameter("@i_padrino", request.ScPadrino));
                cmd.Parameters.Add(new SqlParameter("@i_madrina", request.ScMadrina));
                cmd.Parameters.Add(new SqlParameter("@i_parroco", request.ScParroco));
                cmd.Parameters.Add(new SqlParameter("@i_nombre", request.PeNombre));
                cmd.Parameters.Add(new SqlParameter("@i_edad", request.PeEdad));
                cmd.Parameters.Add(new SqlParameter("@i_fechaNacimiento", request.PeFechaNacimiento));
                cmd.Parameters.Add(new SqlParameter("@i_tipoDoc", request.PeIdTipoDocumento));
                cmd.Parameters.Add(new SqlParameter("@i_sexo", request.PeSexoId));
                cmd.Parameters.Add(new SqlParameter("@i_observacion", request.ScObservaciones));
                cmd.Parameters.Add(new SqlParameter("@i_direccion", request.PeDireccion));
                cmd.Parameters.Add(new SqlParameter("@i_User", deleteUser));
                cmd.Parameters.Add(new SqlParameter("@i_Date", deleteDate));


                con.Open();

                var rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    response.IsSuccess = true;
                    response.Data = rowsAffected > 0;
                    response.Message = ReplyMessage.MESSAGE_UPDATE;
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
    }
}
