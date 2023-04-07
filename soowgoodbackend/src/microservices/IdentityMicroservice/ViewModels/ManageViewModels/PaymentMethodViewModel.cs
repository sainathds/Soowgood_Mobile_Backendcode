using IdentityMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ManageViewModels
{
    public class PaymentMethodViewModel:TableHistory
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public bool IsParent { get; set; } = false;
        public string Description { get; set; }
    }
}
