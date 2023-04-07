using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class AppointmentDaySettings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string AppointmentDayId { get; set; }

        public string AppointmentDayOfWeek { get; set; }

        public string AppointmentSettingId { get; set; }

        public bool isActive { get; set; }
    }
}
