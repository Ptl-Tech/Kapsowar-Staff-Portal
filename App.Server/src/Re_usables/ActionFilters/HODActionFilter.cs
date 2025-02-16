using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Server.src.Re_usables.ActionFilters
{
    public class HODActionFilter : ActionFilterAttribute
    {
        public string except = "";
        public string only = "";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool isExecute = true;
            //excepts
            if (except != "")
            {
                var excepts = except.Split(',');
                foreach (var exceptt in excepts)
                {
                    if (context.ActionDescriptor.DisplayName == exceptt)
                    {
                        isExecute = false;
                        break;
                    }
                }
            }
            //onlys
            else if (only != "")
            {
                isExecute = false;
                var onlys = except.Split(',');
                foreach (var onlyy in onlys)
                {
                    if (context.ActionDescriptor.DisplayName == onlyy)
                    {
                        isExecute = true;
                        break;
                    }
                }
            }
            if (isExecute)
            {
                var httpContext = context.HttpContext;
                if (GeneralController.SessionUser(httpContext) != null && GeneralController.SessionUser(httpContext).isHOD == false)
                {
                    throw new Exception("You must be a HOD to access this section.");
                }
            }
        }
    }
}
