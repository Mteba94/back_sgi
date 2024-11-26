using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Imp.Authentication;
using WebApi_SGI_T.Models.Commons.Request;

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

        [HasPermission(Permission.RegisterUser)]
        [Authorize]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromForm] UserRequestDto requestDto)
        {
            var response = await _usuarioService.RegisterUser(requestDto);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Generate/Token")]
        public async Task<IActionResult> GenerateToken([FromBody] TokenRequestDto requestDto)
        {
            var response = await _usuarioService.GenerateToken(requestDto);
            return Ok(response);
        }

        [HasPermission(Permission.GetUsuarioByUserName)]
        [Authorize]
        [HttpGet("{userName}")]
        public async Task<IActionResult> GetUsuarioByUserName(string userName)
        {
            var response = await _usuarioService.GetUsuarioByUserName(userName);
            return Ok(response);
        }

        [HasPermission(Permission.ListUsuarios)]
        [Authorize]
        [HttpPost("List")]
        public async Task<IActionResult> ListUsuarios([FromBody] BaseFiltersRequest filters)
        {
            var response = await _usuarioService.ListUsuarios(filters);
            return Ok(response);
        }

        [HasPermission(Permission.UpdateUsuario)]
        [Authorize]
        [HttpPut("Update/{userId:int}")]
        public async Task<IActionResult> UpdateUsuario(int userId, [FromForm] UserRequestDto requestDto)
        {
            var response = await _usuarioService.UpdateUser(userId, requestDto);
            return Ok(response);
        }

        [HasPermission(Permission.DeleteUsuario)]
        [Authorize]
        [HttpPut("Delete")]
        public async Task<IActionResult> DeleteUsuario(int userId)
        {
            var response = await _usuarioService.DeleteUser(userId);
            return Ok(response);
        }

        [HasPermission(Permission.ResetPass)]
        [Authorize]
        [HttpGet("Reset/{userId:int}")]
        public async Task<IActionResult> ResetPass(int userId)
        {
            var response = await _usuarioService.resetPassword(userId);
            return Ok(response);
        }
    }
}
