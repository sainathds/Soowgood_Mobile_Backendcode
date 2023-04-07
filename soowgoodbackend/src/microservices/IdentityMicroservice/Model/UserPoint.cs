using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class UserPoint: TableHistory
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public long TransactionId { get; set; }

        [Required]
        public long ServiceTypeId { get; set; }

        [Required]
        public long ServiceId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Rating{ get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Point { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public ApplicationUser User { get; set; }
    }
}
