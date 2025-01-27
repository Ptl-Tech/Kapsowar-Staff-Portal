using System.ComponentModel.DataAnnotations;

namespace App.Server.src.Modules.ESS.Models
{
    public class PurchaseRequestHeader
    {
        [Required]
        public string myAction { get; set; } = "";
        public string? recId { get; set; } = "";
        public string? staffNo { get; set; } = "";
        public string? myUserId { get; set; } = "";
        public dynamic? dimensionSet { get; set; } = "";
        [Required]
        public DateOnly? expectedReceiptDate { get; set; }
        public string? responsibilityCenter { get; set; } = "";
    }
    public class PurchaseRequestLine
    {
        [Required]
        public string myAction { get; set; } = "";
        public int? recId { get; set; }
        public string? staffNo { get; set; } = "";
        public string? myUserId { get; set; } = "";
        [Required]
        public string parentId { get; set; } = "";
        [Required]
        public int type { get; set; }
        [Required]
        public string no { get; set; } = "";
        [Required]
        public string? procurementPlan { get; set; } = "";
        [Required]
        public string requestReason { get; set; } = "";
        [Required]
        public string? location { get; set; } = "";
        [Required]
        public decimal quantity { get; set; }
    }
    public class DeletePurchaseRequestLine
    {
        public string? myAction { get; set; } = "";
        public string? staffNo { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
        public string parentId { get; set; } = "";
        public int? line_No { get; set; }
        public int? recId { get; set; }
    }
    public class DeletePurchaseRequestHeader
    {
        public string? myAction { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
        public string? recId { get; set; } = "";
        public string? staffNo { get; set; } = "";
    }

}
