using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Models.Commons.Request;

namespace WebApi_SGI_T.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HistConstanciaController : ControllerBase
    {
        private readonly HistoricoConstanciasService _historicoConstanciasService;

        public HistConstanciaController(HistoricoConstanciasService historicoConstanciasService)
        {
            _historicoConstanciasService = historicoConstanciasService;
        }

        [HttpPost]
        public async Task<IActionResult> GetHistoricoConstancias([FromBody] BaseFiltersRequest filters)
        {
            var response = await _historicoConstanciasService.GetHistoricoConstancias(filters);

            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> HistConstanciaRegister(HistConstanciaRequest request)
        {
            var response = await _historicoConstanciasService.HistConstanciaRegister(request);

            return Ok(response);
        }

        [HttpPost("Correlativo/{sacramentoId:int}")]
        public async Task<IActionResult> GenararCorrelativo(int sacramentoId)
        {
            var response = await _historicoConstanciasService.GenerarCorrelativo(sacramentoId);
            return Ok(response);
        }
    }
}
