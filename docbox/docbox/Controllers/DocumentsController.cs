using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using docbox.Models;
using System.Web.Security;
using System.Security.Cryptography;
using System.IO;
using docbox.Utilities;

namespace docbox.Controllers
{ 
    public class DocumentsController : Controller
    {
        private dx_docboxEntities db = new dx_docboxEntities();


        //GET : //Documents/ListDocuments
        public ActionResult ListDocuments()
        {
            List<FileModel> model = new List<FileModel>();

            // Get the current logged in userid
            string currentUserId = SessionKeyMgmt.UserId;

            // Select all files for whom current user is the owner
            var allFiles = from filesTable in db.DX_FILES where filesTable.ownerid == currentUserId select filesTable;

            // Iterate through the files list and
            if (allFiles.ToList().Count >= 1)
            {
                foreach (DX_FILES file in allFiles)
                {
                    //what is ur strategy to get the latest version of the files
                    DX_FILEVERSION fileversion = db.DX_FILEVERSION.Single(d => d.fileid==file.fileid);
                    FileModel filemodel = new FileModel();
                    filemodel.FileID = file.fileid.ToString();
                    filemodel.FileName = file.filename;
                    filemodel.Owner = file.ownerid;
                    filemodel.CreationDate = file.creationdate.ToString();
                    filemodel.Description = fileversion.description;
                    filemodel.FileSize = file.size.ToString();
                    model.Add(filemodel);
                }
                    
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "No Files available for view");
            }

            return View(model);
        }

        [HttpPost]
        //ActionResult subm(List)
        //{
        //}

        //
        // GET: /Documents/

        public ViewResult Index()
        {
            var dx_files = db.DX_FILES.Include("DX_USER").Include("DX_USER1");
            return View(dx_files.ToList());
        }

        public void CheckInOut(long id)
        {
            if (ModelState.IsValid)
            {
                DX_FILES dx_files = db.DX_FILES.Single(d => d.fileid == id);
                var text = "";

                if (dx_files.islocked.Equals(true))
                {
                    var lockedBy = dx_files.lockedby;
                    text = "The file is currently locked by" + lockedBy + ". Please try again later";
                }
                else
                {
                    //MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true);
                    dx_files.islocked = "true";
                    dx_files.lockedby = "emkishan@yahoo.com";
                        //currentUser.UserName.ToString();
                    db.DX_FILES.Attach(dx_files);
                    db.ObjectStateManager.ChangeObjectState(dx_files, EntityState.Modified);
                    db.SaveChanges();
                }
            }
        }

        //
        // GET: /Documents/Details/5

        public void Details(long id)
        {
            DX_FILES dx_files = db.DX_FILES.Single(d => d.fileid == id);
            DX_FILEVERSION fileversion = db.DX_FILEVERSION.Single(d => d.fileid==dx_files.fileid);

            byte[] FileData = fileversion.filedata;

            string fullname = dx_files.filename + dx_files.type;

            Response.Clear();
            // Add a HTTP header to the output stream that specifies the default filename
            // for the browser's download dialog
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fullname);
            // Add a HTTP header to the output stream that contains the 
            // content length(File Size). This lets the browser know how much data is being transfered
            Response.AddHeader("Content-Length", FileData.Length.ToString());
            // Set the HTTP MIME type of the output stream
            Response.ContentType = "application/octet-stream";

            Response.BinaryWrite(FileData);
            Response.Flush();

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
            //if (ModelState.IsValid)
            //{
            //    db.DX_FILES.AddObject(dx_files);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");  
            //}

            //ViewBag.lockedby = new SelectList(db.DX_USER, "userid", "fname", dx_files.lockedby);
            //ViewBag.ownerid = new SelectList(db.DX_USER, "userid", "fname", dx_files.ownerid);

            try
            {

                if (Request.Files[0].InputStream != null)
                {

                    HttpPostedFileBase file = Request.Files[0];
                    System.IO.Stream stream = file.InputStream;
                    byte[] fileData = new byte[stream.Length];
                    stream.Read(fileData, 0, fileData.Length);

                    string userid = SessionKeyMgmt.UserId;

                    //Setting properties of the file object
                    dx_files.creationdate = System.DateTime.Now;
                    dx_files.filename = Request.Params.Get("filename");
                    //dx_files.isencrypted = "false";

                    dx_files.ownerid = userid;
                    dx_files.isarchived = "false";
                    dx_files.parentpath = "/" + userid;
                    //dx_files.isencrypted = "false";
                    dx_files.islocked = "false";

                    dx_files.size = (int)stream.Length;
                    dx_files.type = System.IO.Path.GetExtension(file.FileName);

                    db.DX_FILES.AddObject(dx_files);
                    db.SaveChanges();

                    DX_FILEVERSION fileversion = new DX_FILEVERSION();
                    fileversion.fileid = dx_files.fileid;
                    fileversion.versionid = Guid.NewGuid();
                    // // // // //
                    // HARDCODED HERE
                    // // // // // 
                    fileversion.versionnumber = 1;
                    fileversion.updatedate = System.DateTime.Now;
                    fileversion.description = Request.Params.Get("filename");
                    fileversion.size = (int)stream.Length;
                    fileversion.updatedby = userid;
                    //currentUser.UserName;

                    string encrypted = Request.Params.Get("encrypted");
                    if (encrypted == "on")
                    {
                        HttpPostedFileBase keyFile = Request.Files[1];
                        System.IO.Stream keyStream = keyFile.InputStream;
                        byte[] keyData = new byte[keyStream.Length];
                        keyStream.Read(keyData, 0, (int)keyStream.Length);
                        //dx_files.isencrypted = "true";

                        RijndaelManaged Crypto = new RijndaelManaged();
                        Crypto.BlockSize = 128;
                        Crypto.KeySize = 256;
                        Crypto.Mode = CipherMode.CBC;
                        Crypto.Padding = PaddingMode.PKCS7;
                        Crypto.Key = keyData;
                        //Crypto.IV=keyData;

                        ICryptoTransform Encryptor = Crypto.CreateEncryptor(Crypto.Key, Crypto.IV);

                        byte[] cipherText = Encryptor.TransformFinalBlock(fileData, 0, fileData.Length);

                        fileData = cipherText;
                    }

                    fileversion.filedata = fileData;
                    db.AddToDX_FILEVERSION(fileversion);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Please select the file to be uploaded");
                }
            }

            catch (Exception e)
            {
                ModelState.AddModelError("","There is an exception while uploading the document");
            }

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
                return RedirectToAction("ListDocuments");
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
            return RedirectToAction("ListDocuments");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}