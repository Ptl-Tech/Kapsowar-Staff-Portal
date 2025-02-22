using System.ComponentModel.DataAnnotations;

namespace App.Server.src.Modules.ESS.Models
{
    public class PettyCashHeader
    {
        [Required]
        public string myAction { get; set; } = "";
        public string? recId { get; set; } = "";
        public string? staffNo { get; set; } = "";
        public string? staffName { get; set; } = "";
        public string? myUserId { get; set; } = "";
        public dynamic? dimensionSet { get; set; } = "";
        [Required]
        public string narration { get; set; } = "";
        public string? customerNo { get; set; } = "";
        public string? responsibilityCenter { get; set; } = "";
    }
    public class PettyCashLine
    {
        [Required]
        public string myAction { get; set; } = "";
        public int? recId { get; set; }
        public string? staffNo { get; set; } = "";
        public string? myUserId { get; set; } = "";
        [Required]
        public string parentId { get; set; } = "";
        [Required]
        public string type { get; set; } = "";
        public decimal amount { get; set; }
    }
    public class DeletePettyCashLine
    {
        public string? myAction { get; set; } = "";
        public string? staffNo { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
        public string parentId { get; set; } = "";
        public int? line_No { get; set; }
        public int? recId { get; set; }
    }
    public class DeletePettyCash
    {
        public string? myAction { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
        public string? recId { get; set; } = "";
        public string? staffNo { get; set; } = "";
    }

}
