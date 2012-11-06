﻿using System;
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
        [Authorize (Roles="employee,manager,ceo,vp")] 
        
        public ActionResult Index()
        {
            ViewBag.Message = "";
            return RedirectToAction("ListDocuments", "Documents");
        }

       
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    }
}
