using System.ComponentModel.DataAnnotations;

namespace App.Server.src.Modules.ESS.Models
{
    public class ImprestSurrenderHeader
    {
        [Required]
        public string myAction { get; set; } = "";
        public string? recId { get; set; } = "";
        public string? staffNo { get; set; } = "";
        public string? myUserId { get; set; } = "";
        public string? accountNo { get; set; } = "";
        [Required]
        public string imprestNo { get; set; } = "";
        public string? receivedFrom { get; set; } = "";
        public dynamic dimensionSet { get; set; } = "";
    }
    public class ImprestSurrenderLine
    {
        [Required]
        public string myAction { get; set; } = "";
        public int? recId { get; set; }
        public string? staffNo { get; set; } = "";
        public string? myUserId { get; set; } = "";
        [Required]
        public string parentId { get; set; } = "";
        [Required]
        public decimal actualSpentAmount { get; set; }
        public string? cashReceiptNo { get; set; } = "";
        public decimal? cashReceiptAmount { get; set; }
    }
    public class DeleteImprestSurrender
    {
        public string? myAction { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
        public string? recId { get; set; } = "";
        public string? staffNo { get; set; } = "";
    }

}
