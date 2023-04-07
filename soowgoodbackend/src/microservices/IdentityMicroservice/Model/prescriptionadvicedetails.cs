using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class prescriptionadvicedetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string adviceid { get; set; }
        public string prescriptionid { get; set; }
        public string advice { get; set; }
    }
}
