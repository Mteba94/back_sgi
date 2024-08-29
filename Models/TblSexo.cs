namespace WebApi_SGI_T.Models
{
    public partial class TblSexo
    {
        public byte SexoId { get; set; }
        public string SexoNombre { get; set; } = null!;
        public string SexoAbreviacion { get; set; } = null!;
        public byte SexoEstado { get; set; }

        public virtual ICollection<TblPersona> TblPersona { get; set; } = new List<TblPersona>();
        public virtual ICollection<TblUsuario> TblUsuario { get; set; } = new List<TblUsuario>();
    }
}
