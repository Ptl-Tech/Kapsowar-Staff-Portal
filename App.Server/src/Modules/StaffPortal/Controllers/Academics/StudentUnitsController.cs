using App.Server.src.Re_usables.ActionFilters;
using App.Server.src.Modules.ESS.Models;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using NAV;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Nodes;

//using System.Linq;

namespace App.Server.src.Modules.ESS.Controllers
{
    [AuthenticateActionFilter]
    [LecturerActionFilter]
    public class StudentUnitsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index(string semester,string programme, string unit)
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
                response.examSetup = GV.WSclient.ODATAClient(Config.LiveNAVCompany2).QyExamsSetup.ToList();
                var baseQuery = GV.WSclient.ODATAClient(Config.LiveNAVCompany2).QyStudentUnits
                    .Where(obj => obj.Semester == semester)
                    .Where(obj => obj.Programme == programme)
                    .Where(obj => obj.Unit == unit)
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
                    QyStudentUnits ModelInstance = new QyStudentUnits();
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
        [HttpPost]
        public IActionResult MarksEntry([FromBody] MarksEntry[] obj)
        {
            try
            {
                var user = GeneralController.SessionUser(HttpContext);
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext, Config.LiveNAVCompany2).FnMarksEntryAsync(
                    JsonSerializer.Serialize(obj),
                    user.userNo,
                    user.sessionToken)
                    .Result;
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
        //
        [HttpPost]
        public IActionResult SubmitMarks([FromBody] MarksEntry[] obj)
        {
            try
            {
                var user = GeneralController.SessionUser(HttpContext);
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext, Config.LiveNAVCompany2).FnSubmitMarksAsync(
                    JsonSerializer.Serialize(obj),
                    user.userNo,
                    user.sessionToken)
                    .Result;
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
        [HttpPost]
        public IActionResult ClassListReport([FromBody] UnitData obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.fileName = obj.programme.Replace("/", "-")+"-"+obj.unit.Replace("/","-")+"_"+ obj.semester.Replace("/", "-")+ "-"+ obj.stage.Replace("/", "-");
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext, Config.LiveNAVCompany2).FnClassListReportAsync(JsonSerializer.Serialize(obj)).Result;
                if (result.return_value != "")
                {
                    return Ok(new { response = result.return_value });
                }
                else
                {
                    throw new Exception(Config.ErrorGeneralFailure);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }

        }
        //
        [HttpPost]
        public IActionResult MarkSheetReport([FromBody] UnitData obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.fileName = obj.programme.Replace("/", "-") + "-" + obj.unit.Replace("/", "_") + "-" + obj.semester.Replace("/", "-") + "-" + obj.stage.Replace("/", "-");

                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext, Config.LiveNAVCompany2).FnMarksheetReportAsync(JsonSerializer.Serialize(obj)).Result;
                if (result.return_value != "")
                {
                    return Ok(new { response = result.return_value });
                }
                else
                {
                    throw new Exception(Config.ErrorGeneralFailure);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }

        }
        //
        [HttpPost]
        public IActionResult ClassAttendanceReport([FromBody] UnitData obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.fileName = obj.programme.Replace("/", "-") + "-" + obj.unit.Replace("/", "_") + "-" + obj.semester.Replace("/", "-") + "-" + obj.stage.Replace("/", "-");
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext, Config.LiveNAVCompany2).FnClassAttendanceReportAsync(JsonSerializer.Serialize(obj)).Result;
                if (result.return_value != "")
                {
                    return Ok(new { response = result.return_value });
                }
                else
                {
                    throw new Exception(Config.ErrorGeneralFailure);
                }

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

