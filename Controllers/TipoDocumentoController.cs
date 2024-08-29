using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Models.Commons.Request;

namespace WebApi_SGI_T.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TipoDocumentoController : ControllerBase
    {
        private readonly TipoDocumentoService _tipoDocumentoService;

        public TipoDocumentoController(TipoDocumentoService tipoDocumentoService)
        {
            _tipoDocumentoService = tipoDocumentoService;
        }

        [HttpGet("Select")]
        public IActionResult ListTipoDocumentoSelect()
        {
            var response = _tipoDocumentoService.ListSelectTipoDocumento();
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterTipoDocumento([FromBody] TipoDocumentoRequest request)
        {
            var response = await _tipoDocumentoService.RegisterTipoDocumento(request);
            return Ok(response);
        }
    }
}
