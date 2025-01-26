using App.Server.Modules.HMIS.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace App.Server.src.Re_usables.Modules.DynamicsBC
{

    public class GeneralController : ControllerBase
    {
        //returns a list of fields or columns or property names of a given model or class
        public static List<string> GetModelFieldNames(dynamic ModelInstance)
        {
            List<string> propertyNames = new List<string>();
            if (ModelInstance != null)
            {
                foreach (var property in ModelInstance.GetType().GetProperties())
                {
                    var propName = property.Name;
                    if (propName != null)
                    {
                        propertyNames.Add(propName);
                    }
                }
            }
            return propertyNames;
        }
        [HttpGet]
#pragma warning disable CS8604
#pragma warning disable CS8600
        public IActionResult GetModelFieldNamesByModelName(string modelName)
        {
            List<object> propertyNames = new List<object>();
            var fullClassName = "";
            if (Config.solutionCode == "ESS")
            {
                fullClassName = "App.Server.ESS.Models." + modelName;
            }
            else
            {
                fullClassName = modelName;
            }
            // Create an instance of the dynamic class
            object ModelInstance = Activator.CreateInstance(Type.GetType(fullClassName));
            if (ModelInstance != null)
            {
                foreach (var property in ModelInstance.GetType().GetProperties())
                {
                    var propName = property.Name;
                    if (propName != null)
                    {
                        var field = new { name = propName };
                        propertyNames.Add(field);
                    }
                }
            }
            return Ok(JsonSerializer.Serialize(propertyNames));
        }
        //used to return form validation errors
        public static object FnValidationErrors(ModelStateDictionary Modelstate)
        {
            var errors = Modelstate.ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value != null ? kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray() : null
                        );
            return new { valErrors = errors };
        }
        public static bool RequestHasQuery(HttpContext currentContext)
        {
            string fullUrl = currentContext.Request.GetDisplayUrl();
            if (fullUrl.IndexOf("filter=") >= 0 || fullUrl.IndexOf("order_by=") >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static object ProcessException(Exception ex)
        {
            var errorMessage = new { errors = "" };
            if (Config.showExceptionMessage)
            {
                errorMessage = new { errors = ex.Message };
            }
            else
            {
                errorMessage = new { errors = Config.ErrorGeneralFailure };

            }
            return errorMessage;
        }
        public static AuthUser SessionUser(HttpContext context)
        {
            var session = context.Session.GetString("authUser");
            if (session != null && session != "")
            {
                AuthUser emptyUser = new AuthUser();
                var sessionUser = JsonSerializer.Deserialize<AuthUser>(session);
                return sessionUser != null ? sessionUser : emptyUser;
            }
            else
            {
                throw new Exception("Session expired.");
            }
        }
        public void SetNavCompany(HttpContext context)
        {
            string hostName = System.Net.Dns.GetHostName();
            var navCompany = "";
            if (hostName == "DESKTOP-P80SI0S")//dev
            {
                navCompany = Config.DevNAVCompany;
            }
            else
            {
                navCompany = Config.LiveNAVCompany;
            }
            context.Session.SetString("NavCompany", navCompany);
        }
        public string GetNavCompany(HttpContext context)
        {
            var navcompany = context.Session.GetString("NavCompany");
            if (navcompany != null && navcompany != "")
            {
                return navcompany;
            }
            return "";
        }
        public static async Task<string> ConvertFileToBase64(dynamic file)
        {
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            // Convert byte array to Base64 string
            string base64File = Convert.ToBase64String(fileBytes);
            return base64File;
        }
        //public static APIHeader GetAPIHeader(HttpContext context)
        //{
        //    APIHeader apiHeader = new();
        //    apiHeader.staffNo = context.Request.Headers["staffNo"].ToString();
        //    apiHeader.sessionToken = context.Request.Headers["sessionToken"].ToString();
        //    apiHeader.branchCode = context.Request.Headers["branchCode"].ToString();
        //    return apiHeader;
        //}
        //
        [HttpGet]
        public async Task<IActionResult> ODATAFilter(string webservice, string query = "", bool isList = true)
        {
            try
            {
                if (query != "" && query == "$filter=")
                {
                    query = "";
                }
                var data = await GV.WSclient.ODATAFilter(HttpContext, webservice, query, isList);
                dynamic response = new ExpandoObject();
                response.response = data != null ? JsonNode.Parse(data) : null;
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool FnIsAPIResponseSuccess(JsonNode? response)
        {
           if(response != null && response["status"]?.ToString() == "success")
            {
                return true;
            }
            return false;
        }
        public string FnAPIResponseMessage(JsonNode? response)
        {
            var msg = "";
            if(response != null)
            {
                msg = response?["msg"]?.ToString() != "" ? response?["msg"]?.ToString() : Config.ErrorGeneralFailure;
            }
            return msg != null? msg:"";
        }
        public enum DocumentTypes { ImprestRequestHeader,ImprestRequestLine,
            ImprestSurrenderHeader,ImprestSurrenderLine,
            StaffClaimHeader,StaffClaimLine 
        };

    }
}
