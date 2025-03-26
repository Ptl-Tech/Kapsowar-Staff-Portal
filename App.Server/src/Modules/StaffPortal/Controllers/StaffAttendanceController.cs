using App.Server.src.Modules.ESS.Models;
using App.Server.src.Re_usables.ActionFilters;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using NAV;
using System.Dynamic;
//using System.Linq;
using System.Text.Json;

namespace App.Server.src.Modules.ESS.Controllers
{
    [AuthenticateActionFilter]
    public class StaffAttendanceController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index(string status)
        {
            try
            {
                dynamic response = new ExpandoObject();

                var baseQuery = GV.WSclient.ODATAClient(HttpContext, Config.HospitalNavCompany).QyStaffAttendance
                    .Where(obj => obj.Staff_No == GeneralController.SessionUser(HttpContext).userNo)
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
                    QyStaffAttendance ModelInstance = new QyStaffAttendance();
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
        
        [HttpPost]
        public IActionResult Store([FromBody] StaffAttendance obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                //validate dimensions
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.myUserId = GeneralController.SessionUser(HttpContext).myUserId;
                var result = GV.WSclient.CuStaffWebportal(HttpContext,Config.HospitalNavCompany).FnStaffCheckinCheckoutAsync(JsonSerializer.Serialize(obj)).Result;
                if (result.return_value != "")
                {
                    return Ok(new { response = result.return_value });
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
        enum fieldNames { Date};
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

