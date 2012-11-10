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
using System.Text.RegularExpressions;

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
            try
            {
                bool isValidName = false;
                string keyValue = string.Format("{0}:{1}", Name, Argument);
                var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);
                if (value != null)
                {
                    value = new ValueProviderResult(Argument, Argument, null);
                    controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                    isValidName = true;
                    return isValidName;
                }
            }
            catch (HttpException)
            {
                  
            }
            return false;
        }
    }

    [DeleteBrowserHistory]
    [AuditLogAttribute]
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
        public static Int64 MAX_FILE_SIZE = 5 * 1024 * 1024;

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
                        filemodel.LockedBy = file.lockedby;
                        filemodel.IsLocked = (bool)file.islocked;
                        modelList.Add(filemodel);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "No Files available for view");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error getting the document list");
            }

            return modelList;
        }

        [ImportFromTempData]
        [Authorize(Roles = "manager,ceo,vp")]
        public ActionResult DepartmentFiles(string dept, List<FileModel> model)
        {
            if (null != model)
            {
                return View("DepartmentFiles", model);
            }
            TempData["dept"] = dept;
            return View("DepartmentFiles", getDeptDocsModel(dept));
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

        [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
        private List<FileModel> getDeptDocsModel(string dept)
        {
            int deptid = DbCommonQueries.getDepartmentId(dept, db);
            List<FileModel> modelList = new List<FileModel>();
            try
            {
                //get all the files for the current user which he has inherited.
                var files = from filetable in db.DX_FILES join userprev in db.DX_PRIVILEGE on filetable.fileid equals userprev.fileid where userprev.userid == SessionKeyMgmt.UserId && userprev.reason=="inherit" select filetable;

                //filter the files based the department requested
                var allFiles = from fileTable in files join userdept in db.DX_USERDEPT on fileTable.ownerid equals userdept.userid where userdept.deptid == deptid select fileTable;
                
                //if no files exist, throw an error.
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
                    ModelState.AddModelError("", "No Department Files available for view");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error getting the department document list " + ex.Message);
            }

            return modelList;
        }

        private List<FileModel> getArchivedDocsModel()
        {
            List<FileModel> model = new List<FileModel>();
            try
            {
                var archivedfiles = from filetable in db.DX_FILES where filetable.isarchived == true && filetable.ownerid == SessionKeyMgmt.UserId select filetable;
                if (archivedfiles != null && archivedfiles.ToList().Count > 0)
                {
                    foreach (DX_FILES file in archivedfiles)
                    {
                        DX_FILEVERSION fileversion = db.DX_FILEVERSION.Single(versionObj => versionObj.fileid == file.fileid
                            && versionObj.versionnumber == file.latestversion);
                        FileModel archfile = new FileModel();
                        archfile.FileID = (file.fileid).ToString();
                        archfile.FileName = file.filename;
                        archfile.FileVersion = file.latestversion;
                        archfile.Description = fileversion.description;
                        archfile.CreationDate = (file.creationdate).ToString();
                        model.Add(archfile);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "No files have been archived by you");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Error getting the archived documents");
            }
            return model;
        }

        private List<FileShared> getSharedDocsModel()
        {
            List<FileShared> docs = new List<FileShared>();
            try
            {
                var files = from privilegetable in db.DX_PRIVILEGE
                            join filetable in db.DX_FILES
                            on new { key1 = privilegetable.userid, key2 = privilegetable.fileid, key3 = false }
                                equals new { key1 = SessionKeyMgmt.UserId, key2 = filetable.fileid, key3 = filetable.isarchived }
                            join versiontable in db.DX_FILEVERSION on filetable.fileid equals versiontable.fileid
                            select new { filetable, privilegetable, versiontable };

                if (files != null && files.ToList().Count > 0)
                {

                    foreach (var sharedfile in files)
                    {
                        if (sharedfile.filetable.islocked != true)
                        {
                            if (sharedfile.filetable.ownerid != SessionKeyMgmt.UserId)
                            {
                                FileShared share = new FileShared();
                                share.FileID = (sharedfile.filetable.fileid).ToString();
                                share.FileName = sharedfile.filetable.filename;
                                share.Description = sharedfile.versiontable.description;
                                share.FileVersion = sharedfile.versiontable.versionnumber;
                                share.CreationDate = (sharedfile.filetable.creationdate).ToString();
                                share.Owner = sharedfile.filetable.ownerid;
                                share.read = sharedfile.privilegetable.read;
                                share.delete = sharedfile.privilegetable.delete;
                                share.update = sharedfile.privilegetable.update;
                                share.check = sharedfile.privilegetable.check;
                                docs.Add(share);
                            }
                        }
                    }
                   
                }
            }
            catch
            {
                ModelState.AddModelError("", "Error while getting Shared Documents");
            }
            return docs;
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
            //this.TempData.TryGetValue("", out dept);
            string dept = TempData["dept"] as string;
            List<FileModel> model = Search(filename, getDeptDocsModel(dept));
            return View("DepartmentFiles", model);
        }
        
        [MultipleButton(Name = "action", Argument = "SearchArchivedDocs")]
        public ActionResult SearchArchivedDocs()
        {
            string filename = "";
            if (this.Request.Form.AllKeys.Length > 0)
            {
                filename = Request["fileName"];
            }
            List<FileModel> model = Search(filename,getArchivedDocsModel());
            return View("ArchivedFiles", model);
        }

        [MultipleButton(Name = "action", Argument = "SearchSharedDocs")]
        public ActionResult SearchSharedDocs()
        {
            string filename = "";
            if (this.Request.Form.AllKeys.Length > 0)
            {
                filename = Request["filename"];
            }
            List<FileShared> model = SearchShared(filename, getSharedDocsModel());
            return View("SharedFiles", model);
        }

        private List<FileShared> SearchShared(string fileTitle, List<FileShared> model)
        {
            if (ModelState.IsValid)
            {
                IDictionary<string, string> searchConditions = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(fileTitle))
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
                string fileName = GetConditionValue(searchConditions, "fileName");
                var result = (from s in model
                              where (string.IsNullOrEmpty(fileTitle) || s.FileName.IndexOf(fileTitle, StringComparison.InvariantCultureIgnoreCase) != -1)
                              select s).ToList();
                model = result;
            }
            return model;
        }

        [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
        private List<FileModel> Search(string fileTitle, List<FileModel> model)
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
                string fileName = GetConditionValue(searchConditions, "fileName");
                var result = (from s in model
                                where (string.IsNullOrEmpty(fileTitle) || s.FileName.IndexOf(fileTitle,StringComparison.InvariantCultureIgnoreCase) != -1)
                                select s).ToList();
                model = result;
            }
            return model;
        }

        private static string GetConditionValue(IDictionary<string, string> searchConditions, string key)
        {
            string tmpValue = string.Empty;

            if (searchConditions != null)
            {
                searchConditions.TryGetValue(key, out tmpValue);
            }
            return tmpValue;
        }

        [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
        private void SaveCheckInOut(string fileid)
        {
            long intID = Convert.ToInt64(fileid);
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the given fileId is valid
                    DX_FILES dx_files = db.DX_FILES.SingleOrDefault(d => d.fileid == intID);
                    if (dx_files == null)
                    {
                        throw new FileNotFoundException("File not found!");
                    }

                    // Check if the user has checkInOut privileges for the document
                    var privileges = db.DX_PRIVILEGE.SingleOrDefault(r => r.userid == SessionKeyMgmt.UserId && r.fileid == intID);
                    bool hasAccess = privileges != null ? privileges.check : false;

                    // Throw an exception if document is locked/archived/access restricted
                    if (dx_files.isarchived == true || !hasAccess)
                    {
                        ModelState.AddModelError("Permission Denied:", "Cannot access the file. File not found or access denied!");
                    }

                    //check if file is locked.
                    if (dx_files.islocked == true)
                    {
                        var lockedBy = dx_files.lockedby;

                        //if file locked by current user, unlock it
                        if (lockedBy == SessionKeyMgmt.UserId)
                        {
                            dx_files.islocked = false;
                            dx_files.lockedby = null;
                        }
                        else
                        {
                            ModelState.AddModelError("Permission Denied:", "The file is locked by user " + lockedBy + ".");
                        }
                    }
                    else
                    {
                        dx_files.islocked = true;
                        dx_files.lockedby = SessionKeyMgmt.UserId;
                    }
                }catch(Exception){
                    ModelState.AddModelError("", "Failed to lock the file: ");
                }
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("","Failed to update the database with new values");
                }
            }
        }

        [Authorize(Roles = "employee,manager,ceo,vp")]
        [AcceptVerbs(HttpVerbs.Post)]
        [ImportFromTempData]
        public ActionResult CheckInOut(string fileid)
        {
            SaveCheckInOut(fileid);
            return RedirectToAction("ListDocuments");
        }

        [Authorize(Roles = "employee,manager,ceo,vp,guest")]
        [AcceptVerbs(HttpVerbs.Post)]
        [ImportFromTempData]
        public ActionResult CheckInOutShared(string fileid)
        {
            SaveCheckInOut(fileid);
            return RedirectToAction("SharedFiles");
        }

        [Authorize(Roles = "manager,ceo,vp")]
        [AcceptVerbs(HttpVerbs.Post)]
        [ImportFromTempData]
        public ActionResult CheckInOutDept(string fileid)
        {
            SaveCheckInOut(fileid);
            return RedirectToAction("DepartmentFiles", new { dept = TempData["dept"] as string });
        }

        // GET: /Documents/MyDocDetails/5
        [Authorize(Roles = "employee, manager, ceo, vp")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        public ActionResult MyDocDetails(long fileId)
        {
            return Details(fileId, "ListDocuments");
        }
        
        // GET: /Documents/SharedDocDetails/
        [Authorize(Roles = "employee, manager, ceo, vp, guest")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        public ActionResult SharedDocDetails(long fileId)
        {
            return Details(fileId, "SharedFiles");
        }

        // GET: /Documents/DeptDocDetails/5
        [Authorize(Roles = "manager, ceo, vp")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        public ActionResult DeptDocDetails(long fileId)
        {
            return Details(fileId, "DepartmentFiles");
        }

        //
        // GET: /Documents/Details/5
        //[Authorize(Roles = "employee,manager,ceo,vp")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        private ActionResult Details(long fileId, string callerName)
        {
            try
            {
                // Check if the given fileId is valid
                DX_FILES dx_files = db.DX_FILES.SingleOrDefault(d => d.fileid == fileId);
                if (dx_files == null)
                {
                    throw new FileNotFoundException("File not found!");
                }

                // Get the current user 
                string currentUserId = SessionKeyMgmt.UserId;

                // Check if the user has read privileges for the document
                var privileges = db.DX_PRIVILEGE.SingleOrDefault(r => r.userid == currentUserId && r.fileid == fileId);
                bool hasReadAccess = privileges != null ? privileges.read : false;

                if (hasReadAccess == false)
                    throw new AccessViolationException("Insufficient privileges to access the document");
                else if (dx_files.isarchived)
                    throw new AccessViolationException("Document is currently archived and cannot be downloaded.");

                // Construct an array of boolean values indicating if every version 
                // is encrypted/decrypted
                var fileVersions = from fileVersionsTable in db.DX_FILEVERSION
                                   where fileVersionsTable.fileid == fileId
                                   select fileVersionsTable;

                if (fileVersions == null)
                    throw new ArgumentNullException();

                List<bool> cryptoStatus = new List<bool>();
                foreach (DX_FILEVERSION version in fileVersions)
                {
                    bool isThisVersionEncrypted = false;
                    if (version.isencrypted.HasValue)
                        isThisVersionEncrypted = (bool)version.isencrypted;
                    cryptoStatus.Add(isThisVersionEncrypted);
                }

                if (cryptoStatus.Count == 0)
                {
                    throw new InvalidDataException();
                }
                
                // Pass the fileId
                ViewBag.originalCaller = callerName;
                ViewBag.fileId = fileId;
                ViewBag.fileName = dx_files.filename;
                ViewBag.cryptoStatus = cryptoStatus.ToArray();

                // Pass the filename, list of encrytion values, fileid
                return View("Details");
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException ||
                    ex is AccessViolationException)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Error reading the document information");
                }

                // In case of error, return the view where the download was requested from
                switch (callerName)
                {
                    case "ListDocuments": return RedirectToAction("ListDocuments");
                    case "SharedFiles": return RedirectToAction("SharedFiles");
                    case "DepartmentFiles": return RedirectToAction("DepartmentFiles", new { dept = TempData["dept"] as string });
                    default: return RedirectToAction("ListDocuments");
                }
            }
        }

        [HttpPost]
       // [MultipleButton(Name = "action", Argument = "Details")]
        [Authorize(Roles = "employee,manager,ceo,vp, guest")]
        [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
        public ActionResult Download()
        {
            string originalView = "ListDocuments";
            try
            {
                // Get the parameters from request
                long fileId = long.Parse(Request.Params.Get("fileId"));
                string encryptionStatus = Request.Params.Get("selectdrop");
                int index = encryptionStatus.IndexOf('_');
                string isEncryptedStr = encryptionStatus.Substring(0, index);
                int versionNumber = int.Parse(encryptionStatus.Substring(index + 1))+1;

                bool isEncrypted = isEncryptedStr.Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false;

                originalView = Request.Params.Get("originalCaller");

                // Check if the given fileId is valid
                DX_FILES dx_files = db.DX_FILES.SingleOrDefault(d => d.fileid == fileId);
                if (dx_files == null)
                    throw new FileNotFoundException("File not found!");

                // Get the current user 
                string currentUserId = SessionKeyMgmt.UserId;

                // Check if the user has read privileges for the document
                DX_PRIVILEGE privileges = db.DX_PRIVILEGE.SingleOrDefault(r => r.userid == currentUserId && r.fileid == fileId);
                bool hasReadAccess = privileges != null ? privileges.read : false;

                // Throw an exception if document is locked/archived/access restricted
                if (hasReadAccess == false)
                    throw new AccessViolationException("Insufficient privileges to access the document");
                else if (dx_files.isarchived)
                    throw new AccessViolationException("Document is currently archived and cannot be downloaded.");

                // Get the file data
                DX_FILEVERSION selectedVersion = db.DX_FILEVERSION.SingleOrDefault(d => d.fileid == fileId && d.versionnumber == versionNumber);
                byte[] fileData = (selectedVersion != null) ? selectedVersion.filedata : null;

                if (isEncrypted)
                {
                    // Read the encrytion key
                    if (Request.Files[0].InputStream.Length != 0)
                    {
                        try
                        {
                            HttpPostedFileBase keyFile = Request.Files[0];
                            System.IO.Stream keyStream = keyFile.InputStream;
                            byte[] keyData = new byte[keyStream.Length];
                            keyStream.Read(keyData, 0, (int)keyStream.Length);

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

                            ICryptoTransform Decryptor = Crypto.CreateDecryptor(Crypto.Key, Crypto.IV);
                            byte[] originalFile = Decryptor.TransformFinalBlock(fileData, 0, fileData.Length);

                            // Copy the encrypted data to the file data buffer
                            Array.Clear(fileData, 0, fileData.Length);
                            Array.Resize(ref fileData, originalFile.Length);
                            Array.Copy(originalFile, fileData, originalFile.Length);
                        }
                        catch (Exception)
                        {
                            throw new ArgumentException("Error decrypting the document to be downloaded");
                        }
                    }
                    else
                    {
                        throw new FileNotFoundException("Invalid key file given. Please try again!");
                    }
                }
                
                FileContentResult fileRequested = new FileContentResult(fileData, "application/octet-stream");
                fileRequested.FileDownloadName = dx_files.filename;
                return fileRequested;
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is AccessViolationException ||
                    ex is ArgumentException)
                {
                    // Throw a file not found error
                    ModelState.AddModelError("", ex.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Error downloading the selected document");
                }

                // On error, return to the page where download was requested from
                switch (originalView)
                {
                    case "ListDocuments": return RedirectToAction("ListDocuments");
                    case "SharedFiles": return RedirectToAction("SharedFiles");
                    case "DepartmentFiles": return RedirectToAction("DepartmentFiles", new { dept = TempData["dept"] as string });
                    default: return RedirectToAction("ListDocuments");
                }
            }
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
            long newFileId = -1;
            try
            {
                if (Request.Files[0].InputStream.Length != 0)
                {
                    if (Request.Files[0].InputStream.Length < MAX_FILE_SIZE)
                    {
                        HttpPostedFileBase file = Request.Files[0];
                        System.IO.Stream stream = file.InputStream;
                        byte[] fileData = new byte[stream.Length];
                        stream.Read(fileData, 0, fileData.Length);

                        string userid = SessionKeyMgmt.UserId;

                        //Setting properties of the file object

                        string description = Request.Params.Get("description");
                        if (description.Length != 0 || description.Length > 75)
                        {
                            dx_files.ownerid = userid;
                            dx_files.isarchived = false;
                            dx_files.islocked = false;

                            // Get the filename and its extension
                            string filetype = System.IO.Path.GetExtension(file.FileName);
                            string filename = System.IO.Path.GetFileName(file.FileName);
                            
                            dx_files.type = filetype;
                            dx_files.filename = filename;

                            if (supportedFileTypes.Contains(filetype))
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

                                    DX_USER user = db.DX_USER.Single(d => d.userid == userid);
                                    string accesslevel = user.accesslevel;

                                    if(accesslevel !="employee" && accesslevel!="manager" && accesslevel!="vp" && accesslevel!="ceo")
                                    {
                                        ModelState.AddModelError("", "You are not authorized to upload a file");
                                        return View();
                                    }

                                    //Based on the role, the file should be shared with managers
                                    // Create a new file version object
                                    DX_FILEVERSION fileversion = new DX_FILEVERSION();
                                    fileversion.isencrypted = false;

                                    // Encrypt the file data if requested
                                    string encrypted = Request.Params.Get("encrypted");
                                    if (encrypted == "true")
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

                                    var allFiles = from fileversions in db.DX_FILEVERSION
                                                   select fileversions;
                                    double totalSize;
                                    if (allFiles.Count() != 0)
                                    {
                                        totalSize = allFiles.Sum(w => w.size);
                                    }
                                    else
                                        totalSize=0;
                                    totalSize /= (1024 * 1024);

                                    long maxSize = long.Parse(System.Configuration.ConfigurationManager.AppSettings["filestreamMaxSize"]);

                                    if ((totalSize + (fileData.Length / (1024 * 1024)) > maxSize))
                                    {
                                        ModelState.AddModelError("", "Disk space exceeded. Please contact admin");
                                        return View();
                                    }

                                    // Save changes for the DX_FILES object so the new fileid is
                                    // auto generated.
                                    db.DX_FILES.AddObject(dx_files);
                                    db.SaveChanges();

                                    newFileId = dx_files.fileid;

                                    fileversion.versionnumber = (int)dx_files.latestversion;
                                    fileversion.updatedate = System.DateTime.Now;
                                    fileversion.description = description;
                                    fileversion.updatedby = userid;

                                    // Add information about the file version to database
                                    fileversion.filedata = fileData;
                                    fileversion.size = fileData.Length;

                                    fileversion.fileid = dx_files.fileid;
                                    fileversion.versionid = Guid.NewGuid();

                                    db.DX_FILEVERSION.AddObject(fileversion);

                                    //Share with the owner
                                    DX_PRIVILEGE empPriv = new DX_PRIVILEGE();
                                    
                                    empPriv.userid = userid;
                                    empPriv.read = true;
                                    empPriv.update = true;
                                    empPriv.reason = "owner";
                                    empPriv.check = true;
                                    empPriv.delete = true;

                                    empPriv.fileid = dx_files.fileid;
                                    db.DX_PRIVILEGE.AddObject(empPriv);

                                    if (accesslevel == "employee")
                                    {
                                        //Getting the dept id of employee
                                        DX_USERDEPT userdept = db.DX_USERDEPT.Single(d => d.userid == userid);
                                        int deptid = userdept.deptid;

                                        //Getting the user id of manager
                                        var managers = from usersTable in db.DX_USER
                                                       join userdepts in db.DX_USERDEPT on usersTable.userid equals userdepts.userid
                                                       where usersTable.accesslevel == "manager" && userdepts.deptid == deptid
                                                       select usersTable;
                                        if (managers.Count() != 0)
                                        {
                                            DX_PRIVILEGE mgrPriv = new DX_PRIVILEGE();
                                            foreach (DX_USER managerUser in managers)
                                            {
                                                //Providing manager the respective rights
                                                string managerId = managerUser.userid;

                                                mgrPriv.userid = managerId;
                                                mgrPriv.read = true;
                                                mgrPriv.check = true;
                                                mgrPriv.update = true;
                                                mgrPriv.reason = "inherit";
                                                mgrPriv.delete = true;
                                                mgrPriv.fileid = dx_files.fileid;
                                                db.DX_PRIVILEGE.AddObject(mgrPriv);
                                            }
                                        }


                                    }
                                    if (accesslevel == "manager" || accesslevel == "employee")
                                    {
                                        //Getting the dept id of employee
                                        DX_USERDEPT userdept = db.DX_USERDEPT.Single(d => d.userid == userid);
                                        int deptid = userdept.deptid;

                                        var vp = from usersTable in db.DX_USER
                                                 join userdepts in db.DX_USERDEPT on usersTable.userid equals userdepts.userid
                                                 where usersTable.accesslevel == "vp" && userdepts.deptid == deptid
                                                 select usersTable;
                                        if (vp.Count() != 0)
                                        {
                                            foreach (DX_USER vpUser in vp)
                                            {
                                                DX_PRIVILEGE vpPriv = new DX_PRIVILEGE();
                                                string vpId = vpUser.userid;

                                                vpPriv.userid = vpId;
                                                vpPriv.read = true;
                                                vpPriv.check = true;
                                                vpPriv.update = true;
                                                vpPriv.reason = "inherit";
                                                vpPriv.delete = true;
                                                vpPriv.fileid = dx_files.fileid;
                                                db.DX_PRIVILEGE.AddObject(vpPriv);
                                            }
                                        }

                                    }
                                    if (accesslevel == "vp" || accesslevel == "manager" || accesslevel == "employee")
                                    {
                                        var ceo = from usersTable in db.DX_USER
                                                  where usersTable.accesslevel == "ceo"
                                                  select usersTable;
                                        if (ceo.Count() != 0)
                                        {
                                            foreach (DX_USER ceoUser in ceo)
                                            {
                                                DX_PRIVILEGE ceoPriv = new DX_PRIVILEGE();
                                                string ceoId = ceoUser.userid;

                                                ceoPriv.userid = ceoId;
                                                ceoPriv.read = true;
                                                ceoPriv.check = true;
                                                ceoPriv.update = true;
                                                ceoPriv.reason = "inherit";
                                                ceoPriv.delete = true;
                                                ceoPriv.fileid = dx_files.fileid;
                                                db.DX_PRIVILEGE.AddObject(ceoPriv);
                                            }
                                        }

                                    }

                                    db.SaveChanges();

                                    // Show the document list
                                    return RedirectToAction("ListDocuments");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("","Invalid file type. Accepted file types are PDF, Word, Excel, PowerPoint, Text and Image Files");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("","Please enter a valid description");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "File size exceeded 5 MB Limit");
                        return View();
                    }
                }                
                else
                {
                    ModelState.AddModelError("","Please select the file to be uploaded");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("","Error uploading the document ");
                // Check if a document information has been uploaded to DX_FILES
                // and delete it
                if (newFileId != -1)
                {
                    db.DX_FILES.DeleteObject(dx_files);
                    db.SaveChanges();
                }
            }
            return View();
            
        }

        // GET: /Documents/MyDocDetails/5
        [Authorize(Roles = "employee, manager, ceo, vp")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        public ActionResult MyDocEdit(long fileId)
        {
            return EditDocumentDesc(fileId, "ListDocuments");
        }

        // GET: /Documents/SharedDocDetails/
        [Authorize(Roles = "employee, manager, ceo, vp, guest")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        public ActionResult SharedDocEdit(long fileId)
        {
            return EditDocumentDesc(fileId, "SharedFiles");
        }

        // GET: /Documents/DeptDocDetails/5
        [Authorize(Roles = "manager, ceo, vp")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        public ActionResult DeptDocEdit(long fileId)
        {
            return EditDocumentDesc(fileId, "DepartmentFiles");
        }

        //
        // GET: /Documents/Edit/5

        [Authorize(Roles = "employee,manager,ceo,vp,guest")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        public ActionResult EditDocumentDesc(long id, string originalCaller)
        {
            try
            {
                DX_FILES fileObj = db.DX_FILES.Single(d => d.fileid == id);
                if (fileObj == null)
                {
                    throw new FileNotFoundException();
                }

                string userid = SessionKeyMgmt.UserId;

                var privileges = from priv in db.DX_PRIVILEGE
                                 where priv.userid == userid && priv.fileid == fileObj.fileid
                                 select priv;

                if (privileges.Count() == 0)
                {
                    throw new AccessViolationException("You do not have privileges to edit this file");
                }

                if (privileges.First().update == false)
                {
                    throw new AccessViolationException("Insufficient privileges to access the file or file might be archived");
                }

                if ((fileObj.islocked == true) || (fileObj.isarchived==true))
                {
                    throw new AccessViolationException("Insufficient privileges to access the file or file might be archived or locked");
                }

                DX_FILEVERSION fileVer = db.DX_FILEVERSION.Single(d => d.fileid == id && d.versionnumber == fileObj.latestversion);
                ViewData["originalCaller"] = originalCaller;
                ViewData["fileId"] = fileObj.fileid;
                
                return View("Edit");
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException)
                {
                    ModelState.AddModelError("", "The requested file is not found");
                }
                else if (ex is AccessViolationException)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Error reading the document information");
                }
                switch (originalCaller)
                {
                    case "ListDocuments": return RedirectToAction("ListDocuments");
                    case "SharedFiles": return RedirectToAction("SharedFiles");
                    case "DepartMentFiles": return RedirectToAction("DepartmentFiles");
                    default: return RedirectToAction("ListDocuments");
                }
            }
        }

        //
        // POST: /Documents/Edit/5

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
        [Authorize(Roles = "employee,manager,ceo,vp,guest")]
        public ActionResult EditDescription()
        {
            string originalCaller = Request.Params.Get("originalCaller");
            try
            {
                string userId = SessionKeyMgmt.UserId;

                long fileId = long.Parse(Request.Params.Get("fileId"));
                

                string description = Request.Params.Get("description");
                
                DX_FILES mainFile = db.DX_FILES.Single(d => d.fileid == fileId);

                    if (mainFile == null)
                    {
                        ModelState.AddModelError("", "The file does not exist anymore");
                    }

                    DX_PRIVILEGE userPriv = db.DX_PRIVILEGE.Single(d => d.fileid == fileId && d.userid == userId);

                    if (userPriv == null)
                    {
                        ModelState.AddModelError("", "You do not have sufficient privileges to edit this file");
                    }

                    if (userPriv.update == false)
                    {
                        ModelState.AddModelError("", "You do not have sufficient privileges to edit this file");
                    }

                    DX_FILEVERSION fileObj = db.DX_FILEVERSION.Single(d => d.fileid == fileId && d.versionnumber == mainFile.latestversion);

                    if (fileObj == null)
                    {
                        ModelState.AddModelError("", "The file does not exist anymore");

                    }

                    if (description.Length != 0 && description.Length <= 75)
                    {
                    
                        if (fileObj.description != description)
                        {
                            fileObj.updatedate = System.DateTime.Now;
                            fileObj.description = description;
                            fileObj.updatedby = SessionKeyMgmt.UserId;
                            db.ObjectStateManager.ChangeObjectState(fileObj, EntityState.Modified);
                            db.SaveChanges();
                            
                        }
                        else
                        {
                            ModelState.AddModelError("", "File description is same as earlier and hence not updated");
                            
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid file description");
                        
                    }
               
            }
            catch (Exception)
            {
                
            }
            switch (originalCaller)
            {
                case "ListDocuments": return RedirectToAction("ListDocuments");
                case "SharedFiles": return RedirectToAction("SharedFiles");
                case "DepartmentFiles": return RedirectToAction("DepartmentFiles");
                default: return RedirectToAction("SharedFiles");
            }
        }

        //
        // GET: /Documents/Delete/5
        [Authorize(Roles = "employee,manager,ceo,vp")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        public ActionResult DeleteMyDocument(long fileId)
        {
            return DeleteDocumentDetails(fileId, "ListDocuments");
        }

        [Authorize(Roles = "manager,ceo,vp")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        public ActionResult DeleteDepartmentDocument(long fileId)
        {
            return DeleteDocumentDetails(fileId, "DepartmentFiles");
        }

        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        private ActionResult DeleteDocumentDetails(long fileId, string callerName)
        {
            try
            {
                // Check if the given fileId is valid
                DX_FILES dx_files = db.DX_FILES.SingleOrDefault(d => d.fileid == fileId);
                if (dx_files == null)
                    throw new FileNotFoundException("File not found!");

                // Get the current user
                string userId = SessionKeyMgmt.UserId;

                // Check if user has update privileges for this document
                DX_PRIVILEGE documentPrivilege = db.DX_PRIVILEGE.SingleOrDefault(d => d.fileid == fileId && d.userid == userId);
                bool hasDeletePrivileges = documentPrivilege == null ? false : documentPrivilege.delete;

                if (hasDeletePrivileges == false)
                    throw new AccessViolationException("Document cannot be deleted. Access Denied. Please try later.");
                else if (dx_files.isarchived)
                    throw new AccessViolationException("Document is archived and cannot be deleted");

                // Check if document is checked out
                bool isFileLocked = true;
                if (dx_files.islocked.HasValue)
                    isFileLocked = (bool)dx_files.islocked;

                // Do not delete if file is locked
                if (isFileLocked)
                    throw new AccessViolationException("Document is currently checked out and cannot be deleted.");

                ViewBag.originalCaller = callerName;
                ViewBag.fileId = fileId;
                ViewBag.fileName = dx_files.filename;
                ViewBag.createdBy = dx_files.ownerid;

                return View("Delete");
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is AccessViolationException)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Error getting details about document to be updated");
                }

                switch (callerName)
                {
                    case "ListDocuments": return RedirectToAction("ListDocuments");
                    case "DepartmentFiles": return RedirectToAction("DepartmentFiles", new { dept = TempData["dept"] as string });
                    default: return RedirectToAction("ListDocuments");
                }
            }
        }

        //
        [HttpPost]
        [Authorize(Roles = "employee,manager,ceo,vp")]
        [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
        public ActionResult DeleteConfirmed()
        {
            string originalCaller = "ListDocuments";
            try
            {
                // Get the parameters from request
                long fileId = long.Parse(Request.Params.Get("fileId"));
                originalCaller = Request.Params.Get("originalCaller");

                // Basics validations and permission checking
                // Check if the fileId to be updated is still valid
                DX_FILES dx_files = db.DX_FILES.SingleOrDefault(d => d.fileid == fileId);
                if (dx_files == null)
                    throw new FileNotFoundException("File not found!");

                // Get the current userId
                string userId = SessionKeyMgmt.UserId;

                // Check if user has update privileges for this document
                DX_PRIVILEGE documentPrivilege = db.DX_PRIVILEGE.SingleOrDefault(d => d.fileid == fileId && d.userid == userId);
                bool hasDeletePrivileges = documentPrivilege == null ? false : documentPrivilege.delete;

                if (hasDeletePrivileges == false)
                    throw new AccessViolationException("Document cannot be deleted. Access Denied. Please try later.");
                else if (dx_files.isarchived)
                    throw new AccessViolationException("Document is archived and cannot be updated");

                // Check if document is checked out by current user
                bool isFileLocked = true;
                if (dx_files.islocked.HasValue)
                    isFileLocked = (bool)dx_files.islocked;

                if (isFileLocked)
                    throw new AccessViolationException("Document is currently checked out and cannot be deleted");

                // All checks are done. Go ahead and delete the document
                db.DX_FILES.DeleteObject(dx_files);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is AccessViolationException ||
                    ex is ArgumentException)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Error deleting the document");
                }
            }

            // Irrespective of whether document was updated or error
            // return to the original view
            switch (originalCaller)
            {
                case "ListDocuments": return RedirectToAction("ListDocuments");
                case "DepartmentFiles": return RedirectToAction("DepartmentFiles", new { dept = TempData["dept"] as string });
                default: return RedirectToAction("ListDocuments");
            }
        }


        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Archive")]
        [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
        [Authorize(Roles = "employee,manager,ceo,vp")]
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
        //[AcceptVerbs(HttpVerbs.Post), ExportToTempData]
        [Authorize(Roles = "employee,manager,ceo,vp")]
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
        [Authorize(Roles = "employee,manager,ceo,vp")]
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
            var allusers = from usertabel in db.DX_USER where usertabel.userid != SessionKeyMgmt.UserId && usertabel.accesslevel != "admin" && usertabel.accesslevel != "adminless" && usertabel.accesslevel != "deactivated" select new { usertabel.userid };
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
                    foreach (DX_FILES file in shareFiles)
                    {
                        if (file.islocked != true)
                        {
                            model.Files = shareFiles.ToList();
                            SessionKeyMgmt.SharedFiles = model.Files;
                        }
                        else
                        {
                            ModelState.AddModelError("", "One or more file is checked out please check-In so as to do sharing");
                            return RedirectToAction("ListDocuments");
                        }
                    }
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
        [Authorize(Roles = "employee,manager,ceo,vp")]
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
                                    if (existingfile.reason != "inherit" && existingfile.reason != "owner")
                                    {
                                        existingfile.read = true;
                                        existingfile.delete = false;
                                        existingfile.update = files.update;
                                        existingfile.check = files.check;
                                    }
                                }
                            }
                            else
                            {
                                DX_PRIVILEGE sharedfile = new DX_PRIVILEGE();
                                sharedfile.fileid = fileId;
                                sharedfile.userid = user;
                                sharedfile.read = true;
                                sharedfile.delete = false;
                                sharedfile.update = files.update;
                                sharedfile.check = files.check;
                                sharedfile.reason = "shared";
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

        [ImportFromTempData]
        [Authorize(Roles = "employee,manager,ceo,vp,guest")]
        public ActionResult SharedFiles()
        {
            var files = from privilegetable in db.DX_PRIVILEGE
                        join filetable in db.DX_FILES
                        on new { key1 = privilegetable.userid, key2 = privilegetable.fileid,key3=false }
                            equals new { key1 = SessionKeyMgmt.UserId, key2 = filetable.fileid, key3=filetable.isarchived }
                        join versiontable in db.DX_FILEVERSION on filetable.fileid equals versiontable.fileid
                        select new { filetable, privilegetable, versiontable };

            List<FileShared> docs = new List<FileShared>();
            
            if (files != null && files.ToList().Count > 0)
            {
                foreach(var sharedfile in files)
                {
                    if (sharedfile.filetable.isarchived == true)
                    {
                        ModelState.AddModelError("", "Permission Denied: The file is in archived state");
                    }
                    if (sharedfile.filetable.ownerid != SessionKeyMgmt.UserId && sharedfile.filetable.latestversion==sharedfile.versiontable.versionnumber && sharedfile.privilegetable.reason=="shared")
                    {
                        FileShared share = new FileShared();
                        share.FileID = (sharedfile.filetable.fileid).ToString();
                        share.FileName = sharedfile.filetable.filename;
                        share.Description = sharedfile.versiontable.description;
                        share.FileVersion = sharedfile.filetable.latestversion;
                        share.CreationDate = (sharedfile.filetable.creationdate).ToString();
                        share.Owner = sharedfile.filetable.ownerid;
                        share.read = sharedfile.privilegetable.read;
                        share.delete = sharedfile.privilegetable.delete;
                        share.update = sharedfile.privilegetable.update;
                        share.check = sharedfile.privilegetable.check;
                        share.islocked = Convert.ToBoolean(sharedfile.filetable.islocked);
                        share.lockedby = sharedfile.filetable.lockedby;
                        docs.Add(share);
                     }
                }
                return View("SharedFiles",docs);
            }
            else
            {
                ModelState.AddModelError("", "No files have been shared with you");
                return View("SharedFiles",docs);
            }
            
        }

        [Authorize(Roles = "employee, manager, ceo, vp")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        public ActionResult UpdateMyDocs(long fileId)
        {
            return UpdateDocumentDetails(fileId, "ListDocuments");
        }

        [Authorize(Roles = "employee, manager, ceo, vp, guest")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        public ActionResult UpdateSharedDocs(long fileId)
        {
            return UpdateDocumentDetails(fileId, "SharedFiles");
        }

        [Authorize(Roles = "manager, ceo, vp")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        public ActionResult UpdateDepartmentDocs(long fileId)
        {
            return UpdateDocumentDetails(fileId, "DepartmentFiles");
        }

        //[Authorize(Roles = "employee, manager, ceo, vp")]
        [AcceptVerbs(HttpVerbs.Get), ExportToTempData]
        private ActionResult UpdateDocumentDetails(long fileId, string calledFrom)
        {
            try
            {
                // Check if the given fileId is valid
                DX_FILES dx_files = db.DX_FILES.SingleOrDefault(d => d.fileid == fileId);
                if (dx_files == null)
                {
                    throw new FileNotFoundException("File not found!");
                }

                // Get the current user
                string userId = SessionKeyMgmt.UserId;

                // Check if user has update privileges for this document
                DX_PRIVILEGE documentPrivilege = db.DX_PRIVILEGE.SingleOrDefault(d => d.fileid == fileId && d.userid == userId);
                bool hasUpdatePrivileges = documentPrivilege == null ? false : documentPrivilege.check;

                if (hasUpdatePrivileges == false)
                    throw new AccessViolationException("Document cannot be updated. Access Denied. Please try later.");
                else if (dx_files.isarchived)
                    throw new AccessViolationException("Document is archived and cannot be updated");

                // Check if document is checked out by current user
                bool isFileLocked = false;
                if (dx_files.islocked.HasValue)
                    isFileLocked = (bool)dx_files.islocked;

                if (isFileLocked == false)
                    throw new AccessViolationException("Document cannot be updated. Please check out the file before updating.");

                bool isDocumentCheckedOutByUser = isFileLocked && (dx_files.lockedby == userId);

                // Check if document is not archived
                if (isDocumentCheckedOutByUser == false)
                    throw new AccessViolationException("Document cannot be updated now. Some one else has currently checked out the document. Please try later.");

                // Send the file name
                ViewBag.originalCaller = calledFrom;
                ViewBag.fileId = fileId;
                ViewBag.fileName = dx_files.filename;

                return View("Update");
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is AccessViolationException)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Error getting details about document to be updated");
                }

                switch (calledFrom)
                {
                    case "ListDocuments": return RedirectToAction("ListDocuments");
                    case "SharedFiles": return RedirectToAction("SharedFiles");
                    case "DepartmentFiles": return RedirectToAction("DepartmentFiles", new { dept = TempData["dept"] as string });
                    default: return RedirectToAction("ListDocuments");
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "employee, manager, ceo, vp, guest")]
        [AcceptVerbs(HttpVerbs.Post), ExportToTempData]
        public ActionResult Update()
        {
            string originalCaller = "ListDocuments";
            try
            {
                // Get the parameters from request
                long fileId = long.Parse(Request.Params.Get("fileId"));
                string encryptionStatus = Request.Params.Get("encryptionStatus");
                bool isEncrypted = encryptionStatus == "true" ? true : false;
                originalCaller = Request.Params.Get("originalCaller");

                // Basics validations and permission checking
                // Check if the fileId to be updated is still valid
                DX_FILES dx_files = db.DX_FILES.SingleOrDefault(d => d.fileid == fileId);
                if (dx_files == null)
                    throw new FileNotFoundException("File not found!");

                // Get the current userId
                string userId = SessionKeyMgmt.UserId;

                // Check if user has update privileges for this document
                DX_PRIVILEGE documentPrivilege = db.DX_PRIVILEGE.SingleOrDefault(d => d.fileid == fileId && d.userid == userId);
                bool hasUpdatePrivileges = documentPrivilege == null ? false : documentPrivilege.check;

                if (hasUpdatePrivileges == false)
                    throw new AccessViolationException("Document cannot be updated. Access Denied. Please try later.");
                else if (dx_files.isarchived)
                    throw new AccessViolationException("Document is archived and cannot be updated");

                // Check if document is checked out by current user
                bool isFileLocked = false;
                if (dx_files.islocked.HasValue)
                    isFileLocked = (bool)dx_files.islocked;

                if (isFileLocked == false)
                    throw new AccessViolationException("Document cannot be updated. Please check out the file before updating.");

                bool isDocumentCheckedOutByUser = isFileLocked && (dx_files.lockedby == userId);

                // Check if document is not archived
                if (isDocumentCheckedOutByUser == false)
                    throw new AccessViolationException("Document cannot be updated now. Some one else has currently checked out the document. Please try later.");

                // Validate the input file length
                Int64 inputFileLength = Request.Files[0].InputStream.Length;
                if (inputFileLength <= 0)
                {
                    throw new ArgumentException("File uploaded cannot be empty. Please try again.");
                }
                else if (inputFileLength > MAX_FILE_SIZE)
                {
                    throw new ArgumentException("File uploaded cannot be exceed 5 MB. Please try again.");
                }
                else
                {
                    // Check if the new file uploaded will exceed the overall file size
                    var allFiles = from fileversions in db.DX_FILEVERSION
                                   select fileversions;
                    double totalSize = 0;
                    if (allFiles.Count() > 0)
                    {
                        totalSize = allFiles.Sum(w => w.size);
                        totalSize /= (1024 * 1024);
                    }

                    if ((totalSize + (inputFileLength / (1024 * 1024)) > 1024))
                    {
                        ModelState.AddModelError("", "Disk space exceeded. Please contact admin");
                        throw new Exception();
                    }
                }

                // Get the input file and validate the file name
                HttpPostedFileBase inputFile = Request.Files[0];
                if (inputFile.FileName != dx_files.filename)
                    throw new ArgumentException("Name of file uploaded should match the existing file getting updated");

                // Validate the description given
                string fileDescription = Request.Params.Get("description");
                if (fileDescription.Length <= 0 || fileDescription.Length > 75)
                    throw new ArgumentException("Description text should be atleast 1 character and can be a maximum of 75 characters");

                // Read the given file data
                System.IO.Stream inFileStream = inputFile.InputStream;
                byte[] inputFileData = new byte[inFileStream.Length];
                inFileStream.Read(inputFileData, 0, inputFileData.Length);

                DX_FILEVERSION fileVersion = new DX_FILEVERSION();
                fileVersion.isencrypted = false;

                // Check if file needs to be encrpted before storing it
                if (isEncrypted)
                {
                    Int64 keyFileLength = Request.Files[1].InputStream.Length;
                    if (keyFileLength <= 0)
                        throw new ArgumentException("Invalid key file size. Please try again.");

                    try
                    {
                        HttpPostedFileBase keyFile = Request.Files[1];
                        System.IO.Stream keyStream = keyFile.InputStream;
                        byte[] keyData = new byte[keyStream.Length];
                        keyStream.Read(keyData, 0, (int)keyStream.Length);

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
                        byte[] cipherText = Encryptor.TransformFinalBlock(inputFileData, 0, inputFileData.Length);

                        // Copy the encrypted data to the file data buffer
                        Array.Clear(inputFileData, 0, inputFileData.Length);
                        Array.Resize(ref inputFileData, cipherText.Length);
                        Array.Copy(cipherText, inputFileData, cipherText.Length);

                        fileVersion.isencrypted = true;
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException("Error encrypting the document");
                    }
                }

                int newVersionNumber = Convert.ToInt32(dx_files.latestversion) + 1;

                // Construct the fileversion object and update database
                fileVersion.fileid = fileId;
                fileVersion.versionid = Guid.NewGuid();
                fileVersion.versionnumber = newVersionNumber;
                fileVersion.updatedate = System.DateTime.Now;
                fileVersion.description = fileDescription;
                fileVersion.size = inputFileData.Length;
                fileVersion.filedata = inputFileData;
                fileVersion.updatedby = userId;

                db.DX_FILEVERSION.AddObject(fileVersion);

                // Update the version number in DX_FILES
                dx_files.latestversion++;

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is AccessViolationException ||
                    ex is ArgumentException)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                else
                {
                    ModelState.AddModelError("", "Error updating the document");
                }
            }

            // Irrespective of whether document was updated or error
            // return to the original view
            switch (originalCaller)
            {
                case "ListDocuments": return RedirectToAction("ListDocuments");
                case "SharedFiles": return RedirectToAction("SharedFiles");
                case "DepartmentFiles": return RedirectToAction("DepartmentFiles", new { dept = TempData["dept"] as string });
                default: return RedirectToAction("ListDocuments");
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }


}