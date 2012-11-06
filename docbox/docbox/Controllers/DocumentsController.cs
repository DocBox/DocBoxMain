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
using System.Reflection;

namespace docbox.Controllers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleButtonAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext,
    string actionName, MethodInfo methodInfo)
        {
            bool isValidName = false;
            string keyValue = string.Format("{0}:{1}", Name, Argument);
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);
            if (value != null)
            {
                value = new ValueProviderResult(Argument, Argument, null);
                controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                isValidName = true;
            }

            return isValidName;
        }
    }
   
    public class DocumentsController : Controller
    {
        private dx_docboxEntities db = new dx_docboxEntities();
        private static string ivStringConstant = "RkAS_AGth1s4dbka";
        public static string[] supportedFileTypes = { ".doc", ".docx", 
                                                        ".dotx", ".dot", 
                                                        ".xls", ".xlsx", 
                                                        ".xlt", ".xltx", 
                                                        ".ppt", ".pptx", 
                                                        ".potx", ".pot", 
                                                        ".pdf", ".txt", 
                                                        ".jpeg", ".jpg", 
                                                        ".png", ".bmp", 
                                                        ".tiff", ".tif" };

        //GET : //Documents/ListDocuments
        public ActionResult ListDocuments()
        {
            List<FileModel> modelList = new List<FileModel>();
            try
            {            
                var allFiles = from filetabel in db.DX_FILES where filetabel.ownerid == SessionKeyMgmt.UserId select filetabel;
                if (null != allFiles && allFiles.ToList().Count >= 1)
                {
                    foreach (DX_FILES file in allFiles)
                    {
                        //what is ur strategy to get the latest version of the files
                        // For the current file, get details about the latest version
                        DX_FILEVERSION fileversion = db.DX_FILEVERSION.Single(versionObj => versionObj.fileid == file.fileid 
                            && versionObj.versionnumber == file.latestversion);

                        FileModel filemodel = new FileModel();
                        filemodel.FileID = file.fileid.ToString();
                        filemodel.FileName = file.filename;
                        filemodel.Owner = file.ownerid;
                        filemodel.CreationDate = file.creationdate.ToString();
                        filemodel.Description = fileversion.description;
                        filemodel.FileVersion = file.latestversion;
                        filemodel.IsLocked = Convert.ToBoolean(file.islocked);
                        filemodel.LockedBy = file.lockedby;
                        modelList.Add(filemodel);
                    }
                    return View("ListDocuments",modelList);
                }
                else
                {
                    ModelState.AddModelError("", "No Files available for view");
                }
                return View("ListDocuments", modelList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error getting the document list " + ex.Message);
            }
            return View("ListDocuments",modelList);
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

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Search")]
        public ActionResult Search()
        {
            List<FileModel> model = new List<FileModel>();
            if (ModelState.IsValid)
            {
                IDictionary<string, string> searchConditions = new Dictionary<string, string>();
                var allFiles = from filetabel in db.DX_FILES where filetabel.ownerid == SessionKeyMgmt.UserId select filetabel;
                if (allFiles.ToList().Count >= 1)
                {
                    foreach (DX_FILES file in allFiles)
                    {DX_FILEVERSION fileversion = db.DX_FILEVERSION.Single(versionObj => versionObj.fileid == file.fileid 
                            && versionObj.versionnumber == file.latestversion);
                        FileModel filemodel = new FileModel();
                        filemodel.FileID = file.fileid.ToString();
                        filemodel.FileName = file.filename;
                        filemodel.Owner = file.ownerid;
                        filemodel.CreationDate = file.creationdate.ToString();
                        filemodel.Description = fileversion.description;
                        filemodel.FileVersion = file.latestversion;
                        filemodel.IsLocked = Convert.ToBoolean(file.islocked);
                        filemodel.LockedBy = file.lockedby;
                        model.Add(filemodel);
                    }
                }

                if (this.Request.Form.AllKeys.Length > 0)
                //if(null != fileTitle && fileTitle.Length > 0)
                {
                     //Request["fileName"]
                    searchConditions.Add("fileName", Request["fileName"]);
                }
                else
                {
                    object values = null;

                    if (this.TempData.TryGetValue("SearchConditions", out values))
                    {
                        searchConditions = values as Dictionary<string, string>;
                    }
                }

                this.TempData["SearchConditions"] = searchConditions;
                string fileName = GetSearchConditionValue(searchConditions, "fileName");
                var result = (from s in model
                              where (string.IsNullOrEmpty(fileName) || s.FileName.StartsWith(fileName))
                              select s).ToList();
                model = result;
            }
            return View("ListDocuments", model);
        }

        private static string GetSearchConditionValue(IDictionary<string, string> searchConditions, string key)
        {
            string tempValue = string.Empty;

            if (searchConditions != null)
            {
                searchConditions.TryGetValue(key, out tempValue);
            }
            return tempValue;
        }
               
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CheckInOut(string fileid)
        {
            List<FileModel> model = new List<FileModel>();
            if (ModelState.IsValid)
            {
                    long intID = Convert.ToInt64(fileid);
                    var dx_files = from filetabel in db.DX_FILES where filetabel.fileid== intID select filetabel;
                    foreach(DX_FILES dx_file in dx_files)
                    {
                        if (dx_file != null && dx_file.islocked == true)
                        {
                            var lockedBy = dx_file.lockedby;
                            if (lockedBy == SessionKeyMgmt.UserId)
                            {
                                dx_file.islocked = false;
                                dx_file.lockedby = null;
                            }
                            else
                            {
                                ModelState.AddModelError("", "Permission Denied: The file is locked by user " + lockedBy + ".");
                            }
                        }
                        else
                        {
                            dx_file.islocked = true;
                            dx_file.lockedby = SessionKeyMgmt.UserId;
                        }
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        var status = e.StackTrace;
                        ModelState.AddModelError("Cannot update the database with updated value", status);
                    }
            }
            return RedirectToAction("ListDocuments");
        }

        //
        // GET: /Documents/Details/5

        public void Details(long id)
        {
            DX_FILES dx_files = db.DX_FILES.Single(d => d.fileid == id);
            DX_FILEVERSION fileversion = db.DX_FILEVERSION.Single(d => d.fileid==dx_files.fileid);

            byte[] FileData = fileversion.filedata;

            string fullname = dx_files.filename;

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
        [Authorize(Roles = "employee,manager,ceo,vp")]
        public ActionResult Create(DX_FILES dx_files)
        {
            try
            {
                if (Request.Files[0].InputStream.Length != 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    System.IO.Stream stream = file.InputStream;
                    byte[] fileData = new byte[stream.Length];
                    stream.Read(fileData, 0, fileData.Length);

                    string userid = SessionKeyMgmt.UserId;

                    //Setting properties of the file object
                    dx_files.creationdate = System.DateTime.Now;
                    string description = Request.Params.Get("description");
                    if (description.Length != 0)
                    {
                        dx_files.ownerid = userid;
                        dx_files.isarchived = false;
                        dx_files.parentpath = "/" + userid;
                        dx_files.islocked = false;

                        // Get the filename and its extension
                        string filetype = System.IO.Path.GetExtension(file.FileName);
                        string filename = System.IO.Path.GetFileName(file.FileName);
                        string filenamewoext = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        bool validFile = (filenamewoext.IndexOf('.') == -1);
                        dx_files.type = filetype;
                        dx_files.filename = filename;

                        if(supportedFileTypes.Contains(filetype) && validFile)
                        {
                            // Find if there are any files with the same filename
                            var existingFiles = from filesTable in db.DX_FILES 
                                                where filesTable.ownerid == userid && filesTable.filename == filename
                                                select filesTable;

                            // If there already existed a document by this name
                            // increment the verison number
                            if (existingFiles.Count() != 0)
                            {
                                dx_files = existingFiles.First();
                                dx_files.latestversion = dx_files.latestversion + 1;
                            }
                            else
                            {
                                // Creating a new file
                                dx_files.latestversion = 1;
                                db.DX_FILES.AddObject(dx_files);
                            }

                            // Create a new file version object
                            DX_FILEVERSION fileversion = new DX_FILEVERSION();
                            fileversion.isencrypted = false;

                            // Encrypt the file data if requested
                            string encrypted = Request.Params.Get("encrypted");
                            if (encrypted == "on")
                            {                                
                                // Read the encrytion key
                                 if (Request.Files[1].InputStream.Length != 0)
                                {
                                HttpPostedFileBase keyFile = Request.Files[1];
                                System.IO.Stream keyStream = keyFile.InputStream;
                                byte[] keyData = new byte[keyStream.Length];
                                keyStream.Read(keyData, 0, (int)keyStream.Length);
                                fileversion.isencrypted = true;

                                RijndaelManaged Crypto = new RijndaelManaged();
                                Crypto.BlockSize = 128;
                                Crypto.KeySize = 256;
                                Crypto.Mode = CipherMode.CBC;
                                Crypto.Padding = PaddingMode.PKCS7;
                                Crypto.Key = keyData;

                                // Convert the ivString to a byte array
                                byte[] ivArray = new byte[16];
                                System.Buffer.BlockCopy(ivStringConstant.ToCharArray(), 0,
                                    ivArray, 0, ivArray.Length);
                                Crypto.IV = ivArray;

                                ICryptoTransform Encryptor = Crypto.CreateEncryptor(Crypto.Key, Crypto.IV);
                                byte[] cipherText = Encryptor.TransformFinalBlock(fileData, 0, fileData.Length);

                                // Copy the encrypted data to the file data buffer
                                Array.Clear(fileData, 0, fileData.Length);
                                Array.Resize(ref fileData, cipherText.Length);
                                Array.Copy(cipherText, fileData, cipherText.Length);
                                     }
                                else
                                {
                                    ModelState.AddModelError("", "Please enter a valid keyfile");
                                    return View();
                                }
                            }
                            

                            // Save changes for the DX_FILES object so the new fileid is
                            // auto generated.
                            db.SaveChanges();

                            fileversion.fileid = dx_files.fileid;
                            fileversion.versionid = Guid.NewGuid();
                            fileversion.versionnumber = (int)dx_files.latestversion;
                            fileversion.updatedate = System.DateTime.Now;
                            fileversion.description = description;
                            fileversion.updatedby = userid;

                            // Add information about the file version to database
                            fileversion.filedata = fileData;
                            fileversion.size = fileData.Length;
                            db.DX_FILEVERSION.AddObject(fileversion);
                            db.SaveChanges();

                            // Show the document list
                            return RedirectToAction("ListDocuments");
                        }
                        else{
                            throw new Exception("Invalid file type. Accepted file types are PDF, Word, Excel, PowerPoint, Text and Image Files");
                        }
                    }
                    else
                    {
                        throw new Exception("Please enter a valid description");
                    }
                }                
                else
                {
                    throw new Exception("Please select the file to be uploaded");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("","Error uploading the document: " + ex.Message);
            }
            return View();
            
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

       // [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(long id)
        {
            try
            {
                string user = SessionKeyMgmt.UserId;
                DX_FILES dx_files = db.DX_FILES.Single(d => d.fileid == id);
                if (user == dx_files.ownerid)
                {
                    long fileid = dx_files.fileid;
                    db.DX_FILES.DeleteObject(dx_files);
                    db.SaveChanges();
                    return RedirectToAction("ListDocuments");
                }
                else
                {
                    ModelState.AddModelError("", "You do not have privileges to delete this file");
                    
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Exception caught, Please contact admin for more info");
            }
            return View();
        }


        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ListDocuments")]
        public ActionResult ListDocuments(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var filesselected = form.GetValues("Select");
                List<Int64> fileidList = new List<Int64>();
                filesselected.ToList();
                foreach (var fileid in filesselected)
                    fileidList.Add(Convert.ToInt64(fileid));

                var archivedfiles = from filetable in db.DX_FILES where fileidList.Contains(filetable.fileid) && filetable.islocked==true select filetable;
                if (archivedfiles != null && archivedfiles.ToList().Count > 0)
                {
                    archivedfiles.ToList().ForEach(fm => fm.isarchived = true);
                }
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    ModelState.AddModelError("", "Some Error occured. Try after sometime!");
                }
                return RedirectToAction("ListDocuments");
            }
            
            return RedirectToAction("ListDocuments");

        }

        //GET:/Archived Files on Grid
        
        public ActionResult ArchivedFiles()
        {
            List<FileModel> modelList = new List<FileModel>();
            try
            {
                var allFiles = from filetabel in db.DX_FILES where filetabel.ownerid == SessionKeyMgmt.UserId && filetabel.isarchived==true select filetabel;
                if (null != allFiles && allFiles.ToList().Count >= 1)
                {
                    foreach (DX_FILES file in allFiles)
                    {
                        //what is ur strategy to get the latest version of the files
                        // For the current file, get details about the latest version
                        DX_FILEVERSION fileversion = db.DX_FILEVERSION.Single(versionObj => versionObj.fileid == file.fileid
                            && versionObj.versionnumber == file.latestversion);

                        FileModel filemodel = new FileModel();
                        filemodel.FileID = file.fileid.ToString();
                        filemodel.FileName = file.filename;
                        filemodel.Owner = file.ownerid;
                        filemodel.CreationDate = file.creationdate.ToString();
                        filemodel.Description = fileversion.description;
                        filemodel.FileVersion = file.latestversion;
                        filemodel.IsLocked = Convert.ToBoolean(file.islocked);
                        filemodel.LockedBy = file.lockedby;
                        modelList.Add(filemodel);
                    }
                    return View("ArchivedFiles", modelList);
                }
                else
                {
                    ModelState.AddModelError("", "No Files available for view");
                }
                return View("ArchivedFiles", modelList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error getting the document list " + ex.Message);
            }
            return View("ArchivedFiles", modelList);
        }

        // Perform Unarchiving
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "UnArchive")]
        public ActionResult UnArchive(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var selectedfiles = form.GetValues("Select");
                List<Int64> fileId = new List<Int64>();

                foreach (var file in selectedfiles)
                    fileId.Add(Convert.ToInt64(file));

                var archivedfiles = from filetable in db.DX_FILES where fileId.Contains(filetable.fileid) && filetable.isarchived == true select filetable;
                if (archivedfiles.ToList().Count > 0)
                {
                    foreach (DX_FILES file in archivedfiles)
                    {
                        file.isarchived = false;
                    }
                }
                else
                {
                    ModelState.AddModelError("", "No files selected for unarchiving");
                }
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    ModelState.AddModelError("", "Some error occured. Please try after sometime!");
                    return RedirectToAction("ListDocuments");
                }
                return RedirectToAction("ArchivedFiles");
            }
            else
            {
                return RedirectToAction("ArchivedFiles");
            }
        }

        private void populateUsersList()
        {
            var allusers = from usertabel in db.DX_USER select new { usertabel.userid };
            ViewBag.UsersList = allusers != null ? allusers.ToList() : null;

        }


        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Share")]
        [Authorize(Roles = "employee,manager,ceo,vp")]
        public ActionResult Share(FormCollection form)
        {

            var fileselected = form.GetValues("Select");
            List<Int64> listoffiles = new List<Int64>();
            ShareDocuments model = new ShareDocuments();
            if (fileselected != null)
            {
                fileselected.ToList();
                foreach (var file in fileselected)
                    listoffiles.Add(Convert.ToInt64(file));
                var shareFiles = from filetable in db.DX_FILES where listoffiles.Contains(filetable.fileid) && filetable.ownerid == SessionKeyMgmt.UserId select filetable;

                if (shareFiles != null && shareFiles.ToList().Count > 0)
                {
                    model.Files = shareFiles.ToList();
                    SessionKeyMgmt.SharedFiles = model.Files;
                }
            }
            else
            {
                ModelState.AddModelError("", "No Files Selected!");
                return RedirectToAction("ListDocuments", "Documents");
            }

            model.shareWithUsers = new List<string>();
            populateUsersList();
            return View(model);

        }

        //Perform sharing

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "ShareFiles")]
        public ActionResult ShareFiles(ShareDocuments files)
        {

            if (files != null && files.shareWithUsers != null)
            {
                files.Files = SessionKeyMgmt.SharedFiles;
                try
                {
                    List<Int64> fileIdList = new List<Int64>();
                    foreach (DX_FILES file in files.Files)
                    {
                        fileIdList.Add(Convert.ToInt64(file.fileid));
                    }
                    foreach (string user in files.shareWithUsers)
                    {
                        foreach (Int64 fileId in fileIdList)
                        {
                            var listofsharedfiles = from privilegetable in db.DX_PRIVILEGE where privilegetable.fileid == fileId && privilegetable.userid == user select privilegetable;
                            if (listofsharedfiles.ToList().Count > 0)
                            {
                                foreach (DX_PRIVILEGE existingfile in listofsharedfiles)
                                {
                                    existingfile.read = files.read;
                                    existingfile.write = files.write;
                                    existingfile.update = files.update;
                                    existingfile.check = files.check;
                                }

                            }
                            else
                            {
                                DX_PRIVILEGE sharedfile = new DX_PRIVILEGE();
                                sharedfile.fileid = fileId;
                                sharedfile.userid = user;
                                sharedfile.read = files.read;
                                sharedfile.write = files.write;
                                sharedfile.update = files.update;
                                sharedfile.check = files.check;
                                db.DX_PRIVILEGE.AddObject(sharedfile);
                            }
                        }
                    }
                    db.SaveChanges();
                }
                catch
                {
                    ModelState.AddModelError("", "Some Error occured. Try again after sometime!");
                }
                SessionKeyMgmt.SharedFiles = new List<DX_FILES>();
            }
            else
            {
                ModelState.AddModelError("", "Please select users!!");

            }
            return RedirectToAction("ListDocuments");


        }
 
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }


}