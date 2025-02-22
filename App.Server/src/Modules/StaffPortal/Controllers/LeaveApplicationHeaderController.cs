using App.Server.src.Re_usables.ActionFilters;
using App.Server.src.Modules.ESS.Models;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using NAV;
using System.Dynamic;
//using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace App.Server.src.Modules.ESS.Controllers
{
    [AuthenticateActionFilter]
    public class LeaveApplicationHeaderController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index(string status)
        {
            try
            {
                dynamic response = new ExpandoObject();

                var baseQuery = GV.WSclient.ODATAClient(HttpContext,Config.HospitalNavCompany).QyLeaveApplications
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
                    QyLeaveApplications ModelInstance = new QyLeaveApplications();
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
        [HttpPost]
        public IActionResult Store([FromBody] LeaveApplication obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.userNo = GeneralController.SessionUser(HttpContext).userNo;
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext, Config.HospitalNavCompany).FnLeaveApplicationAsync(JsonSerializer.Serialize(obj)).Result;
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
        //
        [HttpGet]
        public IActionResult GetFormData(string myAction, string recId)
        {
            try
            {
                dynamic response = new ExpandoObject();
                if (!myAction.Contains("create"))
                {
                    var formData = GV.WSclient.ODATAClient(HttpContext, Config.HospitalNavCompany).QyLeaveApplications
                        .Where(obj => obj.Document_No == recId)
                        .Where(obj => obj.Employee_No == GeneralController.SessionUser(HttpContext).userNo)
                        .FirstOrDefault();
                    if (formData == null)
                    {
                        throw new Exception("Leave not found");
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
        [HttpPost]
        public IActionResult Delete([FromBody] DeleteLeaveApplication obj)
        {
            try
            {
                obj.myAction = "delete";
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                var result = GV.WSclient.CuStaffWebportal(HttpContext, Config.HospitalNavCompany).FnLeaveApplicationAsync(JsonSerializer.Serialize(obj)).Result;
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
        [HttpGet]
        public IActionResult GetLeaveBalances(string leaveType)
        {
            try
            {
                var obj = new GetLeaveBalance();
                obj.leaveType = leaveType;
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                var result = GV.WSclient.CuStaffWebportal(HttpContext, Config.HospitalNavCompany).FnGetStaffLeaveBalanceAsync(JsonSerializer.Serialize(obj)).Result;
                var response = JsonNode.Parse(result.return_value);
                if (response != null && response["status"]?.ToString() == "success")
                {
                    return Ok(response);
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
        public IActionResult LeaveEndAndReturnDates([FromBody] LeaveReturnDate obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.type = "LeaveEndAndReturnDates";
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;

                var result = GV.WSclient.CuStaffWebportal(HttpContext, Config.HospitalNavCompany).FnGetLeaveDatesAsync(JsonSerializer.Serialize(obj)).Result;
                var response = JsonNode.Parse(result.return_value);
                if (response != null && response["status"]?.ToString() == "success")
                {
                    return Ok(new { response = response });
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
        public IActionResult LeaveDaysAndReturnDate([FromBody] LeaveReturnDate obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.type = "LeaveDaysAndReturnDate";
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                var result = GV.WSclient.CuStaffWebportal(HttpContext, Config.HospitalNavCompany).FnGetLeaveDatesAsync(JsonSerializer.Serialize(obj)).Result;
                var response = JsonNode.Parse(result.return_value);
                if (response != null && response["status"]?.ToString() == "success")
                {
                    return Ok(new {response=response});
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
        ////
        enum fieldNames { Document_No, Leave_Type, Day_Applied, Start_Date, End_Date };
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
                    case fieldNames.Document_No:
                        fieldProps.Caption = "Leave No.";
                        break;
                    case fieldNames.Start_Date:
                        fieldProps.DataType = CustomDataTypes.Date.ToString();
                        break;
                    case fieldNames.End_Date:
                        fieldProps.DataType = CustomDataTypes.Date.ToString();
                        break;
                }
                fieldsProps.Add(fieldProps);
            }
            return Ok(fieldsProps);

        }
    }

}

