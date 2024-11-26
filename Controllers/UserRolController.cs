using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Imp.Authentication;

namespace WebApi_SGI_T.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolController : ControllerBase
    {
        private readonly UserRolService _userRolService;

        public UserRolController(UserRolService userRolService)
        {
            _userRolService = userRolService;
        }

        //[HttpGet("List")]
        //public IActionResult ListUserRol()
        //{
        //    var response = _userRolService.ListUserRol();
        //    return Ok(response);
        //}

        [HasPermission(Permission.RegisterUserRol)]
        [HttpPost("Assign/{userId:int}/{roleId:int}")]
        public async Task<IActionResult> RegisterUserRol(int userId, int roleId)
        {
            var response = await _userRolService.AssignRoleToUser(userId, roleId);
            return Ok(response);
        }

        [HttpGet("GetById/{userId:int}")]
        public async Task<IActionResult> GetUserRolById(int userId)
        {
            var response = await _userRolService.GetUserRolById(userId);
            return Ok(response);
        }

        //[HttpPut("Update/{userRolId:int}")]
        //public async Task<IActionResult> UpdateUserRol(int userRolId, [FromBody] UserRolRequest request)
        //{
        //    var response = await _userRolService.UpdateUserRol(userRolId, request);
        //    return Ok(response);
        //}

        [HasPermission(Permission.DeleteUserRol)]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserRol(int id)
        {
            var response = await _userRolService.DeleteUserRol(id);
            return Ok(response);
        }
    }
}
