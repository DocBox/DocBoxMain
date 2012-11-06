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
using docbox.Filters;

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

    [DeleteBrowserHistory]
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

        [ImportFromTempData]
        [Authorize(Roles = "employee,manager,ceo,vp")]
        public ActionResult ListDocuments(List<FileModel> model)
        {
            if (null != model)
            {
                return View("ListDocuments", model);
            }
            return View("ListDocuments", getMyDocsModel());
        }

        public List<FileModel> getMyDocsModel()
        {
            List<FileModel> modelList = new List<FileModel>();
            try
            {
                var allFiles = from filetabel in db.DX_FILES where filetabel.ownerid == SessionKeyMgmt.UserId && filetabel.isarchived == false select filetabel;

                if (null != allFiles && allFiles.ToList().Count >= 1)
                {
                    foreach (DX_FILES file in allFiles)
                    {
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
                }
                else
                {
                    ModelState.AddModelError("", "No Files available for view");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error getting the document list " + ex.Message);
            }

            return modelList;
        }

        [Authorize(Roles = "manager,ceo,vp")]
        public ActionResult DepartmentFiles(List<FileModel> model)
        {
            if (null != model)
            {
                return View("DepartmentFiles", model);
            }
            return View("DepartmentFiles", getDeptDocsModel());
        }

        private List<string> getRoleHierarchy(string designation)
        {
            List<string> accessibleRole = new List<string>();
            switch (designation)
            {
                case "ceo":
                    {
                        accessibleRole.Add("vp");
                        accessibleRole.Add("manager");
                        accessibleRole.Add("employee");
                        break;
                    }
                case "vp":
                    {
                        accessibleRole.Add("manager");
                        accessibleRole.Add("employee");
                        break;
                    }
                case "manager":
                    {
                        accessibleRole.Add("employee");
                        break;
                    }
                default: break;
            }

            return accessibleRole;
        }

        private List<FileModel> getDeptDocsModel()
        {
            
            List<FileModel> modelList = new List<FileModel>();
            try
            {
                ////get the role for current user
                //var userRole = db.DX_USER.Single(d => d.userid == SessionKeyMgmt.UserId).role;

                ////identify all other users in the same department as current user
                //var otherUsers = from deptTable in db.DX_USERDEPT join userDept in db.DX_USERDEPT on deptTable.deptid equals userDept.deptid where userDept.userid == SessionKeyMgmt.UserId select deptTable.userid;

                ////pull out only those users which are above the hierarchy of current user
                //var filteredUsers = from userTable in db.DX_USER where otherUsers.Contains(userTable.userid) && getRoleHierarchy(userRole).Contains(userTable.role) select userTable.userid; 

                ////get the files owned by these users and is not archived or shared.
                //var allFiles = from filetable in db.DX_FILES where filteredUsers.Contains(filetable.ownerid) && filetable.ownerid != SessionKeyMgmt.UserId  && filetable.isarchived == false select filetable;

                var allFiles = from filetable in db.DX_FILES join userprev in db.DX_PRIVILEGE on filetable.fileid equals userprev.fileid where userprev.userid == SessionKeyMgmt.UserId select filetable;
                if (null != allFiles && allFiles.ToList().Count >= 1)
                {
                    foreach (DX_FILES file in allFiles)
                    {
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
                }
                else
                {
                    ModelState.AddModelError("", "No Files available for view");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error getting the document list " + ex.Message);
            }

            return modelList;
        }
        
        public ViewResult Index()
        {
            var dx_files = db.DX_FILES.Include("DX_USER").Include("DX_USER1");
            return View(dx_files.ToList());
        }

        [MultipleButton(Name = "action", Argument = "SearchMyDocs")]
        public ActionResult SearchMyDocs()
        {
            string filename = "";
            if (this.Request.Form.AllKeys.Length > 0)
            {
                filename = Request["fileName"];
            }
            List<FileModel> model = Search(filename, getMyDocsModel());
            return View("ListDocuments", model);
        }

        [MultipleButton(Name = "action", Argument = "SearchMyDeptDocs")]
        public ActionResult SearchMyDeptDocs()
        {
            string filename = "";
            if (this.Request.Form.AllKeys.Length > 0)
            {
                filename = Request["fileName"];
            }
            List<FileModel> model = Search(filename, getDeptDocsModel());
            return View("DepartmentFiles", model);
        }

        public List<FileModel> Search(string fileTitle, List<FileModel> model)
        {
            if (ModelState.IsValid)
            {
                IDictionary<string, string> searchConditions = new Dictionary<string, string>();
                
                if(!string.IsNullOrEmpty(fileTitle))
                {
                    searchConditions.Add("fileName", fileTitle);
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
                                where (string.IsNullOrEmpty(fileTitle) || s.FileName.StartsWith(fileTitle))
                                select s).ToList();
                model = result;
            }
            return model;
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

        private void SaveCheckInOut(string fileid)
        {
            if (ModelState.IsValid)
            {
                long intID = Convert.ToInt64(fileid);
                var dx_files = from filetabel in db.DX_FILES where filetabel.fileid == intID && filetabel.isarchived == false select filetabel;
                foreach (DX_FILES dx_file in dx_files)
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
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "employee,manager,ceo,vp")]
        public ActionResult CheckInOut(string fileid)
        {
            SaveCheckInOut(fileid);
            return RedirectToAction("ListDocuments");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "manager,ceo,vp")]
        public ActionResult CheckInOutDept(string fileid)
        {
            SaveCheckInOut(fileid);
            return RedirectToAction("DepartmentFiles");
        }

        //
        // GET: /Documents/Details/5

        public void Download(long id)
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
        [Authorize(Roles = "employee,manager,ceo,vp")]
        public ActionResult Create()
        {
            ViewBag.lockedby = new SelectList(db.DX_USER, "userid", "fname");
            ViewBag.ownerid = new SelectList(db.DX_USER, "userid", "fname");
            return View();
        } 

        //
        // POST: /Documents/Create
        public abstract class TempDataTransfer : ActionFilterAttribute
        {
            protected static readonly string Key = typeof(TempDataTransfer).FullName;
        }
        public class ExportToTempData : TempDataTransfer
        {
            public override void OnActionExecuted(ActionExecutedContext filterContext)
            {
                //Only export when ModelState is not valid
                if (!filterContext.Controller.ViewData.ModelState.IsValid)
                {
                    //Export if we are redirecting
                    if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
                    {
                        filterContext.Controller.TempData[Key] = filterContext.Controller.ViewData.ModelState;
                    }
                }
                base.OnActionExecuted(filterContext);
            }
        }

        public class ImportFromTempData : TempDataTransfer
        {
            public override void OnActionExecuted(ActionExecutedContext filterContext)
            {
                ModelStateDictionary modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;
                if (modelState != null)
                {
                    //Only Import if we are viewing
                    if (filterContext.Result is ViewResult)
                    {
                        filterContext.Controller.ViewData.ModelState.Merge(modelState);
                    }
                    else
                    {
                        //Otherwise remove it.
                        filterContext.Controller.TempData.Remove(Key);
                    }
                }
                base.OnActionExecuted(filterContext);
            }
        }



        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
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
                                DX_FILES existingFile = existingFiles.First();
                                if (existingFile.isarchived == true)
                                {
                                    ModelState.AddModelError("", "A file with the same name exists in your archived docs. Cannot upload");
                                    return View();
                                }
                                else
                                {
                                    ModelState.AddModelError("", "A file with same name exists in My Docs. Please update the corresponding file");
                                    return View();
                                }
                            }
                            else
                            {
                                // Creating a new file
                                dx_files.latestversion = 1;
                                dx_files.creationdate = System.DateTime.Now;
                                db.DX_FILES.AddObject(dx_files);
                                db.SaveChanges();

                                DX_USER user = db.DX_USER.Single(d => d.userid == userid);
                                string accesslevel= user.accesslevel;

                                //Share with the owner
                                DX_PRIVILEGE empPriv = new DX_PRIVILEGE();
                                empPriv.fileid = dx_files.fileid;
                                empPriv.userid = userid;
                                empPriv.read = true;
                                empPriv.update = true;
                                empPriv.write = true;
                                empPriv.check = true;
                                db.DX_PRIVILEGE.AddObject(empPriv);
                                
                                
                                //Based on the role, the file should be shared with managers
                                if ( accesslevel== "employee")
                                {
                                    //Getting the dept id of employee
                                    DX_USERDEPT userdept = db.DX_USERDEPT.Single(d => d.userid == userid);
                                    int deptid = userdept.deptid;

                                    //Getting the user id of manager
                                    var managers = from usersTable in db.DX_USER
                                                  where usersTable.accesslevel == "manager"
                                                  join userdepts in db.DX_USERDEPT on usersTable.userid equals userdepts.userid
                                                    select usersTable;
                                    if (managers.Count() != 0)
                                    {
                                        foreach (DX_USER managerUser in managers)
                                        {
                                            //Providing manager the respective rights
                                            string managerId = managerUser.userid;
                                            DX_PRIVILEGE mgrPriv = new DX_PRIVILEGE();
                                            mgrPriv.fileid = dx_files.fileid;
                                            mgrPriv.userid = managerId;
                                            mgrPriv.read = true;
                                            mgrPriv.check = true;
                                            
                                            mgrPriv.update = true;
                                            db.DX_PRIVILEGE.AddObject(mgrPriv);
                                
                                        }
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("", "File could not be shared with the manager");
                                    }
                                    
                                }
                                if (accesslevel == "manager" || accesslevel=="employee")
                                {
                                    //Getting the dept id of employee
                                    DX_USERDEPT userdept = db.DX_USERDEPT.Single(d => d.userid == userid);
                                    int deptid = userdept.deptid;

                                    var vp = from usersTable in db.DX_USER
                                             where usersTable.accesslevel == "vp"
                                             join userdepts in db.DX_USERDEPT on usersTable.userid equals userdepts.userid
                                             select usersTable;
                                    if (vp.Count() != 0)
                                    {
                                        foreach (DX_USER vpUser in vp)
                                        {
                                            string vpId = vpUser.userid;
                                            DX_PRIVILEGE vpPriv = new DX_PRIVILEGE();
                                            vpPriv.fileid = dx_files.fileid;
                                            vpPriv.userid = vpId;
                                            vpPriv.read = true;
                                            vpPriv.check = true;

                                            vpPriv.update = true;
                                            db.DX_PRIVILEGE.AddObject(vpPriv);

                                        }
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("", "File could not be shared with the VP");
                                    }
                                }
                                if (accesslevel == "vp" || accesslevel=="manager" || accesslevel=="employee")
                                {
                                    var ceo = from usersTable in db.DX_USER
                                              where usersTable.accesslevel == "ceo"
                                              select usersTable;
                                    if(ceo.Count() !=0){
                                        foreach (DX_USER ceoUser in ceo)
                                        {
                                            string ceoId = ceoUser.userid;
                                            DX_PRIVILEGE ceoPriv = new DX_PRIVILEGE();
                                            ceoPriv.fileid = dx_files.fileid;
                                            ceoPriv.userid = ceoId;
                                            ceoPriv.read = true;
                                            ceoPriv.check = true;
                                            
                                            ceoPriv.update = true;
                                            db.DX_PRIVILEGE.AddObject(ceoPriv);
                                        }
                                    }
                                    db.SaveChanges();
                                }
                                
                                else
                                {
                                    ModelState.AddModelError("", "You are not authorized to upload a file");
                                    return View();
                                }
                                
                                
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
            DX_FILES fileObj = db.DX_FILES.Single(d => d.fileid == id);
            
            DX_FILEVERSION fileVer = db.DX_FILEVERSION.Single(d => d.fileid == id && d.versionnumber == fileObj.latestversion);
            ViewBag.lockedby = new SelectList(db.DX_USER, "userid", "fname", fileObj.lockedby);
            ViewBag.ownerid = new SelectList(db.DX_USER, "userid", "fname", fileObj.ownerid);
            return View(fileVer);
        }

        //
        // POST: /Documents/Edit/5

        [HttpPost]
        [Authorize(Roles = "employee,manager,ceo,vp")]
        public ActionResult Edit(DX_FILEVERSION filever)
        {
            try
            {
                string description = Request.Params.Get("description");
                if (description.Length != 0)
                {
                    string fileidtrap = Request.Params.Get("fileid");
                    long fileid = long.Parse(fileidtrap.Substring(0, fileidtrap.IndexOf('_')));
                    int fileversion = int.Parse(fileidtrap.Substring(fileidtrap.IndexOf(' ')));
                    string userid = SessionKeyMgmt.UserId;

                    DX_FILES mainFile = db.DX_FILES.Single(d => d.fileid == fileid);

                    DX_PRIVILEGE userPriv = db.DX_PRIVILEGE.Single(d => d.fileid == fileid && d.userid == userid);

                    if (userPriv.update == true)
                    {

                        DX_FILEVERSION fileObj = db.DX_FILEVERSION.Single(d => d.fileid == fileid && d.versionnumber == fileversion);

                        if (fileObj.description != description)
                        {
                            fileObj.updatedate = System.DateTime.Now;
                            fileObj.description = description;
                            fileObj.updatedby = SessionKeyMgmt.UserId;
                            db.ObjectStateManager.ChangeObjectState(fileObj, EntityState.Modified);
                            db.SaveChanges();
                            return RedirectToAction("ListDocuments");
                        }
                        else
                        {
                            ModelState.AddModelError("", "File description is same as earlier and hence not updated");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "You do not have edit permissions on this file");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please enter a valid description");
                    return View();
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("","Exception occured while editing the document");
                return View();
            }
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
                if (dx_files.islocked != true)
                {
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
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The file has been checked out by other user and cannot be deleted");
                    return View();
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Exception caught, Please contact admin for more info");
            }
            return View();
        }


        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Archive")]
        [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
        public ActionResult Archive(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var filesselected = form.GetValues("Select");
                List<Int64> fileidList = new List<Int64>();
                if (filesselected != null && filesselected.ToList().Count>0)
                {
                    foreach (var fileid in filesselected)
                        fileidList.Add(Convert.ToInt64(fileid));


                    var archivedfiles = from filetable in db.DX_FILES where fileidList.Contains(filetable.fileid) select filetable;
                    if (archivedfiles != null && archivedfiles.ToList().Count > 0)
                    {
                        foreach (DX_FILES file in archivedfiles)
                        {

                            if (file.islocked == false)
                            {
                                archivedfiles.ToList().ForEach(fm => fm.isarchived = true);
                            }
                            else
                            {
                                ModelState.AddModelError("", "The file is checked-out by someone you cannot archive it at this time");
                                return RedirectToAction("ListDocuments");
                            }
                        }
                        try
                        {
                            db.SaveChanges();
                        }
                        catch
                        {
                            ModelState.AddModelError("", "Some Error occured. Try after sometime!");
                        }
                    }
                    if (archivedfiles.ToList().Count < fileidList.Count)
                    {
                        ModelState.AddModelError("", "Some of the files got deleted so it was unable to archive them");
                        return RedirectToAction("ListDocuments");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "No Files Selected");
                    return RedirectToAction("ListDocuments");
                }
            }
            
            return RedirectToAction("ListDocuments");
         }

        //GET:/Archived Files on Grid
       // [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
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
        [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
        public ActionResult UnArchive(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var selectedfiles = form.GetValues("Select");
                List<Int64> fileId = new List<Int64>();
                if (selectedfiles != null && selectedfiles.ToList().Count > 0)
                {
                    foreach (var file in selectedfiles)
                        fileId.Add(Convert.ToInt64(file));

                    var archivedfiles = from filetable in db.DX_FILES where fileId.Contains(filetable.fileid) && filetable.isarchived == true select filetable;
                    if (archivedfiles.ToList().Count > 0 && archivedfiles != null)
                    {
                        foreach (DX_FILES file in archivedfiles)
                        {
                            file.isarchived = false;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "No files selected for unarchiving");
                        return RedirectToAction("ArchivedFiles");
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Some error occured. Please try after sometime!");
                        return RedirectToAction("ArchivedFiles");
                    }
                    return RedirectToAction("ArchivedFiles");
                }
                else
                {
                    ModelState.AddModelError("", "No files selected for unarchiving");
                    return RedirectToAction("ArchivedFiles");
                }
            }
                    return RedirectToAction("ArchivedFiles");
        }

        private void populateUsersList()
        {
            var allusers = from usertabel in db.DX_USER select new { usertabel.userid };
            ViewBag.UsersList = allusers != null ? allusers.ToList() : null;

        }


        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
        [Authorize(Roles = "employee,manager,ceo,vp")]
        [MultipleButton(Name = "action", Argument = "Share")]
        public ActionResult Share(FormCollection form)
        {
            var fileselected = form.GetValues("Select");
            List<Int64> listoffiles = new List<Int64>();
            ShareDocuments model = new ShareDocuments();
            
            if (fileselected != null && fileselected.ToList().Count>0)
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
                if(listoffiles.Count > shareFiles.ToList().Count)
                {
                    ModelState.AddModelError("","Some files got deleted before you shared the documents");
                    return RedirectToAction("ListDocuments");
                }
            }
            else
            {
                ModelState.AddModelError("", "No Files Selected!");
                return RedirectToAction("ListDocuments");
            }
            
            model.shareWithUsers = new List<string>();
            populateUsersList();
            return View(model);

        }

        //Perform sharing

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
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