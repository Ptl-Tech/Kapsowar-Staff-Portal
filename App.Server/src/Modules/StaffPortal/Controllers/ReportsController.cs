using App.Server.src.Re_usables.ActionFilters;
using App.Server.src.Modules.ESS.Models;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace App.Server.src.Modules.ESS.Controllers
{
    [AuthenticateActionFilter]
    public class ReportsController : ControllerBase
    {
        public IActionResult PayslipForm()
        {
            try
            {
                dynamic response = new ExpandoObject();
                response.periods = GV.WSclient.ODATAClient(HttpContext, Config.HospitalNavCompany).QyPayrollPeriods
                    .Where(x => x.Closed == true)
                    .ToList();
                return Ok(new { response });
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }

        }
        [HttpPost]
        public IActionResult PayslipReport([FromBody] Payslip obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext, Config.HospitalNavCompany).FnGeneratePayslipAsync(JsonSerializer.Serialize(obj)).Result;
                ;
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
        [HttpGet]
        public IActionResult P9Form()
        {
            try
            {
                dynamic response = new ExpandoObject();
                response.periods = GV.WSclient.ODATAClient(HttpContext, Config.HospitalNavCompany).QyPayrollPeriods
                    .Where(x => x.Closed == true)
                    .ToList();
                return Ok(new { response });
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }

        }
        [HttpPost]
        public IActionResult P9Report([FromBody] P9 obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext, Config.HospitalNavCompany).FnGenerateP9ReportAsync(JsonSerializer.Serialize(obj)).Result;
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
        [HttpPost]
        public IActionResult ImprestRequestReport([FromBody] ImprestReport obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.docNo = obj.No;
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnImprestRequestReportAsync(JsonSerializer.Serialize(obj)).Result;
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
        public IActionResult ImprestSurrenderReport([FromBody] ImprestSurrenderReport obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.docNo = obj.No;
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnImprestSurrenderReportAsync(JsonSerializer.Serialize(obj)).Result;
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
        public IActionResult StaffClaimReport([FromBody] StaffClaimReport obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.docNo = obj.No;
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnStaffClaimReportAsync(JsonSerializer.Serialize(obj)).Result;
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
        public IActionResult PurchaseRequestReport([FromBody] PurchaseRequestReport obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.docNo = obj.No;
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnPurchaseRequestReportAsync(JsonSerializer.Serialize(obj)).Result;
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
        public IActionResult StoreRequestReport([FromBody] StoreRequestReport obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.docNo = obj.No;
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnStoreRequestReportAsync(JsonSerializer.Serialize(obj)).Result;
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
        public IActionResult LeaveStatementReport([FromBody] LeaveStatement obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext, Config.HospitalNavCompany).FnGenerateLeaveStatementReportAsync(JsonSerializer.Serialize(obj)).Result;
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
        public IActionResult TrainingApplicationReport([FromBody] TrainingApplicationReport obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.docNo = obj.Application_No;
                //
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnTrainingReportAsync(JsonSerializer.Serialize(obj)).Result;
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
    }

}

