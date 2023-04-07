using System;
using System.Collections.Generic;
using System.Text;

namespace videocalling_blazor.Shared
{
    
    public class BookData
    {
        public string? Id { get; set; } = "";
        public string? ServiceProvider { get; set; } = "";
        public string? ServiceReceiver { get; set; } = "";
        public string? AppointmentDate { get; set; } = "";
        public string? scheduleTime { get; set; } = "";

        public string? usertype { get; set; } = "";
        public string? callstatus { get; set; } = "";
    }
}
