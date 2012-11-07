using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace docbox.Models
{
    public class UserNeedingApproval
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime creationDate { get; set; }

        [Display(Name = "Access Level")]
        public string accessLevel { get; set; }

        
    }

    //public class TemporaryUsers
    //{
    //    List<UserNeedingApproval> AllTemporaryUsers;
    //}
    public class ExistingUsers {

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime creationDate { get; set; }

        [Display(Name = "Access Level")]
        public string accessLevel { get; set; }

    }
    public class EditUser {

        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage = "Not a valid name")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage = "Not a valid name")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Not valid email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        
        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage = "Not a valid name")]
        [Display(Name = "Position")]
        public string Position { get; set; }

        [Required]
        [Display(Name = "Department")]
        public List<int> Department { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage = "Not a valid name")]
        [Display(Name = "Access Level")]
        public string AccessLevel { get; set; }

    }

    
}