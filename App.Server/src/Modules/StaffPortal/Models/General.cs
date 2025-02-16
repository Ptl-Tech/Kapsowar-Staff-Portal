namespace App.Server.src.Modules.ESS.Models
{
    public class Dashboard
    {
        public string? staffNo { get; set; } = "";
        public string? myUserId { get; set; } = "";
    }
    public class CancelDocumentApproval
    {
        public string recId { get; set; } = "";
        public string staffNo { get; set; } = "";
        public string docType { get; set; } = "";
    }
}
