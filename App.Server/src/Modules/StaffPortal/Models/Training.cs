using System.ComponentModel.DataAnnotations;

namespace App.Server.src.Modules.ESS.Models
{
    public class TrainingHeader
    {
        [Required]
        public string myAction { get; set; } = "";
        public string? recId { get; set; } = "";
        public string? staffNo { get; set; } = "";
        public string? staffName { get; set; } = "";
        public string? myUserId { get; set; } = "";
        public dynamic? dimensionSet { get; set; } = "";
        [Required]
        public string trainingNeed { get; set; } = "";
        [Required]
        public string comments { get; set; } = "";
        [Required]
        public DateOnly startDate { get; set; }
        [Required]
        public string relieverNo { get; set; } = "";
    }
    public class TrainingLine
    {
        [Required]
        public string myAction { get; set; } = "";
        public int? recId { get; set; }
        public string? myUserId { get; set; } = "";
        [Required]
        public string parentId { get; set; } = "";
        [Required]
        public string staffNo { get; set; } = "";
    }
    public class DeleteTrainingLine
    {
        public string? myAction { get; set; } = "";
        public string? staffNo { get; set; } = "";
        [Required]
        public string Application_No { get; set; } = "";
        public string parentId { get; set; } = "";
        public int? line_No { get; set; }
        public int? recId { get; set; }
    }
    public class DeleteTraining
    {
        public string? myAction { get; set; } = "";
        [Required]
        public string Application_No { get; set; } = "";
        public string? recId { get; set; } = "";
        public string? staffNo { get; set; } = "";
    }

}
