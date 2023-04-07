using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class TaskChargeSetting: TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ClinicId { get; set; }
        public string TaskId { get; set; }
        public decimal VATInPercentage { get; set; }
        public decimal TAXInPercentage { get; set; }
        public decimal ServiceChargeInPercentage { get; set; }
        public decimal ProcessingFeesPerTaskInPercentage { get; set; }
        public decimal PointsPerTask { get; set; }


    }
}
