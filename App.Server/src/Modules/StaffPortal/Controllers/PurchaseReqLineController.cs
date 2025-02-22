using App.Server.src.Re_usables.ActionFilters;
using App.Server.src.Modules.ESS.Models;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
using NAV;
using System.Dynamic;
//using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace App.Server.Modules.HMIS.Controllers
{
    [AuthenticateActionFilter]
    public class PurchaseReqLineController : ControllerBase
    {
        [HttpGet] 
        public IActionResult Index(string parentId)
        {
            try
            {
                dynamic response = new ExpandoObject();
                response.header = GV.WSclient.ODATAClient(HttpContext).QyPurchaseHeaders
                    .Where(obj => obj.No == parentId)
                    .FirstOrDefault();
                var baseQuery = GV.WSclient.ODATAClient(HttpContext).QyPurchaseLines.AsQueryable();
                baseQuery = baseQuery.Where(obj => obj.Document_No == parentId);
                if (GeneralController.RequestHasQuery(HttpContext) == false)
                {
                    response.records = baseQuery.ToList();
                }
                else
                {
                    ClassProperties clsProps = new ClassProperties();
                    QyPurchaseLines ModelInstance = new QyPurchaseLines();
                    clsProps.WSName = ModelInstance.GetType().Name;
                    clsProps.WSInstance = ModelInstance;
                    clsProps.pKey = "SystemId";
                    //
                    List<object> baseResults = new List<object>(baseQuery);
                    var records = GV.SearchFilter.FnSearchFilter(HttpContext, baseResults, clsProps);
                    return Ok(new { records = records });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }

        }
        public IActionResult GetFormData(string myAction, string parentId, int recId)
        {
            try
            {
                dynamic response = new ExpandoObject();
                response.header = GV.WSclient.ODATAClient(HttpContext).QyPurchaseHeaders
                    .Where(obj => obj.No == parentId)
                    .FirstOrDefault();
                //
                if (myAction != "create")
                {
                    response.formData = GV.WSclient.ODATAClient(HttpContext).QyPurchaseLines
                    .Where(obj => obj.Line_No == recId)
                    .Where(obj => obj.Document_No == parentId)
                    .FirstOrDefault();
                }
                return Ok(new { response });
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }
        }

        [HttpPost]
        public IActionResult Store([FromBody] PurchaseRequestLine obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.myUserId = GeneralController.SessionUser(HttpContext).myUserId;
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnPurchaseRequestLineAsync(JsonSerializer.Serialize(obj)).Result;
                var response = JsonNode.Parse(result.return_value);
                if (response != null && response["status"]?.ToString() == "success")
                {
                    return Ok(new { response = response["status"]?.ToString() });
                }
                else
                {
                    var msg = response?["msg"]?.ToString() != "" ? response?["msg"]?.ToString() : Config.ErrorGeneralFailure;
                    throw new Exception(msg);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }

        }
        [HttpPost]
        public IActionResult Delete([FromBody] DeletePurchaseRequestLine obj)
        {
            try
            {
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.myAction = "delete";
                obj.recId = obj.line_No;
                obj.parentId = obj.No;
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnPurchaseRequestLineAsync(JsonSerializer.Serialize(obj)).Result;
                var response = JsonNode.Parse(result.return_value);
                if (response != null && response["status"]?.ToString() == "success")
                {
                    return Ok(response = response["status"]?.ToString());
                }
                else
                {
                    var msg = response?["msg"]?.ToString() != "" ? response?["msg"]?.ToString() : Config.ErrorGeneralFailure;
                    throw new Exception(msg);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }
        }
    }

}

