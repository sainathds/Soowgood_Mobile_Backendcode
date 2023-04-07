using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace videocalling_blazor.Server.Model
{
    public class UserActivity : TableHistory
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime ActivityTime { get; set; }
        public ApplicationUser User { get; set; }
    }
}
