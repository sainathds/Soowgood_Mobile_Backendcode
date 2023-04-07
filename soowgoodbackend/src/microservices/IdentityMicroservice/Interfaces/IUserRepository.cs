using IdentityMicroservice.Interfaces;
using IdentityMicroservice.Model;
using IdentityMicroservice.StaticData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityMicroservice.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetUserWithRelations(string Id);
        ApplicationUser GetUserByEmail(string UserName);
        ApplicationUser GetUserByRole(string Email,bool SignupWithEmail);
        //ApplicationUser GetUserByPhoneNumber(string PhoneNumber, string UserRole);

        List<ProfileStatus> GetUserProfileCompletionStatus(string Id);

        List<appuserprofiledata> getuserdatabyid(string Id);

        List<notificationmaster> GetNotification(string Id);


        List<notificationmaster> DeleteAllNotification(string Id);

        List<notificationmaster> MarkAllNotificationAsRead(string Id);

        List<Search> GetProvideReviewRating(string Id);

        List<UserList> GetProviderProfileToApprove();

        List<Clinic> getClinicImages(string userid);


        List<authentication> deleteuserprofiledata(string Id);

    }
}
