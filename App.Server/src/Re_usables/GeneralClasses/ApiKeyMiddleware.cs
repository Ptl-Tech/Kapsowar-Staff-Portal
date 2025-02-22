public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        var route = context.Request.Path.ToString().ToLower();
        /*if (route != "/api/authentication/login" && route != "/api/authentication/otpLogin" && route != "/api/authentication/resetpassword" && route != "/api/authentication/forgotpassword")
        {
            if (!context.Request.Headers.TryGetValue("staffNo", out var extractedStaffNo))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Staff No. was not provided.");
                return;
            }
            if (!context.Request.Headers.TryGetValue("sessionToken", out var extractedSessionToken))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Session token was not provided.");
                return;
            }
            var employee = GV.WSclient.ODATAClient(HttpContext).QyEmployees.Where(obj => obj.No == extractedStaffNo.ToString()).Where(x => x.Status1 == "Active").FirstOrDefault();
            if (employee != null)
            {
                if (employee.PortalSessionToken != extractedSessionToken)
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Session token is invalid");
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Staff no. is invalid");
                return;
            }
        }

        if (route != "/api/authentication/login" && route != "/api/authentication/resetpassword" && route != "/api/authentication/forgotpassword")
        {
            if (!context.Request.Headers.TryGetValue("branchCode", out var extractedBranchCode))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Branch Code was not provided.");
                return;
            }
        }*/
        await _next(context);
    }
}
