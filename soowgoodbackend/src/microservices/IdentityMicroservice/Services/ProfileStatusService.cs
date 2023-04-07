using IdentityMicroservice.Model;
using IdentityMicroservice.StaticData.Manipulator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Services
{
    public class ProfileStatusService : IProfileStatusService
    {
        public List<ProfileStatus> GetUserProfileCompletionStatus(String Id)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@ID", SqlDbType.NVarChar, Id));

            var items = Executor.ExecuteStoredProcedure<ProfileStatus>("GetUserProfileCompletionStatus", _params);
            return items;
        }

        public List<ProfileStatus> GetAllUserProfileCompletionStatus(String StatusID)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@StatusID", SqlDbType.NVarChar, StatusID));

            var items = Executor.ExecuteStoredProcedure<ProfileStatus>("GetAllUserProfileCompletionStatus", _params);
            return items;
        }
    }
}
