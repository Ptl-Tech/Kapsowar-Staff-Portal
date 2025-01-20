namespace App.Server
{
    public class Config
    {
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
        //live env
        public static readonly string LiveHostName = "";
        public static readonly string LiveNAVInstance = "BC140";
        public static readonly string LiveCredentialType = "NavUserPassword";
        public static readonly string LiveSoapBaseUrl = "http://WIN-6U0N5BV0KKH:5047/";
        public static readonly string LiveOdataBaseUrl = "http://WIN-6U0N5BV0KKH:5048/";
        public static readonly string LiveNavUsername = "Admin";
        public static readonly string LiveNavPassword = "Admin@123";
        public static readonly string LiveNAVCompany = "AIC Kapsowar Mission Hospital";
        public static readonly string LiveCodeunit = "CuStaffWebportal";
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
