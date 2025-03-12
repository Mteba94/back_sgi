namespace WebApi_SGI_T.Models.Commons.Response
{
    public class HistConstanciaResponse
    {
        public int ct_ConstanciaId { get; set; }
        public int ct_SacramentoId { get; set; }
        public string? ct_Sacramento { get; set; }
        public string? ct_PeNombre { get; set; }
        public int ct_Correlativo { get; set; }
        public string? ct_FormatoCorrelativo { get; set; }
        public int ct_UsuarioId { get; set; }
        public DateTime ct_FechaImpresion { get; set; }
        public string? ct_Usuario { get; set; }
        public int ct_Estado { get; set; }
        public string? ct_EstadoDescripcion { get; set; }
        public string? ct_Observacion { get; set; }
        public int? ct_UsuarioRechazo { get; set; }
        public string? ct_UsuarioRechazoNombre { get; set; }
        public DateTime? ct_FechaRechazo { get; set; }
    }
}
