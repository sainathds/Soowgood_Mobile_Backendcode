using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using videocalling_blazor.Server.Data;
using videocalling_blazor.Server.Interfaces;
using videocalling_blazor.Server.StaticData;
using videocalling_blazor.Shared;

namespace videocalling_blazor.Server.Controllers
{
    [Route("/api/appointment")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly SoowgoodDbContext _context;
        private readonly IBookingRepository _bookingRepository;
        public AppointmentController(SoowgoodDbContext context, IBookingRepository bookingRepository)
        {
            _context = context;
            _bookingRepository = bookingRepository;
        }


        [HttpPost("GetAppointmentDataForBooking")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<BookData>> GetAppointmentDataForBooking(BookData model)
        {
            var apoointmentBooking = _bookingRepository.GetAppointmentDataForBooking(model.Id);
            return apoointmentBooking;
        }

        [HttpPost("sendnotificationatcallstarted")]
        [Produces("application/json")]
        public ActionResult<IEnumerable<BookData>> sendnotificationatcallstarted(BookData model)
        {
            var apoointmentBooking = _bookingRepository.sendnotificationatcallstarted(model.Id,model.usertype);
            return apoointmentBooking;
        }

        


    }
}
