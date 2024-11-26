namespace WebApi_SGI_T.Models.Commons.Response
{
    public class UsuarioResponse
    {
        public int UsIdUsuario { get; set; }
        public string UsUserName { get; set; } = null!;
        public string UsPass { get; set; } = null!;
        public string? UsImage { get; set; }
        public string UsNombre { get; set; } = null!;
        public DateTime UsFechaNacimiento { get; set; }
        public byte UsIdTipoDocumento { get; set; }
        public int UserIdRole { get; set; }
        public string UserRole { get; set; }
        public string UsNumerodocumento { get; set; } = null!;
        public byte? UsIdGenero { get; set; }
        public string UsDireccion { get; set; } = null!;
        public DateTime UsCreateDate { get; set; }
        public byte UsEstado { get; set; }
        public string EstadoDescripcion { get; set; } = null!;
    }
}
