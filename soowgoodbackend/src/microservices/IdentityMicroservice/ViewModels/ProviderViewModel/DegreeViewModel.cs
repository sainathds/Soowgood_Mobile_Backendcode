using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ProviderViewModel
{
    public class DegreeViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Institution { get; set; }
        public DateTime YearOfCompletion { get; set; }
        public string UserId { get; set; }
    }
}
