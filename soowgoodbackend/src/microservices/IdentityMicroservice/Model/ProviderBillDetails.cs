using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class ProviderBillDetails : TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        public string bankname { get; set; }
        public string branchname { get; set; }

        public string accountname { get; set; }
        public string accountno { get; set; }

        public string accounttype { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
