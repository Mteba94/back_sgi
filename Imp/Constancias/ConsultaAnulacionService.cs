using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Helpers;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;
using static WebApi_SGI_T.Imp.Matrimonio.MatrimonioById;

namespace WebApi_SGI_T.Imp.Constancias
{
    public class ConsultaAnulacionService
    {
        private readonly SgiSacramentosContext _context;
        private readonly SacramentoService _sacramentoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConsultaAnulacionService(SgiSacramentosContext context, SacramentoService sacramentoService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _sacramentoService = sacramentoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public BaseResponse<BaseEntityResponse<ConstanciaAnulacionResponse>> ListConstanciasAnulacion(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<ConstanciaAnulacionResponse>>()
            {
                Data = new BaseEntityResponse<ConstanciaAnulacionResponse>()
            };

            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
                response = ListConstanciasAnulacion(filters, con);
                con.Close();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
            }

            return response;
        }

        private BaseResponse<BaseEntityResponse<ConstanciaAnulacionResponse>> ListConstanciasAnulacion(BaseFiltersRequest filters, SqlConnection con)
        {
            var response = new BaseResponse<BaseEntityResponse<ConstanciaAnulacionResponse>>()
            {
                Data = new BaseEntityResponse<ConstanciaAnulacionResponse>()
            };

            SqlCommand cmd;
            SqlParameter param = new SqlParameter();
            DataSet ds = new DataSet();
            SqlDataReader dr;

            try
            {
                cmd = new SqlCommand();
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_anulacion_constancia";

                var User = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var Date = DateTime.Now;

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "CAC"));
                cmd.Parameters.Add(new SqlParameter("@i_idConstancia", null));
                cmd.Parameters.Add(new SqlParameter("@i_observacion", null));
                cmd.Parameters.Add(new SqlParameter("@i_User", User));
                cmd.Parameters.Add(new SqlParameter("@i_Date", Date));
                cmd.Parameters.Add(new SqlParameter("@i_UserRespuesta", null));
                cmd.Parameters.Add(new SqlParameter("@i_DateRespuesta", null));
                cmd.Parameters.Add(new SqlParameter("@i_estado", null));
                cmd.Parameters.Add(new SqlParameter("@i_motivoRechazo", null));

                con.Open();

                var reader = cmd.ExecuteReader();

                var datos = new List<ConstanciaAnulacionResponse>();

                while (reader.Read())
                {
                    var constanciaAnulacionData = new ConstanciaAnulacionResponse
                    {
                        Sa_IdSolicitud = Convert.ToInt32(reader["sa_IdSolicitud"]),
                        Sa_IdConstancia = Convert.ToInt32(reader["sa_IdConstancia"]),
                        Pe_nombre = reader["pe_nombre"].ToString()!,
                        Ts_nombre = reader["ts_nombre"].ToString()!,
                        sa_IdUsuarioSolicitante = Convert.ToInt32(reader["sa_IdUsuarioSolicitante"]),
                        //us_solicita = reader["us_solicita"].ToString()!,
                        sa_FechaSolicitud = Convert.ToDateTime(reader["sa_FechaSolicitud"]),
                        sa_Estado = reader["sa_Estado"].ToString()!,
                        sa_EstadoDescripcion = reader["sa_EstadoDescripcion"].ToString()!,
                        sa_IdUsuarioAprobador = reader["sa_IdUsuarioAprobador"] == DBNull.Value ? null : Convert.ToInt32(reader["sa_IdUsuarioAprobador"]),
                        us_nombre = reader["us_nombre"].ToString()!,
                        sa_FechaAprobacion = reader["sa_FechaAprobacion"] == DBNull.Value ? null : Convert.ToDateTime(reader["sa_FechaAprobacion"]),
                        sa_MotivoSolicitud = reader["sa_MotivoSolicitud"].ToString()!,
                        sa_MotivoRechazo = reader["sa_MotivoRechazo"] == DBNull.Value ? null : reader["sa_MotivoRechazo"].ToString()
                    };
                    datos.Add(constanciaAnulacionData);
                }

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            datos = datos.Where(x => x.us_solicita.Contains(filters.TextFilter)).ToList();
                            break;
                        //case 2:
                        //    datos = datos.Where(x => x.Ts_nombre.Contains(filters.TextFilter)).ToList();
                        //    break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    //datos = datos.Where(x => x.sa_Estado == filters.StateFilter).ToList();
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    var filteredData = datos.Where(x => x.sa_FechaSolicitud >= Convert.ToDateTime(filters.StartDate)
                                             && x.sa_FechaSolicitud <= Convert.ToDateTime(filters.EndDate));
                    datos = filteredData.ToList();
                }

                if (filters.Sort is null) filters.Sort = "sa_IdSolicitud";

                var queryableData = datos.AsQueryable();

                response.Data.TotalRecords = queryableData.Count();

                datos = OrderingHelper.Ordering(filters, queryableData, !(bool)filters.Download!).ToList();

                response.Data.Items = datos;

                if (response.Data.Items is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }

                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_QUERY;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }

            return response;
        }
    }
}
