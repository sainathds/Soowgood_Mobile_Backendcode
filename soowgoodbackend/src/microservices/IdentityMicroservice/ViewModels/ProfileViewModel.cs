using IdentityMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels
{
    public class ProfileViewModel
    {
        public UserViewModel UserViewModel { get; set; }
        public List<ApplicationUser> RecentContacts { get; set; }
        public List<UserActivity> Activities { get; set; }
    }
}
