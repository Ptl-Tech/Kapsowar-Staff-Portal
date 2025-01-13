using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using System.Dynamic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace App.Server.src.Re_usables.Modules.DynamicsBC
{
    public class DimensionsController : ControllerBase
    {
        [HttpGet]
        //[AuthenticateActionFilter(only = "Dashboard,DownloadAttachment")]
        public void SetCompanyDimensions(HttpContext context)
        {
            var NavCompany = GV.GenController.GetNavCompany(context);
            string content = "[";
            switch (NavCompany)
            {
                case "CHIROMO HOSPITAL GROUP":
                case "TEST":
                    content = content + "{\"DimNo\":1,\"DimCode\":\"BRANCH\",\"DimCaption\":\"Branch Code\"" + "}";
                    break;
            }
            content = content + "]";
            context.Session.SetString("dimsSetup", content);
        }
        public DimsSetup GetDimSetupByCode(HttpContext context, string DimCode)
        {
            var sessionDims = context.Session.GetString("dimsSetup");
            //string dims = "";
            DimsSetup dim = new DimsSetup();
            if (sessionDims != null)
            {
                var objs = JsonSerializer.Deserialize<string>(sessionDims);
                if (objs != null)
                {
                    var objs2 = JsonSerializer.Deserialize<List<DimsSetup>>(objs);
                    if (objs2 != null)
                    {
                        dim = objs2.Where(x => x.DimCode == DimCode).First();
                    }
                }
            }
            return dim;
        }
        public DimsSetup GetDimSetupByNo(HttpContext context, int DimNo)
        {
            var sessionDims = context.Session.GetString("dimsSetup");
            //string dims = "";
            DimsSetup dim = new DimsSetup();
            if (sessionDims != null)
            {
                var objs2 = JsonSerializer.Deserialize<List<DimsSetup>>(sessionDims);
                if (objs2 != null)
                {
                    dim = objs2.Where(x => x.DimNo == DimNo).First();
                }
            }
            return dim;
        }
        public string GetDimDetails(HttpContext context, string dimCode, string dimValue = "")
        {
            if (dimValue != "")
            {
                var response = "{";
                var dimsetup = GetDimSetupByCode(context, dimCode);
                var dim = GV.WSclient.ODATAClient().QyDimensionValues
                .Where(obj => obj.GlobalDimensionNo == dimsetup.DimNo)
                .Where(obj => obj.Code == dimValue)
                .Where(obj => obj.DimensionValueType == "Standard")
                .Where(obj => obj.Blocked == false)
                .FirstOrDefault();

                response = response + $"\"{dimCode}\":" + dim?.ToString();
                response = response + "}";
                return response;
            }
            else
            {
                return "";
            }
        }
        [HttpGet]
        public IActionResult GetDimensionDetails(HttpContext context, int DimNo, string DimCode)
        {
            //int DimNo2 = Int32.Parse(DimNo);
            dynamic response = new ExpandoObject();
            var dim = GV.WSclient.ODATAClient().QyDimensionValues
                .Where(obj => obj.GlobalDimensionNo == DimNo)
                .Where(obj => obj.Code == DimCode)
                .Where(obj => obj.DimensionValueType == "Standard")
                .Where(obj => obj.Blocked == false)
                .FirstOrDefault();
            response.dim = dim;
            return response;
        }
        [HttpGet]
        public IActionResult GetDimensionsSetup()
        {
            dynamic response = new ExpandoObject();
            var record = GV.WSclient.ODATAClient().QyGeneralLedgerSetup
                .FirstOrDefault();
            if (record != null)
            {
                var setup = record;
                if (setup != null)
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    response.Shortcut_Dimension_1_Code = textInfo.ToTitleCase(setup.ShortcutDimension1Code.ToLower()).Replace("_", " ");
                    response.Shortcut_Dimension_2_Code = textInfo.ToTitleCase(setup.ShortcutDimension2Code.ToLower()).Replace("_", " ");
                    response.Shortcut_Dimension_3_Code = textInfo.ToTitleCase(setup.ShortcutDimension3Code.ToLower()).Replace("_", " ");
                    response.Shortcut_Dimension_4_Code = textInfo.ToTitleCase(setup.ShortcutDimension4Code.ToLower()).Replace("_", " ");
                    response.Shortcut_Dimension_5_Code = textInfo.ToTitleCase(setup.ShortcutDimension5Code.ToLower()).Replace("_", " ");
                    response.Shortcut_Dimension_6_Code = textInfo.ToTitleCase(setup.ShortcutDimension6Code.ToLower()).Replace("_", " ");
                    response.Shortcut_Dimension_7_Code = textInfo.ToTitleCase(setup.ShortcutDimension7Code.ToLower()).Replace("_", " ");
                    response.Shortcut_Dimension_8_Code = textInfo.ToTitleCase(setup.ShortcutDimension8Code.ToLower()).Replace("_", " ");
                    //
                    response.Shortcut_Dimension_1_Caption = GetDimNoCaption(HttpContext, 1);
                    response.Shortcut_Dimension_2_Caption = GetDimNoCaption(HttpContext, 2);
                    response.Shortcut_Dimension_3_Caption = GetDimNoCaption(HttpContext, 3);
                    response.Shortcut_Dimension_4_Caption = GetDimNoCaption(HttpContext, 4);
                    response.Shortcut_Dimension_5_Caption = GetDimNoCaption(HttpContext, 5);
                    response.Shortcut_Dimension_6_Caption = GetDimNoCaption(HttpContext, 6);
                    response.Shortcut_Dimension_7_Caption = GetDimNoCaption(HttpContext, 7);
                    response.Shortcut_Dimension_8_Caption = GetDimNoCaption(HttpContext, 8);

                }
            }
            return Ok(new { setup = response });
        }
        public string DocCompanyDims(HttpContext context, string docType)
        {
            //dynamic response = new ExpandoObject();
            string response = "{";
            var dimNos = "";
            var dimNosMandatory = "";
            var NavCompany = GV.GenController.GetNavCompany(context);
            //staff-claim
            if (docType == GeneralController.DocumentTypes.VisitorHeader.GetDisplayName())
            {
                switch (NavCompany)
                {
                    case "CHIROMO HOSPITAL GROUP":
                    case "TEST":
                        dimNos = "1";
                        dimNosMandatory = "1";
                        break;
                }
            }
            //default
            else
            {
                throw new Exception("Document type not found in dimensions setup.");
            }
            response = response + "\"dimNos\":\"" + dimNos + "\",";
            response = response + "\"dimNosMandatory\":\"" + dimNosMandatory + "\"}";
            return response;
        }
        [HttpGet]
        public IActionResult QueryDims(string dimNo, string qString)
        {
            int dimNo2 = int.Parse(dimNo);
            var dims = GV.WSclient.ODATAClient().QyDimensionValues
                .Where(obj => obj.GlobalDimensionNo == int.Parse(dimNo))
                .Where(obj => obj.DimensionValueType == "Standard")
                .Where(obj => obj.Blocked == false)
                .Where(obj => obj.Code.Contains(qString)).ToList().ToString();
            dynamic response = new ExpandoObject();
            response.response = dims;
            return Ok(response);
        }
        [HttpGet]
        public string GetDimsList(HttpContext context, int dimNo)
        {
            dynamic response = new ExpandoObject();
            var dims = GV.WSclient.ODATAClient().QyDimensionValues
                .Where(obj => obj.GlobalDimensionNo == dimNo)
                .Where(obj => obj.DimensionValueType == "Standard")
                .Where(obj => obj.Blocked == false)
                .ToList().ToString();
            return dims != null ? dims : "";
        }
        public static string GetDimSetString(string dimensionSet)
        {
            var dims = JsonNode.Parse(dimensionSet);
            var dim1 = "";
            var dim2 = "";
            var dim3 = "";
            var dim4 = "";
            var dim5 = "";
            var dim6 = "";
            var dim7 = "";
            var dim8 = "";
            var dim9 = "";
            var dim10 = "";
            var dim11 = "";
            var dim12 = "";
            //
            if (dims != null)
            {
                dim1 = dims["Dim_1_Value"]?.ToString();
                dim2 = dims["Dim_2_Value"]?.ToString();
                dim3 = dims["Dim_3_Value"]?.ToString();
                dim4 = dims["Dim_4_Value"]?.ToString();
                dim5 = dims["Dim_5_Value"]?.ToString();
                dim6 = dims["Dim_6_Value"]?.ToString();
                dim7 = dims["Dim_7_Value"]?.ToString();
                dim8 = dims["Dim_8_Value"]?.ToString();
                dim9 = dims["Dim_9_Value"]?.ToString();
                dim10 = dims["Dim_10_Value"]?.ToString();
                dim11 = dims["Dim_11_Value"]?.ToString();
                dim12 = dims["Dim_12_Value"]?.ToString();
            }
            var dimSet = "{";
            dimSet = dimSet + $"\"Dim_1_Value\":\"{dim1}\",";
            dimSet = dimSet + $"\"Dim_2_Value\":\"{dim2}\",";
            dimSet = dimSet + $"\"Dim_3_Value\":\"{dim3}\",";
            dimSet = dimSet + $"\"Dim_4_Value\":\"{dim4}\",";
            dimSet = dimSet + $"\"Dim_5_Value\":\"{dim5}\",";
            dimSet = dimSet + $"\"Dim_6_Value\":\"{dim6}\",";
            dimSet = dimSet + $"\"Dim_7_Value\":\"{dim7}\",";
            dimSet = dimSet + $"\"Dim_8_Value\":\"{dim8}\",";
            dimSet = dimSet + $"\"Dim_9_Value\":\"{dim9}\",";
            dimSet = dimSet + $"\"Dim_10_Value\":\"{dim10}\",";
            dimSet = dimSet + $"\"Dim_11_Value\":\"{dim11}\",";
            dimSet = dimSet + $"\"Dim_12_Value\":\"{dim12}\"";
            dimSet = dimSet + "}";
            return dimSet;
        }
        public string ValidateDims(HttpContext context, string docType, dynamic dimensionSet)
        {

            string dimsSet1 = dimensionSet.ToString();
            var dimsSet = JsonNode.Parse(dimsSet1);
            if (dimsSet != null)
            {
                var docDimsStr = DocCompanyDims(context, docType);
                var objDims = JsonNode.Parse(docDimsStr);
                var dimNos = objDims != null ? objDims["dimNos"] : null;
                var dimNosMandatory = objDims != null ? objDims["dimNosMandatory"] : null;
                if (dimNosMandatory != null && dimNosMandatory.ToString() != "")
                {
                    var dimNums = dimNosMandatory?.ToString().Split(",");
                    if (dimNums != null)
                    {
                        foreach (var dimNum in dimNums)
                        {
                            var dimNumValue = dimsSet["Dim_" + dimNum + "_Value"]?.ToString();
                            if (dimNumValue == "" || dimNumValue == null)
                            {
                                var dimNoDetails = GetDimSetupByNo(context, int.Parse(dimNum));
                                return $"{dimNoDetails.DimCaption} field must be filled.";
                            }
                        }
                    }
                }
            }
            return "";
        }
        public string GetDimNoCaption(HttpContext context, int dimNo)
        {
            var companyDimsSetup = context.Session.GetString("dimsSetup");
            if (companyDimsSetup != null)
            {
                var setup = JsonSerializer.Deserialize<List<DimsSetup>>(companyDimsSetup);
                if (setup != null)
                {
                    var caption = setup.Where(x => x.DimNo == dimNo).FirstOrDefault();
                    return caption != null ? caption.DimCaption : "";
                }
            }
            return "";
        }
    }
    public class DimsSetup
    {
        public string DimCode { get; set; } = "";
        public int DimNo { get; set; }
        public string DimCaption { get; set; } = "";
    }
    //
}
