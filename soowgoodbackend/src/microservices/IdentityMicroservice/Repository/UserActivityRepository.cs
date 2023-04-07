using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Model;
using IdentityMicroservice.Data;
using IdentityMicroservice.Interfaces;

namespace IdentityMicroservice.Repository
{
    public class UserActivityRepository : BaseRepository<UserActivity>, IUserActivityRepository
    {
        private readonly IdentityMicroserviceContext _context;

        public UserActivityRepository(IdentityMicroserviceContext context) : base(context) { _context = context; }

        public async Task<UserActivity> GetUserActivityWithRelations(int Id)
        {
            try
            {
                //var activity = await _context.UserActivities.Include(x => x.User)
                //     .FirstOrDefaultAsync(f => f.Id == Id);
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
