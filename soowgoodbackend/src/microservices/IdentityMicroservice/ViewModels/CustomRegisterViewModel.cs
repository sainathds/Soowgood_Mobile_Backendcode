using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels
{
    public class CustomRegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RegPassword { get; set; }
        public string RegEmail { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
