using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using IdentityMicroservice.ViewModels.ManageViewModels;
using IdentityMicroservice.StaticData;
using System.Transactions;
using IdentityMicroservice.Repository;
using IdentityMicroservice.Interfaces;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentSettingsController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;
        private readonly IBookingRepository _bookingRepository;
        public string[] dayofweek = new string[] { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

        public AppointmentSettingsController(IdentityMicroserviceContext context, IBookingRepository bookingRepository)
        {
            _context = context;
            _bookingRepository = bookingRepository;
        }

        // GET: api/AppointmentSettings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentSettings>>> GetAppointmentSettings()
        {
            return await _context.AppointmentSettings.Where(a => a.IsActive == true).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AppointmentSettings>>> DemoSet(AppointmentSettingsViewModel model)
        {
            return await _context.AppointmentSettings.Where(a => a.IsActive == true).ToListAsync();
        }

        // GET: api/AppointmentSettings/5
        [HttpPost("GetAppointment")]
        public ActionResult<IEnumerable<AppointmentSettingsViewModel>> GetAppointmentSettings(AppointmentSettingsViewModel model)
        {

            var appointmentList = (from a in _context.AppointmentSettings
                                   join b in _context.Clinic on a.ClinicId equals b.Id
                                   //join c in _context.AppointmentType on a.AppointmentTypeId equals c.Id
                                   join d in _context.TaskType on a.TaskTypeId equals d.Id
                                   where a.IsActive == true
                                   select new AppointmentSettingsViewModel
                                   {
                                       IsActive = a.IsActive,
                                       Id = a.Id,
                                       ServiceProviderId = a.ServiceProviderId,
                                       ClinicId = b.Id,
                                       ClinicName = b.Name,
                                       //AppointmentStartingDate = a.AppointmentStartingDate,
                                       //AppointmentEndingDate = a.AppointmentEndingDate,
                                       AppointmentSettingDate = a.AppointmentSettingDate,
                                       //DayofWeek = a.DayofWeek,
                                       DayStartingTime = a.DayStartingTime,
                                       DayEndingTime = a.DayEndingTime,
                                       TimeSlot = a.TimeSlot,
                                       //AppointmentType = c.Name,
                                       TaskType = d.Name
                                   }).ToList();

            if (appointmentList == null)
            {
                return NotFound();
            }

            return appointmentList.ToList();
        }


        [HttpPost("AppointmentList")]
        public ActionResult<IEnumerable<ScheduleAppointment>> GetUserAppointmentList(AppointmentSetting model)
        {
            var appointment = _bookingRepository.getclinicappointment(model.ServiceProviderId, model.AppointmentType);
            return appointment;
        }


        [HttpPost("AppointmentListForBooking")]
        [Produces("application/json")]
        public async Task<IActionResult> GetUserAppointmentListForBooking(AppointmentSetting model)
        {
            //Checking Exiting User
            var appointmentslot = _bookingRepository.getclinicappointmentslot(model.ServiceProviderId, model.AppointmentType);
            if (appointmentslot != null)
            {
                var cliniclist = appointmentslot.Select(p => new { p.ClinicId, p.clinicname, p.cliniccurrentaddress,  p.AppointmentStartingDate, p.AppointmentEndingDate, p.serviceProviderId }).Distinct().ToList();

                var appointmentdates = appointmentslot.Select(p => new { p.ClinicId, p.AppointmentDayOfWeek, p.AppointmentDate, p.caldate }).Distinct().ToList();

                var jsonobject = new { success = "1", errormsg = "", data = appointmentslot, cliniclist = cliniclist, appointmentdatedata = appointmentdates };
                return Ok(jsonobject);
            }
            else
            {
                return Ok(new { success = "0", errormsg = "No Data available." });
            }

        }


        [HttpPost("GetClinicScheduleData")]
        public ActionResult<IEnumerable<ScheduleAppointment>> GetClinicScheduleData(AppointmentSetting model)
        {

            List<ScheduleAppointment> appointmentList = _bookingRepository.GetClinicScheduleData(model);
            return appointmentList;


        }


        [HttpPost("CheckAppointmentDetails")]
        public ActionResult<authentication> CheckAppointmentDetails(AppointmentSetting settings)
        {
            try
            {
                var appointdata = _bookingRepository.CheckAppointmentDetails(settings).FirstOrDefault();
                return appointdata;
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }





        [HttpPost("Appointment")]
        public ActionResult<authentication> PostAppointment(AppointmentSetting settings)
        {
            authentication _userauth = new authentication();
            string success = "";
            string message = "";
            string AppointmentSettingId = "";
            try
            {
                if (settings.AppointmentSettingId == null)
                {
                    AppointmentSettings objSetting = new AppointmentSettings();
                    objSetting.ClinicId = settings.ClinicId;
                    objSetting.ServiceProviderId = settings.ServiceProviderId;
                    objSetting.TaskTypeId = settings.TaskTypeId;
                    objSetting.NoOfPatients = settings.NoOfPatients;
                    objSetting.TimeSlot = settings.TimeSlot;
                    objSetting.DayStartingTime = settings.DayStartingTime;
                    objSetting.DayEndingTime = settings.DayEndingTime;
                    _context.AppointmentSettings.Add(objSetting);
                    _context.SaveChanges();
                    AppointmentSettingId = objSetting.Id;
                    success = "1";
                    message = "Schedule Create Successfully!";
                }
                else
                {

                    var objSetting = _context.AppointmentSettings.Find(settings.AppointmentSettingId);
                    if (objSetting != null)
                    {
                        objSetting.ClinicId = settings.ClinicId;
                        objSetting.ServiceProviderId = settings.ServiceProviderId;
                        objSetting.TaskTypeId = settings.TaskTypeId;
                        objSetting.NoOfPatients = settings.NoOfPatients;
                        objSetting.TimeSlot = settings.TimeSlot;
                        objSetting.DayStartingTime = settings.DayStartingTime;
                        objSetting.DayEndingTime = settings.DayEndingTime;
                        _context.AppointmentSettings.Update(objSetting);
                        _context.SaveChanges();
                        AppointmentSettingId = settings.AppointmentSettingId;
                        success = "1";
                        message = "Schedule Update Successfully!";
                    }
                }

                var appointdata = _bookingRepository.processappointmentsetting(AppointmentSettingId, settings.daysOfWeek, settings.AppointmentType).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _userauth.success = "0";
                _userauth.message = ex.Message;
            }
            return _userauth;
        }






        [HttpPost("deleteCurrentSchedule")]
        public async Task<authentication> DeleteCurrentSchedule(AppointmentSetting settings)
        {
            //&& m.AppointmentTypeId==settings.AppointmentTypeId
            authentication _userauth = new authentication();
            try
            {


                var settingsInfo = _context.AppointmentSettings.Where(m => m.Id == settings.Id).ToList();
                if (settingsInfo != null)
                {
                    settingsInfo.ForEach(setting =>
                    {
                        setting.IsDeleted = true;
                    });
                    await _context.SaveChangesAsync();
                    _userauth.success = "1";
                    _userauth.message = "Schedule has been Delete Successfully!";

                }
                else
                {
                    _userauth.success = "0";
                    _userauth.message = "No record found.";
                }

            }
            catch (DbUpdateException ex)
            {
                _userauth.success = "0";
                _userauth.message = ex.Message;
            }
            return _userauth;
        }



        [HttpPost("UpdateSettings")]
        public ActionResult<AppointmentSettings> PostUpdateSettings(AppointmentSettings settings)
        {
            try
            {
                var settingsInfo = _context.AppointmentSettings.Any(m => m.Id == settings.Id);

                if (settingsInfo)
                {
                    _context.AppointmentSettings.Update(settings);
                    _context.SaveChanges();
                    return CreatedAtAction("GetExperience", new { id = settings.Id }, settings);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }
        }

        // PUT: api/AppointmentSettings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("getAppointmentDetailData")]
        public ActionResult<IEnumerable<ScheduleAppointment>> GetAppointmentDetailData(AppointmentSetting settings)
        {



            var appointmentDayList = (from a in _context.AppointmentDaySettings
                                      join c in _context.appointmentdaymaster on a.AppointmentDayOfWeek equals c.appointmentdayname
                                      where a.AppointmentSettingId == settings.Id && a.isActive == true
                                      orderby c.appointmentdayno
                                      select new AppointmentDaySettings
                                      {
                                          AppointmentSettingId = a.AppointmentSettingId,
                                          AppointmentDayId = a.AppointmentDayId,
                                          AppointmentDayOfWeek = a.AppointmentDayOfWeek


                                      }).ToList();



            var appointmentServiceTypeList = (from a in _context.AppointmentServiceTypeSettings
                                              join c in _context.AppointmentType on a.AppointmentTypeId equals c.Id
                                              where a.AppointmentSettingId == settings.Id
                                              select new AppointmentServiceTypeSetting
                                              {
                                                  AppointmentSettingId = a.AppointmentSettingId,
                                                  AppointmentServiceId = a.AppointmentServiceId,
                                                  AppointmentTypeId = a.AppointmentTypeId,
                                                  AppointmentFees = a.AppointmentFees,
                                                  isActive = a.isActive,
                                                  ConsultancyType = c.Name

                                              }).ToList();


            var appointmentList = (from a in _context.AppointmentSettings
                                   join d in _context.TaskType on a.TaskTypeId equals d.Id
                                   where a.Id == settings.Id
                                   select new ScheduleAppointment
                                   {
                                       AppointmentSettingID = a.Id,
                                       ClinicId = a.ClinicId,
                                       TimeSlot = a.TimeSlot,
                                       TaskType=d.Name,
                                       NoOfPatients = a.NoOfPatients,
                                       TaskTypeId = a.TaskTypeId,
                                       DayEndingTime = a.DayEndingTime.ToString(@"hh\:mm"),
                                       DayStartingTime = a.DayStartingTime.ToString(@"hh\:mm"),
                                       AppointmentDayList = appointmentDayList,
                                       AppointmentServiceTypeList = appointmentServiceTypeList

                                   }).ToList();

            return appointmentList;
        }

        // POST: api/AppointmentSettings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AppointmentSettings>> PostAppointmentSettings(AppointmentSettings appointmentSettings)
        {
            _context.AppointmentSettings.Add(appointmentSettings);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointmentSettings", new { id = appointmentSettings.Id }, appointmentSettings);
        }

        // DELETE: api/AppointmentSettings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AppointmentSettings>> DeleteAppointmentSettings(string id)
        {
            var appointmentSettings = await _context.AppointmentSettings.FindAsync(id);
            if (appointmentSettings == null)
            {
                return NotFound();
            }

            _context.AppointmentSettings.Remove(appointmentSettings);
            await _context.SaveChangesAsync();

            return appointmentSettings;
        }

        private bool AppointmentSettingsExists(string id)
        {
            return _context.AppointmentSettings.Any(e => e.Id == id);
        }
    }
}
