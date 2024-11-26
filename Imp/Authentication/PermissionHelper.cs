namespace WebApi_SGI_T.Imp.Authentication
{
    public class PermissionHelper
    {
        public static string ConvertPermissionsToString(List<Permission> permissions)
        {
            return string.Join(",", permissions);
        }

        public static List<Permission> ConvertStringToPermissions(string permissions)
        {
            if (string.IsNullOrEmpty(permissions))
            {
                return new List<Permission>(); // Maneja cadenas vacías devolviendo una lista vacía
            }

            return permissions.Split(',').Select(p => (Permission)Enum.Parse(typeof(Permission), p)).ToList();
        }
    }
}
