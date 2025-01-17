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
    }
    public class AuthUser
    {
        [DisplayName("Staff No.")]
        public string userNo { get; set; } = "";
        public string UserId { get; set; } = "";
        public string FullName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Gender { get; set; } = "";
        public bool? IsMFAVerified { get; set; }
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public int PendingMyApproval { get; set; } = 0;
        public bool IsApprover { get; set; }
        public string sessionToken { get; set; } = "";
        public bool IsPortalSuperUser { get; set; }
        public bool isAccountVerified { get; set; }
        public string branchCode { get; set; } = "";
        public string responsibilityCenter { get; set; } = "";
        public bool isLecturer { get; set; }
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
