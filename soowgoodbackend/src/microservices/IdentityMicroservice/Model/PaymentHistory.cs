using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class PaymentHistory: TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
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
        public DateTime TransactionTime { get; set; } = DateTime.Now;
        public string TransactionOrigin { get; set; }
        public string Currency { get; set; } = "Dollar";
        public double Amount { get; set; } = 0;
        public string PaymentStaus { get; set; } = "Completed";
        public string ReceiverUserId { get; set; }

    }
}
