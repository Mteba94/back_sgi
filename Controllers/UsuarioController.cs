using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;

namespace WebApi_SGI_T.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("{userName}")]
        public IActionResult GetUsuarioByUserName(string userName)
        {
            var response = _usuarioService.GetUsuarioByUserName(userName);
            return Ok(response);
        }
    }
}
