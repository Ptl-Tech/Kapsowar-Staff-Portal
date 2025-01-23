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
}
