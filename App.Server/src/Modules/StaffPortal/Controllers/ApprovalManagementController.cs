using App.Server;
using App.Server.src.Modules.ESS.Models;
using App.Server.src.Re_usables.ActionFilters;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using NAV;
using System.Dynamic;
using System.Net.Mail;

//using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace webapi.Modules.ESS.Controllers
{
    [AuthenticateActionFilter]
    public class ApprovalManagementController : ControllerBase
    {
        [IsApproverActionFilter]
        public IActionResult Index(string docType, string status)
        {
            try
            {
                var companyName = docType == ApprovalDocumentTypes.LeaveApplication.GetDisplayName() ? Config.HospitalNavCompany : "";
                dynamic response = new ExpandoObject();
                var baseQuery = GV.WSclient.ODATAClient(HttpContext, companyName).QyApprovalEntries
                    .Where(obj => obj.Approver_ID != "")
                    .Where(obj => obj.Approver_ID == GeneralController.SessionUser(HttpContext).myUserId)
                    .Where(obj => obj.Status == status)
                    .Where(x => x.Table_ID == GetDocumentTableID(docType))
                    .AsQueryable();
                var entryDocDetails = new JsonObject();
                //
                if (GeneralController.RequestHasQuery(HttpContext) == false)
                {
                    var maxTake = SearchFilterController.FnGetMaxTake(HttpContext);
                    var records = baseQuery.Take(maxTake).ToList();
                    if (records != null)
                    {
                        response.entryDocDetails = GetApprovalEntriesDocDetails(records, docType, HttpContext);
                    }
                    response.records = records;
                    if (docType == ApprovalDocumentTypes.LeaveApplication.GetDisplayName())
                    {
                        var obj = new Dashboard();
                        obj.staffNo = GeneralController.SessionUser(HttpContext).userNo;
                        obj.myUserId = GeneralController.SessionUser(HttpContext).myUserId;
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
                    dynamic records = GV.SearchFilter.FnSearchFilter(HttpContext, baseResults, clsProps);
                    if (records != null)
                    {
                        response.entryDocDetails = GetApprovalEntriesDocDetails(records, docType, HttpContext);
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
        [IsApproverActionFilter]
        [HttpGet]
        public IActionResult GetFormData(string myAction, string docNo, int entryNo, string docType)
        {
            try
            {
                var companyName = docType == ApprovalDocumentTypes.LeaveApplication.GetDisplayName() ? Config.HospitalNavCompany : "";
                dynamic response = new ExpandoObject();
                var formData = GV.WSclient.ODATAClient(HttpContext, companyName).QyApprovalEntries
                    .Where(obj => obj.Approver_ID != "")
                    .Where(obj => obj.Approver_ID == GeneralController.SessionUser(HttpContext).myUserId)
                    .Where(obj => obj.Entry_No == entryNo)
                    .Where(x => x.Document_No == docNo)
                    .FirstOrDefault();
                response.formData = formData;
                return Ok(new { response });
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }
        }
        [HttpGet]
        public dynamic? GetApprovers(HttpContext context, string? docNo,string docType)
        {
            try
            {
                var tableID = GetDocumentTableID(docType);
                var companyName = docType == ApprovalDocumentTypes.LeaveApplication.GetDisplayName() ? Config.HospitalNavCompany : "";
                var approvers = GV.WSclient.ODATAClient(context, companyName).QyApprovalEntries
                   .Where(obj => obj.Status != "Canceled")
                   .Where(obj => obj.Status != "Rejected")
                   .Where(x => x.Table_ID == tableID)
                   .Where(x => x.Document_No == docNo)
                   .ToList();
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
                var companyName = obj.docType == ApprovalDocumentTypes.LeaveApplication.GetDisplayName() ? Config.HospitalNavCompany : "";
                var result = GV.WSclient.CuStaffWebportal(HttpContext, companyName).FnDocumentApprovalAsync(JsonSerializer.Serialize(obj)).Result;
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
        [HttpPost]
        public IActionResult DelegateOrCancelDocumentApproval([FromBody] DelegateOrCancelApproval obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                var companyName = obj.docType == ApprovalDocumentTypes.LeaveApplication.GetDisplayName() ? Config.HospitalNavCompany : "";
                var result = GV.WSclient.CuStaffWebportal(HttpContext, companyName).FnDelegateOrCancelDocumentApprovalAsync(JsonSerializer.Serialize(obj)).Result;
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
                tableID = 50305;
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
            else if (documentType == ApprovalDocumentTypes.PettyCash.GetDisplayName())
            {
                tableID = 50669;
            }
            return tableID;
        }
        //
        public JsonObject? GetApprovalEntriesDocDetails(List<QyApprovalEntries> entries, string docType, HttpContext context)
        {
            dynamic obj = new JsonObject();
            if (entries != null)
            {
                foreach (var entry in entries)
                {
                    if (docType == ApprovalDocumentTypes.LeaveApplication.GetDisplayName())
                    {
                        var result = GV.WSclient.ODATAClient(context, Config.HospitalNavCompany).QyLeaveApplications
                            .Where(x => x.Document_No == entry.Document_No)
                            .FirstOrDefault();
                        if (result != null)
                        {
                            obj[entry.Document_No + "_" + entry.Entry_No] = JsonSerializer.Serialize(result);
                        }
                    }
                    else if (docType == ApprovalDocumentTypes.ImprestRequest.GetDisplayName())
                    {
                        var result = GV.WSclient.ODATAClient(HttpContext).QyImprestHeaders
                            .Where(x => x.No == entry.Document_No)
                            .FirstOrDefault();
                        if (result != null)
                        {
                            obj[entry.Document_No + "_" + entry.Entry_No] = JsonSerializer.Serialize(result);
                        }
                    }
                    else if (docType == ApprovalDocumentTypes.ImprestSurrender.GetDisplayName())
                    {
                        var result = GV.WSclient.ODATAClient(HttpContext).QyImprestSurrenderHeaders
                            .Where(x => x.No == entry.Document_No)
                            .FirstOrDefault();
                        if (result != null)
                        {
                            obj[entry.Document_No + "_" + entry.Entry_No] = JsonSerializer.Serialize(result);
                        }
                    }
                    else if (docType == ApprovalDocumentTypes.StaffClaim.GetDisplayName())
                    {
                        var result = GV.WSclient.ODATAClient(HttpContext).QyStaffClaimHeaders
                            .Where(x => x.No == entry.Document_No)
                            .FirstOrDefault();
                        if (result != null)
                        {
                            obj[entry.Document_No + "_" + entry.Entry_No] = JsonSerializer.Serialize(result);
                        }
                    }
                    else if (docType == ApprovalDocumentTypes.PurchaseRequest.GetDisplayName())
                    {
                        var result = GV.WSclient.ODATAClient(HttpContext).QyPurchaseHeaders
                            .Where(x => x.No == entry.Document_No)
                            .FirstOrDefault();
                        if (result != null)
                        {
                            obj[entry.Document_No + "_" + entry.Entry_No] = JsonSerializer.Serialize(result);
                        }
                    }
                    else if (docType == ApprovalDocumentTypes.StoreRequest.GetDisplayName())
                    {
                        var result = GV.WSclient.ODATAClient(HttpContext).QyStoreReqHeaders
                            .Where(x => x.No == entry.Document_No)
                            .FirstOrDefault();
                        if (result != null)
                        {
                            obj[entry.Document_No + "_" + entry.Entry_No] = JsonSerializer.Serialize(result);
                        }
                    }
                    //
                    else if (docType == ApprovalDocumentTypes.PettyCash.GetDisplayName())
                    {
                        var result = GV.WSclient.ODATAClient(HttpContext).PgPettyCashList
                            .Where(x => x.No == entry.Document_No)
                            .FirstOrDefault();
                        if (result != null)
                        {
                            obj[entry.Document_No + "_" + entry.Entry_No] = JsonSerializer.Serialize(result);
                        }
                    }

                }
            }
            return obj;
        }
    }
    public enum ApprovalDocumentTypes
    {
        LeaveApplication, ImprestRequest, ImprestSurrender, StaffClaim, PurchaseRequest, StoreRequest,PettyCash
    };

}

