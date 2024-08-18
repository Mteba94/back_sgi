using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Models.Commons.Request;

namespace WebApi_SGI_T.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TipoSacramentoController : ControllerBase
    {
        private readonly TipoSacramentoService _tipoSacramentoService;

        public TipoSacramentoController(TipoSacramentoService tipoSacramentoService)
        {
            _tipoSacramentoService = tipoSacramentoService;
        }
        [HttpPost]
        public IActionResult ListTipoSacramento([FromBody] BaseFiltersRequest filters)
        {
            var response = _tipoSacramentoService.ListTipoSacramento(filters);
            return Ok(response);
        }

        [HttpGet("Select")]
        public IActionResult ListTipoSacramentoSelect()
        {
            var response = _tipoSacramentoService.ListSelectTipoSacramento();
            return Ok(response);
        }

        [HttpGet("{tipoSacramentoId:int}")]
        public IActionResult GetTipoSacramento(int tipoSacramentoId)
        {
            var response = _tipoSacramentoService.GetTipoSacramentoById(tipoSacramentoId);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> CreateTipoSacramento([FromBody] TipoSacramentoRequest request)
        {
            var response = await _tipoSacramentoService.RegisterTipoSacramento(request);
            return Ok(response);
        }

        [HttpPut("Update/{tipoSacramentoId:int}")]
        public async Task<IActionResult> UpdateTipoSacramento(int tipoSacramentoId, [FromBody] TipoSacramentoRequest request)
        {
            var response = await _tipoSacramentoService.UpdateTipoSacramento(tipoSacramentoId, request);
            return Ok(response);
        }

        [HttpPut("Remove/{tipoSacramentoId:int}")]
        public async Task<IActionResult> RemoveTipoSacramento(int tipoSacramentoId)
        {
            var response = await _tipoSacramentoService.DeleteTipoSacramento(tipoSacramentoId);
            return Ok(response);
        }
    }
}
