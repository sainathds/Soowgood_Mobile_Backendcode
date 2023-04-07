using IdentityMicroservice.Data;
using IdentityMicroservice.Interfaces;
using IdentityMicroservice.Model;
using IdentityMicroservice.StaticData;
using IdentityMicroservice.StaticData.Manipulator;
using IdentityMicroservice.ViewModels;
using IdentityMicroservice.ViewModels.ManageViewModels;
using IdentityMicroservice.ViewModels.ProviderViewModel;
using IdentityMicroservice.ViewModels.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Repository
{
    public class BookingRepository: BaseRepository<ApplicationUser>, IBookingRepository
    {
        private readonly IdentityMicroserviceContext _context;

        public BookingRepository(IdentityMicroserviceContext context) : base(context) { _context = context; }

        public List<ScheduleAppointment> getclinicappointmentslot(string providerid, string appointmenttype)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@providerid", SqlDbType.NVarChar, providerid));
            _params.Add(new SqlParam("@appointmenttype", SqlDbType.NVarChar, appointmenttype));
            var items = Executor.ExecuteStoredProcedure<ScheduleAppointment>("pr_booking_getclinicappointmentslot", _params);
            return items;
        }


        public List<ScheduleAppointment> getclinicappointment(string providerid, string appointmenttype)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@providerid", SqlDbType.NVarChar, providerid));
                _params.Add(new SqlParam("@appointmenttype", SqlDbType.NVarChar, appointmenttype));
                var items = Executor.ExecuteStoredProcedure<ScheduleAppointment>("pr_booking_getclinicappointment", _params);
                return items;
            }catch(Exception ex)
            {
                throw ex;
            }
        }


        public List<ScheduleAppointment> GetClinicScheduleData(AppointmentSetting model)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@clinicid", SqlDbType.NVarChar, model.ClinicId));                 
                var items = Executor.ExecuteStoredProcedure<ScheduleAppointment>("pr_clinic_getclinicscheduledata", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<authentication> CheckAppointmentDetails(AppointmentSetting settings)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@clinicid", SqlDbType.NVarChar, settings.ClinicId));
                _params.Add(new SqlParam("@serviceproviderid", SqlDbType.NVarChar, settings.ServiceProviderId));
                _params.Add(new SqlParam("@daysOfWeek", SqlDbType.NVarChar, settings.daysOfWeek));                 
                _params.Add(new SqlParam("@fromtime", SqlDbType.Time, settings.DayStartingTime));
                _params.Add(new SqlParam("@totime", SqlDbType.Time, settings.DayEndingTime));
                _params.Add(new SqlParam("@AppointmentType", SqlDbType.NVarChar, settings.AppointmentType));
                _params.Add(new SqlParam("@AppointmentSettingId", SqlDbType.NVarChar, settings.AppointmentSettingId));
                var items = Executor.ExecuteStoredProcedure<authentication>("pr_appointment_checkappointmentdetails", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<authentication> processappointmentsetting(string appointmentSettingId, string daysOfWeek, string appointmentType)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@appointmentSettingId", SqlDbType.NVarChar, appointmentSettingId));               
                _params.Add(new SqlParam("@daysOfWeek", SqlDbType.NVarChar, daysOfWeek));           
                _params.Add(new SqlParam("@AppointmentType", SqlDbType.Xml, appointmentType));               
                var items = Executor.ExecuteStoredProcedure<authentication>("pr_appointment_processappointmentsetting", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BookingViewModel> GetPatientAppointmentHistory(SearchParameterViewModel model)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@ServiceReceiverId", SqlDbType.NVarChar, model.ServiceReceiverId));
            _params.Add(new SqlParam("@ConsultationFees", SqlDbType.Int, model.ConsultationFees));
            _params.Add(new SqlParam("@Availability", SqlDbType.NVarChar, model.Availability));
            _params.Add(new SqlParam("@Gender", SqlDbType.NVarChar, model.Gender));
            _params.Add(new SqlParam("@AppointmentType", SqlDbType.NVarChar, model.AppointmentType));
            _params.Add(new SqlParam("@bookingtype", SqlDbType.NVarChar, model.bookingtype));
            _params.Add(new SqlParam("@ProviderSpeciality", SqlDbType.NVarChar, model.ProviderSpeciality));
            _params.Add(new SqlParam("@ServiceType", SqlDbType.NVarChar, model.ServiceType));
            var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_getPatientAppointmentHistory", _params);
            return items;
        }


        public List<BookingViewModel> getbookinghistoryforpayment(BookingViewModel model)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@ServiceReceiverId", SqlDbType.NVarChar, model.ServiceReceiverId));             
            var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_getbookinghistoryforpayment", _params);
            return items;
        }


        public List<BookingViewModel> getconfirmbooking(BookingViewModel model)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@tranId", SqlDbType.NVarChar, model.Id));
            var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_getconfirmbooking", _params);
            return items;
        }

        



        public List<BookingViewModel> checkAppointmentForVideoCall(string Id)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@BookId", SqlDbType.NVarChar, Id));
            var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_getappointmentdataforbooking", _params);
            return items;
        }


        public List<BookingViewModel> sendappointmentlink()
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@BookId", SqlDbType.NVarChar, ""));
            var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_sendappointmentlink", _params);
            return items;
        }

        public List<BookingViewModel> GetProviderAppointmentHistory(SearchParameterViewModel model)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@ServiceProviderId", SqlDbType.NVarChar, model.ServiceProviderId));             
            _params.Add(new SqlParam("@bookingtype", SqlDbType.NVarChar, model.bookingtype));             
            var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_getProviderAppointmentHistory", _params);
            return items;
        }



        public List<authentication> BookingCancellationCondition(SearchParameterViewModel model)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@bookingid", SqlDbType.NVarChar, model.Id));           
            var items = Executor.ExecuteStoredProcedure<authentication>("pr_booking_bookingcancellationcondition", _params);
            return items;
        }

        public List<BookingViewModel> GetProviderAppointments(BookingViewModel model)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@ServiceProviderId", SqlDbType.NVarChar, model.ServiceProviderId));
            _params.Add(new SqlParam("@AppointmentTypeId", SqlDbType.NVarChar, model.AppointmentTypeId));
            _params.Add(new SqlParam("@appointmentdate", SqlDbType.DateTime, model.Schedule));
            _params.Add(new SqlParam("@ClinicId", SqlDbType.NVarChar, model.ClinicId));
            var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_getProviderAppointment", _params);
            return items;
        }


        public List<BookingViewModel> GetProviderPatients(UserViewModel model)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@ServiceProviderId", SqlDbType.NVarChar, model.Id));           
            var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_getProviderPatients", _params);
            return items;
        }

        public List<BookingViewModel> GetPatientAppointmentDocumentHistory(BookingViewModel model)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@ServiceReceiverId", SqlDbType.NVarChar, model.ServiceReceiverId));
            _params.Add(new SqlParam("@booingId", SqlDbType.NVarChar, model.booingId));
            var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_getPatientAppointmentDocumentHistory", _params);
            return items;
        }


        public List<BookingViewModel> CancelPendingBooking(BookingViewModel model)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@bookingId", SqlDbType.NVarChar, model.Id));
                var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_cancelotherpendingbooking", _params);
                return items;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<BookingViewModel> GetProviderDataForBooking(BookingViewModel model)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@bookingId", SqlDbType.NVarChar, model.Id));
                var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_getproviderdataforbooking", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BookingViewModel> GetReviewAndRatingdata(BookingViewModel model)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@Id", SqlDbType.NVarChar, model.Id));
                var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_getreviewandratingdata", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        


        public List<BookingViewModel> GetProviderBookingBillDetails(BookingViewModel model)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@Id", SqlDbType.NVarChar, model.Id));
                _params.Add(new SqlParam("@AppointmentTypeName", SqlDbType.NVarChar, model.AppointmentTypeName));
                _params.Add(new SqlParam("@paybackstatus", SqlDbType.NVarChar, model.paybackstatus));
                var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_getproviderbookingbilldetails", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BookingViewModel> GetServiceReceiverForPrescription(BookingViewModel model)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@ServiceReceiverId", SqlDbType.NVarChar, model.ServiceReceiverId));
                _params.Add(new SqlParam("@ServiceProviderId", SqlDbType.NVarChar, model.ServiceProviderId));                
                var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_prescription_getservicereceiver", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<BookingViewModel> GetDoctorVisitForPatient(BookingViewModel model)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@ServiceReceiverId", SqlDbType.NVarChar, model.ServiceReceiverId));
                _params.Add(new SqlParam("@ServiceProviderId", SqlDbType.NVarChar, model.ServiceProviderId));
                var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_prescription_getdoctorvisitforpatient", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<BookingViewModel> GetServiceProviderForPrescription(BookingViewModel model)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@booingId", SqlDbType.NVarChar, model.booingId));
                _params.Add(new SqlParam("@ServiceReceiverId", SqlDbType.NVarChar, model.ServiceReceiverId));
               
                var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_prescription_getserviceprovider", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<BookingViewModel> getPrescriptionData(BookingViewModel model)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();                 
                _params.Add(new SqlParam("@ServiceReceiverId", SqlDbType.NVarChar, model.ServiceReceiverId));
                _params.Add(new SqlParam("@ServiceProviderId", SqlDbType.NVarChar, model.ServiceProviderId));
                var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_prescription_getPrescriptionData", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<BookingViewModel> updatepaymentransaction(BookingViewModel model)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@tranpkid", SqlDbType.NVarChar, model.tranpkid));
                _params.Add(new SqlParam("@bank_tran_id", SqlDbType.NVarChar, model.bank_tran_id));
                _params.Add(new SqlParam("@statuscode", SqlDbType.NVarChar, model.statuscode));
                _params.Add(new SqlParam("@errormessage", SqlDbType.NVarChar, model.errormessage));
                var items = Executor.ExecuteStoredProcedure<BookingViewModel>("pr_booking_updatepaymentstatusforappointment", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<PrescriptionViewModel> addupdateprescriptiondetails(PrescriptionViewModel model)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@Id", SqlDbType.NVarChar, model.Id));
                _params.Add(new SqlParam("@prescriptiondurgdetails", SqlDbType.NVarChar, model.prescriptiondurgdetails));
                _params.Add(new SqlParam("@prescriptionmedicaltestdetails", SqlDbType.NVarChar, model.prescriptionmedicaltestdetails));
                _params.Add(new SqlParam("@prescriptionadvicedetails", SqlDbType.NVarChar, model.prescriptionadvicedetails));
                var items = Executor.ExecuteStoredProcedure<PrescriptionViewModel>("pr_prescription_addupdateprescriptiondetails", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<prescriptionmaster> DeletePrescriptionInfo(PrescriptionViewModel model)
        {
            try
            {
                List<SqlParam> _params = new List<SqlParam>();
                _params.Add(new SqlParam("@Id", SqlDbType.NVarChar, model.Id));                                
                var items = Executor.ExecuteStoredProcedure<prescriptionmaster>("pr_prescription_deleteprescriptioninfo", _params);
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }







    }
}
