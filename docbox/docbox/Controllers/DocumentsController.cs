using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using docbox.Models;

namespace docbox.Controllers
{ 
    public class DocumentsController : Controller
    {
        private dx_docboxEntities db = new dx_docboxEntities();

        //
        // GET: /Documents/

        public ViewResult Index()
        {
            var dx_files = db.DX_FILES.Include("DX_USER").Include("DX_USER1");
            return View(dx_files.ToList());
        }

        //
        // GET: /Documents/Details/5

        public ViewResult Details(long id)
        {
            DX_FILES dx_files = db.DX_FILES.Single(d => d.fileid == id);
            return View(dx_files);
        }

        //
        // GET: /Documents/Create

        public ActionResult Create()
        {
            ViewBag.lockedby = new SelectList(db.DX_USER, "userid", "fname");
            ViewBag.ownerid = new SelectList(db.DX_USER, "userid", "fname");
            return View();
        } 

        //
        // POST: /Documents/Create

        [HttpPost]
        public ActionResult Create(DX_FILES dx_files)
        {
            if (ModelState.IsValid)
            {
                db.DX_FILES.AddObject(dx_files);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.lockedby = new SelectList(db.DX_USER, "userid", "fname", dx_files.lockedby);
            ViewBag.ownerid = new SelectList(db.DX_USER, "userid", "fname", dx_files.ownerid);
            return View(dx_files);
        }
        
        //
        // GET: /Documents/Edit/5
 
        public ActionResult Edit(long id)
        {
            DX_FILES dx_files = db.DX_FILES.Single(d => d.fileid == id);
            ViewBag.lockedby = new SelectList(db.DX_USER, "userid", "fname", dx_files.lockedby);
            ViewBag.ownerid = new SelectList(db.DX_USER, "userid", "fname", dx_files.ownerid);
            return View(dx_files);
        }

        //
        // POST: /Documents/Edit/5

        [HttpPost]
        public ActionResult Edit(DX_FILES dx_files)
        {
            if (ModelState.IsValid)
            {
                db.DX_FILES.Attach(dx_files);
                db.ObjectStateManager.ChangeObjectState(dx_files, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lockedby = new SelectList(db.DX_USER, "userid", "fname", dx_files.lockedby);
            ViewBag.ownerid = new SelectList(db.DX_USER, "userid", "fname", dx_files.ownerid);
            return View(dx_files);
        }

        //
        // GET: /Documents/Delete/5
 
        public ActionResult Delete(long id)
        {
            DX_FILES dx_files = db.DX_FILES.Single(d => d.fileid == id);
            return View(dx_files);
        }

        //
        // POST: /Documents/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {            
            DX_FILES dx_files = db.DX_FILES.Single(d => d.fileid == id);
            db.DX_FILES.DeleteObject(dx_files);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}