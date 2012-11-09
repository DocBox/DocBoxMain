using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using docbox.Filters;

namespace docbox.Controllers
{
    [DeleteBrowserHistory]
    public class HomeController : Controller
    {
        [Authorize (Roles="employee,manager,ceo,vp,guest")] 
        
        public ActionResult Index()
        {
            ViewBag.Message = "";
            return RedirectToAction("ListDocuments", "Documents");
        }

        public ActionResult IndexOfGuest()
        {
            ViewBag.Message = "";
            return RedirectToAction("SharedFiles", "Documents");
        }

        
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    }
}
