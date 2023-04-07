using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.StaticData
{
    public class AuthenticateRequest
    {
        [Required]

        public string channel { get; set; }

        [Required]

        public string uid { get; set; }

        public uint expiredTs { get; set; } = 0;

        public int role { get; set; } = 1;
    }
}
