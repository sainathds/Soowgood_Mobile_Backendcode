using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.Address
{
    public class ContactInfoViewModel
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string ImageURL { get; set; }
        public bool IsDefault { get; set; }
    }
}
