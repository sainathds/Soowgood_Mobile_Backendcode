using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ProviderViewModel
{
    public class ProviderDocumentViewModel
    {
        public string Id { get; set; }
        public string documentname { get; set; }
        public string documentfilename { get; set; }        
        public string UserId { get; set; }

        public IFormFile File { get; set; }

        public string filetype { get; set; }
        
    }
}
