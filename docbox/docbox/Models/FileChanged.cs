using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace docbox.Models
{

    public class FileModel
    {
        public FileModel()
        {
        }

        [Display(Name = "File Id")]
        public string FileID { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "File Version")]
        public long FileVersion { get; set; }

        [Display(Name = "Owner")]
        public string Owner { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Creation Date")]
        public string CreationDate { get; set; }

        [Display(Name = "IsLocked")]
        public bool IsLocked { get; set; }

        [Display(Name = "LockedBy")]
        public string LockedBy { get; set; }

    }

    public class Files
    {
        public List<FileModel> files { get; set; }
    }

}
