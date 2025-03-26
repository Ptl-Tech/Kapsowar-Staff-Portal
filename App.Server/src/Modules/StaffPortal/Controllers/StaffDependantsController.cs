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
using webapi.Modules.ESS.Controllers;

namespace App.Server.src.Modules.ESS.Controllers
{
    [AuthenticateActionFilter]
    public class StaffDependantsController : ControllerBase
    {
        [HttpGet]
        public IActionResult index(string type)
        {
            try
            {
                dynamic response = new ExpandoObject();

                var baseQuery = GV.WSclient.ODATAClient(HttpContext).QyStaffDependants
                    .Where(obj => obj.Type == type)
                    .Where(obj => obj.Employee_Code == GeneralController.SessionUser(HttpContext).userNo)
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
                    QyStaffDependants ModelInstance = new QyStaffDependants();
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
        enum fieldNames { ID_No_Passport_No,Relationship };
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
                    case fieldNames.ID_No_Passport_No:
                        fieldProps.Caption = "ID No/Passport No.";
                        break;
                    //case fieldNames.Date:
                    //    fieldProps.DataType = CustomDataTypes.Date.ToString();
                    //    break;
                }
                fieldsProps.Add(fieldProps);
            }
            return Ok(fieldsProps);

        }
    }

}

