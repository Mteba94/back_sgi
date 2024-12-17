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
    public class CategoriaSacerdoteController : ControllerBase
    {
        private readonly CategoriaSacerdoteService _service;

        public CategoriaSacerdoteController(CategoriaSacerdoteService service)
        {
            _service = service;
        }

        [HasPermission(Permission.ListCategoriaSacerdote)]
        [HttpPost("List")]
        public async Task<IActionResult> ListCategoriaSacerdote([FromBody] BaseFiltersRequest request)
        {
            var response = await _service.ListCategoriaSacerdote(request);
            return Ok(response);
        }

        [HasPermission(Permission.ListCategoriaSacerdoteSelect)]
        [HttpGet("Select")]
        public IActionResult ListCategoriaSacerdoteSelect()
        {
            var response = _service.ListSelectCategoriaSacerdote();
            return Ok(response);
        }

        [HasPermission(Permission.RegisterCategoriaSacerdote)]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterCategoriaSacerdote([FromBody] CategoriaSacerdoteRequest request)
        {
            var response = await _service.RegisterCategoriaSacerdote(request);
            return Ok(response);
        }

        [HasPermission(Permission.GetCategoriaSacerdoteById)]
        [HttpGet("{categoriaId:int}")]
        public async Task<IActionResult> GetCategoriaSacerdoteById(int categoriaId)
        {
            var response = await _service.GetCatSacerdoteById(categoriaId);
            return Ok(response);
        }

        [HasPermission(Permission.UpdateCategoriaSacerdote)]
        [HttpPut("Update/{categoriaId:int}")]
        public async Task<IActionResult> UpdateCategoriaSacerdote(int categoriaId, [FromBody] CategoriaSacerdoteRequest request)
        {
            var response = await _service.UpdateCategoriaSacerdote(categoriaId, request);
            return Ok(response);
        }

        [HasPermission(Permission.DeleteCategoriaSacerdote)]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteCategoriaSacerdote(int id)
        {
            var response = await _service.DeleteCategoriaSacerdote(id);
            return Ok(response);
        }
    }
}
