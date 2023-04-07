using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.StaticData
{
    public class appuserprofiledata
    {
        public string Id { get; set; }
        public string fullname { get; set; }
        public string username { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }

        public string bloodgroup { get; set; }
        public string email { get; set; }
        public string mobileno { get; set; }

        public string maritalstatus { get; set; }

        public int age { get; set; }

        public string specialization { get; set; }

        public string gendertext { get; set; }
        

        public string currentaddress { get; set; }
        public string city { get; set; }
        public string statename { get; set; }
        public string postalcode { get; set; }
        public string country { get; set; }
        public string addressid { get; set; }


        public string aboutme { get; set; }
        public string profilephoto { get; set; }
        public string foldername { get; set; }

    }
}
