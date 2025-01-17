namespace App.Server
{
    public class Config
    {
        //live env
        public static readonly string LiveHostName = "DESKTOP-P80SI0S";
        public static readonly string LiveNAVInstance = "Kapsowar-Live";
        public static readonly string LiveCredentialType = "NavUserPassword";
        public static readonly string LiveSoapBaseUrl = "http://desktop-p80si0s.redcross.or.ke:6022/";
        public static readonly string LiveOdataBaseUrl = "http://desktop-p80si0s.redcross.or.ke:6021/";
        public static readonly string LiveNavUsername = "ALEX";
        public static readonly string LiveNavPassword = "Kenya@2024!";
        public static readonly string LiveNAVCompany = "AIC Kapsowar Mission Hospital";
        public static readonly string LiveCodeunit = "CuStaffWebportal";
        //Development env
        public static readonly string DevHostName = "DESKTOP-P80SI0S";
        public static readonly string DevNAVInstance = "Kapsowar-Live";
        public static readonly string DevCredentialType = "NavUserPassword";
        public static readonly string DevSoapBaseUrl = "http://desktop-p80si0s.redcross.or.ke:6022/";
        public static readonly string DevOdataBaseUrl = "http://desktop-p80si0s.redcross.or.ke:6023/";
        public static readonly string DevNavUsername = "ALEX";
        public static readonly string DevNavPassword = "Kenya@2024!";
        public static readonly string DevNAVCompany = "AIC Kapsowar Mission Hospital";
        public static readonly string DevCodeunit = "CuStaffWebportal";
        /**
         OTHER SETTINGS
         */
        public static readonly string solutionCode = "StaffPortal";
        public static readonly string solutionName = "Staff Portal";
        public static readonly bool showExceptionMessage = true;
        public static readonly int ListMaxRecords = 30;
        public static readonly string AuthenticationMethod = "NavUserName";
        //errors
        public static string ErrorGeneralFailure = "Something went wrong. Kindly try again.";
        public static string ErrorDocumentAccessDenied = "Oops! You do not have access permission to this document. Contact ICT department for help.";
        //user settings
        public static readonly string userNoFieldShemaName = "No";
        public static readonly string userNoFieldCaption = "Staff No.";

    }
}
