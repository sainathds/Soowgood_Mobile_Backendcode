using IdentityMicroservice.Data;
using IdentityMicroservice.Interfaces;
using IdentityMicroservice.Model;
using IdentityMicroservice.StaticData.Manipulator;
using IdentityMicroservice.ViewModels.ManageViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Repository
{
    public class AdminRepository : BaseRepository<ApplicationUser>, IAdminRepository
    {
        private readonly IdentityMicroserviceContext _context;
        public AdminRepository(IdentityMicroserviceContext context) : base(context) { _context = context; }

        public List<BookingViewModel> GetApoointmentBookingList(BookingViewModel model)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@bookingstatus", SqlDbType.NVarChar, model.bookingtype));
            var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_admin_getApoointmentBooking", _params);
            return items;
        }


        public List<BookingViewModel> getPendingPaymentRequest(BookingViewModel model)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();                
                _params.Add(new SqlParam("@AppointmentTypeName", SqlDbType.NVarChar, model.AppointmentTypeName));
                _params.Add(new SqlParam("@paybackstatus", SqlDbType.NVarChar, model.paybackstatus));
                var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_admin_getPendingPaymentRequest", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
