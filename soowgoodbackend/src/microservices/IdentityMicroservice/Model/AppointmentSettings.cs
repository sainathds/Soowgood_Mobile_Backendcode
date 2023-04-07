using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class AppointmentSettings: TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public DateTime? AppointmentSettingDate { get; set; }
        public TimeSpan DayStartingTime { get; set; }
        public TimeSpan DayEndingTime { get; set; } 

        [Required] 
        public string ServiceProviderId { get; set; }
        public string ClinicId { get; set; }
       
        public int TimeSlot { get; set; }
        public int NoOfPatients { get; set; }
        
        public string TaskTypeId { get; set; }

  


    }
}
