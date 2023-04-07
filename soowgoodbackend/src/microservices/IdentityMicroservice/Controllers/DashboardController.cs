using IdentityMicroservice.Data;
using IdentityMicroservice.Interfaces;
using IdentityMicroservice.Model;
using IdentityMicroservice.StaticData;
using IdentityMicroservice.ViewModels.ManageViewModels;
using IdentityMicroservice.ViewModels.ProviderViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;
        private readonly IUserRepository _userRepository;

        public DashboardController(IdentityMicroserviceContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;

        }

        [HttpPost("TodayBooking")]
        public ActionResult<IEnumerable<BookingViewModel>> GetTodayBookingInfo(BookingViewModel model)
        {
            var bookingList = (from a in _context.Booking
                               join b in _context.AppointmentSettings on a.AppointmentSettingId equals b.Id
                               join d in _context.Clinic on b.ClinicId equals d.Id
                               join c in _context.Users on a.ServiceReceiverId equals c.Id
                               where a.Schedule.Date == DateTime.Now.Date && d.Id != null
                               && a.ServiceProviderId == model.ServiceProviderId
                               && a.UserPaymentStatus != "Due" && a.IsActive == true
                                && a.Status != "Completed"
                               && a.IsBookingConfirmed == true
                               && a.IsCancelled == false
                               && a.IsDeleted == false
                               select new BookingViewModel
                               {
                                   Id = a.Id,
                                   AppointmentSettingId = a.Id,
                                   ClinicName = d.Name,
                                   Address = d.CurrentAddress,
                                   ServiceReceiverId = d.Id,
                                   ServiceReceiver = c.FullName,
                                   BookingDate = a.BookingDate,
                                   Schedule = a.Schedule,
                                   DayOfWeek = a.DayOfWeek,
                                   SerialNo = a.SerialNo,
                                   ReportingTime = a.ReportingTime,
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
                                   ReceiverPhone = c.PhoneNumber,
                                   AppointmentDate = a.Schedule,
                                   UserPaymentStatus = a.UserPaymentStatus,
                                   Status = a.IsCancelled == false ? "Approved" : a.IsCancelled == true ? "Cancelled" : "Void",
                                   PaidAmount = Convert.ToDouble(a.PaidAmount)

                               }).ToList();


            if (bookingList == null)
            {
                return NotFound();
            }

            return bookingList;
        }


        [HttpPost("PreviousBooking")]
        public ActionResult<IEnumerable<BookingViewModel>> GetPreviousBookingInfo(BookingViewModel model)
        {
            var bookingList = (from a in _context.Booking
                               join b in _context.AppointmentSettings on a.AppointmentSettingId equals b.Id
                               join d in _context.Clinic on b.ClinicId equals d.Id
                               join c in _context.Users on a.ServiceReceiverId equals c.Id
                               where (a.Schedule.Date < DateTime.Now.Date.AddDays(-7)) && d.Id != null
                               && a.ServiceProviderId == model.ServiceProviderId
                               && a.IsDeleted == false
                               && a.IsActive == true
                               && a.IsCancelled == false
                               && a.IsBookingConfirmed == true
                               select new BookingViewModel
                               {
                                   Id = a.Id,
                                   AppointmentSettingId = a.Id,
                                   ClinicName = d.Name,
                                   Address = d.CurrentAddress,
                                   ServiceReceiverId = d.Id,
                                   ServiceReceiver = c.FullName,
                                   BookingDate = a.BookingDate,
                                   Schedule = a.Schedule,
                                   DayOfWeek = a.DayOfWeek,
                                   SerialNo = a.SerialNo,
                                   ReportingTime = a.ReportingTime,
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
                                   ReceiverPhone = c.PhoneNumber,
                                   AppointmentDate = a.Schedule,
                                   UserPaymentStatus = a.UserPaymentStatus,
                                   Status = a.IsCancelled == false ? "Approved" : a.IsCancelled == true ? "Cancelled" : "Void",
                                   PaidAmount = a.PaidAmount

                               }).ToList();


            if (bookingList == null)
            {
                return NotFound();
            }

            return bookingList;
        }



        //[HttpPost("getNotification")]
        //public ActionResult<IEnumerable<notificationmaster>> GetNotification(BookingViewModel model)
        //{
        //    var notificationList = _context.notificationmaster.Where(m => m.userid == model.Id).ToList();
        //    return notificationList;
        //}

        [HttpPost("getNotification")]
        public List<notificationmaster> GetNotification(BookingViewModel model)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                //Checking Exiting User
                var notificationList = _userRepository.GetNotification(model.Id);
                scope.Complete();
                return notificationList;
            }
        }

        [HttpPost("deleteAllNotification")]
        public List<notificationmaster> DeleteAllNotification(BookingViewModel model)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                //Checking Exiting User
                var notificationList = _userRepository.DeleteAllNotification(model.Id);
                scope.Complete();
                return notificationList;
            }
        }

        


        [HttpPost("markallnotificationasread")]
        public List<notificationmaster> MarkAllNotificationAsRead(BookingViewModel model)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                //Checking Exiting User
                var notificationList = _userRepository.MarkAllNotificationAsRead(model.Id);
                scope.Complete();
                return notificationList;
            }
        }


        [HttpPost("TotalPatient")]
        public String GetTotalNumberOfPatients(BookingViewModel model)
        {
            var numberOfPatients = _context.Booking.Where(m => m.IsBookingConfirmed == true
                                                && m.IsActive == true && m.IsDeleted == false
                                                && m.IsCancelled == false && m.IsProcessed == true
                                                && m.VisitConfirmationStatus == true
                                                && m.UserPaymentStatus == PaymentStatus.Paid
                                                 && m.Status != "Completed"
                                                && m.PaidAmount > 0 && m.ServiceProviderId == model.ServiceProviderId).Count();

            return numberOfPatients.ToString();
        }

        [HttpPost("TodayAppointment")]
        public String GetTodayNoOfAppointment(BookingViewModel model)
        {
            var numberOfPatients = _context.Booking.Where(m => m.IsBookingConfirmed == true
                                                && m.IsActive == true && m.IsDeleted == false
                                                && m.IsCancelled == false && m.IsProcessed == true
                                                && m.VisitConfirmationStatus == true
                                                && m.UserPaymentStatus == PaymentStatus.Paid
                                                && m.Schedule.Date == DateTime.Now.Date
                                                && m.Status != "Completed"
                                                && m.PaidAmount > 0 && m.ServiceProviderId == model.ServiceProviderId).Count();

            return numberOfPatients.ToString();
        }

        [HttpPost("TodayPatient")]
        public String GetTodayNoOfPatients(BookingViewModel model)
        {
            var numberOfPatients = _context.Booking.Where(m => m.IsActive == true && m.IsDeleted == false
                                                && m.IsCancelled == false
                                                && m.Status != "Completed"
                                                && m.Schedule.Date == DateTime.Now.Date
                                                && m.IsBookingConfirmed == true
                                                && m.PaidAmount > 0 && m.ServiceProviderId == model.ServiceProviderId).Count();

            return numberOfPatients.ToString();
        }


        [HttpPost("AllAppointment")]
        public String GetPatientNumOfAppointment(BookingViewModel model)
        {
            var numberOfAppointments = _context.Booking.Where(m => m.IsBookingConfirmed == true
                                                && m.IsActive == true && m.IsDeleted == false
                                                && m.IsCancelled == false
                                                && m.Status != "Completed"
                                                && m.UserPaymentStatus == PaymentStatus.Paid
                                                && m.PaidAmount > 0 && m.ServiceReceiverId == model.ServiceReceiverId).Count();

            return numberOfAppointments.ToString();
        }

        [HttpPost("VisitedDoctor")]
        public String GetNumOfVisitedDoctor(BookingViewModel model)
        {
            var numberOfDoctors = _context.Booking.Where(m => m.IsBookingConfirmed == true
                                                && m.IsActive == true && m.IsDeleted == false
                                                && m.IsCancelled == false
                                                && m.UserPaymentStatus == PaymentStatus.Paid
                                                && m.PaidAmount > 0 && m.ServiceReceiverId == model.ServiceReceiverId).Count();

            return numberOfDoctors.ToString();
        }

        [HttpPost("ReceiverPoint")]
        public String GetReceiverPoints(BookingViewModel model)
        {
            var numberOfDoctors = _context.Booking.Where(m => m.IsBookingConfirmed == true
                                                && m.IsActive == true && m.IsDeleted == false
                                                && m.IsCancelled == false && m.IsProcessed == true
                                                && m.VisitConfirmationStatus == true
                                                && m.UserPaymentStatus == PaymentStatus.Paid
                                                && m.PaidAmount > 0 && m.ServiceReceiverId == model.ServiceReceiverId).Count();

            return numberOfDoctors.ToString();
        }

        [HttpPost("TodayTotalAppointment")]
        public String GetTodayTotalAppointment(BookingViewModel model)
        {
            var bookingList = (from a in _context.Booking
                               join b in _context.AppointmentSettings on a.AppointmentSettingId equals b.Id
                               join d in _context.Clinic on b.ClinicId equals d.Id
                               join c in _context.Users on a.ServiceReceiverId equals c.Id
                               where a.Schedule.Date == DateTime.Now.Date && d.Id != null
                               && a.IsCancelled == false
                               && a.ServiceProviderId == model.ServiceProviderId
                               && a.IsBookingConfirmed == true
                                && a.IsActive == true
                                && a.IsDeleted == false
                                && a.Status != "Completed"
                               select new BookingViewModel
                               {
                                   Id = a.Id
                               }).Count();

            return bookingList.ToString();
        }

        [HttpPost("UpcomingTotalAppointment")]
        public String GetUpcomingTotalAppointment(BookingViewModel model)
        {
            var bookingList = (from a in _context.Booking
                               join b in _context.AppointmentSettings on a.AppointmentSettingId equals b.Id
                               join d in _context.Clinic on b.ClinicId equals d.Id
                               join c in _context.Users on a.ServiceReceiverId equals c.Id
                               where a.Schedule.Date > DateTime.Now.Date && d.Id != null
                               && a.ServiceProviderId == model.ServiceProviderId
                               && a.IsBookingConfirmed == true
                               && a.IsCancelled == false
                                && a.IsActive == true
                                 && a.IsDeleted == false
                                 && a.Status != "Completed"
                               select new BookingViewModel
                               {
                                   Id = a.Id
                               }).Count();

            return bookingList.ToString();
        }

        [HttpPost("NoOfClinic")]
        public String GetNoOfClinic(ClinicViewModel model)
        {
            var clinicInfo = _context.Clinic.Where(m => m.IsDeleted == false && m.IsActive == true && m.UserId == model.UserId).Count();

            return clinicInfo.ToString();
        }

        [HttpPost("TotalAppointmentBill")]
        public String GetTotalAppointmentBill(BookingViewModel model)
        {
            var bookingList = (from a in _context.Booking
                               join b in _context.AppointmentSettings on a.AppointmentSettingId equals b.Id
                               join d in _context.Clinic on b.ClinicId equals d.Id
                               join c in _context.Users on a.ServiceReceiverId equals c.Id
                               where a.Schedule.Date == DateTime.Now.Date && d.Id != null && a.ServiceProviderId == model.ServiceProviderId

                               select new BookingViewModel
                               {
                                   PaidAmount = a.PaidAmount
                               }).Sum(p => p.PaidAmount);

            return bookingList.ToString();
        }
    }
}
