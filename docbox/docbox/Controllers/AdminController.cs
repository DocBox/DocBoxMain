using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using docbox.Models;

namespace docbox.Controllers
{
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
            List<UserNeedingApproval> model;
            model = new List<UserNeedingApproval>();
            if (ModelState.IsValid)
            {

                var allTempUsers = from usertable in database.DX_USER where usertable.accesslevel.Equals(Constants.TEMP_USER) select new { usertable.fname, usertable.lname, usertable.userid, usertable.role};

                if (allTempUsers!=null&&allTempUsers.ToList().Count >= 1)
                {

                    List<DX_USER> users = (List<DX_USER>)allTempUsers;

                    foreach (DX_USER tempuser in users)
                    { 
                        UserNeedingApproval tempUserNeedingApproval= new UserNeedingApproval();
                        tempUserNeedingApproval.Email = tempuser.userid;
                        tempUserNeedingApproval.FirstName = tempuser.fname;
                        tempUserNeedingApproval.LastName = tempuser.lname;
                        tempUserNeedingApproval.Position = tempuser.role;
                        model.Add(tempUserNeedingApproval);

                    }
                }
            }
            return View();
        }
    }
}
