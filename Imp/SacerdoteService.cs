using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Helpers;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp
{
    public class SacerdoteService
    {

        readonly SgiSacramentosContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SacerdoteService(SgiSacramentosContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<BaseEntityResponse<SacerdoteResponse>>> ListSacerdote(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<SacerdoteResponse>>()
            {
                Data = new BaseEntityResponse<SacerdoteResponse>()
            };

            try
            {
                var query = _context.TblSacerdotes
                    .AsNoTracking();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            query = query.Where(x => x.ScNombre!.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    query = query.Where(x => x.ScEstado == filters.StateFilter);
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    query = query.Where(x => x.ScCreateDate >= Convert.ToDateTime(filters.StartDate)
                                             && x.ScCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }

                var totalRecords = await query.CountAsync();
                var mappedData = query.Select(s => new SacerdoteResponse
                {
                    SacerdoteId = s.ScId,
                    SacerdoteNombre = s.ScNombre,
                    SacerdoteIdCategoria = s.ScIdCategoria,
                    SacerdoteCategoria = s.ScIdCategoriaNavigation.CsNombre,
                    SacerdoteFirma = s.ScFirma,
                    SacerdoteEstado = s.ScEstado,
                    SacerdoteEstadoDesc = (s.ScEstado == 1 ? "Activo" : "Inactivo"),
                }).ToList();

                if (filters.Sort is null) filters.Sort = "SacerdoteId";

                var orderedList = OrderingHelper.Ordering(filters, mappedData.AsQueryable(), !(bool)filters.Download!);
                response.Data.Items = orderedList.ToList();
                response.Data.TotalRecords = totalRecords;

                response.IsSuccess = response.Data.Items.Any();
                response.Message = response.IsSuccess ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;


            }
            catch (Exception ex)
            {
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                response.IsSuccess = false;
            }

            return response;
        }

        public BaseResponse<List<SacerdoteSelectResponse>> ListSelectSacerdote()
        {
            var response = new BaseResponse<List<SacerdoteSelectResponse>>
            {
                Data = new List<SacerdoteSelectResponse>()
            };

            try
            {
                var list = _context.TblSacerdotes
                    .Where(x => x.ScDeleteUser == null && x.ScDeleteDate == null)
                    .Select(x => new SacerdoteSelectResponse
                    {
                        ScId = x.ScId,
                        ScNombre = x.ScNombre
                    })
                    .ToList();

                response.Data = list;
                response.Message = ReplyMessage.MESSAGE_QUERY;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<BaseResponse<SacerdoteResponse>> GetSacerdoteById(int id)
        {
            var response = new BaseResponse<SacerdoteResponse>();

            try
            {
                var query = await _context.TblSacerdotes
                    .Where(x => x.ScId == id && x.ScDeleteUser == null && x.ScDeleteDate == null)
                    .Select(s => new SacerdoteResponse
                    {
                        SacerdoteId = s.ScId,
                        SacerdoteNombre = s.ScNombre,
                        SacerdoteIdCategoria = s.ScIdCategoria,
                        SacerdoteFirma = s.ScFirma,
                        SacerdoteEstado = s.ScEstado
                    }).FirstOrDefaultAsync();

                response.Data = query;
                response.IsSuccess = response.Data != null;
                response.Message = response.IsSuccess ? ReplyMessage.MESSAGE_QUERY : ReplyMessage.MESSAGE_QUERY_EMPTY;
            }
            catch (Exception ex)
            {
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<BaseResponse<bool>> RegisterSacerdote(SacerdoteRequest request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var maxId = 0;

                try
                {
                    maxId = _context.TblSacerdotes.Max(x => x.ScId) + 1;
                }
                catch (Exception)
                {
                    maxId = 1;
                }

                var sacerdote = new TblSacerdote
                {
                    ScId = maxId,
                    ScNombre = request.SacerdoteNombre,
                    ScIdCategoria = request.SacerdoteIdCategoria,
                    ScFirma = "No",
                    ScEstado = 1,
                    ScCreateUser = currentUserId,
                    ScCreateDate = DateTime.Now
                };

                await _context.TblSacerdotes.AddAsync(sacerdote);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
            }
            catch (Exception ex)
            {
                response.Message = ReplyMessage.MESSAGE_FAILED;
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> UpdateSacerdote(int sacerdoteId, SacerdoteRequest request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var sacerdote = await _context.TblSacerdotes
                    .Where(x => x.ScId == sacerdoteId && x.ScDeleteUser == null && x.ScDeleteDate == null)
                    .FirstOrDefaultAsync();

                if (sacerdote is null)
                {
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    response.IsSuccess = false;
                    return response;
                }

                sacerdote.ScNombre = request.SacerdoteNombre;
                sacerdote.ScIdCategoria = request.SacerdoteIdCategoria;
                sacerdote.ScUpdateUser = currentUserId;
                sacerdote.ScUpdateDate = DateTime.Now;

                _context.TblSacerdotes.Update(sacerdote);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_UPDATE;
            }
            catch (Exception ex)
            {
                response.Message = ReplyMessage.MESSAGE_FAILED;
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteSacerdote(int id)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var sacerdote = await _context.TblSacerdotes
                    .Where(x => x.ScId == id && x.ScDeleteUser == null && x.ScDeleteDate == null)
                    .FirstOrDefaultAsync();

                if (sacerdote is null)
                {
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    response.IsSuccess = false;
                    return response;
                }

                sacerdote.ScEstado = 0;
                sacerdote.ScDeleteUser = currentUserId;
                sacerdote.ScDeleteDate = DateTime.Now;

                _context.TblSacerdotes.Update(sacerdote);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_DELETE;
            }
            catch (Exception ex)
            {
                response.Message = ReplyMessage.MESSAGE_FAILED;
                response.IsSuccess = false;
            }

            return response;
        }
    }


}
