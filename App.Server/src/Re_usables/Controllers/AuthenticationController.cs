using App.Server.Modules.HMIS.Models;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
using NAV;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
namespace App.Server.Modules.HMIS.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login([FromBody] User obj)
        {
            //Store Session
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                GV.GenController.SetNavCompany(HttpContext);
                var QyUser = GV.WSclient.ODATAClient().QyEmployees.Where(x => x.No == obj.userNo).FirstOrDefault();
                if (QyUser != null)
                {
                    if (FnIsPasswordMatched(obj.password, QyUser.PortalPassword))
                    {
                        GV.DimsController.SetCompanyDimensions(HttpContext);
                        //
                        AuthUser authUser = new();
                        authUser.userNo = QyUser.No;
                        authUser.fullName = QyUser.Full_Name;
                        authUser.firstName = QyUser.First_Name;
                        authUser.middleName = QyUser.Middle_Name;
                        authUser.lastName = QyUser.Last_Name;
                        authUser.email = QyUser.Company_E_Mail;
                        authUser.gender = QyUser.Gender;
                        authUser.isApprover = true;
                        authUser.isPortalSuperUser = true;
                        authUser.branchCode = QyUser.Global_Dimension_1_Code;
                        authUser.responsibilityCenter = QyUser.Responsibility_Center;
                        //if(authUser.responsibilityCenter == "")
                        //{
                        //    throw new Exception("Staff responsibility center not set in Employee card.");
                        //}
                        var QyUserSetup = GV.WSclient.ODATAClient().QyUserSetup.Where(x => x.Employee_No == obj.userNo).FirstOrDefault();
                        if (QyUserSetup != null) {
                            authUser.myUserId = QyUserSetup.User_ID;
                            authUser.customerNo = QyUserSetup.Staff_Travel_Account;
                        }
                        //if no session token
                        if (QyUser.PortalOTPDate.HasValue && QyUser.PortalOTPDate.Value.ToString() == DateTime.Now.ToString("yyyy-MM-dd") && QyUser.PortalOTPDevice == obj.sessionToken)
                        {
                            authUser.sessionToken = obj.sessionToken;
                            authUser.isMFAVerified = true;
                            string authString = JsonSerializer.Serialize(authUser);
                            HttpContext.Session.SetString("authUser", authString);
                            return Ok(new { status = "success", msg = "Login successful.", authUser });
                        }
                        else
                        {
                            var sessionToken = Guid.NewGuid().ToString();
                            var retu = UpdateOTP(QyUser, sessionToken);
                            if (retu.ToLower() == "false")
                            {
                                throw new Exception(Config.ErrorGeneralFailure);
                            }
                            if (retu.ToLower() != "true")
                            {
                                throw new Exception(retu);
                            }
                            authUser.sessionToken = sessionToken;
                            authUser.isMFAVerified = false;
                            string authString = JsonSerializer.Serialize(authUser);
                            HttpContext.Session.SetString("authUser", authString);
                            return Ok(new { status = "success", msg = "OTP login required. OTP Code sent to " + MaskEmail(QyUser.Company_E_Mail), authUser });
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid Password/Staff No.");
                    }
                }
                else
                {
                    throw new Exception("Invalid Password/Staff No.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }
        }
        [HttpPost]
        public IActionResult OTPLogin([FromBody] OTPLogin User)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnMFALoginAsync(JsonSerializer.Serialize(User)).Result;
                var returnV = JsonNode.Parse(result.return_value);
                if (returnV?["status"]?.ToString() == "success")
                {
                    var sessionUser = HttpContext.Session.GetString("authUser");
                    if (sessionUser != null)
                    {
                        var authUserSession = JsonSerializer.Deserialize<AuthUser>(sessionUser);
                        if (authUserSession != null)
                        {
                            authUserSession.isMFAVerified = true;
                        }
                        string authString = JsonSerializer.Serialize(authUserSession);
                        HttpContext.Session.SetString("authUser", authString);
                        return Ok(authUserSession);
                    }
                    else
                    {
                        throw new Exception("User Session not found.");
                    }
                }
                else
                {
                    throw new Exception("OTP code is invalid or has expired.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }
        }
        ////#pragma warning disable CS1998
        [ApiExplorerSettings(IgnoreApi = true)]
        public string UpdateOTP(QyEmployees user,string sessionToken)
        {
            try
            {
                Random random = new Random();
                var token = random.Next(100000, 999999);
                var obj = new UpdateOTP();
                obj.userNo = user.No;
                obj.OTPCode = token.ToString();
                obj.sessionToken = sessionToken;
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnUpdateOTPCodeAsync(JsonSerializer.Serialize(obj)).Result;
                var returnV = JsonNode.Parse(result.return_value);
                if (returnV?["status"]?.ToString() == "success")
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        //
        [HttpPost]
        public IActionResult ResendOTP()
        {
            try
            {
                var authUserSession = GeneralController.SessionUser(HttpContext);
                if (authUserSession != null)
                {
                    Random random = new Random();
                    var token = random.Next(100000, 999999);
                    var obj = new UpdateOTP();
                    obj.userNo = authUserSession.userNo;
                    obj.OTPCode = token.ToString();
                    var result = GV.WSclient.CuStaffWebportal(HttpContext).FnUpdateOTPCodeAsync(JsonSerializer.Serialize(obj)).Result;
                    var returnV = JsonNode.Parse(result.return_value);
                    if (returnV?["status"]?.ToString() == "success")
                    {
                        return Ok(new { response = "success" });
                    }
                    else
                    {
                        throw new Exception(Config.ErrorGeneralFailure);
                    }
                }
                else
                {
                    throw new Exception("User session has expired. Kindly login to proceed.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }
        }
        //
        [HttpPost]
        public IActionResult ForgotPassword([FromBody] ForgotPassword Obj)
        {
            try
            {
                if (Obj.userNo == "")
                {
                    throw new Exception(Config.userNoFieldShemaName + " field is required.");
                }
                var QyUser = GV.WSclient.ODATAClient().QyEmployees.Where(x => x.No == Obj.userNo).FirstOrDefault();
                if (QyUser != null)
                {
                    Random random = new Random();
                    var token = random.Next(100000, 999999);
                    Obj.resetToken = token.ToString();
                    //update token
                    var result = GV.WSclient.CuStaffWebportal(HttpContext).FnSavePasswordResetTokenAsync(JsonSerializer.Serialize(Obj)).Result;
                    var returnV = JsonNode.Parse(result.return_value);
                    if (returnV?["status"]?.ToString() == "success")
                    {
                        //send email
                        string emailMessage = $"Dear {QyUser.First_Name},<br/>Use the code <b>{token}</b> to reset your portal password. Kindly note the code expires after 24 hours.";
                        var receiver = QyUser.Company_E_Mail;
                        var email = GV.WSclient.CuStaffWebportal(HttpContext).FnSendEmailAsync(Config.solutionName+" Reset Password Token", receiver, emailMessage, "").Result;
                        if (email.return_value)
                        {
                            return Ok(new { response = "success", msg = "Password reset token sent to your email (" + MaskEmail(QyUser.Company_E_Mail) + ")" });
                        }
                        else
                        {
                            throw new Exception(Config.ErrorGeneralFailure);
                        }
                    }
                    else
                    {
                        throw new Exception(Config.ErrorGeneralFailure);
                    }
                }
                else
                {
                    throw new Exception("Invalid "+Config.userNoFieldCaption);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }
        }
        [HttpPost]
        public IActionResult ResetPassword([FromBody] ResetPassword obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                if (obj.newPassword != obj.confirmPassword)
                {
                    throw new Exception("New password and confirm password must match.");
                }
                var hashedPass = FnHashPassword(obj.newPassword);
                obj.newPassword = hashedPass;
                var QyUser = GV.WSclient.ODATAClient().QyEmployees.Where(x => x.Company_E_Mail == obj.userNo).FirstOrDefault();
                if (QyUser != null)
                {
                    if (QyUser.PortalPassword != "" && FnIsPasswordMatched(obj.newPassword, QyUser.PortalPassword))
                    {
                        throw new Exception("The new password cannot be the same as the last saved password.");
                    }
                }
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnResetPasswordAsync(JsonSerializer.Serialize(obj)).Result;
                var returnV = JsonNode.Parse(result.return_value);
                if (returnV?["status"]?.ToString() == "success")
                {
                    return Ok(new { response = "success" });
                }
                else
                {
                    throw new Exception(Config.ErrorGeneralFailure);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }
        }
        //
        //[AuthenticateActionFilter]
        [HttpPost]
        public IActionResult ChangePassword([FromBody] ChangePassword User)
        {
            var authUserSession = GeneralController.SessionUser(HttpContext);
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                if (User.NewPassword != User.ConfirmPassword)
                {
                    throw new Exception("New password and confirm password must match.");
                }

                //var empStr = await GV.WSclient.ODATAFilter(HttpContext, WS.Employee().WSName, $"$filter=(No eq '{GeneralController.SessionUser(HttpContext).userNo}')", false);
                var QyUser = GV.WSclient.ODATAClient().QyEmployees.Where(obj => obj.Company_E_Mail == authUserSession.email).FirstOrDefault();
                if (QyUser != null)
                {
                    if (!FnIsPasswordMatched(User.currentPassword, QyUser.PortalPassword))
                    {
                        throw new Exception("The current password is invalid.");
                    }
                    if (QyUser != null && FnIsPasswordMatched(User.NewPassword, QyUser.PortalPassword))
                    {
                        throw new Exception("The new password cannot be the same as the current password.");
                    }
                    var hashedPass = FnHashPassword(User.NewPassword);
                    User.hashedPassword = hashedPass;
                    var result = GV.WSclient.CuStaffWebportal(HttpContext).FnChangePasswordAsync(JsonSerializer.Serialize(User)).Result;
                    var returnV = JsonNode.Parse(result.return_value);
                    if (returnV?["status"]?.ToString() == "success")
                    {
                        return Ok(new { response = "success" });
                    }
                    else
                    {
                        throw new Exception(Config.ErrorGeneralFailure);
                    }

                }
                else
                {
                    throw new Exception("Staff not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }
        }
        //check if user session has expired
        [HttpPost]
        public IActionResult ValidateUserSession([FromBody] AuthUser authUser)
        {
            var session = HttpContext.Session.GetString("authUser");
            if (session != null)
            {
                var authUserSession = JsonSerializer.Deserialize<AuthUser>(session);
                if (authUserSession != null && authUserSession.userNo != null && authUser.userNo != null && authUserSession.userNo == authUser.userNo)
                {
                    if (authUserSession.sessionToken == authUser.sessionToken)
                    {
                        return Ok(new { isExpired = false });
                    }
                }
            }
            return Ok(new { isExpired = true });
        }
        //check if user session has expired
        [HttpPost]
        public IActionResult LogoutUserSession([FromBody] AuthUser authUser)
        {
            var sessionData = HttpContext.Session.GetString("authUser");
            if (sessionData != null)
            {
                var authUserSession = JsonSerializer.Deserialize<AuthUser>(sessionData);
                if (authUserSession != null && authUserSession.userNo == authUser.userNo)
                {
                    if (authUserSession.userNo == authUser.userNo)
                    {
                        HttpContext.Session.Clear();
                        return Ok(new { isLoggedOut = true });
                    }
                }
            }
            return BadRequest(new { error = "Authorization failed." });
        }
#pragma warning disable SYSLIB0023
#pragma warning disable SYSLIB0041
        [ApiExplorerSettings(IgnoreApi = true)]
        public string FnHashPassword(string MyPassword)
        {
            byte[] salt;

            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(MyPassword, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string hashedPasswordHash = Convert.ToBase64String(hashBytes);
            return hashedPasswordHash;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public bool FnIsPasswordMatched(string myPassword, string savedPasswordHash)
        {
            if (savedPasswordHash == "")
            {
                return false;
            }
            bool isMatched;
            isMatched = true;
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(myPassword, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    isMatched = false;
            return isMatched;
        }
        public static string MaskEmail(string email)
        {
            // Split the email into two parts: local part and domain part
            var parts = email.Split('@');

            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid email format.");
            }

            var localPart = parts[0];
            var domainPart = parts[1];

            // Mask the first 4 characters of the local part
            int maskLength = 4; // The number of characters to mask
            if (localPart.Length <= maskLength)
            {
                // If the local part is less than or equal to 5 characters, mask everything except the domain
                localPart = new string('*', localPart.Length);
            }
            else
            {
                // Mask first 5 characters and leave the rest of the local part visible
                localPart = localPart.Substring(0, maskLength) + new string('*', localPart.Length - maskLength);
            }

            // Reconstruct the email with the masked local part
            return localPart + "@" + domainPart;
        }
    }
}
