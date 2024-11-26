using System.ComponentModel.DataAnnotations;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Models.Commons.Request
{
    public class UserRequestDto
    {
        [Required(ErrorMessage = ReplyMessage.MESSAGE_USERNAME)]
        public string UsUserName { get; set; } = null!;
        public string? UsPass { get; set; } = null!;
        public IFormFile? UsImage { get; set; }
        public string UsNombre { get; set; } = null!;
        public DateTime UsFechaNacimiento { get; set; }
        public byte UsIdTipoDocumento { get; set; }
        public string UsNumerodocumento { get; set; } = null!;
        public byte UsIdGenero { get; set; }
        public string UsDireccion { get; set; } = null!;

    }
}
