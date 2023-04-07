using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ManageViewModels
{
    public class AppointmentWeekSchedule
    {
        public string Id { get; set; }

        public string ClinicId { get; set; }

        public string DayofWeek { get; set; }
        public TimeSpan DayStartingTime { get; set; }
        public TimeSpan DayEndingTime { get; set; }
        public string AppointmentType { get; internal set; }
        public string TaskType { get; internal set; }
    }
}
