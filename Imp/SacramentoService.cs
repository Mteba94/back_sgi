using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Helpers;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using System.Linq.Dynamic.Core;
using WebApi_SGI_T.Static;
using Microsoft.Data.SqlClient;
using Azure.Core;

namespace WebApi_SGI_T.Imp
{
    public class SacramentoService
    {
        private readonly SgiSacramentosContext _context;
        public SacramentoService(SgiSacramentosContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<BaseEntityResponse<SacramentoResponse>>> ListSacramentos(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<SacramentoResponse>>()
            {
                Data = new BaseEntityResponse<SacramentoResponse>()
            };

            try
            {

                var query = _context.TblSacramentos.Where(x => x.ScDeleteUser == null && x.ScDeleteDate == null)
                    .AsNoTracking();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            query = query.Where(x => x.ScIdpersonaNavigation.PeNombre.Contains(filters.TextFilter));
                            break;
                        case 2:
                            query = query.Where(x => x.ScIdpersonaNavigation.PeNumeroDocumento.Contains(filters.TextFilter));
                            break;
                        case 3:
                            query = query.Where(x => x.ScNumeroPartida!.Contains(filters.TextFilter));
                            break;
                        case 4:
                            query = query.Where(x => x.ScIdSacramentoNavigation.TsNombre.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    var filteredData = response.Data.Items.Where(x => x.ScCreateDate >= Convert.ToDateTime(filters.StartDate)
                                             && x.ScCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                    response.Data.Items = filteredData.ToList();
                }

                var totalRecords = await query.CountAsync();
                var mappedData = query.Select(s => new SacramentoResponse
                {
                    ScIdSacramento = s.ScIdSacramento,
                    ScNumeroPartida = s.ScNumeroPartida,
                    ScTipoSacramento = s.ScIdSacramentoNavigation.TsNombre,
                    PeNombre = s.ScIdpersonaNavigation.PeNombre,
                    PeNumeroDocumento = s.ScIdpersonaNavigation.PeNumeroDocumento,
                    ScFechaSacramento = s.ScFechaSacramento,
                    ScObservaciones = s.ScObservaciones,
                    ScCreateDate = s.ScCreateDate
                }).ToList();

                if (filters.Sort is null) filters.Sort = "ScIdSacramento";

                var orderedList = OrderingHelper.Ordering(filters, mappedData.AsQueryable(), !(bool)filters.Download!);
                response.Data.Items = orderedList.ToList();
                response.Data.TotalRecords = totalRecords;

                response.IsSuccess = response.Data.Items.Any();
                response.Message = response.IsSuccess ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;

            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<SacramentoResponse>> GetSacramentoById(int id)
        {
            var response = new BaseResponse<SacramentoResponse>();

            try
            {
                // Obtén el Sacramento con el ID especificado
                var query = await _context.TblSacramentos
                    .Include(x => x.ScIdSacramentoNavigation)
                    .Include(x => x.ScIdpersonaNavigation)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ScIdSacramento == id);
                    

                // Verifica si se encontró el Sacramento
                if (query != null)
                {
                    // Mapea el resultado a SacramentoResponse
                    var mappedData = new SacramentoResponse
                    {
                        ScIdSacramento = query.ScIdSacramento,
                        ScNumeroPartida = query.ScNumeroPartida,
                        ScTipoSacramento = query.ScIdSacramentoNavigation.TsNombre,
                        PeNombre = query.ScIdpersonaNavigation.PeNombre,
                        PeNumeroDocumento = query.ScIdpersonaNavigation.PeNumeroDocumento,
                        ScFechaSacramento = query.ScFechaSacramento,
                        ScObservaciones = query.ScObservaciones,
                        ScCreateDate = query.ScCreateDate
                    };

                    response.IsSuccess = true;
                    response.Data = mappedData;
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
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RegisterSacramento(SacramentoRequestDto request)
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
                var createDate = DateTime.Now;

                con.ConnectionString = _context.Database.GetDbConnection().ConnectionString;
                cmd = new SqlCommand();
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_control_sacramento";

                cmd.Parameters.Add(new SqlParameter("@i_operacion", "RNS"));
                cmd.Parameters.Add(new SqlParameter("@i_id_sacramento", request.ScTipoSacramento));
                cmd.Parameters.Add(new SqlParameter("@i_numDocto", request.PeNumeroDocumento));
                cmd.Parameters.Add(new SqlParameter("@i_fechaSacramento", request.ScFechaSacramento));
                cmd.Parameters.Add(new SqlParameter("@i_padre", request.ScPadre));
                cmd.Parameters.Add(new SqlParameter("@i_madre", request.ScMadre));
                cmd.Parameters.Add(new SqlParameter("@i_padrino", request.ScPadrino));
                cmd.Parameters.Add(new SqlParameter("@i_madrina", request.ScMadrina));
                cmd.Parameters.Add(new SqlParameter("@i_parroco", request.ScParroco));
                cmd.Parameters.Add(new SqlParameter("@i_nombre", request.PeNombre));
                cmd.Parameters.Add(new SqlParameter("@i_fechaNacimiento", request.PeFechaNacimiento));
                cmd.Parameters.Add(new SqlParameter("@i_tipoDoc", request.PeIdTipoDocumento));
                cmd.Parameters.Add(new SqlParameter("@i_direccion", request.PeDireccion));
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
    }
}
