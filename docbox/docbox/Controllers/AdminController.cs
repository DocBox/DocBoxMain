using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using docbox.Models;
using docbox.Filters;
using docbox.Utilities;
using System.Data;

namespace docbox.Controllers
{

    [DeleteBrowserHistory]
    public class AdminController : Controller
    {
        private dx_docboxEntities database = new dx_docboxEntities();
        private DX_LOGGEREntities auditDatabase = new DX_LOGGEREntities();
        // GET: /Admin/
        [Authorize(Roles="admin,adminless")]
        public ActionResult ViewAudit()
        {
            List<adminlog> logs = auditDatabase.adminlogs.ToList();
            if (logs == null)
            {
                return View();
            }
            else
            {
                return View(logs);
            }
        }

        //populate department list
        private void populateDepartmenetsList()
        {
            try
            {
                var departments = (from departmenttable in database.DX_DEPARTMENT select departmenttable);
                List<DX_DEPARTMENT> depList = departments == null ? new List<DX_DEPARTMENT>() : departments.ToList();
                ViewBag.Departments = depList;
            }
            catch
            {
                ModelState.AddModelError("", "error while populating Department");
            }
        }
          [Authorize(Roles = "admin,adminless")]
        public ActionResult DeactivateAnExistingUser(string id)
        {
            try
            {

                var allusers = from usertable in database.DX_USER where usertable.userid == id select usertable;
                if (allusers != null && allusers.ToList().Count == 1)
                {
                    DX_USER user = allusers.ToList().First();

                    switch (user.role)
                    {
                        case "ceo": user.accesslevel = Constants.DEACTIVATED_USER_ACCESS;
                            break;
                        case "manager": user.accesslevel = Constants.DEACTIVATED_USER_ACCESS;
                            break;
                        case "employee": user.accesslevel = Constants.DEACTIVATED_USER_ACCESS;
                            break;
                        case "vp": user.accesslevel = Constants.DEACTIVATED_USER_ACCESS;
                            break;
                        default:
                            break;
                    }
                     database.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);
                    int success = database.SaveChanges();

                }
            }
            catch { ModelState.AddModelError("", "Error occured while tdeactivating the user"); }
            return RedirectToAction("AllExistingUsers");
        }

        //Edit an existing user
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult EditAnExistingUser(EditUser model)
        {
            try
            {
                var editedUser = from usertable in database.DX_USER where usertable.userid == model.Email select usertable;
                if (editedUser != null && editedUser.ToList().Count() == 1)
                {


                    //var DepartmentRecord = alldepartments.First();
                    DX_USER user = (DX_USER)editedUser.ToList().First();
                    user.accesslevel = model.AccessLevel;
                    user.role = model.Position;
                    database.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);
                      
                }
                var userCurrentdepartments = from usertable in database.DX_USERDEPT where usertable.userid == model.Email select usertable;

                if (userCurrentdepartments != null && userCurrentdepartments.ToList().Count >= 1)
                {
                    foreach (DX_USERDEPT userdepartment in userCurrentdepartments.ToList())
                    {
                        // DX_USERDEPT userDept = new DX_USERDEPT();
                        database.DX_USERDEPT.DeleteObject(userCurrentdepartments.ToList().First());
                        int success = database.SaveChanges();

                    }
                    var alldepartment = from usertable in database.DX_DEPARTMENT where model.Department.Contains(usertable.deptid) select usertable;
                    if (alldepartment != null && alldepartment.ToList().Count() >= 1)
                    {

                        foreach (DX_DEPARTMENT dept in alldepartment.ToList())
                        {
                            DX_USERDEPT userDept = new DX_USERDEPT();
                            userDept.deptid = dept.deptid;
                            userDept.userid = model.Email;
                            database.DX_USERDEPT.AddObject(userDept);
                            int success = database.SaveChanges();
                        }

                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "Error while updating user details");
            }

            return RedirectToAction("AllExistingUsers");
        }
        //viewing existing user details for updation
        [Authorize(Roles = "admin")]
        public ActionResult EditAnExistingUser(string id)
        {
            EditUser UserToBeEdited = new EditUser();
            try
            {


                if (id != null)
                {
                    var presentUserToBeEdited = from usertable in database.DX_USER where usertable.userid == id select usertable;
                    if (presentUserToBeEdited != null && presentUserToBeEdited.ToList().Count() == 1)
                    {

                        DX_USER user = (DX_USER)presentUserToBeEdited.ToList().First();



                        UserToBeEdited.FirstName = user.fname;
                        UserToBeEdited.LastName = user.lname;
                        UserToBeEdited.Email = user.userid;
                        UserToBeEdited.Position = user.role;
                        List<int> depts = DbCommonQueries.getDepartmentIds(user.userid, database);
                        UserToBeEdited.Department = depts;
                        UserToBeEdited.AccessLevel = user.accesslevel;


                    }

                }


                populateDepartmenetsList();

            }
            catch
            {
                ModelState.AddModelError("", "Error occured while editing existing user");
            }
            return View(UserToBeEdited);
        }

