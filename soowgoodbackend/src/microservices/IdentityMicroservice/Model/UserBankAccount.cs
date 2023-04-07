using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class UserBankAccount: TableHistory
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public ApplicationBank Bank { get; set; }
        public ApplicationBankBranch Branch { get; set; }
        public string SwiftCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ApplicationUser User { get; set; }
    }
}
