﻿using Microsoft.AspNetCore.Mvc;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Models.Certification;

namespace WebApi_SGI_T.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificationController : ControllerBase
    {
        private readonly CertificationService _certificationService;

        public CertificationController(CertificationService certificationService)
        {
            _certificationService = certificationService;
        }

        [HttpPost]
        public IActionResult GeneratePdf([FromBody] CertificationModel model)
        {
            string base64Pdf = _certificationService.GeneratedPdfBase64(model);

            //return Ok(new {fileName = "CertificadoBautismo.pdf", FileData = base64Pdf });
            return Ok(base64Pdf);
        }
    }
}
