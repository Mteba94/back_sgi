using Microsoft.EntityFrameworkCore;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Models.Commons.Response.Dashboard;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp
{
    public class DashboardService
    {
        private readonly SgiSacramentosContext _context;

        public DashboardService(SgiSacramentosContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<BaseEntityResponse<IndicadorSacramentosResponse>>> IndicadorSacramentos(BaseFiltersRequest filters)
        {

            var response = new BaseResponse<BaseEntityResponse<IndicadorSacramentosResponse>>()
            {
                Data = new BaseEntityResponse<IndicadorSacramentosResponse>()
            };
            try
            {

                var query = _context.TblSacramentos
                        .Join(_context.TblPersonas,
                            sac => sac.ScIdpersona,
                            pe => pe.PeIdpersona,
                            (sac, pe) => new { sac, pe })
                        .Join(_context.TblTipoSacramentos,
                            sacPe => sacPe.sac.ScIdTipoSacramento,
                            ts => ts.TsIdTipoSacramento,
                            (sacPe, ts) => new { sacPe.sac, sacPe.pe, ts.TsNombre, sacPe.sac.ScFechaSacramento })
                        .Where(x => x.sac.ScDeleteDate == null && x.sac.ScDeleteUser == null)
                        .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            query = query.Where(x => x.pe.PeSexoId == byte.Parse(filters.TextFilter));
                            break;
                        case 2:
                            //response.Data.Items = response.Data.Items.Where(x => x.TsDescripcion.Contains(filters.TextFilter)).ToList();
                            break;
                    }
                }

                var data = await query
                .GroupBy(x => new { x.TsNombre, anio = x.ScFechaSacramento.Year })
                .Select(group => new IndicadorSacramentosResponse
                {
                    sacramentos = group.Key.TsNombre,
                    total = group.Count(),
                    anio = group.Key.anio
                })
                .ToListAsync();

                var totalRecords = data.Count();

                response.Data.TotalRecords = totalRecords;
                response.Data.Items = data;
                response.Message = ReplyMessage.MESSAGE_QUERY;
                response.IsSuccess = true;
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
