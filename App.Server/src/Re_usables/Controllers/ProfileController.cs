using App.Server.src.Re_usables.ActionFilters;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using System.Dynamic;
using System.Text.Json.Nodes;

namespace App.Server.src.Re_usables.Controllers
{
    [AuthenticateActionFilter]
    public class ProfileController : ControllerBase
    {
        public IActionResult FormData()
        {
            try
            {
                dynamic response = new ExpandoObject();
                var employee = GV.WSclient.ODATAClient(HttpContext).PgEmployeeCard.Where(x => x.No == GeneralController.SessionUser(HttpContext).userNo).FirstOrDefault();
                response.profile = employee;
                return Ok(new { response });
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }

        }
    }
}




