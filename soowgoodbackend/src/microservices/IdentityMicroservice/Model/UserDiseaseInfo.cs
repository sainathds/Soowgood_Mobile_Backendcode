using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class UserDiseaseInfo: TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsExist { get; set; }
        public DateTime Since { get; set; }
        public int Duration { get; set; }
        public bool IsDurationInYear { get; set; } = false;
        public bool IsPresent { get; set; } = false; 
        public bool DoesAnyMemberOfTheFamilyhave { get; set; } = false;
        public string UserId { get; set; }
    }
}
