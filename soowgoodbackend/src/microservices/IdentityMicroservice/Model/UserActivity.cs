using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityMicroservice.Model
{
    public class UserActivity: TableHistory
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime ActivityTime { get; set; }
        public ApplicationUser User { get; set; }
    }
}
