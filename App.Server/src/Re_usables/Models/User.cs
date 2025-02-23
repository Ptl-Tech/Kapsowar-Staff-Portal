using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace App.Server.Modules.HMIS.Models
{
    public class User
    {
        [Required]
        [DisplayName("Staff No.")]
        public string userNo { get; set; } = "";
        [Required]
        public string password { get; set; } = "";
        public string? sessionToken { get; set; } = "";
        [Required]
        public string NavCompany { get; set; } = "";
    }
    public class SessionUser
    {
        public string staffNo { get; set; } = "";
        public string sessionToken { get; set; } = "";
    }
    public class ForgotPassword
    {
        [Required]
        [DisplayName("Staff No.")]
        public string userNo { get; set; } = "";
        public string? resetToken { get; set; } = "";
        [Required]
        public string navCompany { get; set; } = "";
    }
    public class AuthUser
    {
        [DisplayName("Staff No.")]
        public string userNo { get; set; } = "";
        public string fullName { get; set; } = "";
        public string firstName { get; set; } = "";
        public string middleName { get; set; } = "";
        public string lastName { get; set; } = "";
        public string gender { get; set; } = "";
        public bool? isMFAVerified { get; set; }
        public string email { get; set; } = "";
        public string phoneNumber { get; set; } = "";
        public int pendingMyApproval { get; set; } = 0;
        public bool isApprover { get; set; }
        public string sessionToken { get; set; } = "";
        public bool isPortalSuperUser { get; set; }
        public bool isAccountVerified { get; set; }
        public string branchCode { get; set; } = "";
        public string responsibilityCenter { get; set; } = "";
        public bool isLecturer { get; set; } = false;
        public string myUserId { get; set; } = "";
        public string customerNo { get; set; } = "";
        public bool isHOD { get; set; } = false;
        public string? departmentCode { get; set; } = "";
        public string navCompany { get; set; } = "";
    }
    public class ResetPassword
    {
        [Required]
        [DisplayName("Staff No.")]
        public string userNo { get; set; } = "";
        [Required]
        public string newPassword { get; set; } = "";
        [Required]
        public string confirmPassword { get; set; } = "";
        [Required]
        public string resetTokenCode { get; set; } = "";
    }
    public class OTPLogin
    {
        [Required]
        public string userNo { get; set; } = "";
        [Required]
        public string OTPCode { get; set; } = "";
    }
    public class UpdateOTP
    {
        [Required]
        public string userNo { get; set; } = "";
        [Required]
        public string OTPCode { get; set; } = "";
        public string? sessionToken { get; set; } = "";
    }
    public class SendVerificationLink
    {
        [Required]
        public Guid userNo { get; set; }
        [Required]
        public string verificationToken { get; set; } = "";
    }
    public class ChangePassword
    {
        [Required]
        public string currentPassword { get; set; } = "";
        [Required]
        public string NewPassword { get; set; } = "";
        [Required]
        public string ConfirmPassword { get; set; } = "";
        public string? hashedPassword { get; set; } = "";
        public string? userNo { get; set; } = "";
    }
}
