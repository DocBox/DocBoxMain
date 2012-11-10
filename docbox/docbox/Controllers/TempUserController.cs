using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using docbox.Filters;

namespace docbox.Controllers
{
    [DeleteBrowserHistory]
    [AuditLogAttribute]
    public class TempUserController : Controller
    {
        //
        // GET: /TempUser/
        [Authorize(Roles="temp")]
        public ActionResult Index()
        {
            return View();
        }

    }
}
