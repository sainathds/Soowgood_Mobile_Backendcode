using IdentityMicroservice.Model;
using IdentityMicroservice.ViewModels;
using System.Threading.Tasks;

namespace IdentityMicroservice.Interfaces
{
    public interface IUserService
    {
        Task<bool> UpdateUser(ApplicationUser user);
        Task<ProfileViewModel> GetUserProfile(string UserId);
        Task<UserViewModel> GetUserViewModel(string UserId);
        Task<UserProfileEditViewModel> GetUserProfileEditViewModel(string UserId);
    }
}
