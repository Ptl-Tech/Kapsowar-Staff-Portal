using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Server.src.Modules.ESS.Models
{
    public class ApprovalEntry
    {
        [Required]
        public string? docNo { get; set; }
        //
        [Required]
        public string docType { get; set; } = "";
        [Required]
        public int tableID { get; set; }
    }
    public class DocApproval
    {
        [Required]
        public string myAction { get; set; } = "";
        public string? staffNo { get; set; } = "";
        public string? myUserId { get; set; } = "";
        [Required]
        public string docNo { get; set; } = "";
        //
        [Required]
        public string docType { get; set; } = "";
        [Required]
        public int tableID { get; set; }
        public string? entryNo { get; set; }
        [StringLength(250)]
        public string? comments { get; set; }
    }
    public class DelegateOrCancelApproval
    {
        [Required]
        public string myAction { get; set; } = "";
        public string? staffNo { get; set; } = "";
        [Required]
        public string? docNo { get; set; }
        [Required]
        public string docType { get; set; } = "";
    }
}
