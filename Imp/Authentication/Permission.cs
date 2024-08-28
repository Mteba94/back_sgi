namespace WebApi_SGI_T.Imp.Authentication
{
    public enum Permission
    {
        //TipoSacramentoController
        ListTipoSacramento = 1,
        ListTipoSacramentoSelect = 2,
        GetTipoSacramento = 3,
        CreateTipoSacramento = 4,
        UpdateTipoSacramento = 5,
        RemoveTipoSacramento = 6,

        //SacramentoController
        ListSacramentos = 7,
        getSacramentoById = 8,
        RegisterSacramento = 9,
        UpdateSacramento = 10,

        //TipoDocumentoController
        ListTipoDocumentoSelect = 11,

        //UsuarioController
        RegisterUser = 12,
        GenerateToken = 13
    }
}
