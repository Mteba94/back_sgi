using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp.Constancias
{
    public class AnularConstanciaService
    {
        private readonly SgiSacramentosContext _context;
        private readonly SacramentoService _sacramentoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AnularConstanciaService(SgiSacramentosContext context, SacramentoService sacramentoService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _sacramentoService = sacramentoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<bool>> AnulaConstancia(int constanciaId, string observaciones)
        {
            var response = new BaseResponse<bool>();

            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;

                response = await AnulaConstancia(constanciaId, observaciones, con);
                con.Close();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }

            return response;
        }

        private async Task<BaseResponse<bool>> AnulaConstancia(int constanciaId, string observaciones, SqlConnection con)
        {
            var response = new BaseResponse<bool>();

            SqlCommand cmd;
            SqlParameter param = new SqlParameter();
            DataSet ds = new DataSet();
            SqlDataReader dr;

            try
            {
                cmd = new SqlCommand();
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_control_constancias";

                var deleteUser = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var deleteDate = DateTime.Now;

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "AC"));
                cmd.Parameters.Add(new SqlParameter("@i_idConstancia", constanciaId));
                cmd.Parameters.Add(new SqlParameter("@i_observacion", observaciones));
                cmd.Parameters.Add(new SqlParameter("@i_User", deleteUser));
                cmd.Parameters.Add(new SqlParameter("@i_Date", deleteDate));

                con.Open();

                var rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    response.IsSuccess = true;
                    response.Data = rowsAffected > 0;
                    response.Message = ReplyMessage.MESSAGE_ANNULAR;
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
