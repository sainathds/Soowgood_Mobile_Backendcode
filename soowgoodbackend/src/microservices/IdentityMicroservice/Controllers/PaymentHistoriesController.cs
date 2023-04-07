using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using IdentityMicroservice.ViewModels.ManageViewModels;
using System.Transactions;
using IdentityMicroservice.StaticData;
using IdentityMicroservice.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentHistoriesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private String ratingnurl { set; get; }
        public PaymentHistoriesController(IdentityMicroserviceContext context, IEmailSender emailSender, IBookingRepository bookingRepository, IConfiguration configuration)
        {
            _context = context;
            _bookingRepository = bookingRepository;
            _emailSender = emailSender;
            ratingnurl = configuration.GetSection("AppSettings").GetSection("ratingnurl").Value;
        }

        // GET: api/PaymentHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentHistory>>> GetPaymentHistory()
        {
            return await _context.PaymentHistory.ToListAsync();
        }

        // GET: api/PaymentHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentHistory>> GetPaymentHistory(string id)
        {
            var paymentHistory = await _context.PaymentHistory.FindAsync(id);

            if (paymentHistory == null)
            {
                return NotFound();
            }

            return paymentHistory;
        }

        [HttpPost("GetPaymentInfo")]
        public ActionResult<IEnumerable<PaymentHistoryViewModel>> GetPaymentInfo(PaymentHistoryViewModel model)
        {
            var PaymentHistory = (from a in _context.PaymentHistory
                                  join b in _context.PaymentMethod on a.UserPaymentMethodId equals b.Id
                                  join c in _context.Users on a.ReceiverUserId equals c.Id
                                  where a.SenderUserId == model.SenderUserId && a.IsActive == true

                                  select new PaymentHistoryViewModel
                                  {
                                      MenuId = a.MenuId,
                                      TransactionId = a.TransactionId,
                                      TransactionTime = a.TransactionTime,
                                      TransactionOrigin = a.TransactionOrigin,
                                      Amount = a.Amount,
                                      AmountPaidTo = c.FullName,
                                      PaymentStaus = a.PaymentStaus
                                  }).ToList();

            if (PaymentHistory == null)
            {
                return NotFound();
            }
            return PaymentHistory;
        }

        [HttpPost("GetDefaultInfo")]
        public ActionResult<PaymentHistoryViewModel> GetDefaultInfo(PaymentHistoryViewModel model)
        {
            var PaymentHistory = (from a in _context.PaymentHistory
                                  where a.SenderUserId == model.SenderUserId && a.IsActive == true

                                  select new PaymentHistoryViewModel
                                  {
                                      Email = a.Email,
                                      NameOnCard = a.NameOnCard,
                                      CardExpiredDate = a.CardExpiredDate,
                                      CardNumber = a.CardNumber,
                                      CVV = a.CVV,
                                      IsInfoSaved = a.IsInfoSaved,
                                      SenderUserId = a.SenderUserId
                                  }).OrderByDescending(p => p.TransactionTime).FirstOrDefault();

            if (PaymentHistory == null)
            {
                return NotFound();
            }
            return PaymentHistory;
        }

       

        [HttpPost("CancelOtherPendingBooking")]
        public ActionResult<IEnumerable<BookingViewModel>> CancelOtherPendingBooking(string bookingids)
        {
            string _patientname = "";
            string _appointmentdate = "";
            string _appointmenttime = "";
            string _patientemail = "";
            string _doctorname = "";
            string _doctoremail = "";
            string _patientComment = "";
            string _bookingid = "";
            string _clientname = "";
            string _appointmenttype = "";
            string _patientAddress = "";
            if (bookingids.Length > 0)
            {
                bookingids = bookingids.Substring(1, bookingids.Length - 1);
            }

            for (var i = 0; i < bookingids.Split(',').Length; i++)
            {
                BookingViewModel objbooking = new BookingViewModel();
                objbooking.Id = bookingids.Split(',')[i];
                List<BookingViewModel> appointmentList = _bookingRepository.CancelPendingBooking(objbooking);

                appointmentList.ForEach(bookinginfo =>
                {
                    _patientname = bookinginfo.ServiceReceiver;
                    _appointmentdate = bookinginfo.ScheduleAppointmentDate;
                    _appointmenttime = bookinginfo.scheduleTime; ;
                    _patientemail = bookinginfo.ReceiverEmail;
                    _doctorname = bookinginfo.ServiceProvider;
                    _doctoremail = bookinginfo.ProviderEmail;
                    _patientComment = bookinginfo.BeneficiaryComment;
                    _bookingid = bookinginfo.Id;
                    _appointmenttype = bookinginfo.AppointmentTypeName;
                    _clientname = bookinginfo.ClinicName;
                    _patientAddress = bookinginfo.patientAddress;
                    _emailSender.SendEmail(EmailTemplate.AppointmentConfirmedByPatient, EmailTemplate.AppointmentConfirmedToPatient(_patientname, _doctorname, _clientname, _appointmenttype, _patientAddress, ratingnurl + _bookingid, _appointmentdate, _appointmenttime), new List<string> { _patientemail }, "html");
                    _emailSender.SendEmail(EmailTemplate.AppointmentConfirmedByPatient, EmailTemplate.AppointmentConfirmedToDoctor(_doctorname, _patientname, _patientAddress, _clientname, _appointmenttype, _appointmentdate, _appointmenttime), new List<string> { _doctoremail }, "html");

                });
            }
            return GetAllBookingData("");
        }



        [HttpPost("GetAllBookingData")]
        public ActionResult<IEnumerable<BookingViewModel>> GetAllBookingData(String BookingId)
        {
            var appointmentList = (from a in _context.Booking
                                   join b in _context.AppointmentSettings on a.AppointmentSettingId equals b.Id
                                   join c in _context.Clinic on b.ClinicId equals c.Id
                                   join d in _context.Users on a.ServiceProviderId equals d.Id
                                   join e in _context.Users on a.ServiceReceiverId equals e.Id
                                   where a.Id == BookingId
                                   select new BookingViewModel
                                   {
                                       Id = a.Id,
                                       AppointmentSettingId = a.Id,
                                       ClinicName = c.Name,
                                       Address = c.CurrentAddress,
                                       ServiceReceiverId = d.Id,
                                       ServiceReceiver = d.FullName,
                                       ServiceProviderId = e.Id,
                                       ServiceProvider = e.FullName,
                                       BookingDate = a.BookingDate,
                                       Schedule = a.Schedule,
                                       DayOfWeek = a.DayOfWeek,
                                       SerialNo = a.SerialNo,
                                       ReportingTime = a.ReportingTime,
                                       AppointmentDate = a.TentativeDate,
                                       AppointmentTime = a.TentativeTime,
                                       TentativeDate = a.TentativeDate,
                                       TentativeTime = a.TentativeTime,
                                       IsBookingConfirmed = a.IsBookingConfirmed,
                                       IsCancelled = a.IsCancelled,
                                       IsActive = a.IsActive,
                                       IsProcessed = a.IsProcessed,
                                       IsRecurrent = a.IsRecurrent,
                                       RecurringHours = a.RecurringHours,
                                       VisitConfirmationStatus = a.VisitConfirmationStatus,
                                       PaidAmount = a.PaidAmount,
                                       BeneficiaryComment = a.BeneficiaryComment,
                                       ProviderComment = a.ProviderComment

                                   }).ToList();

            if (appointmentList == null)
            {
                return NotFound();
            }

            return appointmentList;
        }


        // PUT: api/PaymentHistories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentHistory(string id, PaymentHistory paymentHistory)
        {
            if (id != paymentHistory.Id)
            {
                return BadRequest();
            }

            _context.Entry(paymentHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentHistoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PaymentHistories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        // DELETE: api/PaymentHistories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentHistory>> DeletePaymentHistory(string id)
        {
            var paymentHistory = await _context.PaymentHistory.FindAsync(id);
            if (paymentHistory == null)
            {
                return NotFound();
            }

            _context.PaymentHistory.Remove(paymentHistory);
            await _context.SaveChangesAsync();

            return paymentHistory;
        }

        private bool PaymentHistoryExists(string id)
        {
            return _context.PaymentHistory.Any(e => e.Id == id);
        }
    }
}
