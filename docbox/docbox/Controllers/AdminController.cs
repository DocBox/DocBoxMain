using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using docbox.Models;
using docbox.Filters;
 
namespace docbox.Controllers
{
    [DeleteBrowserHistory]
    public class AdminController : Controller
    {
        private dx_docboxEntities database = new dx_docboxEntities();
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult TempUsers() {
            List<UserNeedingApproval> AllUsersNeedingApproval;
            AllUsersNeedingApproval = new List<UserNeedingApproval>();
            if (ModelState.IsValid)
            {

                var allTempUsers = from usertable in database.DX_USER where usertable.accesslevel.Equals(Constants.TEMP_USER_ACCESS) select usertable;

                if (allTempUsers!=null&&allTempUsers.ToList().Count >= 1)
                {

                    List<DX_USER> users = (List<DX_USER>)allTempUsers.ToList();

                   
                    foreach (DX_USER tempuser in users)
                    { 
                        UserNeedingApproval tempUserNeedingApproval= new UserNeedingApproval();
                        tempUserNeedingApproval.Email = tempuser.userid;
                        tempUserNeedingApproval.FirstName = tempuser.fname;
                        tempUserNeedingApproval.LastName = tempuser.lname;
                        tempUserNeedingApproval.Position = tempuser.role;
                        AllUsersNeedingApproval.Add(tempUserNeedingApproval);
                    }

                }
            }
            return View(AllUsersNeedingApproval);
        }
    }
}
