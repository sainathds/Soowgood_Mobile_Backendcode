using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.StaticData
{
    public class AppointmentSetting
    {
        public string AppointmentType { get;  set; }

        public string ServiceProviderId { get;  set; }

        public string ClinicId { get; set; }


        public TimeSpan DayStartingTime { get; set; }
        public TimeSpan DayEndingTime { get; set; }

        public string daysOfWeek { get; set; }

        public string AppointmentSettingId { get; set; }

        public string TaskTypeId { get; set; }

        public int TimeSlot { get; set; }
        public int NoOfPatients { get; set; }

        public string Id { get; set; }
    }
}
