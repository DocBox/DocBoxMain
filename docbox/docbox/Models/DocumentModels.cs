using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace docbox.Models
{
    public class ShareDocuments
    {
        [Required]
        [Display(Name = "Select Users")]
        public ICollection<string> shareWithUsers { get; set; }

        [Required]
        [Display(Name = "Files to share")]
        public List<DX_FILES> Files { get; set; }

        [Display(Name = "Read Access")]
        public bool read { get; set; }
        [Display(Name = "Write Access")]
        public bool write { get; set; }
        [Display(Name = "Update Access")]
        public bool update { get; set; }
        [Display(Name = "Check in check out")]
        public bool check { get; set; }
    }
}