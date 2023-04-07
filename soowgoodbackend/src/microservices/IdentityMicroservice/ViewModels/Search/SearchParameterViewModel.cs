using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.Search
{
    public class SearchParameterViewModel
    {
        public string SearchKeyword { get; set; }
        public string Provider { get; set; }
        public string Location { get; set; }
        public string ServiceType { get; set; }
        public string AppointmentType { get; set; }
        public string ProviderType { get; set; }
        public string ProviderSpeciality { get; set; }
        public string Availability { get; set; }
        public string Gender { get; set; }
        public string UserRole { get; set; } = String.Empty;
        public int ConsultationFees { get; set; }
        public DateTime DayStartingTime { get; set; }
        public DateTime DayEndTime { get; set; }
        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 0;

        
        public string ServiceReceiverId { get; set; }

        public string ServiceProviderId { get; set; }

        public string bookingtype { get; set; }

        public string Id { get; set; }

    }
}
