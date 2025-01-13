using App.Server.Modules.HMIS.Models;
using App.Server.src.Re_usables.Modules.DynamicsBC;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Register([FromBody] UserRegistration obj)
        {
            //Store Session
            try
            {
                if (!ModelState.IsValid)
                {
                    return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
                }
                if (obj.password != obj.confirmPassword)
                {
                    throw new Exception("Password and confirm password do not match.");
                }
                Random random = new Random();
                var token = random.Next(100000, 999999);
                obj.verificationToken = token.ToString();
                var userNo = Guid.NewGuid().ToString();
                obj.userNo = userNo;
                var result = GV.WSclient.CuStaffWebportal(HttpContext).FnSupplierRegistrationAsync(JsonSerializer.Serialize(obj)).Result;
                var response = JsonNode.Parse(result.return_value);
                //
                if (GV.GenController.FnIsAPIResponseSuccess(response))
                {
                    return Ok(new {response="success"});
                }
                else
                {
                    throw new Exception(GV.GenController.FnAPIResponseMessage(response));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(GeneralController.ProcessException(ex));
            }
        }
        //[HttpPost]
        //public IActionResult Login([FromBody] User User)
        //{
        //    //Store Session
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
        //        }
        //        GV.GenController.SetNavCompany(HttpContext);
        //        var employee = GV.WSclient.ODATAClient().QyEmployees.Where(obj => obj.No == User.staffNo).Where(x => x.Status1 == "Active").FirstOrDefault();
        //        if (employee != null)
        //        {
        //            if (employee != null)
        //            {
        //                if (FnIsPasswordMatched(User.Password, employee.PortalPassword))
        //                {
        //                    GV.DimsController.SetCompanyDimensions(HttpContext);
        //                    //
        //                    AuthUser authUser = new();
        //                    authUser.staffNo = User.staffNo;
        //                    var sessionToken = Guid.NewGuid().ToString();
        //                    authUser.sessionToken = sessionToken;
        //                    authUser.FullName = employee.SearchName;
        //                    authUser.FirstName = employee.FirstName;
        //                    authUser.MiddleName = employee.MiddleName;
        //                    authUser.LastName = employee.LastName;
        //                    authUser.Email = employee.CompanyEMail;
        //                    authUser.Gender = employee.Gender;
        //                    authUser.IsApprover = false;
        //                    authUser.IsPortalSuperUser = true;
        //                    authUser.branchCode = employee.ShortcutDimension1Code;
        //                    authUser.responsibilityCenter = employee.ResponsibilityCenter;
        //                    //if no session token
        //                    if (employee.PortalOTPCode == "" || employee.PortalOTPDate != DateTime.Now)
        //                    {
        //                        var result = GV.WSclient.CuGenWebPortal(HttpContext).FnSaveSessionTokenAsync(User.staffNo, sessionToken).Result;
        //                        if (result.return_value)
        //                        {
        //                            try
        //                            {
        //                                var retu = UpdateOTP(employee);
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                throw new Exception(ex.Message);
        //                            }
        //                            authUser.IsMFAVerified = false;
        //                            string authString = JsonSerializer.Serialize(authUser);
        //                            HttpContext.Session.SetString("authUser", authString);
        //                            return Ok(new { status = "success", msg = "OTP login required. OTP Code sent to " + MaskEmail(employee.CompanyEMail), authUser });
        //                        }
        //                        else
        //                        {
        //                            throw new Exception("Something went wrong. The session token could not be stored.");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        authUser.IsMFAVerified = true;
        //                        string authString = JsonSerializer.Serialize(authUser);
        //                        HttpContext.Session.SetString("authUser", authString);
        //                        return Ok(new { status = "success", msg = "Login successful.", authUser });
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception("Invalid Password/Employee No");
        //                }
        //            }
        //            else
        //            {
        //                throw new Exception("Invalid Password/Employee No");
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("Invalid Password/Employee No./In-Active Employee No.");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(GeneralController.ProcessException(ex));
        //    }
        //}
        //[HttpPost]
        //public IActionResult OTPLogin([FromBody] OTPLogin User)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
        //        }
        //        var result = GV.WSclient.CuGenWebPortal(HttpContext).FnMFALoginAsync(User.staffNo, User.OTPCode).Result;
        //        if (result.return_value == true)
        //        {
        //            var sessionUser = HttpContext.Session.GetString("authUser");
        //            if (sessionUser != null)
        //            {
        //                var authUserSession = JsonSerializer.Deserialize<AuthUser>(sessionUser);
        //                if (authUserSession != null)
        //                {
        //                    authUserSession.IsMFAVerified = true;
        //                }
        //                string authString = JsonSerializer.Serialize(authUserSession);
        //                HttpContext.Session.SetString("authUser", authString);
        //                return Ok(authUserSession);
        //            }
        //            else
        //            {
        //                throw new Exception("User Session not found.");
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("OTP code is invalid or has expired.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(GeneralController.ProcessException(ex));
        //    }
        //}
        ////#pragma warning disable CS1998
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public IActionResult UpdateOTP(QyEmployees employee)
        //{
        //    try
        //    {
        //        Random random = new Random();
        //        var token = random.Next(100000, 999999);
        //        var result = GV.WSclient.CuGenWebPortal(HttpContext).FnUpdateOTPCodeAsync(employee.No, token.ToString(), "").Result;
        //        if (result.return_value == true)
        //        {
        //            return Ok(new { response = "success" });
        //        }
        //        else
        //        {
        //            throw new Exception(Config.ErrorGeneralFailure);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(GeneralController.ProcessException(ex));
        //    }
        //}
        ////
        //[HttpPost]
        //public IActionResult ResendOTP()
        //{
        //    try
        //    {
        //        var authUserSession = GeneralController.SessionUser(HttpContext);
        //        if (authUserSession != null)
        //        {
        //            Random random = new Random();
        //            var token = random.Next(100000, 999999);
        //            var result = GV.WSclient.CuGenWebPortal(HttpContext).FnUpdateOTPCodeAsync(authUserSession.staffNo, token.ToString(), "").Result;
        //            if (result.return_value == true)
        //            {
        //                return Ok(new { response = "success" });
        //            }
        //            else
        //            {
        //                throw new Exception(Config.ErrorGeneralFailure);
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("User session has expired. Kindly login to proceed.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(GeneralController.ProcessException(ex));
        //    }
        //}
        ////
        //[HttpPost]
        //public IActionResult ForgotPassword([FromBody] ForgotPassword Obj)
        //{
        //    try
        //    {
        //        if (Obj.staffNo == "")
        //        {
        //            throw new Exception("Staff no. field is required.");
        //        }
        //        //var data = await GV.WSclient.ODATAClient(HttpContext, WS.Employee().WSName, $"$filter=No eq '{User.staffNo}'", false);
        //        var employee = GV.WSclient.ODATAClient().QyEmployees.Where(obj => obj.No == Obj.staffNo).FirstOrDefault();
        //        if (employee != null)
        //        {
        //            if (employee.Status1 == "Active")
        //            {
        //                Random random = new Random();
        //                var token = random.Next(100000, 999999);
        //                //update token
        //                var result = GV.WSclient.CuGenWebPortal(HttpContext).FnSavePasswordResetTokenAsync(employee.No, token.ToString());
        //                if (result.Result.return_value == true)
        //                {
        //                    //send email
        //                    string emailMessage = $"Dear {employee.FirstName},<br/>Use the code <b>{token}</b> to reset your staff portal password. Kindly note the code expires after 24 hours.";
        //                    var receiver = employee.CompanyEMail == employee.CompanyEMail ? employee.CompanyEMail : employee.CompanyEMail + ";" + employee.CompanyEMail;
        //                    var email = GV.WSclient.CuGenWebPortal(HttpContext).FnSendEmailAsync("Staff Portal Reset Password Token", receiver, emailMessage, "").Result;
        //                    if (email.return_value)
        //                    {
        //                        return Ok(new { response = "success", msg = "Password reset token sent to your email (" + MaskEmail(employee.CompanyEMail) + ")" });
        //                    }
        //                    else
        //                    {
        //                        throw new Exception(Config.ErrorGeneralFailure);
        //                    }
        //                }
        //                else
        //                {
        //                    throw new Exception(Config.ErrorGeneralFailure);
        //                }
        //            }
        //            else
        //            {
        //                throw new Exception("Employee No. is In-Active");
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("Invalid Employee No");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(GeneralController.ProcessException(ex));
        //    }
        //}
        //[HttpPost]
        //public IActionResult ResetPassword([FromBody] ResetPassword User)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
        //        }
        //        if (User.NewPassword != User.ConfirmPassword)
        //        {
        //            throw new Exception("New password and confirm password must match.");
        //        }
        //        var hashedPass = FnHashPassword(User.NewPassword);
        //        var employee = GV.WSclient.ODATAClient().QyEmployees.Where(obj => obj.No == User.staffNo).FirstOrDefault();
        //        if (employee != null)
        //        {
        //            if (employee.PortalPassword != "" && FnIsPasswordMatched(User.NewPassword, employee.PortalPassword))
        //            {
        //                throw new Exception("The new password cannot be the same as the last saved password.");
        //            }
        //        }
        //        var response = GV.WSclient.CuGenWebPortal(HttpContext).FnResetPasswordAsync(User.staffNo, User.ResetTokenCode, hashedPass).Result;
        //        if (response.return_value)
        //        {
        //            return Ok(new { response = "success" });
        //        }
        //        else
        //        {
        //            throw new Exception(Config.ErrorGeneralFailure);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(GeneralController.ProcessException(ex));
        //    }
        //}
        ////
        //[AuthenticateActionFilter]
        //[HttpPost]
        //public IActionResult ChangePassword([FromBody] ChangePassword User)
        //{
        //    var apiHeaders = GeneralController.GetAPIHeader(HttpContext);
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return base.BadRequest(GeneralController.FnValidationErrors(ModelState));
        //        }
        //        if (User.NewPassword != User.ConfirmPassword)
        //        {
        //            throw new Exception("New password and confirm password must match.");
        //        }

        //        //var empStr = await GV.WSclient.ODATAFilter(HttpContext, WS.Employee().WSName, $"$filter=(No eq '{GeneralController.SessionUser(HttpContext).userNo}')", false);
        //        var employee = GV.WSclient.ODATAClient().QyEmployees.Where(obj => obj.No == apiHeaders.staffNo).FirstOrDefault();
        //        if (employee != null)
        //        {
        //            if (!FnIsPasswordMatched(User.currentPassword, employee.PortalPassword))
        //            {
        //                throw new Exception("The current password is invalid.");
        //            }
        //            if (employee != null && FnIsPasswordMatched(User.NewPassword, employee.PortalPassword))
        //            {
        //                throw new Exception("The new password cannot be the same as the current password.");
        //            }
        //            var hashedPass = FnHashPassword(User.NewPassword);
        //            var response = GV.WSclient.CuGenWebPortal(HttpContext).FnChangePasswordAsync(apiHeaders.staffNo, hashedPass).Result;
        //            if (response.return_value)
        //            {
        //                return Ok(new { response = "success" });
        //            }
        //            else
        //            {
        //                throw new Exception(Config.ErrorGeneralFailure);
        //            }

        //        }
        //        else
        //        {
        //            throw new Exception("Staff not found.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(GeneralController.ProcessException(ex));
        //    }
        //}
        //check if user session has expired
        [HttpPost]
        public IActionResult ValidateUserSession([FromBody] AuthUser authUser)
        {
            var session = HttpContext.Session.GetString("authUser");
            if (session != null)
            {
                var authUserSession = JsonSerializer.Deserialize<AuthUser>(session);
                if (authUserSession != null && authUserSession.staffNo != null && authUser.staffNo != null && authUserSession.staffNo == authUser.staffNo)
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
                if (authUserSession != null && authUserSession.staffNo == authUser.staffNo)
                {
                    if (authUserSession.staffNo == authUser.staffNo)
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
