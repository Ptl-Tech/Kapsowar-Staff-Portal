//using Microsoft.AspNetCore.Mvc;
//using System.Dynamic;
////using System.Linq;
//using System.Text.Json;
//using System.Text.Json.Nodes;

//namespace webapi.Modules.ESS.Controllers
//{
//    [Route("api/[controller]/[action]")]
//    [AuthenticateActionFilter]
//    public class AttachmentController : ControllerBase
//    {
//        public ClassProperties clsProps = new ClassProperties();
//        public QyAttachments ModelInstance = new QyAttachments();
//        public AttachmentController()
//        {
//            clsProps.WSName = WS.StaffClaimAttachment().WSName;
//            clsProps.WSInstance = ModelInstance;
//            clsProps.pKey = "No";
//            clsProps.TableID = WS.StaffClaimAttachment().tableID;
//        }
//        [HttpGet]
//        public async Task<IActionResult> Index(string docNo, int tableID)
//        {
//            try
//            {
//                dynamic response = new ExpandoObject();
//                var filter = $"$filter=Table_Id eq {tableID} and DocumentNo eq '{docNo}'";
//                if (WGeneralController.RequestHasQuery(HttpContext) == false)
//                {
//                    var maxTake = SearchFilterController.FnGetMaxTake(HttpContext);
//                    var attachmentsStr = await GlobalVariables.WSclient.ODATAFilter(HttpContext, WS.Attachment().WSName, filter, true);
//                    var records = attachmentsStr != "[]" ? JsonNode.Parse(attachmentsStr) : null;
//                    response.records = records;
//                }
//                else
//                {
//                    var attachmentsStr = await GlobalVariables.SearchFilter.FnSearchFilterV2(HttpContext, clsProps, filter);
//                    var records = JsonDocument.Parse(attachmentsStr);
//                    response.records = records;
//                }
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(WGeneralController.ProcessException(ex));
//            }

//        }

//        [HttpPost]
//        public IActionResult Store([FromBody] Attachment Attachment)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return base.BadRequest(WGeneralController.FnValidationErrors(ModelState));
//                }
//                var fileName = Attachment.filename.Replace("/", "-");
//                fileName = fileName.Replace(":", "-");
//                var result = GlobalVariables.WSclient.WebPortalClient(HttpContext).FnUploadAttachmentAsync(
//                    Attachment.docNo,
//                    WGeneralController.SessionUser(HttpContext).EmployeeNo,
//                    Attachment.description,
//                    Attachment.attachmentFile.Split(",")[1],
//                    Attachment.tableID,
//                    fileName
//                    ).Result;
//                if (result.return_value)
//                {
//                    return Ok(new { response = result.return_value });
//                }
//                else
//                {
//                    throw new Exception(Config.ErrorGeneralFailure);
//                }

//            }
//            catch (Exception ex)
//            {
//                return BadRequest(WGeneralController.ProcessException(ex));
//            }

//        }
//        [HttpPost]
//        public IActionResult Delete([FromBody] QyAttachments Attachment)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return base.BadRequest(WGeneralController.FnValidationErrors(ModelState));
//                }
//                var result = GlobalVariables.WSclient.WebPortalClient(HttpContext).FnDeleteAttachmentAsync(
//                    Attachment.DocumentNo,
//                    WGeneralController.SessionUser(HttpContext).EmployeeNo,
//                    Attachment.Entry_No
//                    ).Result;
//                if (result.return_value)
//                {
//                    return Ok(new { response = result.return_value });
//                }
//                else
//                {
//                    throw new Exception(Config.ErrorGeneralFailure);
//                }
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(WGeneralController.ProcessException(ex));
//            }
//        }
//        [HttpPost]
//        public IActionResult Download([FromBody] QyAttachments Attachment)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return base.BadRequest(WGeneralController.FnValidationErrors(ModelState));
//                }
//                var result = GlobalVariables.WSclient.WebPortalClient(HttpContext).FnDownloadAttachmentAsync(
//                    Attachment.DocumentNo,
//                    WGeneralController.SessionUser(HttpContext).EmployeeNo,
//                    Attachment.Entry_No
//                    ).Result;
//                //var result = GlobalVariables.WSclient.WebPortalClient(HttpContext).FnDownloadAttachmentWithDetailsAsync(
//                //    Attachment.DocumentNo,
//                //    WGeneralController.SessionUser(HttpContext).EmployeeNo,
//                //    Attachment.Entry_No
//                //    ).Result;
//                if (result.return_value != "")
//                {
//                    return Ok(new { response = result.return_value });
//                }
//                else
//                {
//                    throw new Exception(Config.ErrorGeneralFailure);
//                }
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(WGeneralController.ProcessException(ex));
//            }
//        }

//        [HttpGet]
//        public IActionResult FieldsProps()
//        {
//            var fieldNames = WGeneralController.GetModelFieldNames(ModelInstance);
//            List<object> fieldsProps = new List<object>();
//            foreach (var fieldName in fieldNames)
//            {
//                FieldProperties fieldProps = new FieldProperties();
//                fieldProps.Name = fieldName;
//                fieldProps.Caption = fieldName.Replace("_", " ");
//                switch (fieldName)
//                {
//                    case "Start_Date":
//                    case "End_Date":
//                    case "Application_Date":
//                    case "Request_Date":
//                    case "Document_Date":
//                    case "Return_Date":
//                    case "Posting_Date":
//                        fieldProps.DataType = CustomDataTypes.Date.ToString();
//                        break;
//                    //case "Approvals_Status":
//                    //    fieldProps.DataType = CustomDataTypes.Option.ToString();
//                    //    fieldProps.OptionString = "Open,Approval Pending,Approved,Rejected,Cancelled";
//                    //    fieldProps.ValueStyleIf = new Dictionary<string, string> { { "Open", "background-color: rgb(250 204 21)" }, { "Released", "background-color: rgb(34 197 94)" }, { "Pending Approval", "background-color: rgb(59 130 246)" } };
//                    //    break;
//                    //case "Department":
//                    //    fieldProps.DataType = CustomDataTypes.TableRelation.ToString();
//                    //    fieldProps.TableRelationUrl = "/api/wgeneral/dimension-values?gbl_dim_no=2";
//                    //    fieldProps.DropdownKeyCaption = new { key = "Code", caption = "Name" };
//                    //    break;
//                    //case "Employee_No":
//                    //    fieldProps.DataType = CustomDataTypes.TableRelation.ToString();
//                    //    fieldProps.TableRelationUrl = "/api/employee/employeelist?No="+HttpContext.Session.GetString("authUser_EmployeeNo");
//                    //    fieldProps.DropdownKeyCaption = new { key = "No_", caption = "No_"};
//                    //    break;
//                    default:
//                        fieldProps.DataType = CustomDataTypes.Text.ToString();
//                        break;
//                }
//                fieldsProps.Add(fieldProps);
//            }
//            return Ok(fieldsProps);

//        }

//    }

//}

