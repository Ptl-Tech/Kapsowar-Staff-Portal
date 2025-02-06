using App.Server.Re_usables.GeneralClasses;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using NAV;
using System.Dynamic;

//using System.Linq;

namespace App.Server.src.Modules.ESS.Controllers
{
    [AuthenticateActionFilter]
    public class ProgrammesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index(string category)
        {
            try
            {
                dynamic response = new ExpandoObject();

                var baseQuery = GV.WSclient.ODATAClient(Config.LiveNAVCompany2).PgProgrammesList
                    .AsQueryable();
                if (category != "All")
                {
                    baseQuery = baseQuery.Where(obj => obj.Category == category);
                }
                if (GeneralController.RequestHasQuery(HttpContext) == false)
                {
                    var maxTake = SearchFilterController.FnGetMaxTake(HttpContext);
                    response.records = baseQuery.Take(maxTake).ToList();
                    return Ok(response);
                }
                else
                {
                    ClassProperties clsProps = new ClassProperties();
                    PgProgrammesList ModelInstance = new PgProgrammesList();
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
        enum fieldNames { Code, Description, Category, Department,Student_Registered };
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
                    case fieldNames.Code:
                        fieldProps.Caption = "Code";
                        break;
                }
                fieldsProps.Add(fieldProps);
            }
            return Ok(fieldsProps);

        }
    }

}

