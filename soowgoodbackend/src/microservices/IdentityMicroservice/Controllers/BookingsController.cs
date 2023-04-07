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
using IdentityMicroservice.StaticData;
using System.Transactions;
using System.IO;
using IdentityMicroservice.Interfaces;
using IdentityMicroservice.ViewModels.Search;
using IdentityMicroservice.ViewModels;
using Microsoft.Extensions.Configuration;
using AgoraIO.Media;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using IdentityMicroservice.ViewModels.ProviderViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        private String ratingnurl { set; get; }

        private String storeid { set; get; }

        private String storepassword { set; get; }

        private String storeurl { set; get; }

        private String adminemail { set; get; }


        public BookingsController(IdentityMicroserviceContext context, IUserRepository userRepository, IEmailSender emailSender, IBookingRepository bookingRepository, IConfiguration configuration)
        {
            _context = context;
            _bookingRepository = bookingRepository;
            _emailSender = emailSender;
            _userRepository = userRepository;
            ratingnurl = configuration.GetSection("AppSettings").GetSection("ratingnurl").Value;
            storeid = configuration.GetSection("AppSettings").GetSection("storeid").Value;
            storepassword = configuration.GetSection("AppSettings").GetSection("storepassword").Value;
            storeurl = configuration.GetSection("AppSettings").GetSection("storeurl").Value;
            adminemail = configuration.GetSection("AppSettings").GetSection("adminemail").Value;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking()
        {
            return await _context.Booking.ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(string id)
        {
            var booking = await _context.Booking.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(string id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        [HttpPost("GetBooking")]
        public ActionResult<IEnumerable<BookingViewModel>> GetBooking(BookingViewModel model)
        {
            var bookingList = (from a in _context.Booking
                               join b in _context.AppointmentSettings on a.AppointmentSettingId equals b.Id
                               join d in _context.Clinic on b.ClinicId equals d.Id
                               join c in _context.Users on a.ServiceProviderId equals c.Id
                               where a.ServiceReceiverId == model.ServiceReceiverId && d.Id != null
                               select new BookingViewModel
                               {
                                   Id = a.Id,
                                   ServiceProviderId = a.ServiceProviderId,
                                   ServiceProvider = c.FullName,
                                   BookingDate = (DateTime)b.AppointmentSettingDate,
                                   //DayOfWeek = b.DayofWeek,
                                   DayStartingTime = b.DayStartingTime,
                                   DayEndingTime = b.DayEndingTime,
                                   IsBookingConfirmed = a.IsBookingConfirmed,
                                   Address = d.CurrentAddress,
                                   ClinicName = d.Name,
                                   IsBookingCancelledByProvider = a.IsBookingCancelledByProvider,
                                   IsBookingCancelledByReceiver = a.IsBookingCancelledByReceiver,
                                   VisitConfirmationStatus = a.VisitConfirmationStatus,
                                   BeneficiaryComment = a.BeneficiaryComment,
                                   ProviderComment = a.ProviderComment

                               }).ToList();

            if (bookingList == null)
            {
                return NotFound();
            }

            bookingList = bookingList.Skip(model.PageSize * (model.PageNumber)).Take((model.PageSize)).ToList();

            return bookingList;
        }

        [HttpPost("getProviderAppointments")]
        public ActionResult<IEnumerable<BookingViewModel>> GetProviderAppointments(BookingViewModel model)
        {
            List<BookingViewModel> appointmentList = _bookingRepository.GetProviderAppointments(model);
            return appointmentList;
        }

        [HttpPost("GetBookingInfoByDate")]
        public ActionResult<IEnumerable<BookingViewModel>> GetBookingInfoByDate(BookingViewModel model)
        {

            var bookingList = (from a in _context.Booking
                               join b in _context.AppointmentSettings on a.AppointmentSettingId equals b.Id
                               // join f in _context.AppointmentType on b.AppointmentTypeId equals f.Id
                               join d in _context.Clinic on b.ClinicId equals d.Id
                               join c in _context.Users on a.ServiceReceiverId equals c.Id
                               where a.ServiceProviderId == model.ServiceProviderId && d.Id != null
                               select new BookingViewModel
                               {
                                   Id = a.Id,
                                   // AppointmentType = f.Name,
                                   //  AppointmentTypeId = f.Id,
                                   AppointmentSettingId = a.Id,
                                   ClinicName = d.Name,
                                   ClinicId = d.Id,
                                   Address = d.CurrentAddress,
                                   ServiceReceiverId = c.Id,
                                   ServiceReceiver = c.FullName,
                                   BookingDate = a.BookingDate,
                                   Schedule = a.Schedule,
                                   DayOfWeek = a.DayOfWeek,
                                   DayStartingTime = b.DayStartingTime,
                                   DayEndingTime = b.DayEndingTime,
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

            if (!String.IsNullOrEmpty(model.ClinicId))
            {
                bookingList = bookingList.Where(f => f.ClinicId == model.ClinicId).ToList();
            }

            if (!String.IsNullOrEmpty(model.AppointmentTypeId))
            {
                bookingList = bookingList.Where(f => f.AppointmentTypeId == model.AppointmentTypeId).ToList();
            }

            if (model.StartDate != null && model.EndDate != null)
            {
                bookingList = bookingList.Where(f => f.Schedule >= model.StartDate && f.Schedule <= model.EndDate).ToList();
            }


            if (bookingList == null)
            {
                return NotFound();
            }

            bookingList = bookingList.Skip(model.PageSize * (model.PageNumber)).Take((model.PageSize)).ToList();

            return bookingList;
        }



        [HttpPost("GetBookingInfoForPatient")]
        public ActionResult<IEnumerable<BookingViewModel>> GetBookingInfoForPatient(BookingViewModel model)
        {
            var bookingList = (from a in _context.Booking
                               join c in _context.Users on a.ServiceReceiverId equals c.Id
                               join b in _context.UserAddress on c.Id equals b.UserId
                               join d in _context.AppointmentSettings on a.AppointmentSettingId equals d.Id
                               // join f in _context.AppointmentType on d.AppointmentTypeId equals f.Id
                               where a.Id == model.Id
                               select new BookingViewModel
                               {
                                   Id = a.Id,
                                   ServiceReceiver = c.FullName,
                                   patientAddress = a.patientAddress,
                                   patientCurrentAddress = b.CurrentAddress,
                                   patientName = a.patientName,
                                   //AppointmentType = f.Name

                               }).ToList();

            if (bookingList == null)
            {
                return NotFound();
            }
            return bookingList;
        }


        [HttpPost("UpdatePatientInfoIntoBooking")]
        public async Task<ActionResult<IEnumerable<BookingViewModel>>> UpdatePatientInfoIntoBooking(BookingViewModel model)
        {
            try
            {
                var awardInfo = _context.Booking.Where(m => m.Id == model.Id).FirstOrDefault();
                if (awardInfo != null)
                {
                    awardInfo.patientAddress = model.patientAddress;
                    awardInfo.patientName = model.patientName;
                    _context.Booking.Update(awardInfo);
                    await _context.SaveChangesAsync();
                    return GetAllBookingInfo(model.Id);
                }
                else
                    return NotFound();
            }
            catch (DbUpdateException ex)
            {
                return NotFound(); ;
            }

        }




        [HttpPost("BookingInfo")]
        public async Task<ActionResult<IEnumerable<BookingViewModel>>> PostBooking(Booking booking)
        {
            decimal _commission = 0;
            decimal _pricing = 0;
            decimal _patientcharges = 0;
            try
            {
                var bookingInfo = _context.Booking.Any(m => m.AppointmentSettingId == booking.AppointmentSettingId
                                                        && m.ServiceReceiverId == booking.ServiceReceiverId
                                                        && m.appointmentServiceId == booking.appointmentServiceId
                                                        && m.TentativeDate.Day == booking.TentativeDate.Day
                                                        && m.TentativeDate.Month == booking.TentativeDate.Month
                                                        && m.TentativeDate.Year == booking.TentativeDate.Year
                                                        && m.DayOfWeek == booking.DayOfWeek
                                                                            && m.IsCancelled == false
                                                                            && m.IsActive == true
                                                                            && m.IsDeleted == false
                                                                            && m.scheduleStartTime == booking.scheduleStartTime && m.scheduleEndTime == booking.scheduleEndTime);
                if (!bookingInfo && booking.Id == null)
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        _context.Booking.Add(booking);
                        await _context.SaveChangesAsync();
                        scope.Complete();
                    }
                    await GetTentativeScheduleAsync(booking);
                    return GetAllBookingInfo(booking.Id);
                    //return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
                }

                else if (booking.Id != null)
                {
                    var Info = _context.Booking.Any(m => m.Id == booking.Id);

                    if (Info)
                    {
                        await PostUpdateBooking(booking);
                        return GetAllBookingInfo(booking.Id);
                        //return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
                    }
                    else
                        return NotFound();
                }

                else if (bookingInfo)
                {
                    var bookingduplicateInfo = _context.Booking.Where(m => m.AppointmentSettingId == booking.AppointmentSettingId
                                                       && m.ServiceReceiverId == booking.ServiceReceiverId
                                                       && m.TentativeDate == booking.TentativeDate && m.DayOfWeek == booking.DayOfWeek
                                                                           && m.IsCancelled == false).FirstOrDefault();


                    var Info = _context.Booking.Any(m => m.Id == bookingduplicateInfo.Id);
                    booking.Id = bookingduplicateInfo.Id;
                    if (Info)
                    {
                        await PostUpdateBooking(booking);
                        return GetAllBookingInfo(booking.Id);
                        //return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
                    }


                    return GetAllBookingInfo(bookingduplicateInfo.Id);
                }

                else
                    return Conflict();
            }
            catch (Exception ex)
            {
                return NotFound(); ;
            }
        }

        [HttpPost("removeAppointmentBeforePayment")]
        public async Task<ActionResult<Booking>> RemoveAppointmentBeforePayment(BookingViewModel booking)
        {
            try
            {
                var bookingInfo = _context.Booking.Where(m => m.Id == booking.Id).FirstOrDefault();

                if (bookingInfo != null)
                {
                    bookingInfo.IsActive = false;
                    bookingInfo.IsDeleted = true;
                    _context.Booking.Update(bookingInfo);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetBooking", new { id = bookingInfo.Id }, bookingInfo);
                }

                else
                    return NotFound();

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }







        [HttpPost("cancelBookingByPatient")]
        public async Task<ActionResult<IEnumerable<BookingViewModel>>> CancelBookingByPatient(Booking booking)
        {

            try
            {
                string _patientname = "";
                string _appointmentdate = "";
                string _appointmenttime = "";
                string _patientemail = "";
                string _doctorname = "";
                string _doctoremail = "";
                string _patientComment = "";
                var bookingInfo = _context.Booking.Where(m => m.Id == booking.Id).FirstOrDefault();
                if (bookingInfo != null)
                {
                    // bookingInfo.IsActive = false;
                    bookingInfo.IsCancelled = true;
                    bookingInfo.IsBookingCancelledByReceiver = booking.IsBookingCancelledByReceiver;
                    bookingInfo.BeneficiaryComment = booking.BeneficiaryComment;
                    bookingInfo.isrefunded = 1;
                    _context.Booking.Update(bookingInfo);
                    await _context.SaveChangesAsync();

                    var traninfo = _context.appointmenttransactiondetails.Where(m => m.Id == bookingInfo.transactionid).FirstOrDefault();
                    if (traninfo != null)
                    {
                        var result = new paymentrefundinfo();
                        HttpClient client = new HttpClient()
                        {
                            BaseAddress = new Uri(storeurl)
                        };
                        var url = string.Format("/validator/api/merchantTransIDvalidationAPI.php?bank_tran_id={0}&store_id={1}&store_passwd={2}&refund_amount={3}&refund_remarks={4}", traninfo.transno, storeid, storepassword, booking.PaidAmount, booking.BeneficiaryComment);

                        var response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            var stringResponse = await response.Content.ReadAsStringAsync();
                            result = JsonSerializer.Deserialize<paymentrefundinfo>(stringResponse, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                            var objSetting = _context.appointmenttransactiondetails.Find(traninfo.Id);
                            objSetting.refundstatus = result.status;
                            objSetting.refund_refid = result.refund_ref_id;
                            objSetting.refunddate = System.DateTime.Now;
                            _context.appointmenttransactiondetails.Update(objSetting);
                            _context.SaveChanges();

                        }
                        else
                        {
                            throw new HttpRequestException(response.ReasonPhrase);
                        }
                    }
                    var bookingList = (from a in _context.Booking
                                       join c in _context.Users on a.ServiceReceiverId equals c.Id
                                       join p in _context.Users on a.ServiceProviderId equals p.Id
                                       where a.Id == booking.Id
                                       select new BookingViewModel
                                       {
                                           Id = a.Id,
                                           scheduleStartTime = a.scheduleStartTime,
                                           scheduleEndTime = a.scheduleEndTime,
                                           Schedule = a.Schedule,
                                           ServiceProvider = p.FullName,
                                           ServiceReceiver = c.FullName,
                                           ReceiverEmail = c.Email,
                                           ProviderEmail = p.Email,
                                           ProviderComment = a.ProviderComment,
                                           BeneficiaryComment = a.BeneficiaryComment,
                                           ServiceProviderId = a.ServiceProviderId,
                                           ServiceReceiverId = a.ServiceReceiverId


                                       }).ToList();

                    bookingList.ForEach(bookinginfo =>
                    {
                        _patientname = bookinginfo.ServiceReceiver;
                        _appointmentdate = bookinginfo.Schedule.ToString("dddd, dd MMMM yyyy");
                        _appointmenttime = bookinginfo.scheduleStartTime.ToString("g") + " - " + bookinginfo.scheduleEndTime.ToString("g");
                        _patientemail = bookinginfo.ReceiverEmail;
                        _doctorname = bookinginfo.ServiceProvider;
                        _doctoremail = bookinginfo.ProviderEmail;
                        _patientComment = bookinginfo.BeneficiaryComment;

                        if (_patientname == "")
                        {
                            _patientname = _patientemail;
                        }

                        if (_doctorname == "")
                        {
                            _doctorname = _doctoremail;
                        }


                        _emailSender.SendEmail(EmailTemplate.AppointmentCancelByPatient, EmailTemplate.AppointmentCancelToPatientByPatient(_patientname, _appointmentdate, _appointmenttime), new List<string> { _patientemail }, "html");

                        _emailSender.SendEmail(EmailTemplate.AppointmentCancelToProviderByPatient, EmailTemplate.AppointmentCancelToDoctorByPatient(_doctorname, _patientname, _patientComment, _appointmentdate, _appointmenttime), new List<string> { _doctoremail }, "html");



                        notificationmaster objnotification = new notificationmaster();
                        objnotification.isactive = 1;
                        objnotification.isread = 0;
                        objnotification.showpopup = 0;
                        objnotification.isdeleted = 0;
                        objnotification.notificationtext = _patientname + " have cancelled appointment on " + _appointmentdate + " at " + _appointmenttime + ".";
                        objnotification.userid = bookinginfo.ServiceProviderId;
                        objnotification.notificationtype = "Appointment Cancellation";
                        objnotification.usertype = "Provider";
                        objnotification.notificationdate = System.DateTime.Now;
                        _context.notificationmaster.Add(objnotification);
                        _context.SaveChanges();


                        objnotification = new notificationmaster();
                        objnotification.isactive = 1;
                        objnotification.isread = 0;
                        objnotification.showpopup = 0;
                        objnotification.isdeleted = 0;
                        objnotification.notificationtext = "This notification serve that, you have cancelled your appointment on " + _appointmentdate + " at " + _appointmenttime;
                        objnotification.userid = bookinginfo.ServiceReceiverId;
                        objnotification.notificationtype = "Appointment Cancellation";
                        objnotification.usertype = "Beneficial";
                        objnotification.notificationdate = System.DateTime.Now;
                        _context.notificationmaster.Add(objnotification);
                        _context.SaveChanges();


                    });


                }

                return GetAllBookingInfo(booking.Id);

            }
            catch (Exception ex)
            {
                return NotFound(); ;
            }
        }




        [HttpPost("cancelBookingByProvider")]
        public async Task<ActionResult<IEnumerable<BookingViewModel>>> CancelBookingByProvider(Booking booking)
        {

            try
            {
                string _patientname = "";
                string _appointmentdate = "";
                string _appointmenttime = "";
                string _patientemail = "";
                string _doctorname = "";
                string _doctoremail = "";
                string _doctorComment = "";
                var bookingInfo = _context.Booking.Where(m => m.Id == booking.Id).FirstOrDefault();
                if (bookingInfo != null)
                {
                    //bookingInfo.IsActive = false;
                    bookingInfo.IsCancelled = true;
                    bookingInfo.IsBookingCancelledByProvider = booking.IsBookingCancelledByProvider;
                    bookingInfo.ProviderComment = booking.ProviderComment;
                    bookingInfo.isrefunded = 1;
                    _context.Booking.Update(bookingInfo);
                    await _context.SaveChangesAsync();

                    var traninfo = _context.appointmenttransactiondetails.Where(m => m.Id == bookingInfo.transactionid).FirstOrDefault();
                    if (traninfo != null)
                    {
                        var result = new paymentrefundinfo();
                        HttpClient client = new HttpClient()
                        {
                            BaseAddress = new Uri(storeurl)
                        };
                        var url = string.Format("/validator/api/merchantTransIDvalidationAPI.php?bank_tran_id={0}&store_id={1}&store_passwd={2}&refund_amount={3}&refund_remarks={4}", traninfo.transno, storeid, storepassword, booking.PaidAmount, booking.BeneficiaryComment);
                        var response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            var stringResponse = await response.Content.ReadAsStringAsync();
                            result = JsonSerializer.Deserialize<paymentrefundinfo>(stringResponse, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                            var objSetting = _context.appointmenttransactiondetails.Find(traninfo.Id);
                            objSetting.refundstatus = result.status;
                            objSetting.refund_refid = result.refund_ref_id;
                            objSetting.refunddate = System.DateTime.Now;
                            _context.appointmenttransactiondetails.Update(objSetting);
                            _context.SaveChanges();
                        }
                        else
                        {
                            throw new HttpRequestException(response.ReasonPhrase);
                        }
                    }


                    var bookingList = (from a in _context.Booking
                                       join c in _context.Users on a.ServiceReceiverId equals c.Id
                                       join p in _context.Users on a.ServiceProviderId equals p.Id
                                       where a.Id == booking.Id
                                       select new BookingViewModel
                                       {
                                           Id = a.Id,
                                           scheduleStartTime = a.scheduleStartTime,
                                           scheduleEndTime = a.scheduleEndTime,
                                           Schedule = a.Schedule,
                                           ServiceProvider = p.FullName,
                                           ServiceReceiver = c.FullName,
                                           ReceiverEmail = c.Email,
                                           ProviderEmail = p.Email,
                                           ProviderComment = a.ProviderComment,
                                           BeneficiaryComment = a.BeneficiaryComment,
                                           ServiceProviderId = a.ServiceProviderId,
                                           ServiceReceiverId = a.ServiceReceiverId

                                       }).ToList();

                    bookingList.ForEach(bookinginfo =>
                    {
                        _patientname = bookinginfo.ServiceReceiver;
                        _appointmentdate = bookinginfo.Schedule.ToString("dddd, dd MMMM yyyy");
                        _appointmenttime = bookinginfo.scheduleStartTime.ToString("g") + " - " + bookinginfo.scheduleEndTime.ToString("g");
                        _patientemail = bookinginfo.ReceiverEmail;
                        _doctorname = bookinginfo.ServiceProvider;
                        _doctoremail = bookinginfo.ProviderEmail;
                        _doctorComment = bookinginfo.ProviderComment;

                        if (_patientname == "")
                        {
                            _patientname = _patientemail;
                        }

                        if (_doctorname == "")
                        {
                            _doctorname = _doctoremail;
                        }

                        _emailSender.SendEmail(EmailTemplate.AppointmentCancelByDoctor, EmailTemplate.AppointmentCancelToDoctorByDoctor(_doctorname, _appointmentdate, _appointmenttime), new List<string> { _patientemail }, "html");
                        _emailSender.SendEmail(EmailTemplate.AppointmentCancelToPatientByProvider, EmailTemplate.AppointmentCancelToPatientByDoctor(_doctorname, _patientname, _doctorComment, _appointmentdate, _appointmenttime), new List<string> { _doctoremail }, "html");



                        notificationmaster objnotification = new notificationmaster();
                        objnotification.isactive = 1;
                        objnotification.isread = 0;
                        objnotification.showpopup = 0;
                        objnotification.isdeleted = 0;
                        objnotification.notificationtext = _doctorname + " have cancelled appointment on " + _appointmentdate + " at " + _appointmenttime + ".";
                        objnotification.userid = bookinginfo.ServiceReceiverId;
                        objnotification.notificationtype = "Appointment Cancellation";
                        objnotification.usertype = "Beneficial";
                        objnotification.notificationdate = System.DateTime.Now;
                        _context.notificationmaster.Add(objnotification);
                        _context.SaveChanges();


                        objnotification = new notificationmaster();
                        objnotification.isactive = 1;
                        objnotification.isread = 0;
                        objnotification.showpopup = 0;
                        objnotification.isdeleted = 0;
                        objnotification.notificationtext = "This notification serve that, you have cancelled your appointment on " + _appointmentdate + " at " + _appointmenttime;
                        objnotification.userid = bookinginfo.ServiceProviderId;
                        objnotification.notificationtype = "Appointment Cancellation";
                        objnotification.usertype = "Provider";
                        objnotification.notificationdate = System.DateTime.Now;
                        _context.notificationmaster.Add(objnotification);
                        _context.SaveChanges();
                    });
                }

                return GetAllBookingInfo(booking.Id);

            }
            catch (DbUpdateException ex)
            {
                return NotFound(); ;
            }
        }




        [HttpPost("UpdateBooking")]
        public async Task<ActionResult<Booking>> PostUpdateBooking(Booking booking)
        {
            try
            {
                var awardInfo = _context.Booking.Where(m => m.Id == booking.Id).FirstOrDefault();

                if (awardInfo != null)
                {
                    awardInfo.IsBookingConfirmed = booking.IsBookingConfirmed;
                    awardInfo.IsCancelled = booking.IsCancelled;
                    awardInfo.IsActive = booking.IsActive;
                    awardInfo.IsProcessed = booking.IsProcessed;
                    awardInfo.IsRecurrent = booking.IsRecurrent;
                    awardInfo.RecurringHours = booking.RecurringHours;
                    awardInfo.VisitConfirmationStatus = booking.VisitConfirmationStatus;
                    awardInfo.BeneficiaryComment = booking.BeneficiaryComment;
                    awardInfo.ProviderComment = booking.ProviderComment;
                    awardInfo.scheduleStartTime = booking.scheduleStartTime;
                    awardInfo.scheduleEndTime = booking.scheduleEndTime;

                    awardInfo.IsBookingCancelledByProvider = false;
                    awardInfo.IsCancelled = false;
                    awardInfo.IsBookingCancelledByReceiver = false;



                    _context.Booking.Update(awardInfo);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetBooking", new { id = awardInfo.Id }, awardInfo);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }
        }


        [HttpPost("sumbitratingandreview")]
        public async Task<ActionResult<Booking>> sumbitratingandreview(BookingViewModel booking)
        {
            try
            {
                string _patientname = "";
                string _appointmentdate = "";
                string _appointmenttime = "";
                string _patientemail = "";
                string _doctorname = "";
                string _doctoremail = "";
                string _doctorComment = "";

                var awardInfo = _context.Booking.Where(m => m.Id == booking.Id).FirstOrDefault();
                if (awardInfo != null)
                {

                    awardInfo.ratingpoints = booking.ratingpoints;
                    awardInfo.providerreview = booking.providerreview;
                    awardInfo.Status = PaymentStatus.Completed;
                    _context.Booking.Update(awardInfo);
                    await _context.SaveChangesAsync();

                    var bookingList = (from a in _context.Booking
                                       join c in _context.Users on a.ServiceReceiverId equals c.Id
                                       join p in _context.Users on a.ServiceProviderId equals p.Id
                                       where a.Id == booking.Id
                                       select new BookingViewModel
                                       {
                                           Id = a.Id,
                                           scheduleStartTime = a.scheduleStartTime,
                                           scheduleEndTime = a.scheduleEndTime,
                                           Schedule = a.Schedule,
                                           ServiceProvider = p.FullName,
                                           ServiceReceiver = c.FullName,
                                           ReceiverEmail = c.Email,
                                           ProviderEmail = p.Email,
                                           ProviderComment = a.ProviderComment,
                                           BeneficiaryComment = a.BeneficiaryComment,
                                           ServiceProviderId = a.ServiceProviderId,
                                           ServiceReceiverId = a.ServiceReceiverId

                                       }).ToList();

                    bookingList.ForEach(bookinginfo =>
                    {
                        _patientname = bookinginfo.ServiceReceiver;
                        _appointmentdate = bookinginfo.Schedule.ToString("dddd, dd MMMM yyyy");
                        _appointmenttime = bookinginfo.scheduleStartTime.ToString("g") + " - " + bookinginfo.scheduleEndTime.ToString("g");
                        _patientemail = bookinginfo.ReceiverEmail;
                        _doctorname = bookinginfo.ServiceProvider;
                        _doctoremail = bookinginfo.ProviderEmail;
                        _doctorComment = bookinginfo.ProviderComment;

                        if (_patientname == "")
                        {
                            _patientname = _patientemail;
                        }

                        if (_doctorname == "")
                        {
                            _doctorname = _doctoremail;
                        }

                        //_emailSender.SendEmail(EmailTemplate.AppointmentCancelByDoctor, EmailTemplate.AppointmentCancelToDoctorByDoctor(_doctorname, _appointmentdate, _appointmenttime), new List<string> { _patientemail }, "html");
                        //_emailSender.SendEmail(EmailTemplate.AppointmentCancelToPatientByProvider, EmailTemplate.AppointmentCancelToPatientByDoctor(_doctorname, _patientname, _doctorComment, _appointmentdate, _appointmenttime), new List<string> { _doctoremail }, "html");



                        notificationmaster objnotification = new notificationmaster();
                        objnotification.isactive = 1;
                        objnotification.isread = 0;
                        objnotification.showpopup = 0;
                        objnotification.isdeleted = 0;
                        objnotification.notificationtext = "Appointment on " + _appointmentdate + " at " + _appointmenttime + " has been marked completed by " + _patientname + ".";
                        objnotification.userid = bookinginfo.ServiceProviderId;
                        objnotification.notificationtype = "Appointment Completed";
                        objnotification.usertype = "Provider";
                        objnotification.notificationdate = System.DateTime.Now;
                        _context.notificationmaster.Add(objnotification);
                        _context.SaveChanges();



                    });



                    return CreatedAtAction("GetBooking", new { id = awardInfo.Id }, awardInfo);
                }
                else
                    return NotFound();

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }




        [HttpPost("addupdateBookingDocument")]
        public async Task<ActionResult<IEnumerable<bookingdocuments>>> PostBookingDocument([FromForm] BookingDocumentViewModel bookingdocument)
        {
            string filename = "";
            try
            {
                bookingdocuments bookingdoc = new bookingdocuments();
                bookingdoc.bookingid = bookingdocument.bookingid;
                var uploadFolderUrl = Directory.GetCurrentDirectory() + "\\Data\\appointmentdoc";
                var file = bookingdocument.File;

                //var file = model.File;
                if (file != null)
                {
                    if (!Directory.Exists(uploadFolderUrl))
                        Directory.CreateDirectory(uploadFolderUrl);

                    filename = file.FileName.ToLower();
                    string fullpath = Path.Combine(uploadFolderUrl, file.FileName.ToLower());
                    bool isexists = false;
                    int j = 1;
                    while (isexists == false)
                    {
                        fullpath = Path.Combine(uploadFolderUrl, filename);
                        if (System.IO.File.Exists(fullpath))
                        {
                            string filewithouthext = Path.GetFileNameWithoutExtension(fullpath);
                            string fileextension = Path.GetExtension(fullpath);
                            filename = filewithouthext + "_" + j.ToString() + fileextension.ToLower();
                            j = j + 1;
                        }
                        else
                        {
                            isexists = true;
                        }
                    }
                    using (var fileStream = new FileStream(fullpath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    bookingdoc.filename = filename;
                    _context.BookingDocuments.Add(bookingdoc);
                    await _context.SaveChangesAsync();
                    return GetBookingDocument(bookingdocument.bookingid);

                }
                else
                {
                    return NotFound();
                }
            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }
        }

        [HttpPost("getBookingDocument")]
        public ActionResult<IEnumerable<bookingdocuments>> GetBookingDocument(string bookingid)
        {

            var booking = (from a in _context.BookingDocuments
                           where a.bookingid == bookingid
                           select new bookingdocuments
                           {
                               Id = a.Id,
                               bookingid = a.bookingid,
                               filename = a.filename
                           }).ToList();

            if (booking == null)
            {
                return NotFound();
            }
            else
            {
                return booking;
            }


        }



        [HttpPost("deleteBookingDocument")]
        public ActionResult<IEnumerable<bookingdocuments>> DeleteBookingDocument(bookingdocuments bookingdocument)
        {
            authentication _userauth = new authentication();
            try
            {
                var uploadFolderUrl = Directory.GetCurrentDirectory() + "\\Data\\appointmentdoc";
                string fullpath = string.Empty;
                var bookingInfo = _context.BookingDocuments.Find(bookingdocument.Id);
                if (bookingInfo != null)
                {

                    fullpath = Path.Combine(uploadFolderUrl, bookingInfo.filename);
                    if (System.IO.File.Exists(fullpath))
                    {
                        System.IO.File.Delete(fullpath);

                    }
                    _context.BookingDocuments.Remove(bookingInfo);
                    _context.SaveChanges();
                    return GetBookingDocument(bookingdocument.bookingid);

                }
                else
                {
                    return NotFound();
                }

            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }

        }




        [HttpPost("Schedule")]
        public async Task<ActionResult<IEnumerable<BookingViewModel>>> GetTentativeScheduleAsync(Booking booking)
        {
            try
            {
                var bookingInfo = _context.Booking.Where(m => m.AppointmentSettingId == booking.AppointmentSettingId &&
                                                            //m.ServiceReceiverId == booking.ServiceReceiverId &&
                                                            m.IsActive == true && m.IsCancelled == false && m.IsBookingCancelledByReceiver == false);

                var appointmentInfo = _context.AppointmentSettings.Where(m => m.Id == booking.AppointmentSettingId &&
                                                            m.ServiceProviderId == booking.ServiceProviderId &&
                                                            m.IsActive == true).FirstOrDefault();


                if (bookingInfo != null && appointmentInfo != null)
                {
                    int NoOfPatient = bookingInfo.Count();
                    // DateTime _datetime = booking.TentativeDate.Add(appointmentInfo.DayStartingTime).AddMinutes(NoOfPatient * appointmentInfo.TimeSlot);
                    booking.Schedule = booking.TentativeDate;
                    booking.TentativeTime = booking.TentativeDate.TimeOfDay;
                    booking.TentativeDate = booking.TentativeDate.Date;
                    booking.ReportingTime = booking.TentativeDate.AddMinutes(-30).TimeOfDay;
                    booking.SerialNo = NoOfPatient;
                    _context.Booking.Update(booking);
                    await _context.SaveChangesAsync();
                    return GetAllBookingInfo(booking.Id);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }
        }

        [HttpPost("GetAllBookingInfo")]
        public ActionResult<IEnumerable<BookingViewModel>> GetAllBookingInfo(String BookingId)
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

        [HttpPost("BookingInfoById")]
        public ActionResult<IEnumerable<BookingViewModel>> GetBookingInfoById(BookingViewModel model)
        {
            var appointmentList = (from a in _context.Booking
                                   join b in _context.AppointmentSettings on a.AppointmentSettingId equals b.Id
                                   join c in _context.Clinic on b.ClinicId equals c.Id
                                   join d in _context.Users on a.ServiceProviderId equals d.Id
                                   join e in _context.Users on a.ServiceReceiverId equals e.Id
                                   where a.Id == model.Id
                                   select new BookingViewModel
                                   {
                                       Id = a.Id,
                                       AppointmentSettingId = a.AppointmentSettingId,
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
                                       VisitConfirmationStatus = a.VisitConfirmationStatus,
                                       BeneficiaryComment = a.BeneficiaryComment,
                                       ProviderComment = a.ProviderComment,
                                       PaidAmount = a.PaidAmount

                                   }).ToList();

            if (appointmentList == null)
            {
                return NotFound();
            }

            appointmentList = appointmentList.Skip(model.PageSize * (model.PageNumber)).Take((model.PageSize)).ToList();
            return appointmentList;
        }

        [HttpPost("getbookinghistoryforpayment")]
        public ActionResult<IEnumerable<BookingViewModel>> GetBookingHistoryForPayment(BookingViewModel model)
        {
            List<BookingViewModel> appointmentList = _bookingRepository.getbookinghistoryforpayment(model);
            return appointmentList;
        }


        [HttpPost("processPayment")]
        public async Task<ActionResult<appointmenttransactiondetails>> PostProcessPayment(BookingViewModel model)
        {
            try
            {
                string[] bookingData = model.Id.Split(',');
                string refno = "";
                bool isuniqueno = false;
                while (isuniqueno == false)
                {
                    refno = create16digitstring();
                    if (refno != "")
                    {
                        var transactionInfo = _context.appointmenttransactiondetails.Where(m => m.refno == refno).FirstOrDefault();
                        if (transactionInfo == null)
                        {
                            isuniqueno = true;
                        }
                        else
                        {
                            isuniqueno = false;
                            refno = "";
                        }
                    }
                    else
                    {
                        isuniqueno = false;
                    }
                }
                appointmenttransactiondetails objtrans = new appointmenttransactiondetails();
                objtrans.refno = refno;
                objtrans.transstatus = PaymentStatus.InProcess;
                objtrans.trancurrency = model.trancurrency;
                objtrans.sourcefrom = model.sourcefrom;
                _context.appointmenttransactiondetails.Add(objtrans);
                await _context.SaveChangesAsync();
                if (objtrans.Id != null)
                {
                    for (var i = 0; i < bookingData.Length; i++)
                    {
                        var bookingInfo = _context.Booking.Where(m => m.Id == bookingData[i]).FirstOrDefault();
                        if (bookingInfo != null)
                        {
                            bookingInfo.transactionid = objtrans.Id;
                            _context.Booking.Update(bookingInfo);
                            await _context.SaveChangesAsync();
                        }
                    }
                    return objtrans;
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }



        [HttpPost("getbookingtransactiondata")]
        public async Task<ActionResult<appointmenttransactiondetails>> Getbookingtransactiondata(BookingViewModel model)
        {
            try
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

                List<BookingViewModel> appointmentList = _bookingRepository.getconfirmbooking(model);

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


                    string visitingaddress = "";

                    if (_appointmenttype == "Clinic")
                    {
                        visitingaddress = _clientname;
                    }
                    else if (_appointmenttype == "Online")
                    {
                        visitingaddress = _appointmenttype;
                    }
                    else
                    {
                        visitingaddress = _patientAddress;
                    }

                    notificationmaster objnotification = new notificationmaster();
                    objnotification.isactive = 1;
                    objnotification.isread = 0;
                    objnotification.showpopup = 0;
                    objnotification.isdeleted = 0;
                    objnotification.notificationtext = "Your appointment on [" + _appointmentdate + " - " + _appointmenttime + "] with " + _doctorname + " at " + visitingaddress + " has been confirmed.";
                    objnotification.userid = bookinginfo.ServiceReceiverId;
                    objnotification.notificationtype = "Appointment Confirmation";
                    objnotification.usertype = "Beneficial";
                    objnotification.notificationdate = System.DateTime.Now;
                    _context.notificationmaster.Add(objnotification);
                    _context.SaveChanges();


                    objnotification = new notificationmaster();
                    objnotification.isactive = 1;
                    objnotification.isread = 0;
                    objnotification.showpopup = 0;
                    objnotification.isdeleted = 0;
                    objnotification.notificationtext = "You have new appointment for " + _appointmenttype + " on " + _appointmentdate + " - " + _appointmenttime + ".";
                    objnotification.userid = bookinginfo.ServiceProviderId;
                    objnotification.notificationtype = "Appointment Confirmation";
                    objnotification.usertype = "Provider";
                    objnotification.notificationdate = System.DateTime.Now;
                    _context.notificationmaster.Add(objnotification);
                    _context.SaveChanges();


                });
                var tranInfo = _context.appointmenttransactiondetails.Where(m => m.Id == model.Id).FirstOrDefault();
                return tranInfo;
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }




        public string create16digitstring()
        {
            Random objrng = new Random();
            var builder = new StringBuilder();
            while (builder.Length < 16)
            {
                builder.Append(objrng.Next(10).ToString());
            }
            return builder.ToString();
        }


        [HttpPost("patientAppointmentHistory")]
        public ActionResult<IEnumerable<BookingViewModel>> GetPatientAppointmentHistory(SearchParameterViewModel model)
        {
            List<BookingViewModel> appointmentList = _bookingRepository.GetPatientAppointmentHistory(model);
            appointmentList = appointmentList.Skip(model.PageSize * (model.PageNumber)).Take((model.PageSize)).ToList();
            return appointmentList;
        }




        [HttpPost("providerAppointmentHistory")]
        public ActionResult<IEnumerable<BookingViewModel>> GetProviderAppointmentHistory(SearchParameterViewModel model)
        {
            List<BookingViewModel> appointmentList = _bookingRepository.GetProviderAppointmentHistory(model);

            appointmentList = appointmentList.Skip(model.PageSize * (model.PageNumber)).Take((model.PageSize)).ToList();
            return appointmentList;
        }



        [HttpPost("checkAppointmentCancellation")]
        public ActionResult<IEnumerable<authentication>> GetBookingCancellationCondition(SearchParameterViewModel model)
        {
            List<authentication> appointmentList = _bookingRepository.BookingCancellationCondition(model);
            return appointmentList;
        }



        [HttpPost("GetProviderPatients")]
        public ActionResult<IEnumerable<BookingViewModel>> getProviderPatients(UserViewModel model)
        {
            List<BookingViewModel> appointmentList = _bookingRepository.GetProviderPatients(model);
            return appointmentList;
        }

        [HttpPost("getPatientAppointmentDocumentHistory")]
        public ActionResult<IEnumerable<BookingViewModel>> GetPatientAppointmentDocumentHistory(BookingViewModel model)
        {

            var uploadFolderUrl = Directory.GetCurrentDirectory() + "\\Data\\appointmentdoc";
            List<BookingViewModel> bookingInfo = _bookingRepository.GetPatientAppointmentDocumentHistory(model);

            bookingInfo.ForEach(booking =>
            {
                string fullpath = Path.Combine(uploadFolderUrl, booking.documentname);
                if (System.IO.File.Exists(fullpath))
                {
                    booking.IsActive = true;
                }
                else
                {
                    booking.IsActive = true;
                }

            });
            return bookingInfo;

        }


        [HttpPost("getProviderBookingBillDetails")]
        public ActionResult<IEnumerable<BookingViewModel>> GetProviderBookingBillDetails(BookingViewModel model)
        {
            List<BookingViewModel> appointmentList = _bookingRepository.GetProviderBookingBillDetails(model);
            return appointmentList;
        }


        [HttpPost("makeRequestForPayBack")]
        public async Task<ActionResult<BookingViewModel>> MakeRequestForPayBack(BookingViewModel booking)
        {
            try
            {

                string _appointmentdate = "";
                string _appointmenttime = "";
                string _doctorname = "";
                string _doctoremail = "";
                string _bookingid = "";
                string _appointmenttype = "";
                double _doctorcharges = 0;
                string _adminemail = "";


                bookingpayback objbookingpayback = new bookingpayback();
                objbookingpayback.bookingid = booking.Id;
                objbookingpayback.paybackstatus = "Requested";
                objbookingpayback.requestdate = System.DateTime.Now;
                _context.bookingpayback.Add(objbookingpayback);
                await _context.SaveChangesAsync();

                var bookingList = (from a in _context.Booking
                                   join c in _context.Users on a.ServiceReceiverId equals c.Id
                                   join p in _context.Users on a.ServiceProviderId equals p.Id
                                   where a.Id == booking.Id
                                   select new BookingViewModel
                                   {
                                       Id = a.Id,
                                       scheduleStartTime = a.scheduleStartTime,
                                       scheduleEndTime = a.scheduleEndTime,
                                       Schedule = a.Schedule,
                                       ServiceProvider = p.FullName,
                                       ServiceReceiver = c.FullName,
                                       ReceiverEmail = c.Email,
                                       ProviderEmail = p.Email,
                                       ProviderComment = a.ProviderComment,
                                       BeneficiaryComment = a.BeneficiaryComment,
                                       ServiceProviderId = a.ServiceProviderId,
                                       ServiceReceiverId = a.ServiceReceiverId,
                                       doctorcharges = Convert.ToDouble(a.doctorcharges)

                                   }).ToList();

                bookingList.ForEach(bookinginfo =>
                {
                    _appointmentdate = bookinginfo.Schedule.ToString("dddd, dd MMMM yyyy");
                    _appointmenttime = bookinginfo.scheduleStartTime.ToString("g") + " - " + bookinginfo.scheduleEndTime.ToString("g");
                    _doctorname = bookinginfo.ServiceProvider;
                    _doctoremail = bookinginfo.ProviderEmail;
                    _doctorcharges = bookinginfo.doctorcharges;

                    if (_doctorname == "")
                    {
                        _doctorname = _doctoremail;
                    }
                    _adminemail = adminemail;
                    _emailSender.SendEmail(EmailTemplate.RequestforPayment, EmailTemplate.RequestforPaymentToAdmin(_doctorname, _appointmentdate, _appointmenttime, _doctorcharges), new List<string> { _adminemail }, "html");


                });
                return CreatedAtAction("GetProviderBookingBillDetails", new { id = booking.Id });


            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }



        [HttpPost("markePaymentComplete")]
        public async Task<ActionResult<BookingViewModel>> MarkePaymentComplete(BookingViewModel booking)
        {
            try
            {
                string _appointmentdate = "";
                string _appointmenttime = "";
                string _doctorname = "";
                string _doctoremail = "";
                string _bookingid = "";
                string _appointmenttype = "";
                double _doctorcharges = 0;


                var paybackInfo = _context.bookingpayback.Where(m => m.Id == booking.paybackid).FirstOrDefault();
                if (paybackInfo != null)
                {
                    paybackInfo.paybackstatus = "Completed";
                    paybackInfo.processdate = System.DateTime.Now;
                    _context.bookingpayback.Update(paybackInfo);
                    await _context.SaveChangesAsync();


                    var bookingList = (from a in _context.Booking
                                       join c in _context.Users on a.ServiceReceiverId equals c.Id
                                       join p in _context.Users on a.ServiceProviderId equals p.Id
                                       where a.Id == paybackInfo.bookingid
                                       select new BookingViewModel
                                       {
                                           Id = a.Id,
                                           scheduleStartTime = a.scheduleStartTime,
                                           scheduleEndTime = a.scheduleEndTime,
                                           Schedule = a.Schedule,
                                           ServiceProvider = p.FullName,
                                           ServiceReceiver = c.FullName,
                                           ReceiverEmail = c.Email,
                                           ProviderEmail = p.Email,
                                           ProviderComment = a.ProviderComment,
                                           BeneficiaryComment = a.BeneficiaryComment,
                                           ServiceProviderId = a.ServiceProviderId,
                                           ServiceReceiverId = a.ServiceReceiverId,
                                           doctorcharges = Convert.ToDouble(a.doctorcharges)

                                       }).ToList();

                    bookingList.ForEach(bookinginfo =>
                    {
                        _appointmentdate = bookinginfo.Schedule.ToString("dddd, dd MMMM yyyy");
                        _appointmenttime = bookinginfo.scheduleStartTime.ToString("g") + " - " + bookinginfo.scheduleEndTime.ToString("g");
                        _doctorname = bookinginfo.ServiceProvider;
                        _doctoremail = bookinginfo.ProviderEmail;
                        _doctorcharges = bookinginfo.doctorcharges;

                        if (_doctorname == "")
                        {
                            _doctorname = _doctoremail;
                        }
                        _emailSender.SendEmail(EmailTemplate.CompleteRequestforPayment, EmailTemplate.CompleteRequestforPaymentToAdmin(_doctorname, _appointmentdate, _appointmenttime, _doctorcharges), new List<string> { _doctoremail }, "html");


                        notificationmaster objnotification = new notificationmaster();
                        objnotification.isactive = 1;
                        objnotification.isread = 0;
                        objnotification.showpopup = 0;
                        objnotification.isdeleted = 0;
                        objnotification.notificationtext = "Your request for payment of " + _doctorcharges + "  against appointment on Dated " + _appointmentdate + " has been processed successfully by SoowGood Team.";
                        objnotification.userid = bookinginfo.ServiceProviderId;
                        objnotification.notificationtype = "Payment Request Completed";
                        objnotification.usertype = "Provider";
                        objnotification.notificationdate = System.DateTime.Now;
                        _context.notificationmaster.Add(objnotification);
                        _context.SaveChanges();

                    });



                    return CreatedAtAction("GetProviderBookingBillDetails", new { id = booking.Id });
                }
                else
                    return NotFound();

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }



        [HttpPost("getReviewAndRating")]
        public ActionResult<IEnumerable<BookingViewModel>> GetReviewAndRatingdata(BookingViewModel model)
        {
            List<BookingViewModel> appointmentList = _bookingRepository.GetReviewAndRatingdata(model);
            return appointmentList;
        }



        [HttpPost("GetProviderDataForBooking")]
        public ActionResult<IEnumerable<BookingViewModel>> GetProviderDataForBooking(BookingViewModel model)
        {
            List<BookingViewModel> appointmentList = _bookingRepository.GetProviderDataForBooking(model);
            return appointmentList;
        }

        [HttpPost("checkAppointmentForVideoCall")]
        public ActionResult<IEnumerable<BookingViewModel>> checkAppointmentForVideoCall(BookingViewModel booking)
        {
            var apoointmentBooking = _bookingRepository.checkAppointmentForVideoCall(booking.Id);
            return apoointmentBooking;
        }

        [HttpGet("sendappointmentlink")]
        public ActionResult<IEnumerable<BookingViewModel>> sendappointmentlink()
        {
            string _patientname = "";
            string _appointmentdate = "";
            string _appointmenttime = "";
            string _patientemail = "";
            string _doctorname = "";
            string _doctoremail = "";
            string _patientComment = "";
            string id = "";
            string _appointmentday = "";
            var apoointmentBooking = _bookingRepository.sendappointmentlink();

            apoointmentBooking.ForEach(bookinginfo =>
            {
                _patientname = bookinginfo.ServiceReceiver;
                _appointmentdate = bookinginfo.ScheduleAppointmentDate;
                _appointmenttime = bookinginfo.scheduleTime;
                _patientemail = bookinginfo.ReceiverEmail;
                _doctorname = bookinginfo.ServiceProvider;
                _doctoremail = bookinginfo.ProviderEmail;
                _patientComment = bookinginfo.BeneficiaryComment;
                id = bookinginfo.Id;
                _appointmentday = bookinginfo.DayOfWeek;
                if (_patientname == "")
                {
                    _patientname = _patientemail;
                }

                if (_doctorname == "")
                {
                    _doctorname = _doctoremail;
                }
                string _appointmentdoctorlink = "https://meet.soowgood.com/?_data=" + id + "&_usertype=Provider";
                string _appointmentpatientlink = "https://meet.soowgood.com/?_data=" + id + "&_usertype=Patient"; ;

                string _subject = EmailTemplate.OnlineAppointment + " @ " + _appointmentdate + " " + _appointmenttime;

                _emailSender.SendEmail(_subject, EmailTemplate.AppointmentSchedulePatient(_patientname, _doctorname, _appointmentdate, _appointmenttime, _appointmentday, _appointmentpatientlink), new List<string> { _patientemail }, "html");
                _emailSender.SendEmail(_subject, EmailTemplate.AppointmentScheduleDoctor(_doctorname, _patientname, _appointmentdate, _appointmenttime, _appointmentday, _appointmentdoctorlink), new List<string> { _doctoremail }, "html");

                notificationmaster objnotification = new notificationmaster();
                objnotification.isactive = 1;
                objnotification.isread = 0;
                objnotification.showpopup = 0;
                objnotification.isdeleted = 0;
                objnotification.notificationtext = "Your online visit with " + _doctorname + " will start in a few minutes. Please check email for appointment link.";
                objnotification.userid = bookinginfo.ServiceReceiverId;
                objnotification.notificationtype = "Online Appointment";
                objnotification.usertype = "Beneficial";
                objnotification.notificationdate = System.DateTime.Now;
                _context.notificationmaster.Add(objnotification);
                _context.SaveChanges();


                objnotification = new notificationmaster();
                objnotification.isactive = 1;
                objnotification.isread = 0;
                objnotification.showpopup = 0;
                objnotification.isdeleted = 0;
                objnotification.notificationtext = "Your online visit with " + _patientname + " will start in a few minutes. Please check email for appointment link.";
                objnotification.userid = bookinginfo.ServiceProviderId;
                objnotification.notificationtype = "Online Appointment";
                objnotification.usertype = "Provider";
                objnotification.notificationdate = System.DateTime.Now;
                _context.notificationmaster.Add(objnotification);
                _context.SaveChanges();

            });
            return apoointmentBooking;
        }




        //[HttpPost("GenerateAccessTokenForAgora")]
        //public ActionResult<AuthenticateResponse> GenerateAccessTokenForAgora(AuthenticateRequest request)
        //{


        //    authentication objauth = new authentication();
        //    AccessToken tokenBuilder = new AccessToken(agorappid, appcertificate, request.channel, request.uid);
        //    tokenBuilder.addPrivilege(Privileges.kJoinChannel, request.expiredTs);
        //    tokenBuilder.addPrivilege(Privileges.kPublishAudioStream, request.expiredTs);
        //    tokenBuilder.addPrivilege(Privileges.kPublishVideoStream, request.expiredTs);
        //    tokenBuilder.addPrivilege(Privileges.kPublishDataStream, request.expiredTs);
        //    return Ok(new AuthenticateResponse
        //    {
        //        channel = request.channel,
        //        uid = request.uid,
        //        token = tokenBuilder.build()
        //    });
        //}





        [HttpPost("PHistory")]
        public ActionResult<IEnumerable<BookingViewModel>> GetBookingPHistory(BookingViewModel model)
        {
            var appointmentList = (from a in _context.Booking
                                   join b in _context.AppointmentSettings on a.AppointmentSettingId equals b.Id
                                   join c in _context.Clinic on b.ClinicId equals c.Id
                                   join d in _context.Users on a.ServiceProviderId equals d.Id
                                   join e in _context.Users on a.ServiceReceiverId equals e.Id
                                   where a.ServiceProviderId == model.ServiceProviderId && a.IsActive == true
                                   //&& a.IsCancelled == false && a.IsBookingConfirmed == true

                                   select new BookingViewModel
                                   {
                                       Id = a.Id,
                                       AppointmentSettingId = a.Id,
                                       ClinicName = c.Name,
                                       Address = c.CurrentAddress,
                                       ServiceReceiverId = e.Id,
                                       ServiceReceiver = e.FullName,
                                       ReceiverImage = e.ProfilePicture,
                                       ReceiverEmail = e.Email,
                                       ReceiverPhone = e.PhoneNumber,

                                       ServiceProviderId = d.Id,
                                       ServiceProvider = d.FullName,
                                       ProviderImage = d.ProfilePicture,
                                       ProviderEmail = d.Email,
                                       ProviderPhone = d.PhoneNumber,

                                       BookingDate = a.BookingDate,
                                       Schedule = a.Schedule,
                                       DayOfWeek = a.DayOfWeek,
                                       SerialNo = a.SerialNo,
                                       ReportingTime = a.ReportingTime,
                                       AppointmentDate = a.TentativeDate,
                                       AppointmentTime = a.TentativeTime,
                                       VisitConfirmationStatus = a.VisitConfirmationStatus,
                                       BeneficiaryComment = a.BeneficiaryComment,
                                       ProviderComment = a.ProviderComment
                                   }).ToList();

            if (appointmentList == null)
            {
                return NotFound();
            }

            appointmentList = appointmentList.Skip(model.PageSize * (model.PageNumber)).Take((model.PageSize)).ToList();
            return appointmentList;
        }

        [HttpPost("BookingPaymentInfo")]
        public async Task<ActionResult<IEnumerable<BookingViewModel>>> GetBookingPaymentInfoAsync(BookingViewModel model)
        {

            if (model.VisitConfirmationStatus)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var bookingInfo = _context.Booking.Where(m => m.Id == model.Id).FirstOrDefault();
                    bookingInfo.VisitConfirmationStatus = true;
                    bookingInfo.IsProcessed = true;
                    _context.Booking.Update(bookingInfo);
                    await _context.SaveChangesAsync();
                    scope.Complete();
                }

                var appointmentList = (from a in _context.Booking
                                       join b in _context.AppointmentSettings on a.AppointmentSettingId equals b.Id
                                       join c in _context.Clinic on b.ClinicId equals c.Id
                                       join d in _context.Users on a.ServiceProviderId equals d.Id
                                       join e in _context.Users on a.ServiceReceiverId equals e.Id
                                       where a.IsActive == true
                                       && a.IsCancelled == false && a.Id == model.Id

                                       select new BookingViewModel
                                       {
                                           Id = a.Id,
                                           AppointmentSettingId = a.Id,
                                           ClinicName = c.Name,
                                           Address = c.CurrentAddress,
                                           ServiceReceiverId = e.Id,
                                           ServiceReceiver = e.FullName,
                                           ReceiverImage = e.ProfilePicture,
                                           ReceiverEmail = e.Email,
                                           ReceiverPhone = e.PhoneNumber,

                                           ServiceProviderId = d.Id,
                                           ServiceProvider = d.FullName,
                                           ProviderImage = d.ProfilePicture,
                                           ProviderEmail = d.Email,
                                           ProviderPhone = d.PhoneNumber,

                                           BookingDate = a.BookingDate,
                                           Schedule = a.Schedule,
                                           DayOfWeek = a.DayOfWeek,
                                           SerialNo = a.SerialNo,
                                           ReportingTime = a.ReportingTime,
                                           AppointmentDate = a.TentativeDate,
                                           AppointmentTime = a.TentativeTime,
                                           UserPaymentStatus = a.UserPaymentStatus,
                                           VisitConfirmationStatus = a.VisitConfirmationStatus,
                                           BeneficiaryComment = a.BeneficiaryComment,
                                           ProviderComment = a.ProviderComment
                                       }).ToList();

                appointmentList = appointmentList.Skip(model.PageSize * (model.PageNumber)).Take((model.PageSize)).ToList();
                return appointmentList;
            }
            else
                return NotFound();
        }



        [HttpPost("getServiceReceiverForPrescription")]
        public ActionResult<IEnumerable<BookingViewModel>> GetServiceReceiverForPrescription(BookingViewModel model)
        {
            var apoointmentBooking = _bookingRepository.GetServiceReceiverForPrescription(model);
            return apoointmentBooking;

        }

        [HttpPost("getDoctorVisitForPatient")]
        public ActionResult<IEnumerable<BookingViewModel>> GetDoctorVisitForPatient(BookingViewModel model)
        {
            var apoointmentBooking = _bookingRepository.GetDoctorVisitForPatient(model);
            return apoointmentBooking;

        }


        [HttpPost("getServiceProviderForPrescription")]
        public ActionResult<IEnumerable<BookingViewModel>> GetServiceProviderForPrescription(BookingViewModel model)
        {
            var apoointmentBooking = _bookingRepository.GetServiceProviderForPrescription(model);
            return apoointmentBooking;

        }



        [HttpPost("getPrescription")]
        public ActionResult<IEnumerable<BookingViewModel>> getPrescriptionData(BookingViewModel model)
        {
            var apoointmentBooking = _bookingRepository.getPrescriptionData(model);
            return apoointmentBooking;

        }



        [HttpPost("deletePrescriptionInfo")]
        public ActionResult<authentication> DeletePrescriptionInfo(PrescriptionViewModel model)
        {
            var prescriptioninfo = _bookingRepository.DeletePrescriptionInfo(model);
            var uploadFolderUrl = Directory.GetCurrentDirectory() + "\\Data\\DoctorSignature";
            var prescriptionpdfUrl = Directory.GetCurrentDirectory() + "\\Data\\prescriptionpdf";
            prescriptioninfo.ForEach(prescription =>
            {
                string oldfullpath = Path.Combine(uploadFolderUrl, prescription.signaturename.ToLower());
                if (System.IO.File.Exists(oldfullpath))
                {
                    System.IO.File.Delete(oldfullpath);
                }
                oldfullpath = Path.Combine(prescriptionpdfUrl, prescription.Id + ".pdf");
                if (System.IO.File.Exists(oldfullpath))
                {
                    System.IO.File.Delete(oldfullpath);
                }
            });
            authentication objauthentication = new authentication();
            objauthentication.success = "1";
            objauthentication.message = "Prescription information deleted successfully.";
            return objauthentication;
        }





        [HttpPost("getprescriptiondurgdetails")]
        public async Task<ActionResult<IEnumerable<prescriptiondurgdetails>>> GetPrescriptionDurgDetails(PrescriptionViewModel _objprescriptionmodel)
        {
            var durgInfo = await _context.prescriptiondurgdetails.Where(m => m.prescriptionid == _objprescriptionmodel.Id).ToListAsync(); ;
            return durgInfo;
        }


        [HttpPost("getprescriptionmedicaltestdetails")]
        public async Task<ActionResult<IEnumerable<prescriptionmedicaltestdetails>>> GetPrescriptionMedicalTestDetails(PrescriptionViewModel _objprescriptionmodel)
        {
            var medicaltestInfo = await _context.prescriptionmedicaltestdetails.Where(m => m.prescriptionid == _objprescriptionmodel.Id).ToListAsync(); ;
            return medicaltestInfo;
        }


        [HttpPost("getprescriptionadvicetestdetails")]
        public async Task<ActionResult<IEnumerable<prescriptionadvicedetails>>> GetPrescriptionAdviceDetails(PrescriptionViewModel _objprescriptionmodel)
        {
            var advicedeInfo = await _context.prescriptionadvicedetails.Where(m => m.prescriptionid == _objprescriptionmodel.Id).ToListAsync(); ;
            return advicedeInfo;
        }

        [HttpPost("getprescriptionbyid")]
        public async Task<ActionResult<IEnumerable<prescriptionmaster>>> GetPrescriptionById(PrescriptionViewModel _objprescriptionmodel)
        {
            var advicedeInfo = await _context.prescriptionmaster.Where(m => m.Id == _objprescriptionmodel.Id).ToListAsync(); ;
            return advicedeInfo;
        }

        [HttpPost("savePrescriptionInformation")]
        public async Task<ActionResult<prescriptionmaster>> PostPrescription([FromForm] PrescriptionViewModel _objprescriptionmodel)
        {
            try
            {
                string signaturename = "";
                var uploadFolderUrl = Directory.GetCurrentDirectory() + "\\Data\\DoctorSignature";
                var staticimgurl = Directory.GetCurrentDirectory() + "\\Data\\staticimg";
                string filename = string.Empty;

                if (_objprescriptionmodel.Id != null)
                {
                    var objprescription = await _context.prescriptionmaster.FindAsync(_objprescriptionmodel.Id);
                    _objprescriptionmodel.bookingId = objprescription.bookingId;
                    objprescription.ServiceProviderId = _objprescriptionmodel.ServiceProviderId;
                    objprescription.ServiceReceiverId = _objprescriptionmodel.ServiceReceiverId;
                    objprescription.prescriptiondate = _objprescriptionmodel.prescriptiondate;
                    objprescription.diognosis = _objprescriptionmodel.diognosis;
                    //  objprescription.bookingId = _objprescriptionmodel.bookingId;

                    var file = _objprescriptionmodel.File;
                    if (file != null)
                    {

                        if (objprescription.signaturename != null)
                        {
                            if (objprescription.signaturename.Length > 0)
                            {
                                string oldfullpath = Path.Combine(uploadFolderUrl, objprescription.signaturename.ToLower());
                                if (System.IO.File.Exists(oldfullpath))
                                {
                                    System.IO.File.Delete(oldfullpath);
                                }
                            }
                        }

                        filename = file.FileName.ToLower();
                        string fullpath = Path.Combine(uploadFolderUrl, file.FileName.ToLower());
                        bool isexists = false;
                        int j = 1;
                        while (isexists == false)
                        {
                            fullpath = Path.Combine(uploadFolderUrl, filename);
                            if (System.IO.File.Exists(fullpath))
                            {
                                string filewithouthext = Path.GetFileNameWithoutExtension(fullpath);
                                string fileextension = Path.GetExtension(fullpath);
                                filename = filewithouthext + "_" + j.ToString() + fileextension.ToLower();
                                j = j + 1;
                            }
                            else
                            {
                                isexists = true;
                            }
                        }
                        using (var fileStream = new FileStream(fullpath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        objprescription.signaturename = filename;
                    }
                    _context.prescriptionmaster.Update(objprescription);
                    await _context.SaveChangesAsync();
                    signaturename = objprescription.signaturename;
                }
                else
                {
                    prescriptionmaster objprescription = new prescriptionmaster();
                    objprescription.ServiceProviderId = _objprescriptionmodel.ServiceProviderId;
                    objprescription.ServiceReceiverId = _objprescriptionmodel.ServiceReceiverId;
                    objprescription.prescriptiondate = _objprescriptionmodel.prescriptiondate;
                    objprescription.diognosis = _objprescriptionmodel.diognosis;
                    objprescription.bookingId = _objprescriptionmodel.bookingId;


                    var file = _objprescriptionmodel.File;
                    if (file != null)
                    {
                        filename = file.FileName.ToLower();
                        string fullpath = Path.Combine(uploadFolderUrl, file.FileName.ToLower());
                        bool isexists = false;
                        int j = 1;
                        while (isexists == false)
                        {
                            fullpath = Path.Combine(uploadFolderUrl, filename);
                            if (System.IO.File.Exists(fullpath))
                            {
                                string filewithouthext = Path.GetFileNameWithoutExtension(fullpath);
                                string fileextension = Path.GetExtension(fullpath);
                                filename = filewithouthext + "_" + j.ToString() + fileextension.ToLower();
                                j = j + 1;
                            }
                            else
                            {
                                isexists = true;
                            }
                        }
                        using (var fileStream = new FileStream(fullpath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        objprescription.signaturename = filename;
                    }


                    _context.prescriptionmaster.Add(objprescription);
                    await _context.SaveChangesAsync();
                    _objprescriptionmodel.Id = objprescription.Id;
                    signaturename = objprescription.signaturename;

                }



                var bookinglist = (from a in _context.Booking
                                   join b in _context.AppointmentSettings on a.AppointmentSettingId equals b.Id
                                   join c in _context.Clinic on b.ClinicId equals c.Id
                                   where a.Id == _objprescriptionmodel.bookingId
                                   select new BookingViewModel
                                   {
                                       Id = a.Id,
                                       AppointmentSettingId = a.AppointmentSettingId,
                                       ClinicName = c.Name,
                                       Address = c.CurrentAddress

                                   }).ToList();


                var prescriptionData = _bookingRepository.addupdateprescriptiondetails(_objprescriptionmodel);


                var provideruserdata = _userRepository.getuserdatabyid(_objprescriptionmodel.ServiceProviderId);
                var patientuserdata = _userRepository.getuserdatabyid(_objprescriptionmodel.ServiceReceiverId);

                var medicaltestInfo = _context.prescriptionmedicaltestdetails.Where(m => m.prescriptionid == _objprescriptionmodel.Id).ToList();

                var advicedetailsInfo = _context.prescriptionadvicedetails.Where(m => m.prescriptionid == _objprescriptionmodel.Id).ToList();


                var durgInfo = _context.prescriptiondurgdetails.Where(m => m.prescriptionid == _objprescriptionmodel.Id).ToList();

                int i = 1;
                string html = "<div style='width:100%;text-align:center;font-size:12px;'>";
                provideruserdata.ForEach(provideruser =>
                {
                    html = html + "<h1 style='font-size:15px;'>" + provideruser.fullname + "</h1>" +
                    "<p>" + provideruser.specialization + "</p>";
                    bookinglist.ForEach(booking =>
                    {
                        html = html + "<p>" + booking.Address + "</p>";

                    });
                    html = html + "<p><img src='" + Path.Combine(staticimgurl, "email.jpg") + "' style='width:10px;height:10px;margin-right:5px;'/>&nbsp;" + provideruser.email + " | <img src='" + Path.Combine(staticimgurl, "telephone.jpg") + "' style='width:10px;height:10px;margin-right:5px;' />&nbsp;" + provideruser.mobileno + "</p>";


                });
                html = html + "</div>" +
                    "<div style='width:100%;'><hr/></div>" +
                    "<div style='width:100%;height:850px;'>" +
                                "<div style='width:35%;float:left;height:850px'>" +
                                     "<div style='width:100%;text-align:center;'>" +
                                                        "<h1 style='font-size:15px;'>Tests</h1>" +
                                     "</div>" +
                                     "<div style='width:100%;'>";
                medicaltestInfo.ForEach(medicaltest =>
                {
                    html = html + "<p style='text-align:left;font-size:12px;'>" + i + ") " + medicaltest.medicaltest + "</p>";
                    i = i + 1;
                });
                html = html + "</div>" +
                "<div style='width:100%;text-align:center;'>" +
                    "<h1 style='font-size:15px;'>Advice</h1>" +
                "</div>" +
                "<div style='width:100%;'>";
                i = 1;
                advicedetailsInfo.ForEach(advicedetails =>
                {
                    html = html + "<p style='text-align:left;font-size:12px;'>" + i + ") " + advicedetails.advice + "</p>";
                    i = i + 1;
                });
                html = html + "</div>" +
"</div>" +
"<div style='height: 850px;float:left;width:1px;background-color: black;margin-right:50px;margin-left:20px;'></div>" +
"<div style='float:left;height:850px;padding:10px 10px 10px 30px;'>" +
"<div style='width:100%;'>";

                patientuserdata.ForEach(patientdata =>
                {
                    html = html + "<div style='width:40%;font-size:12px;float:left;'><b> Name :</b>" + patientdata.fullname + "</div>" +
                    "<div style='width:20%;font-size:12px;float:left;'><b> Age :</b>" + patientdata.age + "</div>" +
                    "<div style='font-size:12px;float:left;'><b> Gender :</b>" + patientdata.gendertext + "</div>" +
                    "<div style='font-size:12px;float:left;padding-left:20px;'><b> Date :</b>" + _objprescriptionmodel.prescriptiondate.ToString("dd MMM yyyy") + "</div>";

                });


                html = html + "</div>" +

                     "<div style='width:100%;padding-top:15px;'>" +
                           "<p style='font-size:12px;'><b> Diagnosis : " + _objprescriptionmodel.diognosis + "</b></p>" +
                 "</div>" +
                "<div style='width:100%;'>" +
                           "<h1 style='text-align:left;'>Rx</h1>" +
                 "</div>" +
                "<div style='width:100%;text-align:center;'>";
                i = 1;
                durgInfo.ForEach(durg =>
                {
                    html = html + "<div style='width:100%;text-align:left;margin-bottom:10px'>" +
                                    "<p style='text-align:left;font-size:13px;'>" + i + ").<b>" + durg.durgname + "</b></p>" +
                                    "<p style='text-align:left;font-size:12px;'>" + durg.weeklyschedule + " - <span style='right:0px'>" + durg.timing + " (" + durg.dose + ") </span> </p>" +

                                  "</div>";

                    i = i + 1;
                });
                html = html +
            "</div>" +
             "<div style='width:100%;'>" +
             "<div style='width:35%;float:left;height:100px;'></div>" +
             "<div style='float:left;height:200px;text-align:center;'>" +
                "<img src='" + Path.Combine(uploadFolderUrl, signaturename) + "' style='width:150px;height:100px;' />";
                provideruserdata.ForEach(provideruser =>
                {
                    html = html + "<p><h2 style='font-size:13px;'><b>" + provideruser.fullname + "</b></h2></p>" +
                    "<p><img src='" + Path.Combine(staticimgurl, "email.jpg") + "' style='width:10px;height:10px;margin-right:5px;'/>&nbsp;" + provideruser.email + "</p>" +
                    "<p><img src='" + Path.Combine(staticimgurl, "telephone.jpg") + "' style='width:10px;height:10px;margin-right:5px;' />&nbsp;" + provideruser.mobileno + "</p>";
                });
                html = html + "</div>" +
"</div>" +
        "</div>" +
"</div>";

                Export(html, _objprescriptionmodel.Id);

            }
            catch (Exception ex)
            {
            }
            var objprescription1 = _context.prescriptionmaster.Where(m => m.Id == _objprescriptionmodel.Id).FirstOrDefault();
            return objprescription1;
        }




        [HttpPost("updatepaymentransaction")]
        public async Task<authentication> updatepaymentransaction(BookingViewModel model)
        {
            authentication objauth = new authentication();
            var apoointmentBooking = _bookingRepository.updatepaymentransaction(model);
            objauth.success = "1";
            objauth.message = "Transaction done Successfully!";
            return objauth;
        }



        public void Export(string GridHtml, string Id)
        {
            try
            {
                var uploadFolderUrl = Directory.GetCurrentDirectory() + "\\Data\\prescriptionpdf";
                string filename = Id + ".pdf";
                byte[] bytes;
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    StringReader sr = new StringReader(GridHtml);
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    bytes = stream.ToArray();
                }
                string filePath = Path.Combine(uploadFolderUrl, filename);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                System.IO.File.WriteAllBytes(filePath, bytes);
            }
            catch (Exception ex)
            {
            }
        }




        // POST: api/Bookings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        //{
        //    _context.Booking.Add(booking);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        //}

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Booking>> DeleteBooking(string id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        private bool BookingExists(string id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }

        public static bool HasOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return Min(start1, end1) < Max(start2, end2) && Max(start1, end1) > Min(start2, end2);
        }

        public static DateTime Max(DateTime d1, DateTime d2)
        {
            return d1 > d2 ? d1 : d2;
        }

        public static DateTime Min(DateTime d1, DateTime d2)
        {
            return d2 > d1 ? d1 : d2;
        }
    }
}
