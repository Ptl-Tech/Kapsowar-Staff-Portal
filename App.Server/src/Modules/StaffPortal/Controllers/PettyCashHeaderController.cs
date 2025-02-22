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
    public class PettyCashHeaderController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index(string status)
        {
            try
            {
                dynamic response = new ExpandoObject();

                var baseQuery = GV.WSclient.ODATAClient(HttpContext).PgPettyCashList
                    .Where(obj => obj.Status == status)
                    .Where(obj => obj.PF_No == GeneralController.SessionUser(HttpContext).userNo)
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
                    PgPettyCashList ModelInstance = new PgPettyCashList();
                    clsProps.WSName = ModelInstance.GetType().Name;
                    clsProps.WSInstance = ModelInstance;
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
                var dimsStr = GV.DimsController.DocCompanyDims(HttpContext, GeneralController.DocumentTypes.ImprestRequestHeader.GetDisplayName());
                response.dims = JsonNode.Parse(dimsStr);
                if (!myAction.Contains("create"))
                {
                    var formData = GV.WSclient.ODATAClient(HttpContext).PgPettyCashList
                        .Where(obj => obj.No == recId)
                        .Where(obj => obj.PF_No == GeneralController.SessionUser(HttpContext).userNo)
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
        public IActionResult Store([FromBody] PettyCashHeader obj)
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
                obj.staffName = GeneralController.SessionUser(HttpContext).fullName;
                obj.customerNo = GeneralController.SessionUser(HttpContext).customerNo;
                obj.responsibilityCenter = GeneralController.SessionUser(HttpContext).responsibilityCenter;
                if(obj.customerNo == "")
                {
                    throw new Exception("Staff customer number not setup in user setup. Once it is setup, ensure you logout and login again.");
                }
                if (obj.responsibilityCenter == "")
                {
                    throw new Exception("Staff responsibility center not setup in the staff card. Once it is setup, ensure you logout and login again.");
                }
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnPettyCashHeaderAsync(JsonSerializer.Serialize(obj)).Result;
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
        public IActionResult Delete([FromBody] DeletePettyCash obj)
        {
            try
            {
                obj.myAction = "delete";
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnPettyCashHeaderAsync(JsonSerializer.Serialize(obj)).Result;
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
        enum fieldNames { No, Date, Payee };
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
                    case fieldNames.Date:
                        fieldProps.DataType = CustomDataTypes.Date.ToString();
                        break;
                }
                fieldsProps.Add(fieldProps);
            }
            return Ok(fieldsProps);

        }
    }

}

