using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp.Constancias
{
    public class ConstanciaByIdService
    {
        private readonly SgiSacramentosContext _context;
        private readonly SacramentoService _sacramentoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConstanciaByIdService(SgiSacramentosContext context, SacramentoService sacramentoService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _sacramentoService = sacramentoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<HistConstanciaResponse>> GetConstanciaById(int constanciaId)
        {
            var response = new BaseResponse<HistConstanciaResponse>();
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;

                response = await GetConstanciaById(constanciaId, con);
                con.Close();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
            }

            return response;
        }

        public async Task<BaseResponse<HistConstanciaResponse>> GetConstanciaById(int constanciaId, SqlConnection con)
        {
            var response = new BaseResponse<HistConstanciaResponse>();

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

                var User = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var Date = DateTime.Now;

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "CC"));
                cmd.Parameters.Add(new SqlParameter("@i_idConstancia", constanciaId));
                cmd.Parameters.Add(new SqlParameter("@i_observacion", null));
                cmd.Parameters.Add(new SqlParameter("@i_User", User));
                cmd.Parameters.Add(new SqlParameter("@i_Date", Date));

                con.Open();

                var reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        response.Data = new HistConstanciaResponse
                        {
                            ct_ConstanciaId = reader.GetInt32(0),
                            ct_SacramentoId = reader.GetInt32(1),
                            ct_Sacramento = reader.GetString(2),
                            ct_PeNombre = reader.GetString(3),
                            ct_Correlativo = reader.GetInt32(4),
                            ct_FormatoCorrelativo = reader.GetString(5),
                            ct_UsuarioId = reader.GetInt32(6),
                            ct_FechaImpresion = reader.GetDateTime(7),
                            ct_Usuario = reader.GetString(8),
                            ct_Estado = reader.GetInt32(9),
                            ct_EstadoDescripcion = reader.GetInt32(9) == 1 ? "Entregada" : "Anulada",
                            ct_Observacion = reader.IsDBNull(10) ? null : reader.GetString(10),
                            ct_UsuarioRechazo = reader.IsDBNull(11) ? (int?)null : reader.GetInt32(11),
                            ct_UsuarioRechazoNombre = reader.IsDBNull(12) ? null : reader.GetString(12),
                            ct_FechaRechazo = reader.IsDBNull(13) ? (DateTime?)null : reader.GetDateTime(13)
                        };
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
            catch (Exception ex)
            {

            }
            return response;
        }

    }
}
