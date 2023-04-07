using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ManageViewModels
{
    public class UserPaymentMethodViewModel
    {
        public string Id { get; set; }
        public string PaymentMethodId { get; set; }
        public string UserId { get; set; }
        public string PaymentMethod { get; internal set; }
        public string UserFullName { get; internal set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpiredDate { get; set; }
        public string CVV { get; set; }
        public string MobileNumber { get; set; }
        public string ParentId { get; set; }
    }
}
