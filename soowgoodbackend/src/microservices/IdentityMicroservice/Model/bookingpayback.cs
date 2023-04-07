using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class bookingpayback
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        public string bookingid { get; set; }

        public string paybackstatus { get; set; }
        public DateTime requestdate { get; set; }

        public DateTime processdate { get; set; }
    }
}
