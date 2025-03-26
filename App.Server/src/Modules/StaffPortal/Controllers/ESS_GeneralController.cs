using App.Server.src.Modules.ESS.Models;
using App.Server.src.Re_usables.ActionFilters;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace App.Server.src.Modules.ESS.Controllers
{
    [AuthenticateActionFilter]
    public class ESS_GeneralController : ControllerBase
    {
        [HttpGet]
        public IActionResult Dashboard()
        {
            try
            {
                var obj = new Dashboard();
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.myUserId = GeneralController.SessionUser(HttpContext).myUserId;
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnDashboardStatisticsAsync(JsonSerializer.Serialize(obj)).Result;
                ;
                if (result.return_value != "")
                {
                    var dashboard = JsonNode.Parse(result.return_value);
                    return Ok(new { response = dashboard });
                }
                else
                {
                    var msg = Config.ErrorGeneralFailure;
                    throw new Exception(msg);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }
        }
        
        
        //
    }

}

