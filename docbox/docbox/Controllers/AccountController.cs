using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using docbox.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using docbox.Utilities;

namespace docbox.Controllers
{
    public class AccountController : Controller
    {

        dx_docboxEntities database = new dx_docboxEntities();

        // GET: /Account/LogOn

        public ActionResult LogOn()
        {


            return View();
        }

        public ActionResult Invalid()
        {

            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {


                var allusers = from usertabel in database.DX_USER where usertabel.userid == model.UserName select usertabel;
                if (allusers.ToList().Count == 1)
                {

                    var UserRecord = allusers.First();
                    if (UserRecord.pwdhash.Equals(generateHash(UserRecord.salt, model.Password)))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        //Security checkpoint for preventing open redirect attack
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("RespectiveHome");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }


            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // This will take each user to its home depending upon its role!!
        public ActionResult RespectiveHome()
        {
            String[] roles = Roles.GetRolesForUser();
            if (roles.Contains(Constants.TEMP_USER))
            {
                return RedirectToAction("Index", "TempUser");
            }
            else if (roles.Contains(Constants.EMPLOYEE_USER))
            {
                return RedirectToAction("Index", "Home");
            }
            else if (roles.Contains(Constants.MANAGER_USER))
            {
                return RedirectToAction("Index", "Home");
            }
            else if (roles.Contains(Constants.VP_USER))
            {
                return RedirectToAction("Index", "Home");
            }
            else if (roles.Contains(Constants.CEO_USER))
            {
                return RedirectToAction("Index", "Home");
            }
            else if (roles.Contains(Constants.GUEST_USER))
            {
                return RedirectToAction("Index", "TempUser");
            }
            else
            {
                return RedirectToAction("Invalid", "Account");
            }

        }

        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("About", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            ViewBag.CaptchaGuid = Guid.NewGuid().ToString("N");
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                string captchaid = Request.Form["CaptchaGuid"];
                string captchaValue = Request.Form["Captcha"];


                WebClient captchaCliden = new WebClient();
                string reponseCaptchaService = captchaCliden.DownloadString(
                  "http://www.opencaptcha.com/validate.php?img="
                    + captchaid + "&ans=" + captchaValue);

                if (!"pass".Equals(reponseCaptchaService))
                {
                    ModelState.AddModelError("", "Captcha didn't match, please try again!");
                    return View(model);
                }

                var allusers = from usertabel in database.DX_USER where usertabel.userid == model.Email select usertabel;
                if (allusers.ToList().Count == 1)
                {
                    ModelState.AddModelError("", "A user with this email already present in teh system please use correct email id!");
                    return View(model);

                }
                var alldepartment = from usertabel in database.DX_DEPARTMENT where usertabel.name == model.Department select usertabel;

                if (alldepartment.ToList().Count == 1)
                {

                    var DepartmentRecord = alldepartment.First();


                    DX_USER user = new DX_USER();
                    user.fname = model.FirstName;
                    user.lname = model.LastName;
                    user.phone = model.Phone;
                    user.questionid = Int32.Parse(model.Squestion);
                    user.role = model.Position;
                    user.userid = model.Email;
                    user.anshash = generateHash(model.Answer);
                    user.accesslevel = Constants.TEMP_USER;
                    user.salt = generateSalt();
                    user.pwdhash = generateHash(user.salt, model.Password);
                    user.actcodehash = "dummycode";
                    database.DX_USER.AddObject(user);//Add user

                    DX_USERDEPT userDept = new DX_USERDEPT();
                    userDept.deptid = DepartmentRecord.deptid;
                    userDept.userid = model.Email;
                    database.DX_USERDEPT.AddObject(userDept);//Add department

                    int success = database.SaveChanges();
                    if (success > 0)
                    {
                        String message = Environment.NewLine + "Hi " + model.FirstName + "," + Environment.NewLine
                            + "Thank you for registering with Docbox!" + Environment.NewLine
                            + "You will soon get notification, once you are been approved by Docbox Administrator" + Environment.NewLine
                                + "- Docbox Team";
                        EmailMessaging.sendMessage(model.Email, message, "Notification");
                        FormsAuthentication.SetAuthCookie(model.Email, false);
                        return RedirectToAction("Index", "TempUser");
                    }
                    else
                    {
                        ModelState.AddModelError("", "User can not be registered, Please try after some time!");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Department Select Correct Department");
                    return View(model);
                }




            }

            // If we got this far, something failed, redisplay form
            return View(model);


        }

        //GET : /Account/ForgetPassword
        public ActionResult ForgetPassword()
        {
            ViewBag.CaptchaGuid = Guid.NewGuid().ToString("N");
            return View();

        }

        //Post : /Account/FoegetPassword

        [Authorize]
        public ActionResult ForgetPassword(ForgetPasswordModel model)
        {
            /*  if (ModelState.IsValid)
              {

                  var allusers = from usertabel in database.DX_USER where 
                                     usertabel.fname == model.FirstName
                                     && usertabel.lname== model.LastName
                                     && usertabel.phone== model.Phone
                                     && usertabel.userid == model.Email
                                     && usertabel.role == model.Position
                                     && usertabel.


                                 select usertabel;
                  if (allusers.ToList().Count == 1)
                  {

                      var UserRecord = allusers.First();
                      if (UserRecord.pwdhash.Equals(generateHash(UserRecord.salt, model.Password)))
                      {
                          FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                          //Security checkpoint for preventing open redirect attack
                          if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                              && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                          {
                              return Redirect(returnUrl);
                          }
                          else
                          {
                              return RedirectToAction("RespectiveHome");
                          }
                      }
                      else
                      {
                          ModelState.AddModelError("", "The user name or password provided is incorrect.");
                      }
                  }
                  else
                  {
                      ModelState.AddModelError("", "The user name or password provided is incorrect.");
                  }


              }
              else
              {
                  ModelState.AddModelError("", "The user name or password provided is incorrect.");
              }*/

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        private static string generateSalt()
        {
            byte[] randomSalt = new byte[64];
            RNGCryptoServiceProvider qualityRandom = new RNGCryptoServiceProvider();
            qualityRandom.GetBytes(randomSalt);
            return Convert.ToBase64String(randomSalt);
        }
        private static string generateHash(string SaltValue, string InputPwd)
        {

            string SaltedPassword = String.Concat(InputPwd, SaltValue);
            HashAlgorithm algorithm = new SHA256CryptoServiceProvider();
            byte[] quickHash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(SaltedPassword));
            string hash = Convert.ToBase64String(quickHash);
            return hash;
        }
        private static string generateHash(string answer)
        {


            HashAlgorithm algorithm = new SHA256CryptoServiceProvider();
            byte[] quickHash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(answer));
            string hash = Convert.ToBase64String(quickHash);
            return hash;
        }


        //To do: add our error codes
        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            database.Dispose();
            base.Dispose(disposing);
        }


    }
}
