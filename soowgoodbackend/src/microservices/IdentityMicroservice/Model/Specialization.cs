using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class Specialization: TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string TypeId { get; set; }
        public string SpecializationName { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
