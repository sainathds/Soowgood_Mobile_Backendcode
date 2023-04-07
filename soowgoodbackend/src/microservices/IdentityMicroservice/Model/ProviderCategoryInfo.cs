using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class ProviderCategoryInfo : TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string ParentId { get; set; }
        public bool IsMedicalCareType { get; set; }
        public string CategoryName { get; set; }
        public string ImageURL { get; set; }
        public int SequenceNo { get; set; }
        public bool IsDefault { get; set; }
    }
}
