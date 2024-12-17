namespace WebApi_SGI_T.Models
{
    public class TblSacerdote
    {
        public int ScId { get; set; }
        public string? ScNombre { get; set; }
        public int ScIdCategoria { get; set; }
        public string? ScFirma { get; set; }
        public int ScEstado { get; set; }
        public int ScCreateUser { get; set; }
        public DateTime ScCreateDate { get; set; }
        public int? ScUpdateUser { get; set; }
        public DateTime? ScUpdateDate { get; set; }
        public int? ScDeleteUser { get; set; }
        public DateTime? ScDeleteDate { get; set; }
        public virtual TblCategoriaSacerdote ScIdCategoriaNavigation { get; set; } = null!;
        public virtual ICollection<TblSacramento> TblSacramentos { get; set; } = null!;
    }
}
