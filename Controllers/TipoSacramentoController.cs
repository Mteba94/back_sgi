using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Imp.Authentication;
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

        [HasPermission(Permission.ListTipoSacramento)]
        [HttpPost]
        public IActionResult ListTipoSacramento([FromBody] BaseFiltersRequest filters)
        {
            var response = _tipoSacramentoService.ListTipoSacramento(filters);
            return Ok(response);
        }

        [HasPermission(Permission.ListTipoSacramentoSelect)]
        [HttpGet("Select")]
        public IActionResult ListTipoSacramentoSelect()
        {
            var response = _tipoSacramentoService.ListSelectTipoSacramento();
            return Ok(response);
        }

        [HasPermission(Permission.GetTipoSacramento)]
        [HttpGet("{tipoSacramentoId:int}")]
        public IActionResult GetTipoSacramento(int tipoSacramentoId)
        {
            var response = _tipoSacramentoService.GetTipoSacramentoById(tipoSacramentoId);
            return Ok(response);
        }

        [HasPermission(Permission.CreateTipoSacramento)]
        [HttpPost("Register")]
        public async Task<IActionResult> CreateTipoSacramento([FromBody] TipoSacramentoRequest request)
        {
            var response = await _tipoSacramentoService.RegisterTipoSacramento(request);
            return Ok(response);
        }

        [HasPermission(Permission.UpdateTipoSacramento)]
        [HttpPut("Update/{tipoSacramentoId:int}")]
        public async Task<IActionResult> UpdateTipoSacramento(int tipoSacramentoId, [FromBody] TipoSacramentoRequest request)
        {
            var response = await _tipoSacramentoService.UpdateTipoSacramento(tipoSacramentoId, request);
            return Ok(response);
        }

        [HasPermission(Permission.RemoveTipoSacramento)]
        [HttpPut("Remove/{tipoSacramentoId:int}")]
        public async Task<IActionResult> RemoveTipoSacramento(int tipoSacramentoId)
        {
            var response = await _tipoSacramentoService.DeleteTipoSacramento(tipoSacramentoId);
            return Ok(response);
        }
    }
}
