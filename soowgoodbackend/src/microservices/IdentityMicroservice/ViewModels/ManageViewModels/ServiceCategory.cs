using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ManageViewModels
{
    public class ServiceCategory
    {
        // string MedicareServiceType { get; set; }
        public string ProviderType { get; set; }
        public string Provider { get; set; }
        public string ImageURL { get; set; }

    }

    public class ProviderTypeCategory
    {
        public string ProviderType { get; set; }
        public string Provider { get; set; }
        public string ImageURL { get; set; }

    }
}
