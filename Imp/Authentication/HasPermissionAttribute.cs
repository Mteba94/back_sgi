using Microsoft.AspNetCore.Authorization;

namespace WebApi_SGI_T.Imp.Authentication
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission): base(policy: permission.ToString())
        {
            
        }
    }
}
