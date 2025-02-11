using App.Server.Re_usables.GeneralClasses;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using NAV;
using System.Dynamic;
using System.Text.Json.Nodes;

//using System.Linq;

namespace App.Server.src.Modules.ESS.Controllers
{
    [AuthenticateActionFilter]
    public class LecturerUnitsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index(string semester)
        {
            try
            {
                var semesterRec = GV.WSclient.ODATAClient(Config.LiveNAVCompany2).PgSemestersSetup
                    .Where(obj => obj.Exam_Semester == true)
                    .Where(obj => obj.Code == semester)
                    .FirstOrDefault();
                if (semesterRec == null)
                {
                    throw new Exception($"Semester {semester} not found or is not activated for exam marks entry.");
                }
                dynamic response = new ExpandoObject();

                var baseQuery = GV.WSclient.ODATAClient(Config.LiveNAVCompany2).PgLecturerUnitsDetails
                    //.Where(obj => obj.Lecturer == GeneralController.SessionUser(HttpContext).userNo)
                    .Where(obj => obj.Semester == semester)
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
                    PgLecturersList ModelInstance = new PgLecturersList();
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
        public IActionResult GetFormData()
        {
            try
            {
                dynamic response = new ExpandoObject();
                response.examSemesters = GV.WSclient.ODATAClient(Config.LiveNAVCompany2).PgSemestersSetup
                    .Where(obj => obj.Exam_Semester == true)
                    .ToList();
                return Ok(new { response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }
        //
        [HttpGet]
        public IActionResult GetExamSemesters()
        {
            try
            {
                dynamic response = new ExpandoObject();

                var semesters = GV.WSclient.ODATAClient(Config.LiveNAVCompany2).PgSemestersSetup
                    .Where(obj => obj.Exam_Semester == true)
                    .ToList();
                response.semesters = semesters;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }

        }

        //
        enum fieldNames { Programme, Stage, Semester,Unit};
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
                    case fieldNames.Programme:
                        fieldProps.Caption = "Programme";
                        break;
                }
                fieldsProps.Add(fieldProps);
            }
            return Ok(fieldsProps);

        }
    }

}

