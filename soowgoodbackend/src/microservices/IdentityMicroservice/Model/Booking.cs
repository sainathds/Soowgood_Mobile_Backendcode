using IdentityMicroservice.StaticData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class Booking: TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public DateTime Schedule { get; set; }
        public string AppointmentSettingId { get; set; }

        public string appointmentServiceId { get; set; }

        public bool IsBookingConfirmed { get; set; } = false;
        public bool IsBookingCancelledByReceiver { get; set; } = false;
        public bool IsBookingCancelledByProvider { get; set; } = false;

        [Required]
        public string ServiceReceiverId { get; set; }

        [Required]
        public string ServiceProviderId { get; set; }
        public DateTime TentativeDate { get; set; }
        public TimeSpan TentativeTime { get; set; }
        public TimeSpan ReportingTime { get; set; }
        public string DayOfWeek { get; set; }
        public long SerialNo { get; set; } = 1;
        public bool IsProcessed { get; set; } = false;
        public bool IsCancelled { get; set; } = false;
        public string Status { get; set; } = BookingStatus.Pending;
        public bool IsRecurrent { get; set; } = false;
        public int RecurringHours { get; set; } = 0;
        public int NoOfVisits { get; set; } = 0;
        public string UserPaymentStatus { get; set; } = PaymentStatus.Due;
        public bool VisitConfirmationStatus { get; set; } = false;
        public double PaidAmount { get; set; } = 0;
        public decimal discountamt { get; set; } = 0;
        public decimal appointmentamt { get; set; } = 0;
        public decimal doctorcharges { get; set; } = 0;
        public string BeneficiaryComment { get; set; }
        public string ProviderComment { get; set; }

        public TimeSpan scheduleStartTime { get; set; }
        public TimeSpan scheduleEndTime { get; set; }


        public string patientName { get; set; }
        public string patientAddress { get; set; }

        public int ratingpoints { get; set; }
        public string providerreview { get; set; }


        public string transactionid { get; set; }

        public int isrefunded { get; set; }
        
    }
}
