using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class appointmenttransactiondetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        public DateTime trandate { get; set; }

        public string transstatus { get; set; }

        public string transno { get; set; }

        public string refno { get; set; }

        public string trancurrency { get; set; }

        public string failureremark { get; set; }

        public string refund_refid { get; set; }

        public DateTime refunddate { get; set; }

        public string refundstatus { get; set; }


        public string sourcefrom { get; set; }
        

    }
}
