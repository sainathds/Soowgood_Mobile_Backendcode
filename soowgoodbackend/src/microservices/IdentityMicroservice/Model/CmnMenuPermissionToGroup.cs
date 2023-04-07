using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class CmnMenuPermissionToGroup: TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        [ForeignKey(nameof(Menu))]
        public long MenuId { get; set; }
        public CmnMenuList Menu { get; set; }
        [ForeignKey(nameof(Role))]
        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }
        public bool EnableInsert { get; set; } = true;
        public bool EnableUpdate { get; set; } = true;
        public bool EnableDelete { get; set; } = true;
        public bool EnableView { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }
}
