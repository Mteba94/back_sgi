namespace WebApi_SGI_T.Models.Commons.Response
{
    public class TipoSacramentoResponse
    {
        public int TsIdTipoSacramento { get; set; }

        public string TsNombre { get; set; } = null!;

        public string TsDescripcion { get; set; } = null!;

        public string? TsRequerimiento { get; set; }
        public DateTime? TsCreateDate { get; set; }
        public int TsEstado { get; set; }
        public string? EstadoDescripcion { get; set; }
    }
}
