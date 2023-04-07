using IdentityMicroservice.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.ViewModels.ProviderViewModel
{
    public class PricingViewModel: TableHistory
    {
        public string Id { get; set; }
        public string PlanId { get; set; }
        public long NumberOfUsers { get; set; } = 1;
        public decimal TotalAmount { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long FreeUsagesDurationInDays { get; set; } = 0;
        public long UserLimit { get; set; } = 0;
        public decimal AmountPerUser { get; set; } = 0;
        public long NumberOfFreeUsers { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
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
        public string UserTypeId { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Commission { get; set; }
    }
}
