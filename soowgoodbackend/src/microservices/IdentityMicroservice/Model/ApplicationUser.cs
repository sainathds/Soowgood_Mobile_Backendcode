using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IdentityMicroservice.StaticData;
using Microsoft.AspNetCore.Identity;

namespace IdentityMicroservice.Model
{
    public class ApplicationUser : IdentityUser
    {
        public long InstallationId { get; set; }
        public string OtherEmail { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string OthersMemberEmail { get; set; }
        public string ProfilePicture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; internal set; }
        public DateTime DateOfBirth { get; set; }
        public string Designation { get; set; }
        public DateTime MemberSince { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Status { get; set; }
        public string BloodGroup { get; set; }
        public Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public ICollection<UserActivity> UserActivities { get; set; }
        public string Message { get; set; }
        public string UserRole { get; set; }
        public bool IsOrganizational { get; set; } = false;
        public bool IsAssignedFromOrganization { get; set; } = false;

        public bool IsConfirmedByAdmin { get; set; } = false;
    }
}