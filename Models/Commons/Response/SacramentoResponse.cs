namespace WebApi_SGI_T.Models.Commons.Response
{
    public class SacramentoResponse
    {
        public int ScIdSacramento { get; set; }
        public string? ScNumeroPartida { get; set; }
        public string? ScTipoSacramento { get; set; }
        public string? PeNombre { get; set; }
        public string PeNumeroDocumento { get; set; } = null!;
        public DateTime ScFechaSacramento { get; set; }
        public string? ScObservaciones { get; set; }
        public DateTime ScCreateDate { get; set; }
    }
}
