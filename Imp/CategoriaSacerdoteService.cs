using Microsoft.EntityFrameworkCore;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Helpers;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp
{
    public class CategoriaSacerdoteService
    {
        readonly SgiSacramentosContext _context;

        public CategoriaSacerdoteService(SgiSacramentosContext context = null)
        {
            _context = context;
        }

        public async Task<BaseResponse<BaseEntityResponse<CategoriaSacerdoteResponse>>> ListCategoriaSacerdote(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<CategoriaSacerdoteResponse>>()
            {
                Data = new BaseEntityResponse<CategoriaSacerdoteResponse>()
            };

            try
            {
                var query = _context.TblCategoriaSacerdotes
                    .AsNoTracking();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            query = query.Where(x => x.CsNombre!.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    query = query.Where(x => x.CsEstado == filters.StateFilter);
                }

                var totalRecords = await query.CountAsync();
                var mappedData = query.Select(s => new CategoriaSacerdoteResponse
                {
                    CsId = s.CsId,
                    CsNombre = s.CsNombre,
                    CsAbreviacion = s.CsAbreviacion,
                    CsEstado = s.CsEstado,
                    CsEstadoDesc = (s.CsEstado == 1 ? "Activo" : "Inactivo"),
                }).ToList();

                if (filters.Sort is null) filters.Sort = "CsId";

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

        public BaseResponse<List<CategoriaSacerdoteSelectResponse>> ListSelectCategoriaSacerdote()
        {
            var response = new BaseResponse<List<CategoriaSacerdoteSelectResponse>>
            {
                Data = new List<CategoriaSacerdoteSelectResponse>()
            };

            try
            {
                var list = _context.TblCategoriaSacerdotes
                    .Where(x => x.CsEstado == 1)
                    .Select(x => new CategoriaSacerdoteSelectResponse
                    {
                        CsId = x.CsId,
                        CsNombre = x.CsNombre
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

        public async Task<BaseResponse<CategoriaSacerdoteResponse>> GetCatSacerdoteById(int id)
        {
            var response = new BaseResponse<CategoriaSacerdoteResponse>();

            try
            {
                var tipoDocumento = await _context.TblCategoriaSacerdotes
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.CsId == id);

                if (tipoDocumento != null)
                {
                    var mappedData = new CategoriaSacerdoteResponse
                    {
                        CsId = tipoDocumento.CsId,
                        CsNombre = tipoDocumento.CsNombre
                    };
                    response.Data = mappedData;
                    response.Message = ReplyMessage.MESSAGE_QUERY;
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
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RegisterCategoriaSacerdote(CategoriaSacerdoteRequest request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var maxId = 0;

                try
                {
                    maxId = _context.TblCategoriaSacerdotes.Max(x => x.CsId) + 1;
                }
                catch (Exception)
                {
                    maxId = 1;
                }


                byte newId = byte.Parse(maxId.ToString());

                var categoriaSacerdote = new TblCategoriaSacerdote
                {
                    CsId = newId,
                    CsNombre = request.CsNombre,
                    CsAbreviacion = request.CsAbreviacion,
                    CsEstado = 1
                };

                _context.TblCategoriaSacerdotes.Add(categoriaSacerdote);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ReplyMessage.MESSAGE_FAILED;
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> UpdateCategoriaSacerdote(int categoriaId, CategoriaSacerdoteRequest request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var categoriaSacerdote = await _context.TblCategoriaSacerdotes
                    .FirstOrDefaultAsync(x => x.CsId == categoriaId);

                if (categoriaSacerdote != null)
                {
                    categoriaSacerdote.CsNombre = request.CsNombre!;
                    categoriaSacerdote.CsAbreviacion = request.CsAbreviacion!;

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
                response.Message = ReplyMessage.MESSAGE_FAILED;
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteCategoriaSacerdote(int id)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var categoriaSacerdote = await _context.TblCategoriaSacerdotes
                    .FirstOrDefaultAsync(x => x.CsId == id);

                if (categoriaSacerdote != null)
                {
                    categoriaSacerdote.CsEstado = 0;

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
                response.Message = ReplyMessage.MESSAGE_FAILED;
                response.IsSuccess = false;
            }

            return response;
        }
    }
}
