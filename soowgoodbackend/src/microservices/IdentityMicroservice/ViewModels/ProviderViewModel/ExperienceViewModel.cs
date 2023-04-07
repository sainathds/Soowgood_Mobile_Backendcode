using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ProviderViewModel
{
    public class ExperienceViewModel
    {
        public string Id { get; set; }
        public string HospitalName { get; set; }
        public string Description { get; set; }
        public string Designation { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsPresent { get; set; }
        public string UserId { get; set; }
    }
}
