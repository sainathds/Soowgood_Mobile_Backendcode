using IdentityMicroservice.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ProviderViewModel
{
    public class PrescriptionViewModel
    {
        public IFormFile File { get; set; }


        public string Id { get; set; }

        public string bookingId { get; set; }
        

        public string ServiceProviderId { get; set; }

        public string ServiceReceiverId { get; set; }

        public DateTime prescriptiondate { get; set; }


        public string signaturename { get; set; }

        public string prescriptiondurgdetails { get; set; }

        public string prescriptionmaster { get; set; }

        public string prescriptionadvicedetails { get; set; }

        public string prescriptionmedicaltestdetails { get; set; }

        public string diognosis { get; set; }
        
    }
}
