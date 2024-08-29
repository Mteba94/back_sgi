namespace WebApi_SGI_T.Models
{
    public partial class TblMatrimonio
    {
        public int MatrimonioId { get; set; }
        public int EsposoId { get; set; }
        public int EsposaId { get; set; }
        public byte MatrimonioEstado { get; set; }

        public virtual TblPersona EsposoNavigation { get; set; }
        public virtual TblPersona EsposaNavigation { get; set; }

        public bool IsValid()
        {
            return EsposoId != EsposaId; // Evitar matrimonios entre la misma persona.
        }

    }
}
