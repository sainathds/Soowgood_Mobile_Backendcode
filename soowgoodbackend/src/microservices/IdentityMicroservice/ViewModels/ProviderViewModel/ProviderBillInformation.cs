using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ProviderViewModel
{
    public class ProviderBillInformation
    {
        public string Id { get; set; }
        public string bankname { get; set; }
        public string branchname { get; set; }
        public string accountname { get; set; }
        public string accountno { get; set; }
        public string accounttype { get; set; }
        public string UserId { get; set; }

    }
}
