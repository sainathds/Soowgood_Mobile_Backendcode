using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ProviderViewModel
{
    public class AwardsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string UserId { get; set; }
    }
}
