using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Designation { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Twitter { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Status { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string MemberSince { get; set; }
        public string Interests { get; set; }
        public string About { get; set; }
        public string ProfilePicture { get; set; }
        public string BloodGroup { get; set; }
        public string UserRole { get; set; }
        public IFormFile File { get; set; }
        public List<IdentityUserRole<string>> Roles { get; set; }
        public bool IsOrganizational { get; set; } = false;
        public bool IsAssignedFromOrganization { get; set; }
        public string CurrentAddress { get; internal set; }
        public string State { get; internal set; }
        public string PostalCode { get; internal set; }
        public string Service { get; set; }
        public string Specialization { get; set; }
        public int age { get; set; }
    }
}
