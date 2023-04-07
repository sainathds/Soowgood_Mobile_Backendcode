using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using videocalling_blazor.Server.Interfaces;
using videocalling_blazor.Server.StaticData;
using videocalling_blazor.Server.StaticData.Manipulator;
using videocalling_blazor.Shared;

namespace videocalling_blazor.Server.Repository
{
    public class BookingRepository : IBookingRepository
    {
        public List<BookData> GetAppointmentDataForBooking(string Id)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@BookId", SqlDbType.NVarChar, Id));            
            var items = Executor.ExecuteStoredProcedure<BookData>("pr_booking_getappointmentdataforbooking", _params);
            return items;
        }

        public List<BookData> sendnotificationatcallstarted(string Id,string usertype)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@BookId", SqlDbType.NVarChar, Id));
            _params.Add(new SqlParam("@usertype", SqlDbType.NVarChar, usertype));
            var items = Executor.ExecuteStoredProcedure<BookData>("pr_booking_sendnotificationatcallstarted", _params);
            return items;
        }


        
    }
}
