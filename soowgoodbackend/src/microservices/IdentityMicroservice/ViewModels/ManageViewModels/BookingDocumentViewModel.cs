using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ManageViewModels
{
    public class BookingDocumentViewModel
    {
        public string Id { get; set; }
        public string bookingid { get; set; }
        public IFormFile File { get; set; }

        public string filetype { get; set; }
    }
}
