using App.Server.src.Re_usables.ActionFilters;
using App.Server.src.Modules.ESS.Models;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using NAV;
using System.Dynamic;
using System.Reflection.PortableExecutable;

//using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace App.Server.src.Modules.ESS.Controllers
{
    [AuthenticateActionFilter]
    public class StoreReqHeaderController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index(string status)
        {
            try
            {
                dynamic response = new ExpandoObject();

                var baseQuery = GV.WSclient.ODATAClient(HttpContext).QyStoreReqHeaders
                    .Where(obj => obj.Status == status)
                    .Where(obj => obj.Employee_No == GeneralController.SessionUser(HttpContext).userNo)
                    .AsQueryable();
                if (GeneralController.RequestHasQuery(HttpContext) == false)
                {
                    var maxTake = SearchFilterController.FnGetMaxTake(HttpContext);
                    response.records = baseQuery.Take(maxTake).ToList();
                    return Ok(response);
                }
                else
                {
                    ClassProperties clsProps = new ClassProperties();
                    QyStoreReqHeaders ModelInstance = new QyStoreReqHeaders();
                    clsProps.WSName = ModelInstance.GetType().Name;
                    clsProps.WSInstance = ModelInstance;
                    //clsProps.pKey = "LaboratoryNo";
                    //
                    List<object> baseResults = new List<object>(baseQuery);
                    var records = GV.SearchFilter.FnSearchFilter(HttpContext, baseResults, clsProps);
                    return Ok(new { records });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }

        }
        //
        [HttpGet]
        public IActionResult GetFormData(string myAction, string recId)
        {
            try
            {
                dynamic response = new ExpandoObject();
                var dimsStr = GV.DimsController.DocCompanyDims(HttpContext, GeneralController.DocumentTypes.StoreRequestHeader.GetDisplayName());
                response.dims = JsonNode.Parse(dimsStr);
                if (!myAction.Contains("create"))
                {
                    var formData = GV.WSclient.ODATAClient(HttpContext).QyStoreReqHeaders
                        .Where(obj => obj.No == recId)
                        .Where(obj => obj.Employee_No == GeneralController.SessionUser(HttpContext).userNo)
                        .FirstOrDefault();
                    if (formData == null)
                    {
                        throw new Exception("Document not found");
                    }
                    response.formData = formData;
                }
                return Ok(new { response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }
        //
        [HttpPost]
        public IActionResult Store([FromBody] StoreRequestHeader obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                //validate dimensions
                var dimsSetValidation = GV.DimsController.ValidateDims(HttpContext, GeneralController.DocumentTypes.ImprestRequestHeader.GetDisplayName(), obj.dimensionSet);
                if (dimsSetValidation != "")
                {
                    throw new Exception(dimsSetValidation);
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.myUserId = GeneralController.SessionUser(HttpContext).myUserId;
                obj.responsibilityCenter = GeneralController.SessionUser(HttpContext).responsibilityCenter;
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnStoreRequestHeaderAsync(JsonSerializer.Serialize(obj)).Result;
                var response = JsonNode.Parse(result.return_value);
                if (response != null && response["status"]?.ToString() == "success")
                {
                    return Ok(new { response = response});
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
        public IActionResult Delete([FromBody] DeleteStoreRequestHeader obj)
        {
            try
            {
                obj.myAction = "delete";
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnStoreRequestHeaderAsync(JsonSerializer.Serialize(obj)).Result;
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
        enum fieldNames { No, Request_Date };
        [HttpGet]
        public IActionResult FieldsProps()
        {
            List<object> fieldsProps = new List<object>();
            foreach (var field in Enum.GetValues<fieldNames>())
            {
                FieldProperties fieldProps = new FieldProperties();
                var fieldName = field.GetDisplayName();
                fieldProps.Name = fieldName;
                fieldProps.Caption = fieldName.Replace("_", " ");
                fieldProps.DataType = CustomDataTypes.Text.ToString();
                switch (field)
                {
                    case fieldNames.No:
                        fieldProps.Caption = "No.";
                        break;
                    case fieldNames.Request_Date:
                        fieldProps.DataType = CustomDataTypes.Date.ToString();
                        break;
                }
                fieldsProps.Add(fieldProps);
            }
            return Ok(fieldsProps);

        }
    }

}

