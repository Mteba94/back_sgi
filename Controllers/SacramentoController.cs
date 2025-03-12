using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Imp.Authentication;
using WebApi_SGI_T.Imp.Matrimonio;
using WebApi_SGI_T.Imp.Sacramentos;
using WebApi_SGI_T.Models.Commons.Request;

namespace WebApi_SGI_T.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SacramentoController : ControllerBase
    {
        private readonly SacramentoService _sacramentoService;
        private readonly matrimonioDeleteService _matrimonioDeleteService;
        private readonly MatrimonioById _matrimonioById;
        private readonly EliminarSacramentoService _eliminaSacramento;

        public SacramentoController(SacramentoService sacramentoService, matrimonioDeleteService matrimonioDeleteService, MatrimonioById matrimonioById, EliminarSacramentoService eliminaSacramento)
        {
            _sacramentoService = sacramentoService;
            _matrimonioDeleteService = matrimonioDeleteService;
            _matrimonioById = matrimonioById;
            _eliminaSacramento = eliminaSacramento;
        }

        [HasPermission(Permission.ListSacramentos)]
        [HttpPost]
        public async Task<IActionResult> ListSacramentos([FromBody] BaseFiltersRequest filters)
        {
            var response = await _sacramentoService.ListSacramentos(filters);
            return Ok(response);
        }

        [HasPermission(Permission.getSacramentoById)]
        [HttpGet("{sacramentoId:int}")]
        public async Task<IActionResult> getSacramentoById(int sacramentoId)
        {
            var response = await _sacramentoService.GetSacramentoById(sacramentoId);
            return Ok(response);
        }

        [HasPermission(Permission.getMatrimonioById)]
        [HttpGet("Matrimonio/{sacramentoId:int}")]
        public async Task<IActionResult> getMatrimonioById2(int sacramentoId)
        {
            var response = await _matrimonioById.GetMatrimonioById(sacramentoId);
            return Ok(response);
        }

        [HasPermission(Permission.RegisterSacramento)]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterSacramento([FromBody] SacramentoRequestDto request)
        {
            var response = await _sacramentoService.RegisterSacramento(request);
            return Ok(response);
        }

        [HasPermission(Permission.RegisterMatrimonio)]
        [HttpPost("Register/Matrimonio")]
        public async Task<IActionResult> RegisterMatrimonio([FromBody] MatrimonioRequest request)
        {
            var response = await _sacramentoService.RegisterMatrimonio(request);
            return Ok(response);
        }

        [HasPermission(Permission.UpdateSacramento)]
        [HttpPut("Update/{sacramentoId:int}")]
        public async Task<IActionResult> UpdateSacramento(int sacramentoId, [FromBody] SacramentoRequestDto request)
        {
            var response = await _sacramentoService.UpdateSacramento(sacramentoId, request);
            return Ok(response);
        }

        [HasPermission(Permission.UpdateMatrimonio)]
        [HttpPut("Update/Matrimonio/{sacramentoId:int}")]
        public async Task<IActionResult> UpdateMatrimonio(int sacramentoId, [FromBody] MatrimonioRequest request)
        {
            var response = await _sacramentoService.UpdateMatrimonio(sacramentoId, request);
            return Ok(response);
        }

        [HasPermission(Permission.DeleteSacramento)]
        [HttpPut("Delete/{sacramentoId:int}")]
        public async Task<IActionResult> DeleteSacramento(int sacramentoId, [FromBody] SacramentoRequestDto request)
        {
            var response = await _eliminaSacramento.DeleteSacramento(sacramentoId, request);
            return Ok(response);
        }

        [HasPermission(Permission.DeleteMatrimonio)]
        [HttpPut("Delete/Matrimonio/{sacramentoId:int}")]
        public async Task<IActionResult> DeleteMatrimonio(int sacramentoId, [FromBody] MatrimonioRequest request)
        {
            var response = await _matrimonioDeleteService.DeleteMatrimonio(sacramentoId, request);
            return Ok(response);
        }
    }
}
