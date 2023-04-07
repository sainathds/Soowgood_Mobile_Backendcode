using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.StaticData
{
    public class paymentrefundinfo
    {
        public string APIConnect { get; set; }
        public string bank_tran_id { get; set; }
        public string trans_id { get; set; }
        public string refund_ref_id { get; set; }

        public string status { get; set; }

        public string errorReason { get; set; }
    }
}
