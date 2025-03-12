namespace WebApi_SGI_T.Models
{
    public class TblConstancias
    {
        public int ct_ConstanciaId { get; set; }
        public int ct_SacramentoId { get; set; }
        public int ct_Correlativo { get; set; }
        public string? ct_FormatoCorrelativo { get; set; }
        public int ct_UsuarioId { get; set; }
        public DateTime ct_FechaImpresion { get; set; }
        public int ct_Estado { get; set; }
        public string? ct_Observaciones { get; set; }
        public int? ct_UsuarioRechazo { get; set; }
        public DateTime? ct_FechaRechazo { get; set; }

        public virtual TblSacramento ConstanciaNavigation { get; set; }
        public virtual TblUsuario UsuarioNavigation { get; set; }
        public virtual TblUsuario UsuarioRechazoNavigation { get; set; }

        public virtual ICollection<TblSolicitudesAnulacion>? SolicitudesAnulacion { get; set; }
    }
}
