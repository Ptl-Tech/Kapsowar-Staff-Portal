using App.Server;
using App.Server.src.Modules.ESS.Models;
using App.Server.src.Re_usables.ActionFilters;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using NAV;
using System.Dynamic;
//using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace webapi.Modules.ESS.Controllers
{
    [AuthenticateActionFilter]
    public class ApprovalManagementController : ControllerBase
    {
        [IsApproverActionFilter]
        public async Task<IActionResult> Index(string docType, string status)
        {
            try
            {
                dynamic response = new ExpandoObject();
                var baseQuery = GV.WSclient.ODATAClient(HttpContext).QyApprovalEntries
                    .Where(obj => obj.Approver_ID != "")
                    .Where(obj => obj.Approver_ID == GeneralController.SessionUser(HttpContext).myUserId)
                    .Where(obj => obj.Status == status)
                    .Where(x => x.Table_ID == GetDocumentTableID(docType))
                    .AsQueryable();
                //
                if (GeneralController.RequestHasQuery(HttpContext) == false)
                {
                    var maxTake = SearchFilterController.FnGetMaxTake(HttpContext);
                    var records = baseQuery.Take(maxTake).ToList();
                    //filter = filter + $"&$top={maxTake}&$orderby=Date_Time_Sent_for_Approval asc";
                    if (records != null)
                    {
                        foreach (QyApprovalEntries rec in records)
                        {
                            var details = await GetApprovalDocumentDetails(HttpContext, rec.Document_No, docType);
                            rec.DocDetails = details;
                        }
                    }
                    response.records = records;
                    if (docType == ApprovalDocumentTypes.LeaveApplication.GetDisplayName())
                    {
                        var obj = new Dashboard();
                        obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                        obj.myUserId = GeneralController.SessionUser(HttpContext).userId;
                        var result = GV.WSclient.CuStaffWebportal(HttpContext).FnApprovalStatisticsAsync(JsonSerializer.Serialize(obj)).Result;
                        response.pendingStatistics = result.return_value != "" ? JsonNode.Parse(result.return_value) : "";
                    }
                }
                else
                {
                    ClassProperties clsProps = new ClassProperties();
                    QyApprovalEntries ModelInstance = new QyApprovalEntries();
                    clsProps.WSName = ModelInstance.GetType().Name;
                    clsProps.WSInstance = ModelInstance;
                    List<object> baseResults = new List<object>(baseQuery);
                    var records = GV.SearchFilter.FnSearchFilter(HttpContext, baseResults, clsProps);
                    if (records != null)
                    {
                        foreach (QyApprovalEntries rec in records)
                        {
                            var details = await GetApprovalDocumentDetails(HttpContext, rec.Document_No, docType);
                            rec.DocDetails = details;
                        }
                    }
                    response.records = records;
                }
                response.docType = docType;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }
        }
        //[IsApproverActionFilter]
        //[HttpGet]
        //public async Task<IActionResult> GetFormData(string myAction, string docNo, string entryNo, string docType)
        //{
        //    try
        //    {
        //        dynamic response = new ExpandoObject();
        //        if (docType == "timesheet" || docType == "leave")
        //        {
        //            var filter = $"$filter=Employee_ID eq '{entryNo}' and Approver_ID eq '{GeneralController.SessionUser(HttpContext).EmployeeNo}' and No eq '{docNo}'&$orderby = Date_Time_Sent_for_Approval desc";
        //            var approvalEntryStr = await GV.WSclient.ODATAFilter(HttpContext, WS.HRApprovalEntry().WSName, filter, false);
        //            var formData = approvalEntryStr != null ? JsonNode.Parse(approvalEntryStr) : null;
        //            response.formData = formData;
        //            response.docType = docType;
        //        }
        //        else
        //        {
        //            var approvalEntryStr = "";
        //            if (docType == "TOR" || docType == "Mission-Report" || docType == "memo" || docType == "staff-request" || docType == "ME-projects-report" || docType == "ME-branch-report" || docType == "ME-annual-report" || docType == "access-request")
        //            {
        //                var filter = $"$filter=Entry_No eq {int.Parse(entryNo)} and Document_No eq '{docNo}' and Employee_Approver_ID eq '{GeneralController.SessionUser(HttpContext).EmployeeNo}'";
        //                approvalEntryStr = await GV.WSclient.ODATAFilter(HttpContext, WS.ApprovalEntry().WSName, filter, false);
        //            }
        //            else if (docType == "transport")
        //            {
        //                var filter = $"$filter=Entry_No eq {int.Parse(entryNo)} and Document_No eq '{docNo}' and Approver_ID eq '{GeneralController.SessionUser(HttpContext).EmployeeNo}'";
        //                approvalEntryStr = await GV.WSclient.ODATAFilter(HttpContext, WS.ApprovalEntry().WSName, filter, false);
        //            }
        //            else if (docType == "store-request" || docType == "purchase-request")
        //            {
        //                var filter = $"$filter=Entry_No eq {int.Parse(entryNo)} and Document_No eq '{docNo}' and Approver_ID eq '{GeneralController.SessionUser(HttpContext).UserId}'";
        //                approvalEntryStr = await GV.WSclient.ODATAFilter(HttpContext, WS.ApprovalEntry().WSName, filter, false);
        //            }
        //            else
        //            {
        //                approvalEntryStr = await GV.WSclient.ODATAFilter(HttpContext, WS.ApprovalEntry().WSName, $"$filter=Entry_No eq {int.Parse(entryNo)} and Document_No eq '{docNo}' and Approver_ID eq '{GeneralController.SessionUser(HttpContext).EmployeeNo}'", false);
        //            }
        //            var formData = approvalEntryStr != null ? JsonNode.Parse(approvalEntryStr) : null;
        //            response.formData = formData;
        //        }
        //        return Ok(new { response });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(GeneralController.ProcessException(ex));
        //    }
        //}
        [HttpGet]
        public dynamic? GetApprovers(HttpContext context, string? docNo, int tableID)
        {
            try
            {
                var approvers = GV.WSclient.ODATAClient(HttpContext).QyApprovalEntries
                   .Where(obj => obj.Status != "Cancelled")
                   .Where(obj => obj.Status != "Rejected")
                   .Where(x => x.Table_ID == tableID)
                   .Where(x => x.Document_No == docNo)
                   .AsQueryable();
                if (approvers != null)
                {
                    foreach (QyApprovalEntries approver in approvers)
                    {
                        var genCon = new GeneralController();
                        string approverName = "";
                        approverName = GV.GenController.GetEmployeeNameByUserId(context, approver.Approver_ID);
                        approver.Approver_ID = approverName;
                    }
                    return approvers;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }
        //
        [HttpPost]
        public IActionResult DocumentApproval([FromBody] DocApproval obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                if (obj.myAction == "reject" && obj.comments == "")
                {
                    throw new Exception("Rejection comments are required.");
                }
                obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                obj.myUserId = GeneralController.SessionUser(HttpContext).myUserId;
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnDocumentApprovalAsync(JsonSerializer.Serialize(obj)).Result;
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
        enum fieldNames { Date_Time_Sent_for_Approval };
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
                switch (field)
                {
                    default:
                        fieldProps.DataType = CustomDataTypes.Text.ToString();
                        break;
                }
                fieldsProps.Add(fieldProps);
            }
            return Ok(fieldsProps);

        }
        //
        public dynamic? GetApprovalDocumentDetails(HttpContext context, string docNo, string docType)
        {
            try
            {
                dynamic response = new ExpandoObject();
                if (docType == ApprovalDocumentTypes.LeaveApplication.GetDisplayName())
                {
                    response.result = GV.WSclient.ODATAClient(context, Config.HospitalNavCompany).QyLeaveApplications
                        .Where(obj => obj.Document_No == docNo)
                        .FirstOrDefault();
                }
                else if (docType == ApprovalDocumentTypes.ImprestRequest.GetDisplayName())
                {
                    response.result = GV.WSclient.ODATAClient(HttpContext).QyImprestHeaders
                        .Where(obj => obj.No == docNo)
                        .FirstOrDefault();
                }
                else if (docType == ApprovalDocumentTypes.ImprestSurrender.GetDisplayName())
                {
                    response.result = GV.WSclient.ODATAClient(HttpContext).QyImprestSurrenderHeaders
                        .Where(obj => obj.No == docNo)
                        .FirstOrDefault();
                }
                else if (docType == ApprovalDocumentTypes.StaffClaim.GetDisplayName())
                {
                    response.result = GV.WSclient.ODATAClient(HttpContext).QyStaffClaimHeaders
                        .Where(obj => obj.No == docNo)
                        .FirstOrDefault();
                }
                else if (docType == ApprovalDocumentTypes.PurchaseRequest.GetDisplayName())
                {
                    response.result = GV.WSclient.ODATAClient(HttpContext).QyPurchaseHeaders
                        .Where(obj => obj.No == docNo)
                        .FirstOrDefault();
                }
                else if (docType == ApprovalDocumentTypes.StoreRequest.GetDisplayName())
                {
                    response.result = GV.WSclient.ODATAClient(HttpContext).QyStoreReqHeaders
                        .Where(obj => obj.No == docNo)
                        .FirstOrDefault();
                }
                return response;
            }
            catch (Exception)
            {
                return null;
            }

        }
        //
        [HttpPost]
        public IActionResult DelegateOrCancelDocumentApproval([FromBody] DelegateOrCancelApproval obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnDelegateOrCancelDocumentApprovalAsync(JsonSerializer.Serialize(obj)).Result;
                if (result.return_value)
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
        public int GetDocumentTableID(string documentType)
        {
            var tableID = 0;
            if (documentType == ApprovalDocumentTypes.LeaveApplication.GetDisplayName())
            {
                tableID = 50079;
            }
            else if (documentType == ApprovalDocumentTypes.ImprestRequest.GetDisplayName())
            {
                tableID = 50688;
            }
            else if (documentType == ApprovalDocumentTypes.ImprestSurrender.GetDisplayName())
            {
                tableID = 50480;
            }
            else if (documentType == ApprovalDocumentTypes.StaffClaim.GetDisplayName())
            {
                tableID = 50578;
            }
            else if (documentType == ApprovalDocumentTypes.PurchaseRequest.GetDisplayName())
            {
                tableID = 38;
            }
            else if (documentType == ApprovalDocumentTypes.StoreRequest.GetDisplayName())
            {
                tableID = 50375;
            }
            return tableID;
        }
    }
    public enum ApprovalDocumentTypes
    {
        LeaveApplication, ImprestRequest, ImprestSurrender, StaffClaim, PurchaseRequest, StoreRequest
    };

}

