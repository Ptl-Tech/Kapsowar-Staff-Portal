namespace App.Server
{
    public class Config
    {
        //live env
        public static readonly string LiveNAVInstance = "KRCS";
        public static readonly string LiveCredentialType = "NavUserPassword";
        public static readonly string LiveSoapBaseUrl = "http://desktop-p80si0s.redcross.or.ke:7013/";
        public static readonly string LiveOdataBaseUrl = "http://desktop-p80si0s.redcross.or.ke:7012/";
        public static readonly string LiveNavUsername = "ALEX";
        public static readonly string LiveNavPassword = "Kenya@2024!";
        public static readonly string LiveNAVCompany = "KRCS Test";
        public static readonly string LiveCodeunit = "CuStaffWebportal";
        //Development env
        public static readonly string DevHostName = "DESKTOP-P80SI0S";
        public static readonly string DevNAVInstance = "KRCS";
        public static readonly string DevCredentialType = "NavUserPassword";
        public static readonly string DevSoapBaseUrl = "http://desktop-p80si0s.redcross.or.ke:7013/";
        public static readonly string DevOdataBaseUrl = "http://desktop-p80si0s.redcross.or.ke:7012/";
        public static readonly string DevNavUsername = "ALEX";
        public static readonly string DevNavPassword = "Kenya@2024!";
        public static readonly string DevNAVCompany = "KRCS Test";
        public static readonly string DevCodeunit = "CuStaffWebportal";
        /**
         OTHER SETTINGS
         */
        public static readonly string solutionCode = "SupplierPortal";
        public static readonly bool showExceptionMessage = true;
        public static readonly int ListMaxRecords = 30;
        public static readonly string AuthenticationMethod = "NavUserName";
        public static string ErrorGeneralFailure = "Something went wrong. Kindly try again.";
        public static string ErrorDocumentAccessDenied = "Oops! You do not have access permission to this document. Contact ICT department for help.";


    }
}
