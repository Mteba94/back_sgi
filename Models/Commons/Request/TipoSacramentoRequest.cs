namespace WebApi_SGI_T.Models.Commons.Request
{
    public class TipoSacramentoRequest
    {
        public string TsNombre { get; set; } = null!;

        public string TsDescripcion { get; set; } = null!;

        public string? TsRequerimiento { get; set; }

    }
}
