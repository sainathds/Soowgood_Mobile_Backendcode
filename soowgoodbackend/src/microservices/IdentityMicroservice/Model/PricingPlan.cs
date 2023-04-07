using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Model
{
    public class PricingPlan: TableHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserTypeId { get; set; }
        public DateTime PlanStartingDate { get; set; }
        public DateTime TrialPeriodOver { get; set; }
        public long NumberOfFreeUsers { get; set; } = 0;
        public decimal AmountPerUser { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal Commission { get; set; }
        public decimal RegistrationFees { get; set; } = 0;
        public decimal YearlyFees { get; set; } = 0;
        public bool IsDiscount { get; set; } = false;
        public bool IsTrialPeriodOver { get; set; } = false;
        public bool IsOrganization { get; set; } = false;
        public bool IsFree { get; set; } = false;
        public bool IsMonthly { get; set; } = false;
        public bool IsWeekly { get; set; } = false;
        public bool IsCustomized { get; set; } = false;
        public bool IsYearly { get; set; } = false;
        public bool IsDefault { get; set; } = false;
        public string Currency { get; set; } = CultureInfo.CurrentCulture.Name;
        public string CurrencySymbol { get; set; } = NumberFormatInfo.CurrentInfo.CurrencySymbol;

    }
}
