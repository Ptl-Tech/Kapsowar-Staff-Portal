using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Server.Re_usables.GeneralClasses
{
    public class IsApproverActionFilter : ActionFilterAttribute
    {
        public string except = "";
        public string only = "";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool isExecute = true;
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
                if (GeneralController.SessionUser(httpContext) != null && GeneralController.SessionUser(httpContext).isApprover == false)
                {
                    throw new Exception("Oops! access denied. Only approvers can access this section.");
                }
            }

        }
    }
}
