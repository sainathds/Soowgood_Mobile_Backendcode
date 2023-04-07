using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class CmnMenuList: TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long MenuId { get; set; }
        public long ModuleId { get; set; } = 0;
        public string MenuName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IconPath { get; set; }
        public long? ParentMenuId { get; set; }
        public string Report { get; set; }
        public string ReportPath { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
