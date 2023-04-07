using IdentityMicroservice.Model;
using IdentityMicroservice.StaticData;
using IdentityMicroservice.ViewModels;
using IdentityMicroservice.ViewModels.ManageViewModels;
using IdentityMicroservice.ViewModels.ProviderViewModel;
using IdentityMicroservice.ViewModels.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Interfaces
{
    public interface IBookingRepository : IRepository<ApplicationUser>
    {
        List<ScheduleAppointment> getclinicappointmentslot(string providerid,string appointmenttype);

        List<BookingViewModel> GetPatientAppointmentHistory(SearchParameterViewModel model);


        List<BookingViewModel> getbookinghistoryforpayment(BookingViewModel model);



        List<BookingViewModel> getconfirmbooking(BookingViewModel model);

        

        List<authentication> BookingCancellationCondition(SearchParameterViewModel model);

        List<BookingViewModel> GetProviderAppointments(BookingViewModel model);

        List<BookingViewModel> GetProviderPatients(UserViewModel model);

        List<BookingViewModel> GetPatientAppointmentDocumentHistory(BookingViewModel model);

        List<BookingViewModel> GetProviderBookingBillDetails(BookingViewModel model);
        

        List<BookingViewModel> CancelPendingBooking(BookingViewModel model);

        List<BookingViewModel> GetProviderAppointmentHistory(SearchParameterViewModel model);

        List<BookingViewModel> GetProviderDataForBooking(BookingViewModel model);

        List<BookingViewModel> GetReviewAndRatingdata(BookingViewModel model);

        


        List<BookingViewModel> checkAppointmentForVideoCall(string Id);

        List<BookingViewModel> sendappointmentlink();


        List<ScheduleAppointment> getclinicappointment(string providerid, string appointmenttype);



        List<ScheduleAppointment> GetClinicScheduleData(AppointmentSetting model);

        List<authentication> CheckAppointmentDetails(AppointmentSetting settings);

        List<authentication> processappointmentsetting(string appointmentSettingId, string daysOfWeek, string appointmentType);



        List<BookingViewModel> GetServiceReceiverForPrescription(BookingViewModel model);


        List<BookingViewModel> GetDoctorVisitForPatient(BookingViewModel model);


        List<BookingViewModel> GetServiceProviderForPrescription(BookingViewModel model);

        List<PrescriptionViewModel> addupdateprescriptiondetails(PrescriptionViewModel model);

        List<BookingViewModel> getPrescriptionData(BookingViewModel model);


        List<prescriptionmaster> DeletePrescriptionInfo(PrescriptionViewModel model);


        List<BookingViewModel> updatepaymentransaction(BookingViewModel model);


    }
}
