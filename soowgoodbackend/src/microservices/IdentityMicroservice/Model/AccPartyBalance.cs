using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class AccPartyBalance: TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public string UserTypeId { get; set; }
        public string COAID { get; set; }
        public string MenuID { get; set; }
        public string ReferenceId { get; set; }
        public string UserId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal BillAmount { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal CollectionAmount { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal AdvancedReceivedAmount { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal BalanceAmount { get; set; }

        public decimal Status { get; set; }
    }
}
