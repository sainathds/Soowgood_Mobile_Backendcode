using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class prescriptionmaster
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        
        public string bookingId { get; set; }        
        
        public string ServiceProviderId { get; set; }

        public string diognosis { get; set; }
        public string ServiceReceiverId { get; set; }

        public DateTime prescriptiondate { get; set; }

        public string signaturename { get; set; }
    }
}
