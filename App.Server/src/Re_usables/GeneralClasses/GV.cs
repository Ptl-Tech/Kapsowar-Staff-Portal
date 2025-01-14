using App.Server.src.Re_usables.Modules.DynamicsBC;
using System.Net;

namespace App.Server
{
    public static class GV
    {
        public static readonly HttpClient client = new HttpClient(new HttpClientHandler { Credentials = new NetworkCredential(System.Net.Dns.GetHostName() != Config.DevHostName ? Config.LiveNavUsername : Config.DevNavUsername, System.Net.Dns.GetHostName() != Config.DevHostName ? Config.LiveNavPassword : Config.DevNavPassword) });
        public static HttpClient httpClient => client;
        //
        public static WSClient WSclient = new();
        public static SearchFilterController SearchFilter = new();
        //public static ApprovalManagementController ApprovalMgt = new();
        public static GeneralController GenController = new();
        public static DimensionsController DimsController = new();
        //public static PermissionsController PermissionsController = new();
        //public static DimensionNameNo DimNameNo = new();
        //public enum DimensionNames { DONOR, PROJECT, BUDGETLINE, REGION, DEPARTMENT, COUNTY, MODULE };

    }
}
