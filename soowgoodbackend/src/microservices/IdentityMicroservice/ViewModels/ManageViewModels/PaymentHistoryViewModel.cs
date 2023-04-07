using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ManageViewModels
{
    public class PaymentHistoryViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserPaymentMethodId { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public DateTime CardExpiredDate { get; set; }
        public string CVV { get; set; }
        public bool IsInfoSaved { get; set; } = false;
        public string SenderUserId { get; set; }
        public long MenuId { get; set; }
        public string TransactionId { get; set; }
        public DateTime TransactionTime { get; set; }
        public string TransactionOrigin { get; set; }
        public string Currency { get; set; } = "Dollar";
        public double Amount { get; set; } = 0;
        public string PaymentStaus { get; set; } = "Completed";
        public string ReceiverUserId { get; set; }
        public string AmountPaidTo { get; internal set; }

        public string callstatus { get;  set; }
    }
}
