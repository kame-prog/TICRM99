using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TICRM.UI.ASPNetMVC.Models
{

    public class EmailModel
    {
        public string Name { get; set; }
    }
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage = "Please enter email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter email")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter first name")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "First name is always alphabetic")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Last name is always alphabetic")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Please enter valid email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [MinLength(11, ErrorMessage = ("Phone number must be 11 digit"))]
        [MaxLength(11, ErrorMessage = ("Phone number must be 11 digit"))]
        [Required(ErrorMessage = "Please enter phone number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must be numeric")]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Please enter company name")]
        [Display(Name = "Company")]
        public string Company { get; set; }


        [Required(ErrorMessage = "Please enter password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Please confirm password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    //public class UserRoleViewModel
    //{
    //    [Required(ErrorMessage = "Please enter first name")]
    //    [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "First name is always alphabetic")]
    //    [Display(Name = "FirstName")]
    //    public string FirstName { get; set; }

    //    [Required(ErrorMessage = "Please enter last name")]
    //    [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Last name is always alphabetic")]
    //    [Display(Name = "LastName")]
    //    public string LastName { get; set; }

    //    [Required(ErrorMessage = "Please enter email")]
    //    [EmailAddress(ErrorMessage = "Please enter valid email")]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }

    //    [MinLength(11, ErrorMessage = ("Phone number must be 11 digit"))]
    //    [MaxLength(11, ErrorMessage = ("Phone number must be 11 digit"))]
    //    [Required(ErrorMessage = "Please enter phone number")]
    //    [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must be numeric")]
    //    [Display(Name = "PhoneNumber")]
    //    public string PhoneNumber { get; set; }


    //    //[Required(ErrorMessage = "Please enter company name")]
    //    [Display(Name = "Company")]
    //    public string Company { get; set; }


    //    [Required(ErrorMessage = "Please enter password")]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password { get; set; }


    //    [Required(ErrorMessage = "Please confirm password")]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm password")]
    //    [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
    //    public string ConfirmPassword { get; set; }
    //    public string RoleName { get; set; }
    //    public string Name { get; set; }
    //}
    //public class ResetPasswordViewModel
    //{
    //    [Required(ErrorMessage = "Please enter email")]
    //    [EmailAddress(ErrorMessage = "Please enter valid email")]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }

    //    [Required(ErrorMessage = "Please enter password")]
    //    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password { get; set; }

    //    [DataType(DataType.Password)]
    //    [Required(ErrorMessage = "Please confirm password")]
    //    [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
    //    public string ConfirmPassword { get; set; }

    //    public string Code { get; set; }
    //}

    //public class ForgotPasswordViewModel
    //{

    //    [Required(ErrorMessage = "Please enter email")]
    //    [EmailAddress(ErrorMessage = "Please enter valid email")]
    //    [Display(Name = "Email")]
    //    public string Email { get; set; }
    //}
}