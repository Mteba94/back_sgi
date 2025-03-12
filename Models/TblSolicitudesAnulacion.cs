namespace WebApi_SGI_T.Models
{
    public class TblSolicitudesAnulacion
    {
        public int saIdSolicitudAnulacion { get; set; }
        public int saIdConstancia { get; set; }
        public int saIdUsuarioSolicitante { get; set; }
        public DateTime saFechaSolicitud { get; set; }
        public int saEstado { get; set; }
        public int? saIdUsuarioAprobador { get; set; }
        public DateTime? saFechaAprobacion { get; set; }
        public string saMotivoSolicitud { get; set; }
        public string? saMotivoRechazo { get; set; }

        public virtual TblConstancias saIdConstanciaNavigation { get; set; } = null!;
        public virtual TblUsuario saIdUsuarioSolicitanteNavigation { get; set; } = null!;
        public virtual TblUsuario saIdUsuarioAprobadorNavigation { get; set; } = null!;
    }
}
