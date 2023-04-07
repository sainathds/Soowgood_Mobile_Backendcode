using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.StaticData
{
    public class AuthenticateResponse
    {
        public string channel { get; set; }

        public dynamic uid { get; set; }

        public string token { get; set; }
    }
}
