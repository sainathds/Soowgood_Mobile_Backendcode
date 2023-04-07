using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.StaticData
{
    public class AppointmentServiceTypeSetting
    {
        public string AppointmentServiceId { get; set; }

        public string AppointmentTypeId { get; set; }

        public string AppointmentSettingId { get; set; }


        public decimal AppointmentFees { get; set; }


        public bool isActive { get; set; }


        public string ConsultancyType { get; set; }
    }
}
