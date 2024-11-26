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
    public class RolController : ControllerBase
    {
        private readonly RolService _rolService;

        public RolController(RolService rolService)
        {
            _rolService = rolService;
        }

        [HasPermission(Permission.ListRolSelect)]
        [HttpGet("Select")]
        public IActionResult ListRolSelect()
        {
            var response = _rolService.ListSelectRol();
            return Ok(response);
        }

        [HasPermission(Permission.RegisterRol)]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterRol([FromBody] RolRequest request)
        {
            var response = await _rolService.RegisterRol(request);
            return Ok(response);
        }

        [HasPermission(Permission.GetRolById)]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetRolById(int id)
        {
            var response = await _rolService.GetRolById(id);
            return Ok(response);
        }

        [HasPermission(Permission.UpdateRol)]
        [HttpPut("Update/{rolId:int}")]
        public async Task<IActionResult> UpdateRol(int rolId, [FromBody] RolRequest request)
        {
            var response = await _rolService.UpdateRol(rolId, request);
            return Ok(response);
        }

        [HasPermission(Permission.DeleteRol)]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var response = await _rolService.DeleteRol(id);
            return Ok(response);
        }
    }
}
