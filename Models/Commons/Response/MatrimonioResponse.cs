namespace WebApi_SGI_T.Models.Commons.Response
{
    public class MatrimonioResponse
    {
        public int? ma_IdMatrimonio { get; set; }
        public int? ma_esposo { get; set; }
        public int? ma_esposa { get; set; }
        public int? scIdSacramentoEsposo { get; set; }
        public int? scIdSacramentoEsposa { get; set; }
        public int? MatrimonioEstado { get; set; }
        public int? ScIdTipoSacramento { get; set; }
        public string? ScNumeroPartida { get; set; }
        public string? PeNombreEsposo { get; set; }
        public string? PeNombreEsposa { get; set; }
        public DateTime PeFechaNacimientoEsposo { get; set; }
        public DateTime PeFechaNacimientoEsposa { get; set; }
        public byte PeIdTipoDocumentoEsposo { get; set; }
        public byte PeIdTipoDocumentoEsposa { get; set; }
        public string PeNumeroDocumentoEsposo { get; set; } = null!;
        public string PeNumeroDocumentoEsposa { get; set; } = null!;
        public byte PeSexoIdEsposo { get; set; }
        public byte PeSexoIdEsposa { get; set; }
        public string? PeDireccionEsposo { get; set; }
        public string? PeDireccionEsposa { get; set; }
        public string? ScPadreEsposo { get; set; }
        public string? ScPadreEsposa { get; set; }
        public string? ScMadreEsposo { get; set; }
        public string? ScMadreEsposa { get; set; }
        public string? ScTestigo1 { get; set; }
        public string? ScTestigo2 { get; set; }
        public int? ScParrocoId { get; set; }
        public DateTime? ScFechaSacramento { get; set; }
        public string? ScObservaciones { get; set; }
    }
}
