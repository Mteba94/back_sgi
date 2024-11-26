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
        getMatrimonioById = 11,
        RegisterMatrimonio = 12,
        UpdateMatrimonio = 13,

        //TipoDocumentoController
        ListTipoDocumentoSelect = 14,
        RegisterTipoDocumento = 15,
        GetTipoDocumentoById = 16,
        UpdateTipoDocumento = 17,
        DeleteTipoDocumento = 18,

        //UsuarioController
        RegisterUser = 19,
        GenerateToken = 20,
        GetUsuarioByUserName = 21,
        ListUsuarios = 22,
        UpdateUsuario = 23,
        DeleteUsuario = 24,
        ResetPass = 42,

        //CertificationController
        GeneratePdf = 25,

        //DashboardController
        GetCantidadSacramentos = 26,

        //HistConstanciaController
        GetHistoricoConstancias = 27,
        HistConstanciaRegister = 28,
        GenararCorrelativo = 29,

        //RolController
        ListRolSelect = 30,
        RegisterRol = 31,
        GetRolById = 32,
        UpdateRol = 33,
        DeleteRol = 34,

        //TipoSexoController
        ListTipoSexoSelect = 35,
        GetTipoSexoById = 36,
        RegisterTipoSexo = 37,
        UpdateTipoSexo = 38,
        DeleteTipoSexo = 39,

        //UserRolController
        RegisterUserRol = 40,
        DeleteUserRol = 41,
    }
}
