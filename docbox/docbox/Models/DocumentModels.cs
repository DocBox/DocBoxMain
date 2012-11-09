using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using docbox.Utilities;

namespace docbox.Models
{
    public class ShareDocuments
    {
        public ShareDocuments()
        { }
        [Required]
        [Display(Name = "Select Users")]
        public ICollection<string> shareWithUsers { get; set; }

        [Required]
        [Display(Name = "Files to share")]
        public List<DX_FILES> Files { get; set; }

        [Display(Name = "Read Access")]
        public bool read { get; set; }
        [Display(Name = "Delete Access")]
        public bool delete { get; set; }
        [Display(Name = "Update Access")]
        public bool update { get; set; }
        [Display(Name = "Check in check out")]
        public bool check { get; set; }
          
    }

    public class FileShared
    {
        public FileShared()
        { }
        [Required]
        [Display(Name="File Id")]
        public string FileID { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "File Version")]
        public long FileVersion { get; set; }

        [Display(Name = "Owner")]
        public string Owner { get; set; }

        [Display(Name = "Is Locked")]
        public bool islocked { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Creation Date")]
        public string CreationDate { get; set; }

        [Display(Name = "Locked By")]
        public string lockedby { get; set; }
       
        [Display(Name = "Read Access")]
        public bool read { get; set; }
        [Display(Name = "Delete Access")]
        public bool delete { get; set; }
        [Display(Name = "Update Access")]
        public bool update { get; set; }
        [Display(Name = "Check in check out")]
        public bool check { get; set; }
        [Display(Name = "reason")]
        public string reason { get; set; }
    }
    public class SharedDocs
    {
        public List<FileShared> usershareddocs { get; set; }
    }
}