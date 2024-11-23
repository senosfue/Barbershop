using BarberShop.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BarberShop.Web.Core.Attributes
{
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute(string permission, string module) : base(typeof(CustomAuthorizeFilter))
        {
            Arguments = [permission, module];
        }
    }
    public class CustomAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly string _permission;
        private readonly string _module;
        private readonly IUsersServices _usersServices;

        public CustomAuthorizeFilter(string permission, string module, IUsersServices usersServices)
        {
            _permission = permission;
            _module = module;
            _usersServices = usersServices;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool isAuthorized = await _usersServices.CurrentUserIsAuthorizedAsync(_permission, _module);
            if (!isAuthorized)
            {
                context.Result = new ForbidResult();
            }
        }
    }

}
