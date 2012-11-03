using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
namespace docbox.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ForgetPasswordModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage = "Not a valid name")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage = "Not a valid name")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$", ErrorMessage = "Not valid email address (Criteria is minimum length 10, atleast one special character from !@#$% and atleast one digit)")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Not a correct phone number!")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage = "Not a valid name")]
        [Display(Name = "Position")]
        public string Position { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage = "Not a valid name")]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "Enter Characters")]
        public string Captcha { get; set; }


        [Required]
        [Display(Name = "Secret Question")]
        public string Squestion { get; set;}

        [Required]
        [Display(Name = "Answer")]
        public string Answer { get; set; }

        [Required]
        [Display(Name = "Notification Method")]
        public bool NotificationChoice { get; set; }

    }

    public class generateActivationCode{
    
    }

    public class LogOnModel
    {
        [Required]
        [RegularExpression(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$", ErrorMessage = "Not valid email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage="Not a valid name")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage = "Not a valid name")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$", ErrorMessage="Not valid email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(\d{10})$",ErrorMessage="Not a correct phone number!")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }


        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^.*(?=.{6,18})(?=.*\d)(?=.*[A-Za-z])(?=.*[@%&#]{0,}).*$", ErrorMessage = "Password doesn't meet the requirements")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage = "Not a valid name")]
        [Display(Name = "Position")]
        public string Position { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage = "Not a valid name")]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "Enter Characters")]
        public string Captcha { get; set; }

        [Required]
        [Display(Name = "Secret Question")]
        public string Squestion { get; set; }

        [Required]
        [Display(Name = "Answer")]
        public string Answer { get; set; }

    }
}
