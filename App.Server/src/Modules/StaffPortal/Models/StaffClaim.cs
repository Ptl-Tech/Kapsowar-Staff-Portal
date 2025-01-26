using System.ComponentModel.DataAnnotations;

namespace App.Server.src.Modules.ESS.Models
{
    public class StaffClaimHeader
    {
        [Required]
        public string myAction { get; set; } = "";
        public string? recId { get; set; } = "";
        public string? staffNo { get; set; } = "";
        public string? myUserId { get; set; } = "";
        public string? accountNo { get; set; } = "";
        public dynamic? dimensionSet { get; set; } = "";
        [Required]
        public string description { get; set; } = "";
        public string? responsibilityCenter { get; set; } = "";
    }
    public class StaffClaimLine
    {
        [Required]
        public string myAction { get; set; } = "";
        public int? recId { get; set; }
        public string? staffNo { get; set; } = "";
        public string? myUserId { get; set; } = "";
        [Required]
        public string parentId { get; set; } = "";
        [Required]
        public string claimType { get; set; } = "";
        public decimal amount { get; set; }
        public decimal? medicalAmount { get; set; }
        public string? claimReceiptNo { get; set; } = "";
        [Required]
        public DateOnly? expenditureDate { get; set; }
        public string? expenditureDescription { get; set; } = "";
    }
    public class DeleteStaffClaimLine
    {
        public string? myAction { get; set; } = "";
        public string? staffNo { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
        public string parentId { get; set; } = "";
        public int? line_No { get; set; }
        public int? recId { get; set; }
    }
    public class DeleteStaffClaimHeader
    {
        public string? myAction { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
        public string? recId { get; set; } = "";
        public string? staffNo { get; set; } = "";
    }

}
