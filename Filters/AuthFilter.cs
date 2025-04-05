using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using WebAppSalesManagement.Services;

namespace WebAppSalesManagement.Filters
{
    public class AuthFilter : IAuthorizationFilter
    {
        private readonly AuthService _authService;

        public AuthFilter(AuthService authService)
        {
            _authService = authService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!_authService.IsAuthenticated())
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }

    public class AuthAttribute : TypeFilterAttribute
    {
        public AuthAttribute() : base(typeof(AuthFilter))
        {
        }
    }
}