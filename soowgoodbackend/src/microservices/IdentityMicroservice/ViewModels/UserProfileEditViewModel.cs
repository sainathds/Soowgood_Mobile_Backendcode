using IdentityMicroservice.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels
{
    public class UserProfileEditViewModel
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public int SelectedGender { get; set; }
        public string DateOfBirth { get; set; }
        public int SelectedMaritalStatus { get; set; }
        public string Designation { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Status { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string MemberSince { get; set; }
        public string Interests { get; set; }
        public string InstituteName { get; set; }
        public string DegreeName { get; set; }
        public string Session { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public Gender Gender { get; set; }
    }
}
