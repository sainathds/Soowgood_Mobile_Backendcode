using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace videocalling_blazor.Server.StaticData
{
    public class BookingData
    {
        public string Id { get; set; }
        public string ServiceProvider { get; set; }
        public string ServiceReceiver { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
    }
}
