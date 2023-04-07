using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class AppointmentServiceTypeSettings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string AppointmentServiceId { get; set; }

        public string AppointmentTypeId { get; set; }

        public string AppointmentSettingId { get; set; }


        public decimal AppointmentFees { get; set; }


        public bool isActive { get; set; }
    }
}
