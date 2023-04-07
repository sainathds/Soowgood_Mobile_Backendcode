using IdentityMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.StaticData
{
    public class ScheduleAppointment
    {
        public string AppointmentSettingID { get; set; }

        public string dayno { get; set; }

        public string AppointmentDayOfWeek { get; set; }


        public string ClinicId { get; set; }

        public string TaskType { get; set; }

        public int TimeSlot { get; set; }

        public string TaskTypeId { get; set; }
        

        public string AppointmentDate { get; set; }

        public int NoOfPatients { get; set; }
        public decimal AppointmentFees { get; set; }

        public DateTime? caldate { get; set; }

        public string DayStartingTime { get; set; }

        public string DayEndingTime { get; set; }

        public string appointmentstartime { get; set; }

        public string appointmentendtime { get; set; }

        public string clinicname { get; set; }

        public string cliniccurrentaddress { get; set; }

        public string AppointmentEndingDate { get; set; }

        public string AppointmentStartingDate { get; set; }

        public string serviceProviderId { get; set; }

        public int alreadybooked { get; set; }

        public TimeSpan AppointmentEndingTime { get; set; }

        public TimeSpan AppointmentStartingTime { get; set; }

        public string AppointmentTypeId { get; set; }

        public string mindayname { get; set; }

        public string maxdayname { get; set; }


        public string ScheduleType { get; set; }

        public double PaidAmount { get; set; } = 0;


        public double doctorcharges { get; set; } = 0;


        public double admincomission { get; set; } = 0;


        public double patientcharges { get; set; } = 0;


        public string AppointmentServiceId { get; set; }

        public List<AppointmentDaySettings> AppointmentDayList { get; set; }

        public List<AppointmentServiceTypeSetting> AppointmentServiceTypeList { get; set; }
        
    }
}
