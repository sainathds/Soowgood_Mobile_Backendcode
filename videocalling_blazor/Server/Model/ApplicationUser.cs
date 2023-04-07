
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using videocalling_blazor.Server.StaticData;

namespace videocalling_blazor.Server.Model
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
