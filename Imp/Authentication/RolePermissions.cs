using WebApi_SGI_T.Controllers;

namespace WebApi_SGI_T.Imp.Authentication
{
    public static class RolePermissions
    {
        public static List<Permission> AdminPermissions => Enum.GetValues(typeof(Permission)).Cast<Permission>().ToList();
        public static List<Permission> SecretaryPermissions => new List<Permission>
        {
            //TipoSacramentoController
            Permission.ListTipoSacramento,
            Permission.ListTipoSacramentoSelect,
            Permission.GetTipoSacramento,

            //SacramentoController
            Permission.ListSacramentos,
            Permission.getSacramentoById,
            Permission.RegisterSacramento,
            Permission.UpdateSacramento,
            Permission.getMatrimonioById,
            Permission.RegisterMatrimonio,
            Permission.UpdateMatrimonio,


            //TipoDocumentoController
            Permission.ListTipoDocumentoSelect,
            Permission.GetTipoDocumentoById,

            //UsuarioController
            Permission.GenerateToken,
            Permission.GetUsuarioByUserName,
            Permission.UpdateUsuario,

            //CertificationController
            Permission.GeneratePdf,

            //DashboardController
            Permission.GetCantidadSacramentos,

            //HistConstanciaController
            Permission.GetHistoricoConstancias,
            Permission.HistConstanciaRegister,
            Permission.GenararCorrelativo,
            Permission.ListConstanciasAnulacion,

            //TipoSexoController
            Permission.ListTipoSexoSelect,
            Permission.GetTipoSexoById,

            //SacerdoteController
            Permission.ListSacerdote,
            Permission.GetSacerdoteById,

            //FirmaController
            Permission.ListSelectFirma,
            Permission.RegisterFirma,
            Permission.DeleteFirma

        };
    }
}
