using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
namespace docbox.Models
{

    public class ResetPasswordModel
    {


        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [RegularExpression(@"^.*(?=.{10,18})(?=.*\d)(?=.*[A-Za-z])(?=.*[@%&#]{0,}).*$", ErrorMessage = "Password doesn't meet the requirements[10-18 characters, atleast one letter, one digit, one special character from (@,%,&,#) ]")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [Required]
        [Display(Name = "Enter Characters")]
        public string Captcha { get; set; }
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
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Not valid email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Enter Characters")]
        public string Captcha { get; set; }
       

    }

    public class VerifySecrete
    {
        
        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage = "Not a valid name needs only characters and max 20 characters")]
        [Display(Name = "Answer")]
        public string Answer { get; set; }

        [Display(Name = "Enter Characters")]
        public string Captcha { get; set; }

    }
    public class EnterActivationCode{

        
        [Required]
        [Display(Name = "Activation Code")]
        public string ActivationCode { get; set; }

        [Required]
        [Display(Name = "Enter Characters")]
        public string Captcha { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Not valid email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [RegularExpression(@"^.*(?=.{10,18})(?=.*\d)(?=.*[A-Za-z])(?=.*[@%&#]{0,}).*$", ErrorMessage = "Password doesn't meet the requirements[10-18 characters, atleast one letter, one digit, one special character from (@,%,&,#) ]")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }


   

    

    
    public class RegisterModel
    {

       public RegisterModel()
        {
            Department = new List<int>();
        }
        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage="Not a valid name")]
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
        [RegularExpression(@"^(\d{10})$",ErrorMessage="Not a correct phone number!")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }


        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [RegularExpression(@"^.*(?=.{10,18})(?=.*\d)(?=.*[A-Za-z])(?=.*[@%&#]{0,}).*$", ErrorMessage = "Password doesn't meet the requirements[10-18 characters, atleast one letter, one digit, one special character from (@,%,&,#) ]")]
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
        [Display(Name = "Department")]
        public ICollection<int> Department { get; set; }

        [Required]
        [Display(Name = "Enter Characters")]
        public string Captcha { get; set; }

        [Required]
        [Display(Name = "Secret Question")]
        public int Squestion { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{1,20}$", ErrorMessage = "Not a valid name needs only characters and max 20 characters")]
        [Display(Name = "Answer")]
        public string Answer { get; set; }

    }
}
