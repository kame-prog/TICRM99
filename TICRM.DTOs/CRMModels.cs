using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using TICRM.DTOs.Base;

namespace TICRM.DTOs
{
    public class ChangePasswordViewModel
    {
        [Required (ErrorMessage ="Please enter current password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please enter new password.")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least 8 letters, 1 uppercase, 1 lowercase, 1 special character and 1 digit.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please enter confirm password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "New password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class PaymentDto
    {
        public System.Guid ID { get; set; }
        [Required(ErrorMessage = "Please enter payment link")]
        public string PaymentLink { get; set; }
        [Required(ErrorMessage = "Please enter description")]
        public string Description { get; set; }
    }
    public class EmailIntegrationDto
    {
        public System.Guid id { get; set; }

        [Required(ErrorMessage = "Please enter sender email")]
        [EmailAddress(ErrorMessage = "Please enter valid sender email")]
        public string SenderEmail { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Please enter host")]
        public string Host { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter Port")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Port must be numeric")]
        public string Port { get; set; }
        public Nullable<System.Guid> Company { get; set; }
        public string Role { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdateBy { get; set; }
    }
    public class OrderDeviceDto
    {
        public System.Guid Order_id { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Device count must be numeric")]
        [Required(ErrorMessage ="Please enter device count")]
        public string Devices { get; set; }
        [Required(ErrorMessage = "Please enter device price")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Price must be numeric")]
        public Nullable<decimal> Price { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderBy { get; set; }
        [Required(ErrorMessage = "Please enter street 1")]
        public string Street_1 { get; set; }
        [Required(ErrorMessage = "Please enter street 2")]
        public string Street_2 { get; set; }
        [Required(ErrorMessage = "Please enter city")]
        public string City { get; set; }
        [Required(ErrorMessage = "Please enter zip code")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Please select country")]
        public string Country { get; set; }
        public Nullable<System.Guid> Company { get; set; }
        public SelectList CountryDropdown { get; set; }

    }

    public class EditUserDto
    {
        public string id { get; set; }
        [Required(ErrorMessage = "Please enter first name.")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "First name is always alphabetic.")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name.")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Last name is always alphabetic.")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        public string Email { get; set; }

        [MinLength(11, ErrorMessage = ("Phone number must be 11 digit."))]
        [MaxLength(11, ErrorMessage = ("Phone number must be 11 digit."))]
        [Required(ErrorMessage = "Please enter phone number.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must be numeric.")]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }


        //[Required(ErrorMessage = "Please enter company name")]
        [Display(Name = "Company")]
        public Guid Company { get; set; }

        [Required(ErrorMessage = "Please select country.")]
        public Guid Countryid { get; set; }
        public SelectList CountryDropdown { get; set; }

        [Required(ErrorMessage = "Please select industry.")]
        public Guid Industryid { get; set; }
        public SelectList IndustryDropdown { get; set; }

    }
    public class UserRegisterDto
    {
        public System.Guid id { get; set; }
        [Required(ErrorMessage = "Please enter first name.")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "First name is always alphabetic.")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name.")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Last name is always alphabetic.")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter email.")]
        [EmailAddress(ErrorMessage = "Please enter valid email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [MinLength(11, ErrorMessage = ("Phone number must be 11 digit."))]
        [MaxLength(11, ErrorMessage = ("Phone number must be 11 digit."))]
        [Required(ErrorMessage = "Please enter phone number.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must be numeric.")]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }


        //[Required(ErrorMessage = "Please enter company name")]
        [Display(Name = "Company")]
        public Guid Company { get; set; }

        [Required(ErrorMessage ="Please select country.")]
        public Guid Countryid { get; set; }
        public SelectList CountryDropdown { get; set; }

        [Required(ErrorMessage = "Please select industry.")]
        public Guid Industryid { get; set; }
        public SelectList IndustryDropdown { get; set; }



        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",ErrorMessage ="Password must contain at least 8 letters, 1 uppercase, 1 lowercase, 1 special character and 1 digit.")]
        [Required(ErrorMessage = "Please enter password.")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Please enter confirm password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public partial class CompanyDto
    {
        public System.Guid Company_ID { get; set; }

        [Required(ErrorMessage ="Please enter company name.")]
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }

    public class SuperAdminLoginDto
    {
        [Required(ErrorMessage = "Please enter email.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter valid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Role { get; set; }
    }
    public class LoginDto
    {
        [Required(ErrorMessage = "Please enter email.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter valid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    public class UserRoleDto
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


        //[Required(ErrorMessage = "Please enter company name")]
        [Display(Name = "Company")]
        public Guid Company { get; set; }


        [Required(ErrorMessage = "Please enter password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Please confirm password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Please select role")]
        public string RoleName { get; set; }
        public string Name { get; set; }
    }
    public class ForgotPasswordDto
    {

        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Please enter valid email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class RegenrateConfirmationDto
    {
        [Required(ErrorMessage ="Please enter email")]
        public string Email { get; set; }
    }
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Please enter valid email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
    public class AccountDto : BaseDropDownDto
    {
        public System.Guid AccountId { get; set; }
        [Required(ErrorMessage ="Please enter name")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Name is always alphabetic")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please select shipping address")]
        public Nullable<System.Guid> ShippingAddress { get; set; }
        public List<AccountDto> acc { get; set; }
        [Required(ErrorMessage = "Please select billing address")]
        public Nullable<System.Guid> BillingAddress { get; set; }

        public SelectList AddressDropdown { get; set; }
        public SelectList BillingAddressDropdown { get; set; }
        [Required(ErrorMessage = "Please select type")]
        public Nullable<System.Guid> AccountTypeId { get; set; }

        public SelectList AccountTypeDropdown { get; set; }

        
        [MinLength(11, ErrorMessage = ("Phone number must be 11 digit"))]
        [MaxLength(11,ErrorMessage =("Phone number must be 11 digit"))]
        [Required(ErrorMessage = "Please enter Phone Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must be numeric")]
        public string PhoneOffice { get; set; }
        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [MinLength(10, ErrorMessage = "Fax number must have minimum 10 digit")]
        [MaxLength(13, ErrorMessage = "Fax number must have maximum 13 digit")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Fax number must be numeric")]
        public string Fax { get; set; }

        [Url(ErrorMessage = "URL format is wrong!")]
        public string WebSite { get; set; }
        [Required(ErrorMessage = "Please select account size")]
        public Nullable<System.Guid> AccountSizeId { get; set; }
        public SelectList AccountSizeDropdown { get; set; }
        [Required(ErrorMessage = "Please select industry")]
        public Nullable<System.Guid> IndustryId { get; set; }
        public SelectList IndustryDropdown { get; set; }
        [Required(ErrorMessage = "Please enter description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please select Currency")]
        public Nullable<System.Guid> CurrencyId { get; set; }
        public SelectList CurrencyDropdown { get; set; }
        public int OppCount { get; set; }
        public int DeviceCount { get; set; }
        public int AssetCount { get; set; }
        public int LocationCount { get; set; }
        public string currency { get; set; }
        public string Company { get; set; }
        public virtual AccountSizeDto AccountSize { get; set; }
        public virtual AccountTypeDto AccountType { get; set; }
        public virtual AddressDto Address { get; set; }
        public virtual AddressDto Address1 { get; set; }
        public virtual IndustryDto Industry { get; set; }
        public virtual List<OpportunityDto>  Opportunities { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
    public class OpportunityDto : BaseDropDownDto
    {
        public System.Guid OpportunityId { get; set; }
        [Required(ErrorMessage = "Please enter opportunity amount")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Amount must be numeric")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G0}")]
        public Nullable<decimal> Amount { get; set; }
        [Required(ErrorMessage = "Please select probability")]
        public Nullable<System.Guid> ProbabilityId { get; set; }
        [Required(ErrorMessage = "Please select opportunity staged")]
        public Nullable<System.Guid> OpportunityStageId { get; set; }
        [Required(ErrorMessage = "Please enter opportunity title")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Title is always alphabetic")]
        [MaxLength(50, ErrorMessage ="Please enter below the 50 characters.")]
        public string Title { get; set; }
       
        public Nullable<System.Guid> CurrencyId { get; set; }
        [Required(ErrorMessage = "Please enter Description")]
        public string Description { get; set; }
        public SelectList CurrencyDropdown { get; set; }
        public SelectList OpportunityStageDropdown { get; set; }
        public SelectList ProbabilityDropdown { get; set; }

        public virtual CurrencyDto Currency { get; set; }

        public virtual OpportunityStageDto OpportunityStage { get; set; }
        public virtual PobabilityDto Pobability { get; set; }
        [Required(ErrorMessage = "Please enter Latitude")]
        [RegularExpression(@"^(\+|-)?((\d((\.)|\.\d{1,6})?)|(0*?[0-8]\d((\.)|\.\d{1,6})?)|(0*?90((\.)|\.0{1,6})?))$", ErrorMessage = "Latitude must be in between -90 to 90")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G8}")]
        public Nullable<decimal> Latitude { get; set; }
        [Required(ErrorMessage = "Please enter Longitude")]
        [RegularExpression(@"^(\+|-)?((\d((\.)|\.\d{1,6})?)|(0*?\d\d((\.)|\.\d{1,6})?)|(0*?1[0-7]\d((\.)|\.\d{1,6})?)|(0*?180((\.)|\.0{1,6})?))$", ErrorMessage = "Longitude must be in between -180 to 180")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G8}")]
        public Nullable<decimal> Longitude { get; set; }
        [Required(ErrorMessage = "Please select account")]
        public Nullable<System.Guid> AccountId { get; set; }
        public virtual AccountDto Account { get; set; }
        public virtual List<CaseDto> OpportunityCasesList { get; set; }

    }
    public class LeadDto : BaseDropDownDto
    {
        public System.Guid LeadId { get; set; }
        [Required(ErrorMessage ="Please enter name")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Name is always alphabetic")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please select lead type")]
        public Nullable<System.Guid> LeadTypeId { get; set; }
        [Required(ErrorMessage = "Please select lead source")]
        public Nullable<System.Guid> LeadSourceId { get; set; }
        [MinLength(7)]
        [MaxLength(15)]
        [Required(ErrorMessage = "Please enter Phone Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must be numeric")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [MaxLength(50, ErrorMessage = "Please enter the text below 50 characters.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please select address")]
        public Nullable<System.Guid> AddressId { get; set; }
        [Required(ErrorMessage = "Please select industry")]
        public Nullable<System.Guid> IndustryId { get; set; }
        [Required(ErrorMessage = "Please enter description")]
        [MaxLength(500, ErrorMessage = "Please enter the text below 500 characters.")]
        public string Description { get; set; }
        public SelectList AddressDropdown { get; set; }
        public SelectList IndustryDropdown { get; set; }
        public SelectList LeadTypeDropdown { get; set; }
        public SelectList LeadSourceDropdown { get; set; }

        public virtual AddressDto Address { get; set; }
        public virtual IndustryDto Industry { get; set; }
        public virtual LeadSourceDto LeadSource { get; set; }
        public virtual LeadTypeDto LeadType { get; set; }

    }
    //added by zaman khan for contact entity

    public class UnitCostDto : BaseDto
    {
        public System.Guid UnitCostId { get; set; }
        public Nullable<decimal> PerUnitCost { get; set; }
        public string CostUnit { get; set; }
    }

    public class ContactDto : BaseDropDownDto
    {
        public long ContactId { get; set; }
        [Required(ErrorMessage ="Please select Account")]
        public Nullable<System.Guid> AccountId { get; set; }

        [Required(ErrorMessage = "Please enter Contact Name")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Name is always alphabetic")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [MaxLength(50, ErrorMessage = "Please enter the text below 50 characters.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter Phone Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must be numeric")]
        //[DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please enter Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please enter street address 2")]
        public string StreetAddress_2 { get; set; }
        [Required(ErrorMessage = "Please enter city")]
        public string City { get; set; }
        [Required(ErrorMessage = "Please enter zip coder")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Please select country")]
        public string Country { get; set; }
        public string AccountName { get; set; }
        public virtual AccountDto Account { get; set; }
        public virtual List<CaseDto> ContactCases { get; set; }
    }

    public class CustomerAssetDto : BaseDropDownDto
    {
        public System.Guid CustomerAssetId { get; set; }
        [Required(ErrorMessage = "Please enter Customer Asset Title")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Title is always alphabetic")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please select Asset Type")]
        public Nullable<System.Guid> CustomerAssetTypeId { get; set; }
        public SelectList CustomerAssetTypeDropdown { get; set; }
        public string Manufacture { get; set; }
        public string Model { get; set; }
        public Nullable<int> YearOfManufacture { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<decimal> DepriciatedValue { get; set; }
        [Required(ErrorMessage = "Please enter SKU")]
        public string SKU { get; set; }
        [Required(ErrorMessage = "Please enter description")]
        public string Description { get; set; }
        public virtual CustomerAssetTypeDto CustomerAssetType { get; set; }
        [Required(ErrorMessage = "Please select Account")]
        public Nullable<System.Guid> AccountId { get; set; }
        [Required(ErrorMessage = "Please select Location")]
        public Nullable<System.Guid> LocationId { get; set; }

        public SelectList LocationDropdown { get; set; }


        public virtual AccountDto Account { get; set; }
        public virtual LocationDto Location { get; set; }

    }
    public class LocationDto : BaseDropDownDto
    {
        public System.Guid LocationId { get; set; }
        [Required(ErrorMessage = "Please enter the Name")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Name is always alphabetic")]


        public string Name { get; set; }
        [Required(ErrorMessage ="Please enter the Address")]
        public Nullable<System.Guid> AddressId { get; set; }
        [Required(ErrorMessage = "Please enter the Location")]

        public Nullable<System.Guid> LocationTypeId { get; set; }
        [Required(ErrorMessage = "Please enter description")]

        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter Latitude")]
        [RegularExpression(@"^(\+|-)?((\d((\.)|\.\d{1,6})?)|(0*?[0-8]\d((\.)|\.\d{1,6})?)|(0*?90((\.)|\.0{1,6})?))$", ErrorMessage = "Latitude must be in between -90 to 90")]
        public Nullable<decimal> Latitude { get; set; }
        [Required(ErrorMessage = "Please enter Longitude")]
        [RegularExpression(@"^(\+|-)?((\d((\.)|\.\d{1,6})?)|(0*?\d\d((\.)|\.\d{1,6})?)|(0*?1[0-7]\d((\.)|\.\d{1,6})?)|(0*?180((\.)|\.0{1,6})?))$", ErrorMessage = "Longitude must be in between -180 to 180")]
        public Nullable<decimal> Longitude { get; set; }

        public virtual LocationTypeDto LocationType { get; set; }
        

        public virtual AddressDto Address { get; set; }

        public virtual AccountDto Account { get; set; }
        [Required(ErrorMessage = "Please select Account")]


        public Nullable<System.Guid> AccountId { get; set; }
        public SelectList AddressDropdown { get; set; }
        public SelectList LocationTypeDropdown { get; set; }
    }

    public class AlertDto : BaseDropDownDto
    {
        public System.Guid AlertId { get; set; }
        public string Title { get; set; }
        public System.Guid UrgencyId { get; set; }
        public string Description { get; set; }
        public SelectList UrgencyDropdown { get; set; }

        public virtual UrgencyDto Urgency { get; set; }


    }

    public class FirmwareDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter firmware version.")]
        public string version { get; set; }
        [Required(ErrorMessage = "Please enter description.")]
        public string description { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        [Required(ErrorMessage ="Please enter Server.")]
        [Url(ErrorMessage = "URL format is wrong!")]
        public string server { get; set; }
        //[Required(ErrorMessage = "Please attach a file")]
        public string File { get; set; }
        public string Location { get; set; }
        [Required(ErrorMessage = "Please  select a device.")]
        public string Device { get; set; }
        public string Status { get; set; }
        public string DeviceName { get; set; }
        public SelectList DevicesDropDown { get; set; }
    }

    public class DeviceDto : BaseDropDownDto
    {
        public System.Guid DeviceId { get; set; }
        [Required(ErrorMessage = "Please enter Device Name")]
        //[RegularExpression(@"^[^\s\,]+$", ErrorMessage = "Name Cannot Have Spaces")]
        public string Name { get; set; }
        [MinLength(17, ErrorMessage = "Mac must have 17 characters")]
        [MaxLength(17, ErrorMessage = "Mac must have 17 characters")]
        [Required(ErrorMessage = "Please enter MAC Address")]
        public string Mac { get; set; }
        [MinLength(15, ErrorMessage = "EMEI must have minimum 15 characters")]
        [MaxLength(15, ErrorMessage = "EMEI must have maximum 15 characters")]
        [Required(ErrorMessage = "Please enter EMEI Number")]
        public string EMEINumber { get; set; }
        [Required(ErrorMessage = "Please select cloud service")]
        public string CloudServices { get; set; }
        public string CloudData { get; set; }
        [Required(ErrorMessage = "Please enter registration date")]
        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(CustomValidationMethods), nameof(CustomValidationMethods.RegistrationDate))]
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public Nullable<System.DateTime> ServiceDate { get; set; }
        [Required(ErrorMessage = "Please enter Latitude")]
        [RegularExpression(@"^(\+|-)?((\d((\.)|\.\d{1,6})?)|(0*?[0-8]\d((\.)|\.\d{1,6})?)|(0*?90((\.)|\.0{1,6})?))$", ErrorMessage = "Latitude must be in between -90 to 90")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G8}")]
        public Nullable<decimal> Latitude { get; set; }
        [Required(ErrorMessage = "Please enter Longitude")]
        [RegularExpression(@"^(\+|-)?((\d((\.)|\.\d{1,6})?)|(0*?\d\d((\.)|\.\d{1,6})?)|(0*?1[0-7]\d((\.)|\.\d{1,6})?)|(0*?180((\.)|\.0{1,6})?))$", ErrorMessage = "Longitude must be in between -180 to 180")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G8}")]
        public Nullable<decimal> Longitude { get; set; }
        [Required(ErrorMessage = "Please Select account")]
        public Nullable<System.Guid> AccountId { get; set; }
        [Required(ErrorMessage = "Please select customer asset")]
        public Nullable<System.Guid> CustomerAssetId { get; set; }
        [Required(ErrorMessage = "Please select maintenance type")]
        public string Maintenance { get; set; }
        public Nullable<System.DateTime> LastMessage {get;set;}
        public bool IsGateway { get; set; }
        public Nullable<System.Guid> GatewayReference { get; set; }
       
        public string Data { get; set; }
        public string MqttPublishtopic { get; set; }

        public SelectList CustomerAssetDropdown { get; set; }
        public SelectList GatewayReferenceDropdown { get; set; }
        public SelectList CloudServicesDropdown { get; set; }
        public SelectList MaintenanceDropdown { get; set; }
        public virtual ICollection<DeciveConfigurationDto> DeciveConfigurations { get; set; }
        public virtual ICollection<DeviceSensorDto> DeviceSensors { get; set; }

        public virtual AccountDto Account { get; set; }
        public virtual CustomerAssetDto CustomerAsset { get; set; }
    }
    //Custom Validation Class
    public class CustomValidationMethods
    {
        public static ValidationResult RegistrationDate(DateTime date)
        {
            return DateTime.Compare(date, DateTime.Now) > 0 ? new ValidationResult("Registration date must be less then the current date") : ValidationResult.Success;
        }
    }
    public class NotesDto
    {
        public System.Guid NoteId { get; set; }
        public string Note1 { get; set; }
        public string RelatedTo { get; set; }
        public System.Guid RelatedToId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }

    public class CaseDto : DropDownDto
    {
        public System.Guid CaseId { get; set; }
        [Required(ErrorMessage = "Please enter Case title")]
        public string CaseTitle { get; set; }
        [Required(ErrorMessage = "Please select Case type")]
        public System.Guid CaseTypeId { get; set; }
        [Required(ErrorMessage = "Please select contact type")]
        public long ContactId { get; set; }
        public string Origin { get; set; }
        [Required(ErrorMessage = "Please enter Case description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please select related to case type")]
        public string RelatedTo { get; set; }
        [Required(ErrorMessage = "Please select case status")]
        public Nullable<System.Guid> CaseStatusId { get; set; }
        public Nullable<System.Guid> ResolutionType { get; set; }
        public string Resolution { get; set; }
        public Nullable<int> TotalTime { get; set; }
        public Nullable<int> BillableTime { get; set; }
        public string Remarks { get; set; }
        [Required(ErrorMessage = "Please select case related to case account")]
        public System.Guid RelatedToId { get; set; }
        public string RelatedToName { get; set; }
        public Nullable<System.DateTime> Scheduling { get; set; }
        public string Reading { get; set; }
        public Nullable<System.Guid> SensorId { get; set; }
        public SelectList ContactsDropdown { get; set; }
        public SelectList CaseResolutionDropdown { get; set; }
        public SelectList CaseStatusDropdown { get; set; }
        public SelectList CaseTypeDropdown { get; set; }
        public SelectList RelatedToDropdown { get; set; }
        public SelectList RelatedToIdDropdown { get; set; }
        public SelectList ActivityType { get; set; }
        public SelectList ResulutionTypeDropdown { get; set; }
        public virtual CaseResolutionDto CaseResolutionDto { get; set; }
        public virtual CaseStatusDto CaseStatusDto { get; set; }
        public virtual CaseTypeDto CaseTypeDto { get; set; }
        public virtual ContactDto ContactDto { get; set; }
        public virtual List<ActivityDTO> CasesActivities { get; set; }
        public virtual List<NotesDto> CaseNotes { get; set; }
        public virtual SensorDto SensorDto { get; set; }
        public string dLat { get; set; }
        public string dLong { get; set; }
        public Nullable<bool> IsScheduled { get; set; }

    }


    public class CaseResolutionDto
    {
        public System.Guid CaseResolutionType { get; set; }
        public string Name { get; set; }
    }

    public class CaseStatusDto
    {

        public System.Guid CaseStatusId { get; set; }
        public string Name { get; set; }
    }

    public class CaseTypeDto
    {
        public System.Guid CaseTypeId { get; set; }
        public string Name { get; set; }
    }

    public class ReadingDto : BaseDropDownDto
    {
        public System.Guid ReadingId { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<System.Guid> ReadingTypeId { get; set; }
        public Nullable<System.Guid> ReadingUnitId { get; set; }
        public Nullable<decimal> MarginOfErrorInPercent { get; set; }
        public string Description { get; set; }
        public SelectList ReadingTypeDropdown { get; set; }
        public SelectList ReadingUnitDropdown { get; set; }
        public virtual ReadingTypeDto ReadingType { get; set; }
        public virtual ReadingUnitDto ReadingUnit { get; set; }
    }

    public class ReadingUnitDto
    {
        public System.Guid ReadingUnitId { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> Type { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public SelectList ReadingTypeDropdown { get; set; }
        public virtual ReadingTypeDto ReadingType { get; set; }
    }

    public class ServiceCallDto : BaseDropDownDto
    {
        public System.Guid ServiceCallId { get; set; }
        [Required(ErrorMessage ="Please enter Service call Title")]
        [MaxLength(50, ErrorMessage = "Please enter the text below 50 characters.")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "Please enter the text below 500 characters.")]
        public string Detail { get; set; }
        [Required(ErrorMessage = "Please select Urgency")]
        public Nullable<System.Guid> UrgencyId { get; set; }
        [Required(ErrorMessage = "Please select service call stage")]
        public Nullable<System.Guid> ServiceCallStageId { get; set; }
        [Required(ErrorMessage = "Please enter description")]
        [MaxLength(500, ErrorMessage = "Please enter the text below 500 characters.")]
        public string Description { get; set; }
        public SelectList UrgencyDropdown { get; set; }
        public SelectList ServiceCallStageDropdown { get; set; }
        public virtual UrgencyDto Urgency { get; set; }

        public virtual WorkStageDto WorkStage { get; set; }
    }

    public class CostDto : BaseDto
    {
        public System.Guid CostsId { get; set; }
        public string Cost1 { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.Guid> UnitCostId { get; set; }
        public Nullable<System.Guid> AccountId { get; set; }

        public virtual UnitCostDto UnitCost { get; set; }
        public virtual AccountDto Account { get; set; }
    }

    public  class ConsumptionDTO : BaseDto
    {
        public System.Guid ConsumptionID { get; set; }
        public Nullable<System.Guid> DeviceId { get; set; }
        public string Current { get; set; }
        public string Voltage { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Unit { get; set; }
        public Nullable<System.Guid> AccountId { get; set; }

        public virtual DeviceDto Device { get; set; }
        public virtual AccountDto Account { get; set; }
    }

    public class DisconnectionDto 
    {
        public System.Guid Id { get; set; }
        public Guid? DeviceId { get; set; }
        public Guid? AccountId { get; set; }
        public DateTime? Date { get; set; }

        public virtual AccountDto AccountDto { get; set; }
        public virtual DeviceDto DeviceDto { get; set; }
    }

    public class CategoryDto : BaseDropDownDto
    {
        public System.Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class ResourceDto : BaseDropDownDto
    {
        public System.Guid ResourceId { get; set; }
        [Required(ErrorMessage ="Please enter resource Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please select address")]
        public Nullable<System.Guid> Address { get; set; }
        [Required(ErrorMessage = "Please select current address")]
        public Nullable<System.Guid> CurrentAddress { get; set; }
        [MinLength(11)]
        [MaxLength(14)]
        [Required(ErrorMessage = "Please enter Phone Number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must be numeric")]
        public string PhoneHome { get; set; }
        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Url(ErrorMessage = "URL format is wrong!")]
        public string Website { get; set; }
        [MinLength(11)]
        [MaxLength(14)]
        [Required(ErrorMessage = "Please enter Phone Office")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Office must be numeric")]
        public string PhoneOffice { get; set; }
        [Required(ErrorMessage = "Please enter description")]
        public string Description { get; set; }
        public SelectList AddressDorpdown { get; set; }
        public SelectList CurrentAddressDorpdown { get; set; }
        public virtual AddressDto Address1 { get; set; }
        public virtual AddressDto Address2 { get; set; }
    }

    public class ResourceAvailabilityDto : BaseDto
    {
        public System.Guid ResourceAvailabilityId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<decimal> Hour { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string Description { get; set; }

    }
    


    public class ResourceSkillDto : BaseDto
    {
        public System.Guid ResourceSkillId { get; set; }
        public Nullable<System.Guid> SkillId { get; set; }
        public Nullable<System.Guid> SkillLevelId { get; set; }
        public string Description { get; set; }
        public virtual SkillLevelDto SkillLevel { get; set; }
        public virtual SkillDto Skill { get; set; }

    }

    public class SkillLevelDto
    {
        public System.Guid SkillLevelId { get; set; }
        public string Name { get; set; }


    }

    public class SkillDto
    {
        public System.Guid SkillId { get; set; }
        public string Name { get; set; }
    }

    public partial class WorkStageDto
    {
        public System.Guid WorkStageId { get; set; }
        public string Name { get; set; }
    }

    public class ReadingTypeDto
    {
        public System.Guid ReadingTypeId { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

    }

    public class DeviceSensorDto
    {


        public System.Guid DeviceSensorId { get; set; }
        public string SensorName { get; set; }
        public Nullable<System.Guid> DeviceId { get; set; }
        public string SensorType { get; set; }
        public Nullable<System.Guid> StatusId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public virtual DeviceDto Device { get; set; }
        public virtual StatusDto Status { get; set; }

        public virtual ICollection<SensorDataDto> SensorDatas { get; set; }
    }

    public class SensorDataDto
    {
        public long SensorDataId { get; set; }
        public Nullable<System.Guid> DeviceSensorId { get; set; }
        public Nullable<double> SensorValue { get; set; }
        public Nullable<System.DateTime> RecordDate { get; set; }

        public virtual DeviceSensorDto DeviceSensor { get; set; }
    }

    public class DeciveConfigurationDto
    {
        public System.Guid DeviceConfigurationId { get; set; }
        public Nullable<System.Guid> DeviceId { get; set; }
        public Nullable<bool> IsSleepModeEnabled { get; set; }
        public Nullable<int> SleepModeValueInSeconds { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public virtual DeviceDto Device { get; set; }
    }

    public class UrgencyDto
    {
        public System.Guid UrgencyId { get; set; }
        public string Name { get; set; }
    }

    public class LocationTypeDto
    {
        public System.Guid LocationTypeId { get; set; }
        public string Name { get; set; }
    }

    public class CustomerAssetTypeDto
    {
        public System.Guid CustomerAssetTypeId { get; set; }
        public string Name { get; set; }
    }

    public class LeadTypeDto
    {

        public System.Guid LeadTypeId { get; set; }
        public string Name { get; set; }


    }

    public class LeadSourceDto
    {
        public System.Guid LeadSourceId { get; set; }
        public string Name { get; set; }


    }

    public class AccountLeadDto
    {
        public long Id { get; set; }
        public Nullable<long> AccountId { get; set; }
        public Nullable<long> LeadId { get; set; }
    }

    public class AccountTypeDto
    {
        public System.Guid AccountTypeId { get; set; }
        public string Name { get; set; }
        public string Font_Class { get; set; }

    }

    public class AccountSizeDto
    {
        public System.Guid AccountSizeId { get; set; }
        public string Name { get; set; }
    }

    public class AddressDto
    {

        public System.Guid AddressId { get; set; }
        [Required(ErrorMessage ="Please enter street 1")]
        [MaxLength(100, ErrorMessage ="Please Enter below the 100 characters.")]
        public string Street1 { get; set; }
        [Required(ErrorMessage = "Please enter street 2")]
        [MaxLength(100, ErrorMessage = "Please Enter below the 100 characters.")]

        public string Street2 { get; set; }
        [Required(ErrorMessage = "Please enter the City name")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "City is always alphabetic")]
        public string City { get; set; }
        [Required(ErrorMessage = "Please enter the State")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "State is always alphabetic")]
        public string State { get; set; }
        [Required(ErrorMessage = "Please enter the Zip code")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Zip code  must be numeric")]
        public string Zip { get; set; }
        [Required(ErrorMessage = "Please select  the Country name")]
        public string Country { get; set; }
        public SelectList CountryDropdown { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

    }
    public class CountryDto
    {
        public System.Guid ID { get; set; }
        public string Country_Name { get; set; }
    }

    public class IndustryDto
    {
        public System.Guid IndustryId { get; set; }
        public string Name { get; set; }
    }

    public class StatusDto
    {
        public System.Guid StatusId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class TeamDto : BaseBasicDto
    {
        public System.Guid TeamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }

    }

    public class UserDto : BaseBasicDto
    {

        public System.Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<bool> IsAssigned { get; set; }
        public string Location { get; set; }
        public string AssignedItem { get; set; }
        public Nullable<System.Guid> AssignedItemId { get; set; }
        public Nullable<System.DateTime> AssignedItemTime { get; set; }

    }

    public class CurrencyDto
    {
        public System.Guid CurrencyId { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Value { get; set; }
        public string Sign { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

    }

    public class OpportunityStageDto
    {
        public System.Guid OpportunityStageId { get; set; }
        public string Name { get; set; }

    }

    public class PobabilityDto
    {
        public System.Guid ProbabilityId { get; set; }
        public string Name { get; set; }
    }

    public class DeviceSensorGraphDto
    {
        public System.Guid DeviceSensorGraphId { get; set; }
        [Required(ErrorMessage ="Please select device")]
        public Nullable<System.Guid> DeviceId { get; set; }
        [Required(ErrorMessage = "Please select sensor")]
        public Nullable<System.Guid> SensorId { get; set; }
        [Required(ErrorMessage = "Please select graph")]
        public Nullable<System.Guid> GraphId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        [Required(ErrorMessage = "Please select Channel")]
        public string Channel { get; set; }
        [Required(ErrorMessage = "Please select Network")]
        public string MqttPublishtopic { get; set; }
        [Required(ErrorMessage = "Please select Mqtt topic")]
        public string Network { get; set; }
        [Required(ErrorMessage = "Please select Data Duration")]
        public string Data { get; set; }
        public Nullable<int> TankLevel { get; set; }
        public string DeviceName { get; set; }
        public string GraphName { get; set; }
        public string SensorName { get; set; }
        public SelectList ChannelsDropDown { get; set; }
        public SelectList NetworksDropDown { get; set; }
        public SelectList DataDropDown { get; set; }
        public SelectList DevicesDropDown { get; set; }
        public SelectList SensorsDropDown { get; set; }
        public SelectList GraphsDropDown { get; set; }

    }

    public class ChannelDto
    {
        public System.Guid Channel_Id { get; set; }
        public string Channel { get; set; }
    }
    public class Data_DurationDto
    {
        public System.Guid Duration_Id { get; set; }
        public string Data { get; set; }
    }
    public class NetworkDto
    {
        public System.Guid Network_Id { get; set; }
        public string Network { get; set; }
    }

    public class GraphDto
    {
        public System.Guid GraphId { get; set; }
        public string GraphName { get; set; }
    }

    public class SensorDto
    {
        public System.Guid SensorId { get; set; }
        public string SensorName { get; set; }
    }

    public class DeviceSensorTemplateDto
    {
        public System.Guid DeviceSensorTemplateId { get; set; }
        public Nullable<System.Guid> DeviceId { get; set; }
        public Nullable<System.Guid> DeviceSensorId { get; set; }
        public Nullable<System.Guid> SensorTemplateId { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string MqttPublishtopic { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class SensorTemplateDto
    {
        public System.Guid SensorTemplateId { get; set; }
        public string SensorTemplateName { get; set; }
    }

    public class GlobalSearchDto
    {
        public System.Guid GlobalSearchId { get; set; }
        [Required(ErrorMessage ="Please enter Global Search name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter Global Search URL")]
        public string URL { get; set; }
        public string Type { get; set; }
    }

    public class SearchDataViewModel
    {
        public string Result { get; set; }
        public string FirstURL { get; set; }
        public string Text { get; set; }
        public string value { get; set; }
        public string Type { get; set; }
        public string JS_function { get; set; }
    }

    public class ActivityDTO : BaseDropDownDto

    {
        public System.Guid ActivityId { get; set; }

        [Required(ErrorMessage ="Please enter activity name")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Name is always alphabetic")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select activity type")]
        public string Type { get; set; }

        public SelectList TypeDropdown { get; set; }

        [Required(ErrorMessage = "Please select Related to")]
        public string RelatedTo { get; set; }
        public SelectList RelatedToDropdown { get; set; }

        [Required(ErrorMessage = "Please select Related to ID")]
        public Nullable<System.Guid> RelatedToID { get; set; }
        public SelectList RelatedToIDDropdown { get; set; }

        [Required(ErrorMessage = "Please enter activity description")]
        public string Description { get; set; }

        public Nullable<bool> IsDeleted { get; set; }
        public string RelatedToName { get; set; }

        //public Nullable<System.Guid> ActivityPartyId { get; set; }
        //public Nullable<System.Guid> ActivityPointerId { get; set; }

        //public virtual ICollection<EmailDTO> Emails { get; set; }
        //public virtual ICollection<MeetingDTO> Meetings { get; set; }
        //public virtual ICollection<PhoneCallDTO> PhoneCalls { get; set; }
        //public virtual ICollection<TaskDTO> Tasks { get; set; }
    }

    public class CalendarEventDTO : BaseDto
    {
        public string Description { get; set; }
        public virtual string Location { get; set; }
        public virtual DateTime OriginalStartTime { get; set; }
        public virtual DateTime Start { get; set; }
        public virtual string Summary { get; set; }
        public virtual string Kind { get; set; }
        public virtual string HangoutLink { get; set; }
        public virtual List<EventAttendee> Attendees { get; set; }
        public virtual string ColorId { get; set; }
        public virtual string CreatedRaw { get; set; }
        public virtual string HtmlLink { get; set; }
        public virtual DateTime End { get; set; }

    }

    public class EventAttendee
    {
        public virtual string Comment { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string Email { get; set; }
    }

    public partial class ActivityAccountDTO : BaseDto
    {
        public System.Guid ActivityAccountId { get; set; }
        public Nullable<System.Guid> ActivityId { get; set; }
        public Nullable<System.Guid> AccountId { get; set; }
    }

    public class EmailDTO : BaseDto
    {
        public System.Guid EmailId { get; set; }
        public Nullable<System.Guid> ActivityId { get; set; }
    }

    public class MeetingDTO : BaseDto
    {
        public System.Guid MeetingId { get; set; }
        public Nullable<System.Guid> ActivityId { get; set; }

        public virtual ActivityDTO Activity { get; set; }
    }

    public class PhoneCallDTO : BaseDto
    {
        public System.Guid PhoneCallId { get; set; }
        public Nullable<System.Guid> ActivityId { get; set; }

        public virtual ActivityDTO Activity { get; set; }
    }

    public partial class TaskDTO : BaseDto
    {
        public System.Guid TaskId { get; set; }
        public Nullable<System.Guid> ActivityId { get; set; }

        public virtual ActivityDTO Activity { get; set; }
    }
   
    public class WorkFlowNodeDTO
    {
        public System.Guid NodeDataId { get; set; }
        public string text { get; set; }
        public string key { get; set; }
        public string figure { get; set; }
        public string fill { get; set; }
        public string loc { get; set; }
    }

    public class ActivityTemplateDTO : BaseDto
    {
        public System.Guid ActivityTemplateId { get; set; }
        public string ActivityTemplateType { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public string PropertyType { get; set; }
    }

    public class workflowDataTypeDTO
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
    }

    public class ActionColumnAndValuesDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class WorkFlowTypeDDdto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class MeetingActionDTO
    {
        public string Name { set; get; }
        public string Email { set; get; }
        public string Location { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string Subject { set; get; }
        public string Body { set; get; }
    }

    public class EventMonitorDTO
    {
        public System.Guid EventMonitorId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }
        public string Color { get; set; }
        public string IPAddress { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class EventLogDTO
    {
        public System.Guid EventLogId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }
        public string Color { get; set; }
        public string IPAddress { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class EventNotificationDTO
    {
        public System.Guid EventNotificationId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }
        public string Color { get; set; }
        public string IPAddress { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }

    public partial class EmailConfigurationDTO
    {
        public System.Guid EmailConfigurationId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }

    public partial class EmailTemplateDTO
    {
        public System.Guid EmailTemplateId { get; set; }
        public Nullable<System.Guid> EmailConfigurationId { get; set; }
        public Nullable<System.Guid> WorkFlowId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual EmailConfigurationDTO EmailConfiguration { get; set; }
        public virtual WorkFlowDTO WorkFlow { get; set; }
    }

    public partial class ProductCatelogDTO : BaseDropDownDto
    {
        public System.Guid ProductId { get; set; }
        public Nullable<long> SerialNumber { get; set; }
        public string ProductName { get; set; }
        public Nullable<System.Guid> CategoryId { get; set; }
        public Nullable<System.DateTime> ValidFrom { get; set; }
        public Nullable<System.DateTime> ValidTo { get; set; }
        public string Description { get; set; }
        public string ProductNote { get; set; }
        public SelectList CategoryDropdown { get; set; }

    }

    public partial class ProductPriceListDTO : BaseDto
    {
        public System.Guid ProductPriceId { get; set; }
        public Nullable<System.Guid> CurrencyId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.Guid> ProductId { get; set; }
        public string Description { get; set; }
        public virtual CurrencyDto Currency { get; set; }
        public virtual ProductCatelogDTO ProductCatelog { get; set; }
    }

    public partial class DiscountDTO : BaseDto
    {
        public System.Guid DiscountId { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> MinQuantity { get; set; }
        public Nullable<decimal> MaxQuantity { get; set; }
        public Nullable<System.Guid> ProductPriceId { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public string Description { get; set; }
    }

    public partial class BulkDevicDTO : BaseDropDownDto
    {

    }

    public class WorkFlowMappingDTO : BaseDto
    {
        public System.Guid WorkFlowMappingId { get; set; }
        public Nullable<System.Guid> WorkFlowId { get; set; }
        public string SourceType { get; set; }
        public string SourceColumn { get; set; }
        public string SourceValue { get; set; }
        public string SourceData { get; set; }
        public string Action { get; set; }
        public Nullable<bool> IsDone { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public SelectList WorkflowDropdown { get; set; }
        public SelectList SourceTypeDropdown { get; set; }
        public SelectList DestinationTypeDropdown { get; set; }
        public SelectList SourceColumnDropdown { get; set; }
        public SelectList DestinationColumnDropdown { get; set; }
        public SelectList ActionDropdown { get; set; }
        public virtual WorkFlowDTO WorkFlow { get; set; }
    }

    public class WorkFlowReportDTO : BaseDto
    {
        public System.Guid WorkFlowReportId { get; set; }
        public Nullable<System.Guid> WorkFlowId { get; set; }
        public string WorkFlowStatus { get; set; }
        public string Action { get; set; }
        public string WorkFlowActionStatus { get; set; }
        public string WorkFlowDesign { get; set; }
        public string AppliedTo { get; set; }
        public Nullable<int> Frequency { get; set; }
        public Nullable<int> Priority { get; set; }
        public string DeviceName { get; set; }
        public string AccountName { get; set; }
        public Nullable<System.Guid> AccountID { get; set; }
        public Nullable<System.Guid> DeviceID { get; set; }
        public virtual WorkFlowDTO WorkFlow { get; set; }
        public virtual AccountDto Account { get; set; }
        public virtual DeviceDto Device { get; set; }
           public SelectList WorkFlowIdDropdown { get; set; }
        public SelectList AccountIdDropdown { get; set; }
        public SelectList DeviceIdDropdown { get; set; }

    }

    public class WorkFlowDTO 
    {
        public System.Guid WorkFlowId { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Name is always alphabetic")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please select sensor")]
        public string TargetOn { get; set; }
        [Required(ErrorMessage = "Please enter description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please select priority")]
        public Nullable<int> Priority { get; set; }
        public string PriorityName { get; set; }
        public string WorkFlowDesign { get; set; }
        [Required(ErrorMessage = "Please select action")]
        public string Action { get; set; }
        [Required(ErrorMessage = "Please select account")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "Please enter Min Threshold")]
        public string MinThreshold { get; set; }
        [Required(ErrorMessage = "Please enter Max Threshold")]
        [GreaterThan("MinThreshold")]
        public string Threshold { get; set; }
        public string TotalThreshold { get; set; }
        [Required(ErrorMessage = "Please select device")]
        public string DeviceName { get; set; }
        public string AccountName { get; set; }
        public string TargetOnName { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public string TriggerCondition { get; set; }      //may be
        //public Nullable<System.DateTime> TriggerIn { get; set; }    //may be
        //public Nullable<System.DateTime> TriggerOut { get; set; }    //may be
        //public Nullable<int> FrequencyOut { get; set; }
        public string WorkFlowStatus { get; set; }
        public string AppliedTo { get; set; }
        public Nullable<int> Frequency { get; set; }
        //public string DeviceMac { get; set; }
        //public string Cloud { get; set; }
        //public Nullable<System.Guid> RelatedToId { get; set; }

        public SelectList DevicesDropDown { get; set; }
        public SelectList SensorsDropDown { get; set; }
        public SelectList PriorityDropDown { get; set; }
        public SelectList AccountsDropdown { get; set; }
        public SelectList ActionDropdown { get; set; }
        //public SelectList TriggerConditionDropdown { get; set; }
        //public SelectList AppliedToDropdown { get; set; }

        //public SelectList TargetOnDropdown { get; set; }
        //public SelectList RelatedToIdDropdown { get; set; }

       
    }
    public class ActionDTO
    {
        public System.Guid ID { get; set; }
        public string Name { get; set; }
    }
    public  class PriorityDTO
    {
        public System.Guid ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Value { get; set; }
    }
    public class WorkOrderDto : BaseDropDownDto
    {
        public System.Guid WorkOrderId { get; set; }
        [Required(ErrorMessage ="Please enter Work Order Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter NTE")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "NTE must be numeric")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G8}")]
        public Nullable<decimal> NTE { get; set; }
        [Required(ErrorMessage = "Please select Work Order Stage")]
        public Nullable<System.Guid> WorkOrderStageId { get; set; }
        [Required(ErrorMessage = "Please select Account")]
        public Nullable<System.Guid> AccountId { get; set; }
        [Required(ErrorMessage = "Please enter Description")]
        public string Description { get; set; }
        public SelectList WorkStagesDropdown { get; set; }
        public virtual WorkStageDto WorkStage { get; set; }
        public virtual AccountDto Account { get; set; }

    }

    #region Enum Classess


    public enum ActivityType
    {
        Email,
        Meeting,
        PhoneCalls
    }


    public enum RelatedToEnum
    {
        Account,
        Oppertunities,
        Leads,
        Cases,
        Contacts,
        Device
    }
    public enum RelatedToEnumforactivity
    {
        Account,
        Opportunity,
        Leads,
        Cases,
        Device
    }
    public enum RelatedToEnumforcase
    {
        Account,
        Opportunity,
        Leads,
        Device
    }
    public enum EntityTypes
    {
        Account,
        Lead,
        Cases,
        Oppertunity,
        Device,
        WorkOrder,
        WorkFlow,
        WorkFlowReport,
        ActivityTemplate,
        WorkFlowMapping,
        EventLog,
        EventMonitor,
        EventNotification,
        EmailConfiguration,
        EmailTemplate,
        GlobalSearch,
        Industry, Location,
        Reading,
        ReadingType,
        Resource,
        Team,
        User, Address, Alert,
        AccountSize, CustomerAsset, ServiceCall, ReadingUnit
    }

    public static class EventType
    {
        public const string Log = "Log";
        public const string Exception = "Exception";
        public const string Notification = "Notification";
    }

    public static class EventColor
    {
        public const string yellow = "yellow";
        public const string red = "red";
        public const string green = "green";
    }

    public static class DeviceMaintenance
    {
        public const string None = "None";
        public const string IsRepaired = "IsRepaired";
        public const string IsServiced = "IsServiced";
    }

    public static class CloudServiceForDD
    {
        public const string Swuich = "Swuich";
        public const string IBM = "IBM";
        public const string Google = "Google";
        public const string Microsoft = "Microsoft";
        public const string Amazon = "Amazon";
    }

    public static class ExcludedColumns
    {
        public const string StatusId = "StatusId";
        public const string AccountId = "AccountId";
        public const string LeadId = "LeadId";
        public const string DeviceId = "DeviceId";
        public const string AssignedUser = "AssignedUser";
        public const string AssignedTeam = "AssignedTeam";
        public const string CreatedBy = "CreatedBy";
        public const string CreatedDate = "CreatedDate";
        public const string UpdatedBy = "UpdatedBy";
        public const string UpdatedDate = "UpdatedDate";
    }

    public static class TrigegrConditionConstant
    {
        public const string Pre_Event = "Pre Event";
        public const string Post_Event = "Post Event";
    }

    public static class appliedToConstant
    {
        public const string OnCreate = "OnCreate";
        public const string OnUpdate = "OnUpdate";
        public const string CreateAndUpdate = "Create And Update";
    }

    public static class WFStatusConstant
    {
        public const string In_Progress = "In Progress";
        public const string Waiting_for_User_Action = "Waiting for User Action";
        public const string Paused = "Paused";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
        public const string Error = "Error";
    }

    public static class WFActionConstant
    {
        public const string Email = "Email";
        public const string Note = "Note";
        public const string Alert = "Alert";
        public const string Meeting = "Meeting";
        public const string Account = "Account";
        public const string Lead = "Lead";
        public const string SugarCRM = "SugarCRM";
        public const string Dyn = "Dynamics 365";
        public const string WorkOrder = "Workorder";
    }

    public static class ActionStatusConstant
    {
        public const string Success = "Success";
        public const string Failure = "Failure";
    }


    #endregion



}
