using AutoMapper;
using IdentityMicroservice.Interfaces;
using IdentityMicroservice.Model;
using IdentityMicroservice.StaticData;
using IdentityMicroservice.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ProfileViewModel> GetUserProfile(string UserId)
        {
            try
            {
                var user = await _userRepository.GetUserWithRelations(UserId);
                var userViewModel = _mapper.Map<ApplicationUser, UserViewModel>(user);
                var profileViewModel = new ProfileViewModel
                {
                    UserViewModel = userViewModel,
                    Activities = user.UserActivities.ToList(),
                };
                return profileViewModel;
            }catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<UserViewModel> GetUserViewModel(string UserId)
        {
            try
            {
                var user = await _userRepository.GetUserWithRelations(UserId);
                var userViewModel = _mapper.Map<ApplicationUser, UserViewModel>(user);

                return userViewModel;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<UserProfileEditViewModel> GetUserProfileEditViewModel(string UserId)
        {
            try
            {
                var user = await _userRepository.GetUserWithRelations(UserId);
                var userViewModel = _mapper.Map<ApplicationUser, UserProfileEditViewModel>(user);

                return userViewModel;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> UpdateUser(ApplicationUser user)
        {
            try
            {
                _userRepository.Update(user);
                await _userRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


     


    }
}
