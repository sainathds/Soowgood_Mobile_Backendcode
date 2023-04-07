using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class prescriptiondurgdetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string durgId { get; set; }
        public string prescriptionid { get; set; }
        public string durgname { get; set; }
        public string weeklyschedule { get; set; }
        public string timing { get; set; }
        public string dose { get; set; }
    }
}
