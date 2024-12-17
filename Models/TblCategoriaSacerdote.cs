namespace WebApi_SGI_T.Models
{
    public class TblCategoriaSacerdote
    {
        public int CsId { get; set; }
        public string? CsNombre { get; set; }
        public string? CsAbreviacion { get; set; }
        public int CsEstado { get; set; }
        public virtual ICollection<TblSacerdote> TblSacerdote { get; set; } = null!;
    }
}
