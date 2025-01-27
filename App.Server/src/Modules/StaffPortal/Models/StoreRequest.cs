using System.ComponentModel.DataAnnotations;

namespace App.Server.src.Modules.ESS.Models
{
    public class StoreRequestHeader
    {
        [Required]
        public string myAction { get; set; } = "";
        public string? recId { get; set; } = "";
        public string? staffNo { get; set; } = "";
        public string? myUserId { get; set; } = "";
        public dynamic? dimensionSet { get; set; } = "";
        [Required]
        public DateOnly? requiredDate { get; set; }
        public string? responsibilityCenter { get; set; } = "";
        [Required]
        public string? description { get; set; } = "";
    }
    public class StoreRequestLine
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
        public string? issuingStore { get; set; } = "";
        [Required]
        public decimal quantity { get; set; }
    }
    public class DeleteStoreRequestLine
    {
        public string? myAction { get; set; } = "";
        public string? staffNo { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
        public string parentId { get; set; } = "";
        public int? line_No { get; set; }
        public int? recId { get; set; }
    }
    public class DeleteStoreRequestHeader
    {
        public string? myAction { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
        public string? recId { get; set; } = "";
        public string? staffNo { get; set; } = "";
    }
}
