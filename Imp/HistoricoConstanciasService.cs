using Microsoft.EntityFrameworkCore;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Helpers;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApi_SGI_T.Imp
{
    public class HistoricoConstanciasService
    {
        private readonly SgiSacramentosContext _context;
        private readonly SacramentoService _sacramentoService;

        public HistoricoConstanciasService(SgiSacramentosContext context, SacramentoService sacramentoService)
        {
            _context = context;
            _sacramentoService = sacramentoService;
        }

        public async Task<BaseResponse<BaseEntityResponse<HistConstanciaResponse>>> GetHistoricoConstancias(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<HistConstanciaResponse>>()
            {
                Data = new BaseEntityResponse<HistConstanciaResponse>()
            };

            try
            {
                var constancias = _context.TblConstancias
                        .Include(c => c.ConstanciaNavigation)
                        .ThenInclude(cn => cn.ScIdSacramentoNavigation)
                        .Include(c => c.ConstanciaNavigation.ScIdpersonaNavigation)
                        .Include(c => c.UsuarioNavigation)
                        .AsNoTracking();

                if(constancias != null)
                {
                    if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                    {
                        switch (filters.NumFilter)
                        {
                            case 1:
                                constancias = constancias.Where(x => x.ConstanciaNavigation.ScIdpersonaNavigation.PeNombre.Contains(filters.TextFilter));
                                break;
                            case 2:
                                constancias = constancias.Where(x => x.ConstanciaNavigation.ScIdpersonaNavigation.PeNumeroDocumento.Contains(filters.TextFilter));
                                break;
                            case 3:
                                constancias = constancias.Where(x => x.ConstanciaNavigation.ScIdSacramentoNavigation.TsNombre.Contains(filters.TextFilter));
                                break;
                            case 4:
                                constancias = constancias.Where(x => x.ConstanciaNavigation.ScNumeroPartida.Contains(filters.TextFilter));
                                break;
                        }
                    }

                    if (filters.StateFilter is not null)
                    {
                        constancias = constancias.Where(x => x.ConstanciaNavigation.ScIdSacramentoNavigation.TsIdTipoSacramento == filters.StateFilter);
                    }

                    if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                    {
                        constancias = constancias.Where(x => x.ct_FechaImpresion >= Convert.ToDateTime(filters.StartDate)
                                                 && x.ct_FechaImpresion <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                    }

                    if (filters.Sort is null) filters.Sort = "ct_ConstanciaId";

                    var totalRecords = await constancias.CountAsync();
                    var mappedData = constancias.Select(constancia => new HistConstanciaResponse
                    {
                        ct_ConstanciaId = constancia.ct_ConstanciaId,
                        ct_SacramentoId = constancia.ct_SacramentoId,
                        ct_Correlativo = constancia.ct_Correlativo,
                        ct_FormatoCorrelativo = constancia.ct_FormatoCorrelativo,
                        ct_UsuarioId = constancia.ct_UsuarioId,
                        ct_FechaImpresion = constancia.ct_FechaImpresion,
                        ct_PeNombre = constancia.ConstanciaNavigation.ScIdpersonaNavigation.PeNombre,
                        ct_Sacramento = constancia.ConstanciaNavigation.ScIdSacramentoNavigation.TsNombre,
                        ct_Usuario = constancia.UsuarioNavigation.UsNombre
                    }).ToList();

                    var orderedList = OrderingHelper.Ordering(filters, mappedData.AsQueryable(), !(bool)filters.Download!);

                    response.IsSuccess = true;
                    response.Data.TotalRecords = totalRecords;
                    response.Data.Items = orderedList.ToList();
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> HistConstanciaRegister(HistConstanciaRequest request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var maxId = 0;

                try
                {
                    maxId = _context.TblConstancias.Max(x => x.ct_ConstanciaId) + 1;
                }
                catch (Exception)
                {
                    maxId = 1;
                }

                var ultimoCorrelativo = await _context.TblConstancias
                   .OrderByDescending(h => h.ct_Correlativo)
                   .Select(h => h.ct_Correlativo)
                   .FirstOrDefaultAsync();

                int nuevoCorrelativo = (ultimoCorrelativo != null) ? ultimoCorrelativo + 1 : 1;

                var constancia = new TblConstancias
                {
                    ct_ConstanciaId = maxId,
                    ct_SacramentoId = request.ct_SacramentoId,
                    ct_Correlativo = nuevoCorrelativo,
                    ct_FormatoCorrelativo = request.ct_correlativo,
                    ct_UsuarioId = 1,
                    ct_FechaImpresion = DateTime.Now
                };

                _context.TblConstancias.Add(constancia);
                await _context.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }

            return response;
        }

        public async Task<BaseResponse<String>> GenerarCorrelativo(int TipoSacramento)
        {
            var response = new BaseResponse<String>();


            var ultimoCorrelativo = await _context.TblConstancias
                .OrderByDescending(h => h.ct_Correlativo)
                .Select(h => h.ct_Correlativo)
                .FirstOrDefaultAsync();

            var tSacramento = "";

            var sacramento = await _sacramentoService.GetSacramentoById(TipoSacramento);
            var idTsacramento = sacramento.Data?.scIdTipoSacramento;

            switch (idTsacramento)
            {
                case 1:
                    tSacramento = "BAU";
                    break;
                case 2:
                    tSacramento = "PRC";
                    break;
                case 3:
                    tSacramento = "CON";
                    break;
                case 4:
                    tSacramento = "MAT";
                    break;
            }

            int nuevoCorrelativo = (ultimoCorrelativo != null) ? ultimoCorrelativo + 1 : 1;
            string correlativoFormateado = $"{tSacramento}-{DateTime.Now.Year}-{nuevoCorrelativo:D3}";

            if(!string.IsNullOrEmpty(correlativoFormateado))
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_QUERY;
                response.Data = correlativoFormateado;
            }
            else
            {
                response.IsSuccess = false;
                response.Message += ReplyMessage.MESSAGE_QUERY_EMPTY;
                response.Data = null;
            }

            return response;
        }
    }
}
