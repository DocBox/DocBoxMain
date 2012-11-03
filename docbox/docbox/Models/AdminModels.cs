using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace docbox.Models
{
    public class UserNeedingApproval
    {
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }

        [Display(Name = "Department")]
        public List<string> Department { get; set; }

    }

    public class TemporaryUsers
    {
        List<UserNeedingApproval> AllTemporaryUsers;
    }
}