using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace App.Server
{
    public class WSClient
    {
        private static string navUsername = "";
        private static string navPassword = "";
        private static string defaultNavCompany = "";
        private static string defaultCodeunit = "";
        private static string odataBaseUrl = "";
        private static string soapBaseUrl = "";
        private static string navInstance = "";
        private static string navCompany = "";
        public static void initiateService()
        {
            string hostName = System.Net.Dns.GetHostName();
            if (hostName == "DESKTOP-P80SI0S")//dev
            {
                navUsername = Config.DevNavUsername;
                navPassword = Config.DevNavPassword;
                soapBaseUrl = Config.DevSoapBaseUrl;
                odataBaseUrl = Config.DevOdataBaseUrl;
                soapBaseUrl = Config.DevSoapBaseUrl;
                navInstance = Config.DevNAVInstance;
                navCompany = Config.DevNAVCompany;
                defaultCodeunit = Config.DevCodeunit;
            }
            else//live
            {
                navUsername = Config.LiveNavUsername;
                navPassword = Config.LiveNavPassword;
                soapBaseUrl = Config.LiveSoapBaseUrl;
                odataBaseUrl = Config.LiveOdataBaseUrl;
                soapBaseUrl = Config.LiveSoapBaseUrl;
                navInstance = Config.LiveNAVInstance;
                navCompany = Config.LiveNAVCompany;
                defaultCodeunit = Config.LiveCodeunit;
            }
        }
        public string GetSOAPUrl(HttpContext context, string varCompanyName ="", string varCodeunit="")
        {
            try
            {
                initiateService();
                varCompanyName = navCompany;
                varCodeunit = varCodeunit == "" ? defaultCodeunit : varCodeunit;
                return soapBaseUrl + navInstance + "/WS/"+ varCompanyName + "/Codeunit/" + varCodeunit;
            }
            catch (Exception)
            {
                return "Link Unavailable";
            }
        }
        //WebPortalClient - Used when executing functions in codeunit.
        public CuStaffWebportal.CuStaffWebportal_PortClient CuStaffWebportal(HttpContext context, string companyName = "",string WebServiceName = "")
        {
            try
            {
                if(Config.AuthenticationMethod != "Windows")
                {
                    initiateService();
                    companyName = navCompany;
                    BasicHttpBinding _binding = new BasicHttpBinding();
                    _binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                    _binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
                    _binding.MaxReceivedMessageSize = 50000000;
                    string uri = GetSOAPUrl(context,(companyName == "" ? defaultNavCompany : companyName), (WebServiceName == "" ? defaultCodeunit : WebServiceName));
                    CuStaffWebportal.CuStaffWebportal_PortClient _ws = new CuStaffWebportal.CuStaffWebportal_PortClient(_binding, new EndpointAddress(uri));
                    _ws.ClientCredentials.UserName.UserName = navUsername;
                    _ws.ClientCredentials.UserName.Password = navPassword;
                    return _ws;
                }
                else
                {
                    companyName = navCompany;
                    string uri = GetSOAPUrl(context,(companyName == "" ? defaultNavCompany : companyName), (WebServiceName == "" ? defaultCodeunit : WebServiceName));
                    BasicHttpBinding binding = new BasicHttpBinding();
                    EndpointAddress address = new EndpointAddress(uri);
                    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
                    binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                    binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
                    binding.MessageEncoding = WSMessageEncoding.Text;
                    binding.MaxReceivedMessageSize = 50000000;
                    binding.TextEncoding = Encoding.UTF8;
                    binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                    CuStaffWebportal.CuStaffWebportal_PortClient _client = new CuStaffWebportal.CuStaffWebportal_PortClient(binding, address);
                    if (_client.ClientCredentials != null)
                    {
                        _client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(navUsername, navPassword);
                        _client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;

                    }
                    return _client;
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static JsonSerializerOptions JsonSerializationOptions()
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new DecimalConverter(),new JsonStringEnumConverter() }
            };
            return options;
        }
        //General ODATA Client - called when querying data using ODATA
        public NAV.NAV ODATAClient(string companyName = "")
        {
            try
            {
                initiateService();
                //string odataURL = odataBaseUrl + navInstance + "/{OdataV}/Company('" + companyName == "" ? navCompany : companyName + "')/";
                string odataURL = $"{odataBaseUrl}{navInstance}/ODataV4/Company('{navCompany}')/";
                Uri uri = new Uri(odataURL);
                NAV.NAV Navclient = new NAV.NAV(uri);
                //Navclient.BuildingRequest
                String username = navUsername; //Case sensitive
                String password = navPassword; // Service Authentication Key;
                AuthenticationHeaderValue auth = new AuthenticationHeaderValue("Authorization", Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" +password)));
                Navclient.BuildingRequest += (sender, eventArgs) => eventArgs.Headers.Add("Authorization", "Basic " + auth.Parameter);
                return Navclient;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //
        [HttpGet]
        public async Task<string> ODATAFilter(HttpContext context, string? webservice, string filter, bool isList = false)
        {
            try
            {
                initiateService();
                string odataURL = $"{odataBaseUrl}{navInstance}/ODataV4/Company('{navCompany}')/{webservice}?{filter}&$format=json";
                var response = await GV.httpClient.GetAsync(odataURL);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonObject.Parse(content);
                    if (jsonObject != null)
                    {
                        string jsonString = "";
                        if (isList)
                        {
                            jsonString = JsonSerializer.Serialize(jsonObject["value"]);
                        }
                        else
                        {
                            if (jsonObject["value"]?.ToString() != "[]")
                            {
                                jsonString = JsonSerializer.Serialize(jsonObject["value"]?[0]);
                            }
                        }
                        return jsonString;
                    }
                }
                throw new Exception("Connection to web services failed. Kindly contact the ICT department for help.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }   
}
//sometimes decimals from ODATA are converted to string on querying
//this class helps in converting the strings back to decimals
public class DecimalConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (decimal.TryParse(reader.GetString(), out decimal decimalValue))
            {
                return decimalValue;
            }
        }
        throw new JsonException("Expected string containing decimal representation.");
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
