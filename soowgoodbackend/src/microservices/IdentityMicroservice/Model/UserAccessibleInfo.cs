using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class UserAccessibleInfo: TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        [Required]
        public string AccessibleName { get; set; }
        public string Note { get; set; }
        public string UserId { get; set; }
        public bool CurrentStatus { get; set; }
        public DateTime SinceWhen { get; set; } // Kobe Theke
        public DateTime TillWhen { get; set; } // Kobe Porjanto
    }
}
