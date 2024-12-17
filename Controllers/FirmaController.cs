using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Imp.Authentication;

namespace WebApi_SGI_T.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FirmaController : ControllerBase
    {
        private readonly FirmaService _firmaService;

        public FirmaController(FirmaService firmaService)
        {
            _firmaService = firmaService;
        }

        [HasPermission(Permission.ListSelectFirma)]
        [HttpGet("ListSelectFirma")]
        public IActionResult ListSelectFirma()
        {
            var response = _firmaService.ListSelectFirma();
            return Ok(response);
        }

        [HasPermission(Permission.RegisterFirma)]
        [HttpPost("RegisterFirma/{sacerdoteId:int}")]
        public async Task<IActionResult> RegisterFirma(int sacerdoteId)
        {
            var response = await _firmaService.RegisterFirma(sacerdoteId);
            return Ok(response);
        }

        [HasPermission(Permission.DeleteFirma)]
        [HttpPut("DeleteFirma")]
        public async Task<IActionResult> DeleteFirma(int id)
        {
            var response = await _firmaService.DeleteFirma(id);
            return Ok(response);
        }
    }
}
