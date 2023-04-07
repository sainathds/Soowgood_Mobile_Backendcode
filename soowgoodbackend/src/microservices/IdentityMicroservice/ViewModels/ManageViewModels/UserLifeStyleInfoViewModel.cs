using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ManageViewModels
{
    public class UserLifeStyleInfoViewModel
    {
        public string Id { get; set; }
        public string LifeStyleId { get; set; }
        public string LifeStyleName { get; set; }
        public string Note { get; set; }
        public bool CurrentStatus { get; set; }
        public DateTime SinceWhen { get; set; } // Kobe Theke
        public DateTime TillWhen { get; set; } // Kobe Porjanto
        public string UserId { get; set; }
        public int QuantityPerDay { get; set; }
    }
}
