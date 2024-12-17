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
    public class SacerdoteController : ControllerBase
    {
        private readonly SacerdoteService _service;

        public SacerdoteController(SacerdoteService service)
        {
            _service = service;
        }

        [HasPermission(Permission.ListSacerdote)]
        [HttpPost("List")]
        public async Task<IActionResult> ListSacerdote([FromBody] BaseFiltersRequest request)
        {
            var response = await _service.ListSacerdote(request);
            return Ok(response);
        }

        [HasPermission(Permission.ListSelectSacerdote)]
        [HttpGet("Select")]
        public IActionResult ListSelectSacerdote()
        {
            var response = _service.ListSelectSacerdote();
            return Ok(response);
        }

        [HasPermission(Permission.GetSacerdoteById)]
        [HttpGet("{sacerdoteId:int}")]
        public async Task<IActionResult> GetSacerdoteById(int sacerdoteId)
        {
            var response = await _service.GetSacerdoteById(sacerdoteId);
            return Ok(response);
        }

        [HasPermission(Permission.RegisterSacerdote)]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterSacerdote([FromBody] SacerdoteRequest request)
        {
            var response = await _service.RegisterSacerdote(request);
            return Ok(response);
        }

        [HasPermission(Permission.UpdateSacerdote)]
        [HttpPut("Update/{sacerdoteId:int}")]
        public async Task<IActionResult> UpdateSacerdote(int sacerdoteId, [FromBody] SacerdoteRequest request)
        {
            var response = await _service.UpdateSacerdote(sacerdoteId, request);
            return Ok(response);
        }

        [HasPermission(Permission.DeleteSacerdote)]
        [HttpPut("Delete/{sacerdoteId:int}")]
        public async Task<IActionResult> DeleteSacerdote(int sacerdoteId)
        {
            var response = await _service.DeleteSacerdote(sacerdoteId);
            return Ok(response);
        }

    }
}
