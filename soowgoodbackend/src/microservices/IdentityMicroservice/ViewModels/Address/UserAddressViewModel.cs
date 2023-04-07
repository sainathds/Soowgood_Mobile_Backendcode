using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.Address
{
    public class UserAddressViewModel
    {
        public string Id { get; set; }
        public string CurrentAddress { get; set; }
        public string OptionalAddress { get; set; }
        public string PreferableAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public DateTime ActivityTime { get; set; }
        public string UserId { get; set; }
    }
}
