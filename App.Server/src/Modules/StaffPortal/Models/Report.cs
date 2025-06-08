using System.ComponentModel.DataAnnotations;

namespace App.Server.src.Modules.ESS.Models
{
    public class Payslip
    {
        public string? staffNo { get; set; } = "";
        [Required]
        //[MinLength(4)]
        public int year { get; set; } = 0;
        [Required]
        //[Range(1, 12)]
        public int month { get; set; } = 0;
        
    }
    public class P9
    {
        public string? staffNo { get; set; } = "";
        [Required]
        //[MinLength(4)]
        public int year { get; set; }
    }
    public class ImprestReport
    {
        public string? staffNo { get; set; } = "";
        public string? docNo { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
    }
    public class ImprestSurrenderReport
    {
        public string? staffNo { get; set; } = "";
        public string? docNo { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
    }
    public class StaffClaimReport
    {
        public string? staffNo { get; set; } = "";
        public string? docNo { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
    }
    //
    public class PurchaseRequestReport
    {
        public string? staffNo { get; set; } = "";
        public string? docNo { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
    }
    //
    public class StoreRequestReport
    {
        public string? staffNo { get; set; } = "";
        public string? docNo { get; set; } = "";
        [Required]
        public string No { get; set; } = "";
    }
    public class LeaveStatement
    {
        public string? staffNo { get; set; } = "";
    }
    public class TrainingApplicationReport
    {
        public string? staffNo { get; set; } = "";
        public string? docNo { get; set; } = "";
        [Required]
        public string Application_No { get; set; } = "";
    }
}
