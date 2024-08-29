using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Models;
using WebApi_SGI_T.Models.Commons.Request;

namespace WebApi_SGI_T.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoSexoController : ControllerBase
    {
        private readonly TipoSexoService _tipoSexoService;

        public TipoSexoController(TipoSexoService tipoSexoService)
        {
            _tipoSexoService = tipoSexoService;
        }

        [HttpGet("Select")]
        public IActionResult ListTipoSexoSelect()
        {
            var response = _tipoSexoService.ListSelectTipoSexo();
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterTipoSexo([FromBody] TipoSexoRequest request)
        {
            var response = await _tipoSexoService.RegisterTipoSexo(request);
            return Ok(response);
        }
    }
}
