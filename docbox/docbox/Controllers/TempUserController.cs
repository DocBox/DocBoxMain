using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using docbox.Filters;

namespace docbox.Controllers
{
    [DeleteBrowserHistory]
    public class TempUserController : Controller
    {
        //
        // GET: /TempUser/

        public ActionResult Index()
        {
            return View();
        }

    }
}
