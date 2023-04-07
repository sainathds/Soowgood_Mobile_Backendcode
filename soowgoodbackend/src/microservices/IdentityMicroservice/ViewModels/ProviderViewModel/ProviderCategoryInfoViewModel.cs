using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ProviderViewModel
{
    public class ProviderCategoryInfoViewModel
    {
        public string Id { get; internal set; }
        public string Provider { get; internal set; }
        public string MedicalCareType { get; internal set; }
        public object ProviderType { get; internal set; }
    }
}
