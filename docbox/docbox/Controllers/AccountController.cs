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
using docbox.Filters;
using System.Text.RegularExpressions;

namespace docbox.Controllers
{
    [AuditLogAttribute]
    public class AccountController : Controller
    {

        dx_docboxEntities database = new dx_docboxEntities();

        // GET: /Account/LogOn
        //[RequireHttps]
        [DeleteBrowserHistory]
        public ActionResult LogOn()
        {
            ViewBag.CaptchaGuid = Guid.NewGuid().ToString("N");
            LogOnModel model = new LogOnModel();
            return View(model);
        }
        //[RequireHttps]
        [DeleteBrowserHistory]
        public ActionResult Invalid()
        {

            return View();
        }
        //[RequireHttps]
         [Authorize(Roles = "deactivated")]
        public ActionResult IndexOfDeactivatedUser()
        {
            return View();
        }
        //
        // POST: /Account/LogOn

        private bool logonValidations(LogOnModel model)
        {
            if (!Regex.IsMatch(model.UserName, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                ModelState.AddModelError("", "Email-id or password incorrect please try agian!!.");
                return false;
            }

            if (!Regex.IsMatch(model.Password, @"^.*(?=.{10,18})(?=.*\d)(?=.*[A-Za-z])(?=.*[@%&#]{0,}).*$"))
            {
                ModelState.AddModelError("", "Email-id or password incorrect please try agian!!.");
                return false;
            }

            return true;
        }
        [HttpPost]
        [DeleteBrowserHistory]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            try
            {
                //Login attempts
                if (SessionKeyMgmt.LoginAttempts == 0)
                {
                    SessionKeyMgmt.LoginAttempts = 1;
                }
                else
                {
                    int count = SessionKeyMgmt.LoginAttempts;
                    count++;
                    SessionKeyMgmt.LoginAttempts = count;

                    if (model.Captcha != null)
                    {
                        if (verifyCaptcha() == false)
                        {
                            ViewBag.CaptchaGuid = Guid.NewGuid().ToString("N");
                            return View(model);

                        }
                        ViewBag.CaptchaGuid = Guid.NewGuid().ToString("N");
                    }
                }

                if (model.Captcha == null)
                {
                    model.Captcha = "";
                }
                //Login attempts end

                if (logonValidations(model) == false)
                {
                    return View(model);
                }

                if (ModelState.IsValid)
                {

                    var allusers = from usertabel in database.DX_USER where usertabel.userid == model.UserName select usertabel;
                    if (allusers != null && allusers.ToList().Count == 1)
                    {

                        var UserRecord = allusers.First();
                        if (UserRecord.pwdhash.Equals(generateHash(UserRecord.salt, model.Password)))
                        {


                            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                            //Set userid in session
                            SessionKeyMgmt.UserId = model.UserName;

                            //Get the department
                            SessionKeyMgmt.UserDept = DbCommonQueries.getDepartmentName(model.UserName, database);

                            SessionKeyMgmt.LoginAttempts = 0;

                          //  Roles.DeleteCookie();
                           
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
                            ModelState.AddModelError("", "Email-id or password provided is incorrect please try again!!");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email-id or password incorrect please try agian!!");
                    }


                }
                else
                {
                    ModelState.AddModelError("", "This is invalid request. Please provide email and passwod");
                }
                // If we got this far, something failed, redisplay form
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Can not process request, please try after some time!");
            }
            return View(model);
        }

        public ActionResult LogOnAsGuestUser(string returnUrl)
        {
            LogOnModel model = new LogOnModel();
            model.UserName = "guest@docbox.com";
            model.Password = "AmR/O3@Qw5l5Z&o";

            try
            {
                if (ModelState.IsValid)
                {

                    var allusers = from usertabel in database.DX_USER where usertabel.userid == model.UserName select usertabel;
                    if (allusers != null && allusers.ToList().Count == 1)
                    {

                        var UserRecord = allusers.First();
                        if (UserRecord.pwdhash.Equals(generateHash(UserRecord.salt, model.Password)))
                        {

                            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                            //Set userid in session
                            SessionKeyMgmt.UserId = model.UserName;

                            //Get the department
                            SessionKeyMgmt.UserDept = DbCommonQueries.getDepartmentName(model.UserName, database);

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
                            ModelState.AddModelError("", "password provided is incorrect.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email id incorrect please try again!");
                    }


                }
                else
                {
                    ModelState.AddModelError("", "Email id and password provided is incorrect.");
                }
                // If we got this far, something failed, redisplay form
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Can not process request, please try after some time!");
            }
            return View(model);
        }

        // This will take each user to its home depending upon its role!!
        [DeleteBrowserHistory]
        public ActionResult RespectiveHome()
        {
            String[] roles = Roles.GetRolesForUser();

            if (roles.Contains(Constants.ADMIN_USER_ACCESS))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (roles.Contains(Constants.ADMINLESS_USER_ACCESS))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
                if (roles.Contains(Constants.TEMP_USER_ACCESS))
                {
                    return RedirectToAction("Index", "TempUser");
                }
                else if (roles.Contains(Constants.EMPLOYEE_USER_ACCESS))
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (roles.Contains(Constants.MANAGER_USER_ACCESS))
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (roles.Contains(Constants.VP_USER_ACCESS))
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (roles.Contains(Constants.CEO_USER_ACCESS))
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (roles.Contains(Constants.GUEST_USER_ACCESS))
                {
                    return RedirectToAction("IndexOfGuest", "Home");
                }
                else if (roles.Contains(Constants.DEACTIVATED_USER_ACCESS))
                {
                    return RedirectToAction("IndexOfDeactivatedUser", "Account");
                }
                else
                {
                    return RedirectToAction("Invalid", "Account");
                }

        }

        //
        // GET: /Account/LogOff
        [Authorize]
        [DeleteBrowserHistory]
        public ActionResult LogOff()
        {

            SessionKeyMgmt.UserId = "";
            SessionKeyMgmt.UserDept = new List<string>();
            SessionKeyMgmt.SharedFiles = new List<DX_FILES>();
            SessionKeyMgmt.SecreteQuestion = "";
            SessionKeyMgmt.LoginAttempts = 0;
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("LogOn");


        }

        private void populateDepartmenetsList()
        {
            try
            {
                var departments = (from departmenttable in database.DX_DEPARTMENT select departmenttable);
                List<DX_DEPARTMENT> depList = departments == null ? new List<DX_DEPARTMENT>() : departments.ToList();
                ViewBag.Departments = depList;
            }
            catch (Exception e)
            {
                ViewBag.Departments = new List<DX_DEPARTMENT>();
                ModelState.AddModelError("","Some error occured please try after some time");
            }
        }
        //
        // GET: /Account/Register
        [DeleteBrowserHistory]
        public ActionResult Register()
        {
            ViewBag.CaptchaGuid = Guid.NewGuid().ToString("N");
            RegisterModel model = new RegisterModel();
            model.Department = new List<int>();
            populateDepartmenetsList();
            return View(model);
        }


        private bool validateModelRegister(RegisterModel model)
        {
            bool isValid = true;
            try
            {

                string captchaid = Request.Form["CaptchaGuid"];
                string captchaValue = Request.Form["Captcha"];


                if (model.FirstName == null || model.LastName == null
                    || model.Phone == null || model.Password == null || model.Position == null
                    || model.Email == null || model.ConfirmPassword == null
                    || model.Captcha == null || model.Answer == null)
                {

                    ModelState.AddModelError("", "Invalid Values!");
                    return false;
                }

                if (isRegisterRegexValid(model) == false)
                {
                    return false;
                }

                if (!("ceo".Equals(model.Position) || "vp".Equals(model.Position) || "employee".Equals(model.Position) || "manager".Equals(model.Position)))
                {
                    ModelState.AddModelError("", "Invalid Role");
                    return false;
                }

                foreach (int i in model.Department)
                {
                    if (i < 1 || i > 7)
                    {
                        ModelState.AddModelError("", "Incorrect department");
                        return false;
                    }

                }

                if (model.Squestion > 9 || model.Squestion < 1)
                {
                    ModelState.AddModelError("", "Incorrect secrate question");
                    return false;
                }



                //Validate captcha

                WebClient captchaCliden = new WebClient();
                string reponseCaptchaService = captchaCliden.DownloadString(
                  "http://www.opencaptcha.com/validate.php?img="
                    + captchaid + "&ans=" + captchaValue);

                if (!"pass".Equals(reponseCaptchaService))
                {
                    ModelState.AddModelError("", "Captcha didn't match, please try again!");
                    return false;
                }



                if ((Constants.POSITION_MANAGER_USER.Equals(model.Position) || Constants.POSITION_EMPLOYEE_USER.Equals(model.Position)) && model.Department.ToList().Count > 1)
                {
                    ModelState.AddModelError("", "Your position can not have multiple departments!");
                    return false;
                }


            }
            catch (Exception)
            {
                isValid = false;
                ModelState.AddModelError("", "Invalid request Please try after some time!");

            }
            return isValid;
        }


        private bool isRegisterRegexValid(RegisterModel model)
        {
            if (!Regex.IsMatch(model.FirstName, @"^[a-zA-Z]{1,20}$"))
            {
                ModelState.AddModelError("", "First name incorrect please try agian!!.");
                return false;
            }
            if (!Regex.IsMatch(model.LastName, @"^[a-zA-Z]{1,20}$"))
            {
                ModelState.AddModelError("", "Last name incorrect please try agian!!.");
                return false;
            }

            if (!Regex.IsMatch(model.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                ModelState.AddModelError("", "Email-id incorrect please try agian!!.");
                return false;
            }

            if (!Regex.IsMatch(model.Phone, @"^(\d{10})$"))
            {
                ModelState.AddModelError("", "Phone incorrect please try agian!!.");
                return false;
            }
            if (!Regex.IsMatch(model.Password, @"^.*(?=.{10,18})(?=.*\d)(?=.*[A-Za-z])(?=.*[@%&#]{0,}).*$"))
            {
                ModelState.AddModelError("", "Password incorrect please try agian!!.");
                return false;
            }
            if (!Regex.IsMatch(model.ConfirmPassword, @"^.*(?=.{10,18})(?=.*\d)(?=.*[A-Za-z])(?=.*[@%&#]{0,}).*$"))
            {
                ModelState.AddModelError("", "Confirm Password incorrect please try agian!!.");
                return false;
            }
            if (!Regex.IsMatch(model.Position, @"^[a-zA-Z]{1,20}$"))
            {
                ModelState.AddModelError("", "Position incorrect please try agian!!.");
                return false;
            }
            if (!Regex.IsMatch(model.Answer, @"^[a-zA-Z]{1,20}$"))
            {
                ModelState.AddModelError("", "Answer incorrect please try agian!!.");
                return false;
            }

            return true;
        }


        //
        // POST: /Account/Register


        [HttpPost]
        [DeleteBrowserHistory]
        public ActionResult Register(RegisterModel model)
        {
            try
            {
                populateDepartmenetsList();

                if (ModelState.IsValid)
                {


                    FormsAuthentication.SignOut();
                    if (validateModelRegister(model) == false)
                    {
                        ViewBag.CaptchaGuid = Guid.NewGuid().ToString("N");
                        return View(model);
                    }

                    ViewBag.CaptchaGuid = Guid.NewGuid().ToString("N");

                    var allusers = from usertabel in database.DX_USER where usertabel.userid == model.Email select usertabel;
                    if (allusers.ToList().Count == 1)
                    {
                        ModelState.AddModelError("", "Email id not unique, please enter a diffrent valid email id!");
                        return View(model);

                    }
                    var alldepartment = from usertabel in database.DX_DEPARTMENT where model.Department.Contains(usertabel.deptid) select usertabel;

                    if (Constants.POSITION_CEO_USER.Equals(model.Position))
                    {

                        alldepartment = from usertabel in database.DX_DEPARTMENT select usertabel;


                    }

                    if (alldepartment.ToList().Count >= 1)
                    {

                        DX_USER user = new DX_USER();
                        user.fname = model.FirstName;
                        user.lname = model.LastName;
                        user.phone = model.Phone;
                        user.questionid = model.Squestion;
                        user.role = model.Position;
                        user.userid = model.Email;
                        user.anshash = generateHash(model.Answer.ToLower());
                        user.accesslevel = Constants.TEMP_USER_ACCESS;
                        user.salt = generateSalt();
                        user.pwdhash = generateHash(user.salt, model.Password);
                        user.actcodehash = "dummycode";
                        database.DX_USER.AddObject(user);//Add user

                        foreach (DX_DEPARTMENT dept in alldepartment.ToList())
                        {
                            DX_USERDEPT userDept = new DX_USERDEPT();
                            userDept.deptid = dept.deptid;
                            userDept.userid = model.Email;
                            database.DX_USERDEPT.AddObject(userDept);//Add department
                        }

                        int success = database.SaveChanges();
                        if (success > 0)
                        {
                            String message = Environment.NewLine + "Hi " + model.FirstName + "," + Environment.NewLine
                                + "Thank you for registering with Docbox!" + Environment.NewLine
                                + "You will soon get notification, once you are been approved by Docbox Administrator" + Environment.NewLine
                                    + "- Docbox Team";
                            try
                            {
                                EmailMessaging.sendMessage(model.Email, message, "Notification");
                            }
                            catch
                            {
                                ModelState.AddModelError("", "User created but unabe to log in at this point of time try logging in after some time!");

                                return View(model);
                            }

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
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Invalid request please try after some time! ");
            }
            // If we got this far, something failed, redisplay form
            return View(model);


        }

        //Get://Account/EnterActivationCode

        //Post:/Account/VerifySecret
        [HttpPost]
        [DeleteBrowserHistory]
        public ActionResult VerifySecret(VerifySecrete secretModel)
        {
            ViewBag.SecQ = SessionKeyMgmt.SecreteQuestion;
            if (ModelState.IsValid)
            {
                if (SessionKeyMgmt.UserId != null && !"".Equals(SessionKeyMgmt.UserId))
                {

                    if (verifyCaptcha() == false)
                    {
                        return View(secretModel);
                    }

                    var allusers = from usertabel in database.DX_USER
                                   where usertabel.userid == SessionKeyMgmt.UserId
                                   select usertabel;

                    if (allusers != null && allusers.ToList().Count == 1)
                    {
                        DX_USER user = allusers.ToList().First();
                        if (secretModel.Answer != null && !"".Equals(secretModel.Answer) && generateHash(secretModel.Answer.ToLower()).Equals(user.anshash))
                        {
                            if (sendNotificationCode())
                            {
                                return RedirectToAction("EnterActivationCode", "Account");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Problem in sending notificatoin code please try recovering the password later!");
                                return RedirectToAction("LogOn", "Account");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Your request is invalid, sorry we cant process it!");
                            return View(secretModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Your request is invalid, sorry we cant process it!");
                        return View(secretModel);
                    }
                }

            }
            return View(secretModel);
        }

        //Get: /Account/PasswordSuccess
        [DeleteBrowserHistory]
        public ActionResult PasswordSuccess()
        {
            return View();
        }

        //Get: /Accoint/ResetPassword/
        [DeleteBrowserHistory]
        public ActionResult ResetPassword()
        {
            ViewBag.CaptchaGuid = Guid.NewGuid().ToString("N");
            return View();
        }

        //Post: /Account/ResetPassword/
        [HttpPost]
        [DeleteBrowserHistory]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid && verifyCaptcha())
            {
                if (model.Password.Equals(model.ConfirmPassword))
                {
                    try
                    {
                        if (SessionKeyMgmt.UserId != null && !"".Equals(SessionKeyMgmt.UserId))
                        {
                            var allusers = from usertabel in database.DX_USER where usertabel.userid == SessionKeyMgmt.UserId select usertabel;
                            if (allusers != null && allusers.ToList().Count == 1)
                            {
                                DX_USER user = allusers.ToList().First();
                                user.salt = generateSalt();
                                user.pwdhash = generateHash(user.salt, model.Password);
                            }
                            database.SaveChanges();
                            return RedirectToAction("PasswordSuccess", "Account");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Could not reset the password please try after some time");
                        }

                    }
                    catch (Exception)
                    {

                        ModelState.AddModelError("", "Could not reset the password please try after some time");

                    }

                }
                else
                {
                    ModelState.AddModelError("", "Password don't match!!");
                }

            }
            return View(model);
        }

        //Get: //Acount/EnterActivationCode
        [DeleteBrowserHistory]
        public ActionResult EnterActivationCode()
        {
            ViewBag.CaptchaGuid = Guid.NewGuid().ToString("N");
            return View();
        }

        //Post: /Account/EnterActivationCode
        [HttpPost]
        [DeleteBrowserHistory]
        public ActionResult EnterActivationCode(EnterActivationCode activationModel)
        {

            if (verifyCaptcha() == false)
                return View(activationModel);

            if (!"".Equals(SessionKeyMgmt.UserId) && SessionKeyMgmt.UserId != null)
            {
                var allusers = from usertabel in database.DX_USER where usertabel.userid == SessionKeyMgmt.UserId select usertabel;
                if (allusers != null && allusers.ToList().Count == 1)
                {
                    if (generateHash(activationModel.ActivationCode).Equals(allusers.ToList().First().actcodehash))
                    {
                        // Valid user success!!

                        //Invalidate initial code
                        allusers.ToList().First().actcodehash = generateActivationCode();
                        database.SaveChanges();

                        // Redirect to reset password
                        return RedirectToAction("ResetPassword", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Activation code is incorrect!!");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Id was invalid!!");
            }
            ViewBag.CaptchaGuid = Guid.NewGuid().ToString("N");
            return View(activationModel);
        }

        private bool sendNotificationCode()
        {

            System.Configuration.Configuration webConfig =
           System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/docbox/web.config");
            System.Configuration.KeyValueConfigurationElement hostEmailId =
              webConfig.AppSettings.Settings["adminEmail"];
            var allusers = from usertabel in database.DX_USER
                           where usertabel.userid == SessionKeyMgmt.UserId
                           select usertabel;
            if (allusers == null && allusers.ToList().Count != 1)
            {
                return false;
            }
            try
            {

                string activationcode = generateActivationCode();
                allusers.ToList().First().actcodehash = generateHash(activationcode);
                EmailMessaging.sendMessage(allusers.ToList().First().userid, "Activation code is:" + activationcode, "Activation Code");
                database.SaveChanges();


            }
            catch (Exception)
            {

                return false;

            }

            return true;

        }
        //GET:/Account/VerifySecret
        [DeleteBrowserHistory]
        public ActionResult VerifySecret()
        {
            VerifySecrete model = new VerifySecrete();
            ViewBag.CaptchaGuid = Guid.NewGuid().ToString("N");
            if ("".Equals(SessionKeyMgmt.SecreteQuestion) || SessionKeyMgmt.SecreteQuestion == null)
            {
                ModelState.AddModelError("", "Your request is invalid, sorry we cant process it!");
                SessionKeyMgmt.SecreteQuestion = "";
                return View(model);
            }
            ViewBag.SecQ = SessionKeyMgmt.SecreteQuestion;



            return View(model);
        }

        //GET : /Account/ForgetPassword
        [DeleteBrowserHistory]
        public ActionResult ForgetPassword()
        {
            ViewBag.CaptchaGuid = Guid.NewGuid().ToString("N");

            return View();

        }
        private bool verifyCaptcha()
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
                return false;
            }

            return true;
        }
        private bool isUserInfoCorrect(ForgetPasswordModel model)
        {

            if (verifyCaptcha() == false)
            {
                return false;
            }

            var allusers = from usertabel in database.DX_USER
                           where
                               usertabel.fname == model.FirstName
                               && usertabel.lname == model.LastName

                               && usertabel.userid == model.Email
                           select usertabel;


            string sQuestion = "";
            //ok if one user and more than one dept
            if (allusers != null && allusers.ToList().Count == 1)
                Constants.secrateQuestionList.TryGetValue(allusers.ToList().First().questionid, out sQuestion);
            else
            {
                SessionKeyMgmt.SecreteQuestion = "";
                SessionKeyMgmt.UserId = "";

                return false;
            }

            SessionKeyMgmt.SecreteQuestion = sQuestion;
            SessionKeyMgmt.UserId = model.Email;

            return allusers.ToList().Count == 1;
        }


        //Post : /Account/FoegetPassword       
        [HttpPost]
        [DeleteBrowserHistory]
        public ActionResult ForgetPassword(ForgetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (isUserInfoCorrect(model))
                {
                    return RedirectToAction("VerifySecret", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "User information incorrect, you can not proceed!");
                }

            }
            else
            {
                ModelState.AddModelError("", "User information incorrect, you can not proceed!");
            }

            return View(model);
        }








        //
        // GET: /Account/ChangePassword


        [DeleteBrowserHistory]
        public ActionResult ChangePassword()
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

        private static string generateActivationCode()
        {
            byte[] randomSalt = new byte[10];
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
