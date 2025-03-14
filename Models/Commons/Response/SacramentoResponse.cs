﻿namespace WebApi_SGI_T.Models.Commons.Response
{
    public class SacramentoResponse
    {
        public int ScIdSacramento { get; set; }
        public string? ScNumeroPartida { get; set; }
        public int scIdTipoSacramento { get; set; }
        public string? ScTipoSacramento { get; set; }
        public int? scMatrimonioId { get; set; }
        public string? PeNombre { get; set; }
        public int? PeEdad { get; set; }
        public DateTime PeFechaNacimiento { get; set; }
        public int PeIdTipoDocumento { get; set; }
        public string? PeTipoDocumento { get; set; }
        public string? PeNumeroDocumento { get; set; } = null!;
        public int? PeSexoId { get; set; }
        public string? PeDireccion { get; set; }
        public string? ScNombrePadre { get; set; }
        public string? ScNombreMadre { get; set; }
        public string? ScNombrePadrino { get; set; }
        public string? ScNombreMadrina { get; set; }
        public DateTime ScFechaSacramento { get; set; }
        public int? ScParrocoId { get; set; }
        public string? ScObservaciones { get; set; }
        public DateTime ScCreateDate { get; set; }
        public string? ScLugarBautizo { get; set; }
    }
}
