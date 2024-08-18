using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;

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

        [HttpGet("Select")]
        public IActionResult ListTipoDocumentoSelect()
        {
            var response = _tipoDocumentoService.ListSelectTipoDocumento();
            return Ok(response);
        }
    }
}
