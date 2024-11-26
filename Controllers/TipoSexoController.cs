using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Imp.Authentication;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Request;

namespace WebApi_SGI_T.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TipoSexoController : ControllerBase
    {
        private readonly TipoSexoService _tipoSexoService;

        public TipoSexoController(TipoSexoService tipoSexoService)
        {
            _tipoSexoService = tipoSexoService;
        }

        [HasPermission(Permission.ListTipoSexoSelect)]
        [HttpGet("Select")]
        public IActionResult ListTipoSexoSelect()
        {
            var response = _tipoSexoService.ListSelectTipoSexo();
            return Ok(response);
        }

        [HasPermission(Permission.GetTipoSexoById)]
        [HttpGet("{idTipoGenero:int}")]
        public async Task<IActionResult> GetTipoSexoById(int idTipoGenero)
        {
            var response = await _tipoSexoService.GetTipoSexoById(idTipoGenero);
            return Ok(response);
        }

        [HasPermission(Permission.RegisterTipoSexo)]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterTipoSexo([FromBody] TipoSexoRequest request)
        {
            var response = await _tipoSexoService.RegisterTipoSexo(request);
            return Ok(response);
        }

        [HasPermission(Permission.UpdateTipoSexo)]
        [HttpPut("Update/{idTipoGenero:int}")]
        public async Task<IActionResult> UpdateTipoSexo(int idTipoGenero, [FromBody] TipoSexoRequest request)
        {
            var response = await _tipoSexoService.UpdateTipoSexo(idTipoGenero, request);
            return Ok(response);
        }

        [HasPermission(Permission.DeleteTipoSexo)]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteTipoSexo(int id)
        {
            var response = await _tipoSexoService.DeleteTipoSexo(id);
            return Ok(response);
        }
    }
}
