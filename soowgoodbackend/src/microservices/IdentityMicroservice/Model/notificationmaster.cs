using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class notificationmaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        public string notificationtext { get; set; }


        public int isactive { get; set; }

        public string userid { get; set; }

        public string notificationtype { get; set; }

        public string usertype { get; set; }
        

        public int isread { get; set; }

        public int isdeleted { get; set; }

        public int showpopup { get; set; }

        public DateTime notificationdate { get; set; }
    }
}
