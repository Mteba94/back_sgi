namespace WebApi_SGI_T.Models.Commons.Response
{
    public class SacerdoteResponse
    {
        public int SacerdoteId { get; set; }
        public string? SacerdoteNombre { get; set; }
        public int SacerdoteIdCategoria { get; set; }
        public string? SacerdoteCategoria { get; set; }
        public string? SacerdoteFirma { get; set; }
        public int SacerdoteEstado { get; set; }
        public string? SacerdoteEstadoDesc { get; set; }
    }
}
