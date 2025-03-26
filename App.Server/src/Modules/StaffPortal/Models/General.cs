using System.ComponentModel.DataAnnotations;

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
    public class StaffAttendance
    {
        public string? staffNo { get; set; } = "";
        public string? myUserId { get; set; } = "";
        [Required]
        public string type { get; set; } = "";
        [Required]
        public string location { get; set; } = "";
        [Required]
        public string coordinates { get; set; } = "";
    }
}
