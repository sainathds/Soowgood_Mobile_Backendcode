using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class ApplicationBankBranch: TableHistory
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public ApplicationBank Bank { get; set; }
    }
}
