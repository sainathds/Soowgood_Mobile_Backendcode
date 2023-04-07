using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ManageViewModels
{
    public class UserAccessibleInfoViewModel
    {
        public string Id { get; set; }
        public string AccessibleId { get; set; }
        public string AccessibleName { get; set; }
        public string Note { get; set; }
        public bool CurrentStatus { get; set; }
        public DateTime SinceWhen { get; set; } // Kobe Theke
        public DateTime TillWhen { get; set; } // Kobe Porjanto
        public string UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
