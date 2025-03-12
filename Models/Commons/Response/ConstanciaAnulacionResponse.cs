namespace WebApi_SGI_T.Models.Commons.Response
{
    public class ConstanciaAnulacionResponse
    {
        public int Sa_IdSolicitud {  get; set; }
        public int Sa_IdConstancia { get; set; }
        public string Pe_nombre { get; set; }
        public string Ts_nombre { get; set; }
        public int sa_IdUsuarioSolicitante { get; set; }
        public string us_solicita { get; set; }
        public DateTime sa_FechaSolicitud { get; set; }
        public string sa_Estado {  get; set; }
        public string sa_EstadoDescripcion { get; set; }
        public int? sa_IdUsuarioAprobador { get; set; }
        public string us_nombre { get; set; }
        public DateTime? sa_FechaAprobacion { get; set; }
        public string sa_MotivoSolicitud { get; set; }
        public string? sa_MotivoRechazo { get; set; }
    }
}
