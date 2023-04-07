using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using IdentityMicroservice.ViewModels.ProviderViewModel;
using IdentityMicroservice.StaticData;

namespace IdentityMicroservice.Data
{
    public class IdentityMicroserviceContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityMicroserviceContext (DbContextOptions<IdentityMicroserviceContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            builder.Entity<IdentityRole>().ToTable("AspNetRoles");
            builder.Entity<ApplicationRoleType>().ToTable("ApplicationRoleType");
            builder.Entity<UserCurrentRole>().ToTable("UserCurrentRole");
            builder.Entity<ApplicationBank>().ToTable("ApplicationBank");
            builder.Entity<ApplicationBankBranch>().ToTable("ApplicationBankBranch");
            builder.Entity<UserAddress>().ToTable("UserAddress");
            builder.Entity<UserBankAccount>().ToTable("UserBankAccount");
            builder.Entity<UserPoint>().ToTable("UserPoint");
        }

        public DbSet<IdentityMicroservice.Model.UserAddress> UserAddress { get; set; }

        public DbSet<IdentityMicroservice.Model.Clinic> Clinic { get; set; }

        public DbSet<IdentityMicroservice.Model.UserAboutMe> UserAboutMe { get; set; }

        public DbSet<IdentityMicroservice.Model.Pricing> Pricing { get; set; }

        public DbSet<IdentityMicroservice.Model.Degree> Degree { get; set; }

        public DbSet<IdentityMicroservice.Model.Experience> Experience { get; set; }

        public DbSet<IdentityMicroservice.Model.providerdocument> ProviderDocument { get; set; }

        public DbSet<IdentityMicroservice.Model.bookingdocuments> BookingDocuments { get; set; }

        public DbSet<IdentityMicroservice.Model.Awards> Awards { get; set; }

        public DbSet<IdentityMicroservice.Model.Membership> Membership { get; set; }

        public DbSet<IdentityMicroservice.Model.Specialization> Specialization { get; set; }

        public DbSet<IdentityMicroservice.Model.AppointmentSettings> AppointmentSettings { get; set; }

        public DbSet<IdentityMicroservice.Model.AppointmentDaySettings> AppointmentDaySettings { get; set; }


        public DbSet<IdentityMicroservice.Model.AppointmentServiceTypeSettings> AppointmentServiceTypeSettings { get; set; }

        public DbSet<IdentityMicroservice.Model.appointmentdaymaster> appointmentdaymaster { get; set; }

        public DbSet<IdentityMicroservice.Model.Booking> Booking { get; set; }


        public DbSet<IdentityMicroservice.Model.notificationmaster> notificationmaster { get; set; }

        public DbSet<IdentityMicroservice.Model.bookingpayback> bookingpayback { get; set; }

        public DbSet<IdentityMicroservice.Model.appointmenttransactiondetails> appointmenttransactiondetails { get; set; }

        public DbSet<IdentityMicroservice.Model.CmnMenuList> CmnMenuList { get; set; }

        public DbSet<IdentityMicroservice.Model.CmnMenuPermissionToGroup> CmnMenuPermissionToGroup { get; set; }

        public DbSet<IdentityMicroservice.Model.CmnUserPermissionToGroup> CmnUserPermissionToGroup { get; set; }

        public DbSet<IdentityMicroservice.Model.ProviderCategoryInfo> ProviderCategoryInfo { get; set; }

        public DbSet<IdentityMicroservice.Model.PaymentMethod> PaymentMethod { get; set; }

        public DbSet<IdentityMicroservice.Model.PaymentHistory> PaymentHistory { get; set; }

        public DbSet<IdentityMicroservice.Model.ProviderInfo> ProviderInfo { get; set; }

        public DbSet<IdentityMicroservice.Model.PricingPlan> PricingPlan { get; set; }

        public DbSet<IdentityMicroservice.Model.Disease> Disease { get; set; }

        public DbSet<IdentityMicroservice.Model.Symptom> Symptom { get; set; }

        public DbSet<IdentityMicroservice.Model.LifeStyle> LifeStyle { get; set; }

        public DbSet<IdentityMicroservice.Model.AccChartOfAccount> AccChartOfAccount { get; set; }

        public DbSet<IdentityMicroservice.Model.AccJournalType> AccJournalType { get; set; }

        public DbSet<IdentityMicroservice.Model.AccJournalMaster> AccJournalMaster { get; set; }

        public DbSet<IdentityMicroservice.Model.AccJournalDetail> AccJournalDetail { get; set; }

        public DbSet<IdentityMicroservice.Model.AccPartyBalance> AccPartyBalance { get; set; }

        public DbSet<IdentityMicroservice.Model.TaskType> TaskType { get; set; }

        public DbSet<IdentityMicroservice.Model.UserPaymentMethod> UserPaymentMethod { get; set; }

        public DbSet<IdentityMicroservice.Model.Search> Search { get; set; }

        public DbSet<IdentityMicroservice.Model.UserAccessibleInfo> UserAccessibleInfo { get; set; }

        public DbSet<IdentityMicroservice.Model.UserDiseaseInfo> UserDiseaseInfo { get; set; }

        public DbSet<IdentityMicroservice.Model.UserLifeStyleInfo> UserLifeStyleInfo { get; set; }

        public DbSet<IdentityMicroservice.Model.Medicine> Medicine { get; set; }

        public DbSet<IdentityMicroservice.Model.UserMedicine> UserMedicine { get; set; }

        public DbSet<IdentityMicroservice.Model.AccessibleInfo> AccessibleInfo { get; set; }

        public DbSet<IdentityMicroservice.Model.Communication> Communication { get; set; }

        public DbSet<IdentityMicroservice.Model.ContactInfo> ContactInfo { get; set; }

        public DbSet<IdentityMicroservice.Model.AppointmentType> AppointmentType { get; set; }

        public DbSet<IdentityMicroservice.Model.TaskChargeSetting> TaskChargeSetting { get; set; }

        public DbSet<IdentityMicroservice.Model.ProviderBillDetails> ProviderBillDetails { get; set; }

        public DbSet<IdentityMicroservice.Model.prescriptionmaster> prescriptionmaster { get; set; }
        public DbSet<IdentityMicroservice.Model.prescriptiondurgdetails> prescriptiondurgdetails { get; set; }
        public DbSet<IdentityMicroservice.Model.prescriptionmedicaltestdetails> prescriptionmedicaltestdetails { get; set; }
        public DbSet<IdentityMicroservice.Model.prescriptionadvicedetails> prescriptionadvicedetails { get; set; }

        public DbSet<IdentityMicroservice.Model.userdeviceinfo> userdeviceinfo { get; set; }

        public DbQuery<ScheduleAppointment> scheduleappointmentlist { get; set; }
    }
}
