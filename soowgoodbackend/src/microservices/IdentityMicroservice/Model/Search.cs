using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class Search
    {
        public string Id { get; set; }
        public string ServiceType { get; set; }
        public string ProviderType { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }

        public string DayOfWeek { get; set; }

        public string ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string ClinicAddress { get; set; }
        public string Specialization { get; set; }
        public string Service { get; set; }
        public string BookNow { get; set; }
        public string providerImage { get; set; }
        public string Availability { get; set; }
        public string ConsultationFees { get; set; }
        public string DayStartingTime { get; set; }
        public string DayEndingTime { get; set; }

        public int totalratingpoint { get; set; }

        public int totalreview { get; set; }

        public string AppointmentType { get; set; }
    }
}
