using Microsoft.EntityFrameworkCore;
using WebApi_SGI_T.Imp.Authentication;
using WebApi_SGI_T.Models;

namespace WebApi_SGI_T.Imp
{
    public class PermissionService
    {
        readonly SgiSacramentosContext _context;

        public PermissionService(SgiSacramentosContext context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetPermissionsByUserIdAsync(int userId)
        {
            var userRoles = await (from ur in _context.TblUserRols
                                   join r in _context.TblRols on ur.UrIdRol equals r.RoIdRol
                                   where ur.UrIdUsuario == userId && ur.UrEstado == 1
                                   select r).ToListAsync();

            var permissions = new List<Permission>();
            foreach (var role in userRoles)
            {
                var rolePermissions = PermissionHelper.ConvertStringToPermissions(role.Permissions);
                permissions.AddRange(rolePermissions);
            }

            return permissions.Distinct().ToList();
        }
    }
}
