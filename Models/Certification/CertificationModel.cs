namespace WebApi_SGI_T.Models.Certification
{
    public class CertificationModel
    {
        public int idTipoSacramento { get; set; }
        public string? TipoSacramento { get; set; }
        public string? Numero { get; set; }
        public string? Folio { get; set; }
        public string? Partida { get; set; }
        public string? Dia { get; set; }
        public string? Mes { get; set; }
        public string? Anio { get; set; }
        public string? correlativo {  get; set; }
        public string? NombreBautizado { get; set; }
        public string? NombreEsposa { get; set; }
        public string? FechaNacimiento { get; set; }
        public string? FechaNacimientoEsposa { get; set; }
        public int edad { get; set; }
        public string? NombrePadre { get; set; }
        public string? NombrePadreEsposa { get; set; }
        public string? NombreMadre { get; set; }
        public string? NombreMadreEsposa { get; set; }
        public string[]? NombrePadrinos { get; set; }
        public string? NombreSacerdote { get; set; }
        public string? SacerdoteRealizaCat { get; set; }
        public string? AnotacionMarginal { get; set; }
        public string? DiaExpedicion { get; set; }
        public string? MesExpedicion { get; set; }
        public string? AnioExpedicion { get; set; }
        public string? SacerdoteFirma { get; set; }
        public string? SacerdoteCat { get; set; }
        public string? tituloSacerdotal { get; set; }
    }
}
