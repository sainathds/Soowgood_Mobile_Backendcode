using IdentityMicroservice.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Model;
using IdentityMicroservice.Data;
using System.Linq;
using System.Collections.Generic;
using IdentityMicroservice.StaticData.Manipulator;
using System.Data;
using IdentityMicroservice.StaticData;

namespace IdentityMicroservice.Repository
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        private readonly IdentityMicroserviceContext _context;
        public UserRepository(IdentityMicroserviceContext context) : base(context) { _context = context; }

        public ApplicationUser GetUserByEmail(string UserName)
        {
            try
            {                 
                    var user = _context.Users
                     .FirstOrDefault(f => f.UserName == UserName);
                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ApplicationUser GetUserByRole(string Email, bool SignupWithEmail)
        {
            try
            {
                if (SignupWithEmail == true)
                {
                    var user = _context.Users
                         .FirstOrDefault(f => f.Email == Email);
                    return user;
                }
                else
                {
                    var user = _context.Users
                         .FirstOrDefault(f => f.PhoneNumber == Email);
                    return user;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public ApplicationUser GetUserByPhoneNumber(string PhoneNumber, string UserRole)
        //{
        //    try
        //    {
        //        var user = _context.Users
        //             .FirstOrDefault(f => f.PhoneNumber == PhoneNumber && f.UserRole == UserRole);
        //        return user;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public async Task<ApplicationUser> GetUserWithRelations(string Id)
        {
            try
            {
                var user = await _context.Users.Include(x => x.UserActivities)
                     .FirstOrDefaultAsync(f=>f.Id == Id);
                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ProfileStatus> GetUserProfileCompletionStatus(String Id)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@ID", SqlDbType.NVarChar, Id));


            var items = Executor.ExecuteStoredProcedure<ProfileStatus>("GetUserProfileCompletionStatus", _params);
            return items;
        }


        public List<appuserprofiledata> getuserdatabyid(String Id)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@userid", SqlDbType.NVarChar, Id));


            var items = Executor.ExecuteStoredProcedure<appuserprofiledata>("pr_app_getuserdatabyid", _params);
            return items;
        }

        public List<notificationmaster> GetNotification(String Id)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@userid", SqlDbType.NVarChar, Id));
            var items = Executor.ExecuteStoredProcedure<notificationmaster>("pr_users_getnotification", _params);
            return items;
        }


        public List<notificationmaster> DeleteAllNotification(String Id)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@userid", SqlDbType.NVarChar, Id));
            var items = Executor.ExecuteStoredProcedure<notificationmaster>("pr_users_deleteAllNotification", _params);
            return items;
        }


        public List<notificationmaster> MarkAllNotificationAsRead(String Id)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@userid", SqlDbType.NVarChar, Id));
            var items = Executor.ExecuteStoredProcedure<notificationmaster>("pr_users_markallnotificationasread", _params);
            return items;
        }



        public List<Search> GetProvideReviewRating(String Id)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@ID", SqlDbType.NVarChar, Id));


            var items = Executor.ExecuteStoredProcedure<Search>("pr_user_GetProvideReviewRating", _params);
            return items;
        }

        public List<UserList> GetProviderProfileToApprove()
        {
            List<SqlParam> _params = new List<SqlParam>();
            
            var items = Executor.ExecuteStoredProcedure<UserList>("pr_admin_getproviderprofiletoapprove", _params);
            return items;
        }

        public List<Clinic> getClinicImages(string userId)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@UserId", SqlDbType.NVarChar, userId));
            var items = Executor.ExecuteStoredProcedure<Clinic>("pr_m_getClinicImages", _params);
            return items;
        }


        public List<authentication> deleteuserprofiledata(String Id)
        {
            List<SqlParam> _params = new List<SqlParam>();
            _params.Add(new SqlParam("@userid", SqlDbType.NVarChar, Id));
            var items = Executor.ExecuteStoredProcedure<authentication>("pr_users_deleteuserprofiledata", _params);
            return items;
        }
    }
}
