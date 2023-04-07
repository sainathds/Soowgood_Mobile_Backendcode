using IdentityMicroservice.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ManageViewModels
{
    public class BookingViewModel
    {
        public string ServiceProvider { get; internal set; }
        public TimeSpan DayStartingTime { get; internal set; }
        public TimeSpan DayEndingTime { get; internal set; }
        public string Address { get; internal set; }
        public string ClinicName { get; internal set; }

        public string ClinicAddress { get; internal set; }

        public string ClinicId { get; set; }
        public string Id { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public DateTime Schedule { get; set; }
        public string AppointmentSettingId { get; set; }
        public bool IsBookingConfirmed { get; set; } = true;
        public bool IsBookingCancelledByReceiver { get; set; } = false;
        public bool IsBookingCancelledByProvider { get; set; } = false;
        public string ServiceReceiverId { get; set; }
        public string ServiceProviderId { get; set; }
        public string AppointmentTypeId { get; set; }
        public string AppointmentType { get; set; }
        public DateTime TentativeDate { get; set; }
        public TimeSpan TentativeTime { get; set; }
        public TimeSpan ReportingTime { get; set; }
        public string DayOfWeek { get; set; }
        public long SerialNo { get; set; } = 1;
        public bool IsProcessed { get; set; } = false;
        public bool IsCancelled { get; set; } = false;
        public string ServiceReceiver { get; internal set; }
        public DateTime AppointmentDate { get; internal set; }
        public TimeSpan AppointmentTime { get; internal set; }
        public string ReceiverImage { get; internal set; }
        public string ProviderImage { get; internal set; }
        public string ReceiverEmail { get; internal set; }
        public string ReceiverPhone { get; internal set; }
        public string ProviderEmail { get; internal set; }
        public string ProviderPhone { get; internal set; }
        public decimal AppointmentFees { get; internal set; }
        public decimal Amount { get; internal set; }
        public string Status { get; internal set; }
        public bool IsRecurrent { get; set; } = false;
        public int RecurringHours { get; set; } = 0;
        public int NoOfVisits { get; set; } = 0;
        public string UserPaymentStatus { get; set; }
        public bool VisitConfirmationStatus { get; set; } = false;
        public double PaidAmount { get; set; } = 0;

        public double doctorcharges { get; set; } = 0;
        public double admincomission { get; set; } = 0;
         

        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string BeneficiaryComment { get; set; }
        public string ProviderComment { get; set; }
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 0;

        public string SpecializationName { get; set; }

        public string bookingtype { get; set; }

        public string TentativeBookingDate { get; internal set; }

        public int isPaymentDone { get; set; } = 0;

        public int isnewpatient { get; set; } = 0;

        public string appointmentStatus { get; set; }

        public string AppointmentTypeName { get; set; }

        public String LastApointmentDate { get; set; }

        public string TentativeAppointmentDate { get; set; }

        public String patientAddress { get; set; }

        public string BloodGroup { get; set; }

        public int age { get; set; }

        public String Gender { get; set; }

        public int totalratingpoint { get; set; }
        public int totalreview { get; set; }



        public int ratingpoints { get; set; }
        public String providerreview { get; set; }


        public String patientName { get; set; }

        public String patientCurrentAddress { get; set; }

        public String callstatus { get; set; }
        
        public String documentname { get; set; }
        public String booingId { get; set; }
        public String AppointmentNo { get; set; }
        public string ScheduleAppointmentDate { get; set; }
        public string BookingAppointmentDate { get; set; }
        public string trancurrency { get; set; }
        public TimeSpan scheduleStartTime { get; set; }
        public TimeSpan scheduleEndTime { get; set; }
        public String scheduleTime { get; set; }
        public String bookingstatus { get; set; }

        public string transno { get; set; }

        public string paybackstatus { get; set; }
        
        public int srno { get; set; }

        public string paybackid { get; set; }

        public string diognosis { get; set; }


        public string appointmentno { get; set; }

        public string bookingId { get; set; }

        public string serviceType { get; set; }

        public string Provider { get; set; }
        

        public DateTime? prescriptiondate { get; set; }

        public string sourcefrom { get; set; }




        public string tranpkid { get; set; }

        public string bank_tran_id { get; set; }

        public string statuscode { get; set; }

        public string errormessage { get; set; }
    }
}
