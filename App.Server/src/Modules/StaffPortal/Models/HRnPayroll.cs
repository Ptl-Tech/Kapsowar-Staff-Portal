using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Server.src.Modules.ESS.Models
{
    public class LeaveApplication
    {
        [Required]
        public string myAction { get; set; } = "";
        public string? recId { get; set; } = "";
        public string userNo { get; set; } = "";
        public string? comments { get; set; } = "";
        [Required]
        public string leaveType { get; set; } = "";
        [Required]
        public DateOnly startDate { get; set; }
        public DateOnly? endDate { get; set; }
        [Required]
        public decimal noOfDays { get; set; }
        [Required]
        public string reliever { get; set; } = "";
        public string? attachment { get; set; } = "";
    }
    public class DeleteLeaveApplication
    {
        [Required]
        public string myAction { get; set; } = "";
        public string? recId { get; set; } = "";
        public string staffNo { get; set; } = "";
        public string? Document_No { get; set; } = "";
    }
    public class GetLeaveBalance
    {
        [Required]
        public string leaveType { get; set; } = "";
        public string staffNo { get; set; } = "";
    }
    public class LeaveReturnDate
    {
        public string? staffNo { get; set; } = "";
        [Required]
        [DisplayName("Leave Type")]
        public string leaveType { get; set; } = "";
        //
        [Required]
        [DisplayName("Start Date")]
        public DateOnly startDate { get; set; }
        //
        [DisplayName("End Date")]
        public DateOnly endDate { get; set; }
        //
        [DisplayName("No. of Days")]
        public int noOfDays { get; set; }
        public string? type { get; set; } = "";

    }
}
