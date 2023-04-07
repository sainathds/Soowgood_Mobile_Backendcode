using IdentityMicroservice.Model;
using IdentityMicroservice.ViewModels.ManageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Interfaces
{
    public interface IAdminRepository : IRepository<ApplicationUser>
    {
        public List<BookingViewModel> GetApoointmentBookingList(BookingViewModel model);

        public List<BookingViewModel> getPendingPaymentRequest(BookingViewModel model);
        
    }
}
