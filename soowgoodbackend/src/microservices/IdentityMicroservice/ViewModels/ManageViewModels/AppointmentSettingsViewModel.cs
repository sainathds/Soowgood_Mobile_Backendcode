using IdentityMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ManageViewModels
{
    public class AppointmentSettingsViewModel: TableHistory
    {
        public string Id { get; set; }
        public string DayofWeek { get; set; }
        public TimeSpan DayStartingTime { get; set; }
        public TimeSpan DayEndingTime { get; set; }
        //public string timeSlot { get; set; }
        public string ServiceProviderId { get; set; }
        public string ClinicId { get; set; }
        public DateTime? AppointmentSettingDate { get; set; }
        public DateTime? AppointmentStartingDate { get; set; }
        public DateTime? AppointmentEndingDate { get; set; }
        public string ClinicName { get; internal set; }
        public int TimeSlot { get; set; }
        public int NoOfPatients { get; set; }
        public decimal AppointmentFees { get; set; } = 0;
        public string ClinicAddress { get; internal set; }
        public string TaskTypeId { get; set; }
        public string AppointmentTypeId { get; set; }
        public string AppointmentType { get; internal set; }
        public string TaskType { get; internal set; }

        public IList<AppointmentWeekSchedule> weeklyAppointmentList { get; set; }
    }
}
