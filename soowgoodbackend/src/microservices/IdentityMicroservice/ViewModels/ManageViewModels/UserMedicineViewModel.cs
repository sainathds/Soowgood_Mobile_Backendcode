using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ManageViewModels
{
    public class UserMedicineViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string GenericName { get; set; }
        public string Note { get; set; }
        public string UserId { get; set; }
        public bool CurrentStatus { get; set; }
        public DateTime SinceWhen { get; set; } // Kobe Theke
        public DateTime TillWhen { get; set; } // Kobe Porjanto
        public int QuantityPerDay { get; set; }
    }
}
