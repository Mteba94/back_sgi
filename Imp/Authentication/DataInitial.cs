using Microsoft.EntityFrameworkCore;
using WebApi_SGI_T.Models;

namespace WebApi_SGI_T.Imp.Authentication
{
    public class DataInitial
    {
        readonly SgiSacramentosContext _context;

        public DataInitial(SgiSacramentosContext context)
        {
            _context = context;

        }

        public async Task SeedRolesAsync()
        {
            if (!_context.TblRols.Any())
            {
                var adminRole = new TblRol
                {
                    RoNombre = "Administrador",
                    RoDescripcion = "Administrador del sistema",
                    RoEstado = 1,
                    Permissions = PermissionHelper.ConvertPermissionsToString(RolePermissions.AdminPermissions)
                };

                var userRole = new TblRol
                {
                    RoNombre = "Secretario",
                    RoDescripcion = "Secretario de la Iglesia",
                    RoEstado = 1,
                    Permissions = PermissionHelper.ConvertPermissionsToString(RolePermissions.SecretaryPermissions)
                };

                _context.TblRols.Add(adminRole);
                _context.TblRols.Add(userRole);
                await _context.SaveChangesAsync();
            }
            else
            {
                await UpdateExistingRolesPermissionsAsync();
            }
        }

        public async Task UpdateExistingRolesPermissionsAsync()
        {
            var roles = await _context.TblRols.ToListAsync();
            foreach (var role in roles)
            {
                if (role.RoNombre == "Administrador")
                {
                    role.Permissions = PermissionHelper.ConvertPermissionsToString(Enum.GetValues(typeof(Permission)).Cast<Permission>().ToList());
                }
                else if (role.RoNombre == "Usuario")
                {
                    role.Permissions = PermissionHelper.ConvertPermissionsToString(new List<Permission>
            {
                Permission.ListTipoDocumentoSelect,
                // Agrega otros permisos según corresponda
            });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
