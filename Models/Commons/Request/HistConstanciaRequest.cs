namespace WebApi_SGI_T.Models.Commons.Request
{
    public class HistConstanciaRequest
    {
        public int ct_SacramentoId { get; set; }
        public int ct_UsuarioId { get; set; }
        public string? ct_correlativo { get; set; }
        public string? ct_observaciones { get; set; }
    }
}
