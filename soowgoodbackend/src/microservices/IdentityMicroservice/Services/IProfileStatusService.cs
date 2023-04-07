using IdentityMicroservice.Model;
using System.Collections.Generic;

namespace IdentityMicroservice.Services
{
    public interface IProfileStatusService
    {
        List<ProfileStatus> GetAllUserProfileCompletionStatus(string StatusID);
        List<ProfileStatus> GetUserProfileCompletionStatus(string Id);
    }
}