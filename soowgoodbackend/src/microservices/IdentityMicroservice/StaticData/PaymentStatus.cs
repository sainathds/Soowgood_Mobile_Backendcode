using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.StaticData
{
    public class PaymentStatus
    {
        public const string Due = "Due";
        public const string Paid = "Paid";
        public const string Completed = "Completed";
        public const string InProcess = "InProcess";
    }
}
