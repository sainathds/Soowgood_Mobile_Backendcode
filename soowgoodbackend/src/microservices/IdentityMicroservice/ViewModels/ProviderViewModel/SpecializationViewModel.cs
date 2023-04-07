using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ProviderViewModel
{
    public class SpecializationViewModel
    {
        public string Id { get; set; }
        public string SpecializationName { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string SearchKeyword { get; set; }
        public string TypeId { get; internal set; }
        public string Type { get; internal set; }
    }
}