        //delete an existing user
        [Authorize(Roles = "admin")]
        public ActionResult DeleteExistingUser(string id)
        {
            try
            {
                if (id != null)
                {
                    var userToBeDeleted = from usertable in database.DX_USER where usertable.userid == id select usertable;

                    if (id != null && userToBeDeleted.ToList().Count == 1)
                    {
                        database.DX_USER.DeleteObject(userToBeDeleted.ToList().First());
                        int success = database.SaveChanges();

                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "error occured while deleting user");
            }
            return RedirectToAction("AllExistingUsers");
        }

        //view all existing users
        [Authorize(Roles = "admin,adminless")]
        public ActionResult AllExistingUsers()
        {

            List<ExistingUsers> CurrentUsers = new List<ExistingUsers>();
            try
            {
                if (ModelState.IsValid)
                {
                    var allUsersNeeded = from usertable in database.DX_USER where usertable.accesslevel != Constants.TEMP_USER_ACCESS select usertable;

                    if (allUsersNeeded != null && allUsersNeeded.ToList().Count >= 1)
                    {

                        List<DX_USER> users = (List<DX_USER>)allUsersNeeded.ToList();

                        foreach (DX_USER presentuser in users)
                        {
                            ExistingUsers CurrentExistingUser = new ExistingUsers();
                            CurrentExistingUser.Email = presentuser.userid;
                            CurrentExistingUser.Name = presentuser.fname + " " + presentuser.lname;
                            CurrentExistingUser.Position = presentuser.role;
                            CurrentExistingUser.accessLevel = presentuser.accesslevel;
                            List<string> depts = DbCommonQueries.getDepartmentName(presentuser.userid, database);
                            string department = "";
                            foreach (string dept in depts) { department = department + dept + ", "; };
                            CurrentExistingUser.Department = department;
                            CurrentExistingUser.creationDate = new DateTime();
                            CurrentUsers.Add(CurrentExistingUser);
                        }

                    }

                }

            }
            catch { ModelState.AddModelError("", "Error occured while populating existing users"); }
            return View(CurrentUsers);
        }

        //reject a user registration request
        [Authorize(Roles = "admin,adminless")]
        public ActionResult RejectRequest(string id)
        {
            try
            {
                if (id != null)
                {
                    var userToBeDeleted = from usertable in database.DX_USER where usertable.userid == id select usertable;

                    if (id != null && userToBeDeleted.ToList().Count == 1)
                    {
                        String message = Environment.NewLine + "Hi " + userToBeDeleted.ToList().First().fname
     + "," + Environment.NewLine
                                    + "You request has been Rejected!" + Environment.NewLine
                                    + "You have not provided valid details while registering" + Environment.NewLine
                                        + "- Docbox Team";
                        try
                        {
                            EmailMessaging.sendMessage(id, message, "Notification");
                        }
                        catch
                        {
                            ModelState.AddModelError("", "User approved, but notification not send");

                            return View("Error");
                        }


                        database.DX_USER.DeleteObject(userToBeDeleted.ToList().First());
                        int success = database.SaveChanges();

                    }
                }
            }
            catch { ModelState.AddModelError("", "error occured while deleting the user"); }
            return RedirectToAction("Index");
        }
        //accepting a user request to assign access level
        [Authorize(Roles = "admin,adminless")]
        public ActionResult AssignAccessLevel(string id)
        {
            try
            {

                if (id != null)
                {
                    var allusers = from usertabel in database.DX_USER where usertabel.userid == id select usertabel;
                    if (allusers != null && allusers.ToList().Count == 1)
                    {
                        DX_USER user = allusers.ToList().First();
                        

                        switch (user.role)
                        {
                            case "ceo": user.accesslevel = Constants.CEO_USER_ACCESS;
                                break;
                            case "manager": user.accesslevel = Constants.MANAGER_USER_ACCESS;
                                
                                break;
                            case "employee": user.accesslevel = Constants.EMPLOYEE_USER_ACCESS;
                                break;
                            case "vp": user.accesslevel = Constants.VP_USER_ACCESS;
                                break;
                            default:
                                break;
                        }
                        database.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);
                        int success = database.SaveChanges();
                        if (success > 0)
                        {
                            String message = Environment.NewLine + "Hi " + user.fname + "," + Environment.NewLine
                                + "You request has been approved!" + Environment.NewLine
                                + "You Can now login to your account to access your files" + Environment.NewLine
                                    + "- Docbox Team";
                            try
                            {
                                EmailMessaging.sendMessage(id, message, "Notification");
                            }
                            catch
                            {
                                ModelState.AddModelError("", "User approved, but notification not send");

                                return View("Error");
                            }

                            //FormsAuthentication.SetAuthCookie(id, false);
                        }

                    }
                }
            }
            catch { ModelState.AddModelError("", "Error occured while assigning access level to the user"); }
            return RedirectToAction("Index");
        }
        //
        // GET: /Admin/
        //view registration requests 
        [Authorize(Roles = "admin,adminless")]
        public ActionResult Index()
        {
            List<UserNeedingApproval> AllUsersNeedingApproval = new List<UserNeedingApproval>();
            try
            {
                if (ModelState.IsValid)
                {

                    var allTempUsers = from usertable in database.DX_USER where usertable.accesslevel.Equals(Constants.TEMP_USER_ACCESS) select usertable;

                    if (allTempUsers != null && allTempUsers.ToList().Count >= 1)
                    {

                        List<DX_USER> users = (List<DX_USER>)allTempUsers.ToList();


                        foreach (DX_USER tempuser in users)
                        {
                            UserNeedingApproval tempUserNeedingApproval = new UserNeedingApproval();

                            tempUserNeedingApproval.Email = tempuser.userid;
                            tempUserNeedingApproval.Name = tempuser.fname + " " + tempuser.lname;
                            tempUserNeedingApproval.Position = tempuser.role;
                            List<string> depts = DbCommonQueries.getDepartmentName(tempuser.userid, database);
                            string department = "";
                            foreach (string dept in depts) { department = department + dept + ", "; };
                            tempUserNeedingApproval.Department = department;
                            tempUserNeedingApproval.creationDate = new DateTime();
                            AllUsersNeedingApproval.Add(tempUserNeedingApproval);
                        }

                    }
                }
            }
            catch { ModelState.AddModelError("", "Error occured while populating all user requests"); }
            return View(AllUsersNeedingApproval);
        }
          [Authorize(Roles = "admin,adminless")]
        public ActionResult ActivateAnExistingUser(string id)
        {
            try
            {


                var allusers = from usertable in database.DX_USER where usertable.userid == id select usertable;
                if (allusers != null && allusers.ToList().Count == 1)
                {
                    DX_USER user = allusers.ToList().First();
                                         
                    if (user.accesslevel == Constants.DEACTIVATED_USER_ACCESS)
                    {
                        user.accesslevel = user.role;

                    }

                     database.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);
                     
                        int success = database.SaveChanges();


                }
            }
            catch { ModelState.AddModelError("", "Error occured while activating the user"); }
            return RedirectToAction("AllExistingUsers");
        }


        // public ICollection<int> dept { get; set; }
    }
}


