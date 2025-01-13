using Microsoft.AspNetCore.Mvc;

namespace App.Server.Re_usables.Controllers
{
    public class SessionController: ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string? NavCompany()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var navcompany =  httpContext != null? httpContext.Session.GetString("NavCompany"):"";
            return navcompany??null;
        }
        public HttpContext? http_context()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            return httpContext;
        }
    }
}
