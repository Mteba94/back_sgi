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
    public class TipoDocumentoController : ControllerBase
    {
        private readonly TipoDocumentoService _tipoDocumentoService;

        public TipoDocumentoController(TipoDocumentoService tipoDocumentoService)
        {
            _tipoDocumentoService = tipoDocumentoService;
        }

        [HasPermission(Permission.ListTipoDocumentoSelect)]
        [HttpGet("Select")]
        public IActionResult ListTipoDocumentoSelect()
        {
            var response = _tipoDocumentoService.ListSelectTipoDocumento();
            return Ok(response);
        }

        [HasPermission(Permission.RegisterTipoDocumento)]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterTipoDocumento([FromBody] TipoDocumentoRequest request)
        {
            var response = await _tipoDocumentoService.RegisterTipoDocumento(request);
            return Ok(response);
        }

        [HasPermission(Permission.GetTipoDocumentoById)]
        [HttpGet("{tipoDocumentoId:int}")]
        public async Task<IActionResult> GetTipoDocumentoById(int tipoDocumentoId)
        {
            var response = await _tipoDocumentoService.GetTipoDocumentoById(tipoDocumentoId);
            return Ok(response);
        }

        [HasPermission(Permission.UpdateTipoDocumento)]
        [HttpPut("Update/{tipoDocumentoId:int}")]
        public async Task<IActionResult> UpdateTipoDocumento(int tipoDocumentoId, [FromBody] TipoDocumentoRequest request)
        {
            var response = await _tipoDocumentoService.UpdateTipoDocumento(tipoDocumentoId, request);
            return Ok(response);
        }

        [HasPermission(Permission.DeleteTipoDocumento)]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteTipoDocumento(int id)
        {
            var response = await _tipoDocumentoService.DeleteTipoDocumento(id);
            return Ok(response);
        }
    }
}
