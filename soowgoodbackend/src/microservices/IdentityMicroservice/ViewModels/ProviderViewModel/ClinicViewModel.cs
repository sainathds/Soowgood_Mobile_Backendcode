using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ProviderViewModel
{
    public class ClinicViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OptionalAddress { get; set; }
        public string CurrentAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string ImageURL { get; set; }
        public string ImageURLOptionalOne { get; set; }
        public string ImageURLOptionalTwo { get; set; }
        public string ImageURLOptionalThree { get; set; }
        public string UserId { get; set; }
        public IFormFile File1 { get; set; }
        public IFormFile File2 { get; set; }
        public IFormFile File3 { get; set; }
        public bool IsImageURL { get; set; }
        public string IsImageURLOptionalOne { get; set; }
        public string IsImageURLOptionalTwo { get; set; }
        public string IsImageURLOptionalThree { get; set; }
    }
}
