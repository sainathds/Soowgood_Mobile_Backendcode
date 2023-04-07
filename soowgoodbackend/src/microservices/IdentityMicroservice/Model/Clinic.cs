using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class Clinic: TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
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

        [Required]
        public string UserId { get; set; }
    }
}
