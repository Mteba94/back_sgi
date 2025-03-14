﻿namespace WebApi_SGI_T.Models.Commons.Request
{
    public class SacramentoRequestDto
    {
        public string? ScNumeroPartida { get; set; }
        public int? ScIdTipoSacramento { get; set; }
        public string? PeNombre { get; set; }
        public string? PeEdad { get; set; }
        public DateTime PeFechaNacimiento { get; set; }
        public byte PeIdTipoDocumento { get; set; }
        public string PeNumeroDocumento { get; set; } = null!;
        public int PeSexoId { get; set; }
        public string? PeDireccion { get; set; }
        public string? ScPadre { get; set; }
        public string? ScMadre { get; set; }
        public string? ScPadrino { get; set; }
        public string? ScMadrina { get; set; }
        public int ScParroco { get; set; }
        public DateTime ScFechaSacramento { get; set; }
        public string? ScObservaciones { get; set; }
        public string? ScLugarBautizo { get; set; }
    }
}
