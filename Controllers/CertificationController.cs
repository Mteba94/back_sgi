using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Imp.Authentication;
using WebApi_SGI_T.Models.Certification;

namespace WebApi_SGI_T.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CertificationController : ControllerBase
    {
        private readonly CertificationService _certificationService;

        public CertificationController(CertificationService certificationService)
        {
            _certificationService = certificationService;
        }

        [HasPermission(Permission.GeneratePdf)]
        [HttpPost]
        public IActionResult GeneratePdf([FromBody] CertificationModel model)
        {
            try
            {
                var response = _certificationService.GeneratedPdfBase64(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
