using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Models.Commons.Request;

namespace WebApi_SGI_T.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpPost("CantidadSacramentos")]
        public async Task<IActionResult> GetCantidadSacramentos([FromBody] BaseFiltersRequest filters)
        {
            var response = await _dashboardService.IndicadorSacramentos(filters);
            return Ok(response);
        }
    }
}
