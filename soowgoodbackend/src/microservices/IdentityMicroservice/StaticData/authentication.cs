using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.StaticData
{
    public class authentication
    {
        public string verificationotp { get; set; }
        public string success { get; set; }
        public string message { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }

        public string userRole { get; set; }

        public string currentverificationotp { get; set; }

        public string userName { get; set; }

        public string id { get; set; }

        public string concurrencyStamp { get; set; }

        public string profilePicture { get; set; }


        public string code { get; set; }

        public string access_token { get; set; }

        public List<elements> elements { get; set; }
        
    }
}
