using System.ComponentModel.DataAnnotations;

namespace App.Server.src.Modules.ESS.Models
{
    public class MarksEntry
    {
        public string? StaffNo { get; set; } = "";
        [Required]
        public string Reg_Transacton_ID { get; set; } = "";
        [Required]
        public string Register_for { get; set; } = "";
        [Required]
        public string Semester { get; set; } = "";
        [Required]
        public string Stage { get; set; } = "";
        [Required]
        public string Student_Name { get; set; } = "";
        [Required]
        public string Student_No { get; set; } = "";
        [Required]
        public string Unit { get; set; } = "";
        [Required]
        public string Programme { get; set; } = "";
        [Required]
        public string Lecturer { get; set; } = "";
        [Required]
        public decimal Q1 { get; set; }
        [Required]
        public decimal Q2 { get; set; }
        [Required]
        public decimal Q3 { get; set; }
        [Required]
        public decimal Q4 { get; set; }
        public int ENo { get; set; }
    }
    public class UnitData
    {
        public string? staffNo { get; set; } = "";
        public string? fileName { get; set; } = "";
        [Required]
        public string programme { get; set; } = "";
        [Required]
        public string semester { get; set; } = "";
        [Required]
        public string stage { get; set; } = "";
        [Required]
        public string unit { get; set; } = "";
    }
}
