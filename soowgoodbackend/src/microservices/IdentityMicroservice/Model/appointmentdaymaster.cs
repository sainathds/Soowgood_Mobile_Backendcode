using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class appointmentdaymaster
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string daysid { get; set; }

        public string appointmentdayname { get; set; }

        public int appointmentdayno { get; set; }

    }
}
