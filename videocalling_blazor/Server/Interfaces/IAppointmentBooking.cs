using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using videocalling_blazor.Server.Model;
using videocalling_blazor.Server.StaticData;
using videocalling_blazor.Shared;

namespace videocalling_blazor.Server.Interfaces
{
    public interface IBookingRepository  
    {

        List<BookData> GetAppointmentDataForBooking(string Id);

        List<BookData> sendnotificationatcallstarted(string Id,string usertype);
        

    }
}
