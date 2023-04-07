using IdentityMicroservice.StaticData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.UserViewModels
{
    public class RegisterViewModel
    {
        //[Required]
        //[EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required]
        //[Phone]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        //[Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        //[Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        //[Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        //[Required]
        [Display(Name = "DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        //[Required]
        [Display(Name = "Gender")]
        public int Gender { get; set; }

        //[Required]
        [Display(Name = "Country")]
        public String Country { get; set; }

        //[Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 9)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //[Required]
        [Display(Name = "UserRole")]
        public string UserRole { get; set; }
        public bool IsOrganizational { get; set; }
        public bool IsAssignedFromOrganization { get; set; }
    }
}
