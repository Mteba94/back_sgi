using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Imp.Authentication;
using WebApi_SGI_T.Imp.Constancias;
using WebApi_SGI_T.Models.Commons.Request;

namespace WebApi_SGI_T.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HistConstanciaController : ControllerBase
    {
        private readonly HistoricoConstanciasService _historicoConstanciasService;
        private readonly ConstanciaByIdService _constanciaByIdService;
        private readonly AnularConstanciaService _anularConstanciaService;
        private readonly ConsultaAnulacionService _consultaAnulacionService;

        public HistConstanciaController(HistoricoConstanciasService historicoConstanciasService, ConstanciaByIdService constanciaByIdService, AnularConstanciaService anularConstanciaService, ConsultaAnulacionService consultaAnulacionService)
        {
            _historicoConstanciasService = historicoConstanciasService;
            _constanciaByIdService = constanciaByIdService;
            _anularConstanciaService = anularConstanciaService;
            _consultaAnulacionService = consultaAnulacionService;
        }

        [HasPermission(Permission.GetHistoricoConstancias)]
        [HttpPost]
        public async Task<IActionResult> GetHistoricoConstancias([FromBody] BaseFiltersRequest filters)
        {
            var response = await _historicoConstanciasService.GetHistoricoConstancias(filters);

            return Ok(response);
        }

        [HasPermission(Permission.ConstanciabyId)]
        [HttpGet("{constanciaId:int}")]
        public async Task<IActionResult> GetConstanciaById(int constanciaId)
        {
            var response = await _constanciaByIdService.GetConstanciaById(constanciaId);
            return Ok(response);
        }

        [HasPermission(Permission.HistConstanciaRegister)]
        [HttpPost("Register")]
        public async Task<IActionResult> HistConstanciaRegister(HistConstanciaRequest request)
        {
            var response = await _historicoConstanciasService.HistConstanciaRegister(request);

            return Ok(response);
        }

        [HasPermission(Permission.GenararCorrelativo)]
        [HttpPost("Correlativo/{sacramentoId:int}")]
        public async Task<IActionResult> GenararCorrelativo(int sacramentoId)
        {
            var response = await _historicoConstanciasService.GenerarCorrelativo(sacramentoId);
            return Ok(response);
        }

        [HasPermission(Permission.AnularConstancia)]
        [HttpPut("Anular/{constanciaId:int}")]
        public async Task<IActionResult> AnularConstancia(int constanciaId, [FromBody] HistConstanciaRequest request)
        {
            var response = await _anularConstanciaService.AnulaConstancia(constanciaId, request.ct_observaciones!);
            return Ok(response);
        }

        [HasPermission(Permission.ListConstanciasAnulacion)]
        [HttpPost("ConsultaConstancias")]
        public async Task<IActionResult> ListConstanciasAnulacion([FromBody] BaseFiltersRequest filters)
        {
            var response = _consultaAnulacionService.ListConstanciasAnulacion(filters);

            return Ok(response);
        }
    }
}
