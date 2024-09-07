using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Imp.Authentication;
using WebApi_SGI_T.Models.Commons.Request;

namespace WebApi_SGI_T.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SacramentoController : ControllerBase
    {
        private readonly SacramentoService _sacramentoService;

        public SacramentoController(SacramentoService sacramentoService)
        {
            _sacramentoService = sacramentoService;
        }

        //[HasPermission(Permission.ListSacramentos)]
        [HttpPost]
        public async Task<IActionResult> ListSacramentos([FromBody] BaseFiltersRequest filters)
        {
            var response = await _sacramentoService.ListSacramentos(filters);
            return Ok(response);
        }

        [HttpGet("{sacramentoId:int}")]
        public async Task<IActionResult> getSacramentoById(int sacramentoId)
        {
            var response = await _sacramentoService.GetSacramentoById(sacramentoId);
            return Ok(response);
        }

        [HttpGet("Matrimonio/{sacramentoId:int}")]
        public async Task<IActionResult> getMatrimonioById(int sacramentoId)
        {
            var response = await _sacramentoService.GetMatrimonioById(sacramentoId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterSacramento([FromBody] SacramentoRequestDto request)
        {
            var response = await _sacramentoService.RegisterSacramento(request);
            return Ok(response);
        }

        [HttpPost("Register/Matrimonio")]
        public async Task<IActionResult> RegisterMatrimonio([FromBody] MatrimonioRequest request)
        {
            var response = await _sacramentoService.RegisterMatrimonio(request);
            return Ok(response);
        }

        [HttpPut("Update/{sacramentoId:int}")]
        public async Task<IActionResult> UpdateSacramento(int sacramentoId, [FromBody] SacramentoRequestDto request)
        {
            var response = await _sacramentoService.UpdateSacramento(sacramentoId, request);
            return Ok(response);
        }

        [HttpPut("Update/Matrimonio/{sacramentoId:int}")]
        public async Task<IActionResult> UpdateMatrimonio(int sacramentoId, [FromBody] MatrimonioRequest request)
        {
            var response = await _sacramentoService.UpdateMatrimonio(sacramentoId, request);
            return Ok(response);
        }
    }
}
