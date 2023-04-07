using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using IdentityMicroservice.ViewModels.ManageViewModels;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccessibleInfoesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public UserAccessibleInfoesController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/UserAccessibleInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAccessibleInfo>>> GetUserAccessibleInfo()
        {
            return await _context.UserAccessibleInfo.ToListAsync();
        }

        // GET: api/UserAccessibleInfoes/5
        [HttpPost("GetUserAccessibleInfo")]
        public async Task<ActionResult<IEnumerable<UserAccessibleInfo>>> GetUserAccessibleInfo(UserAccessibleInfoViewModel model)
        {

            var accessibleInfo = await _context.UserAccessibleInfo.Where(m => m.IsDeleted == false && m.UserId == model.UserId).ToListAsync();

            if (accessibleInfo == null)
            {
                return NotFound();
            }
            else
            {
                return accessibleInfo;
            }
        }

        // PUT: api/UserAccessibleInfoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("UserAccessibleInfo")]
        public async Task<ActionResult<IEnumerable<UserAccessibleInfoViewModel>>> PostUserAccessibleInfo(UserAccessibleInfo accessible)
        {
            try
            {

                if (!String.IsNullOrEmpty(accessible.AccessibleName))
                {
                    string[] arrItemsPlanner = accessible.AccessibleName.Split(",");

                    foreach (string name in arrItemsPlanner)
                    {
                        if (!String.IsNullOrEmpty(name))
                        {
                            var accessibleMasterInfo = _context.AccessibleInfo.Any(p => p.Name.Equals(name.Trim()));

                            if (!accessibleMasterInfo && !String.IsNullOrEmpty(name))
                            {
                                var item = new AccessibleInfo();
                                item.Name = name.Trim();
                                _context.AccessibleInfo.Add(item);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }

               var accessibleInfo = _context.UserAccessibleInfo.Any(m => m.AccessibleName == accessible.AccessibleName && m.Note == accessible.Note
                                                                             && m.IsActive == true && m.UserId == accessible.UserId);

                if (!accessibleInfo && accessible.Id == null)
                {
                    _context.UserAccessibleInfo.Add(accessible);
                    await _context.SaveChangesAsync();
                    return GetUserAllAccessibleInfo(accessible.UserId);
                }

                else if (accessible.Id != null)
                {
                    var Info = _context.UserAccessibleInfo.Any(m => m.Id == accessible.Id);

                    if (Info)
                    {
                        await PostUpdateUserAccessibleInfo(accessible);
                        return GetUserAllAccessibleInfo(accessible.UserId);
                    }
                    else
                        return NotFound();
                }

                else
                    return Conflict();
            }
            catch (DbUpdateException ex)
            {
                return NotFound(); ;
            }
        }

        private ActionResult<IEnumerable<UserAccessibleInfoViewModel>> GetUserAllAccessibleInfo(string userId)
        {

            var accessibleInfoList = (from a in _context.UserAccessibleInfo
                                      where a.UserId == userId
                                      select new UserAccessibleInfoViewModel
                                      {
                                          Id = a.Id,
                                          AccessibleName = a.AccessibleName,
                                          Note = a.Note,
                                          UserId = a.UserId,
                                          CurrentStatus = a.CurrentStatus,
                                          SinceWhen = a.SinceWhen,
                                          TillWhen = a.TillWhen
                                      }).ToList();

            if (accessibleInfoList == null)
            {
                return NotFound();
            }

            return accessibleInfoList;

        }

        [HttpPost("UpdateUserAccessibleInfo")]
        public async Task<ActionResult<UserAccessibleInfo>> PostUpdateUserAccessibleInfo(UserAccessibleInfo accessible)
        {
            try
            {
                var accessibleInfo = _context.UserAccessibleInfo.Any(m => m.Id == accessible.Id);

                if (accessibleInfo)
                {
                    _context.UserAccessibleInfo.Update(accessible);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetUserAccessibleInfo", new { id = accessible.Id }, accessible);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }
        }

        // POST: api/UserAccessibleInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<UserAccessibleInfo>> PostUserAccessibleInfo(UserAccessibleInfo userAccessibleInfo)
        //{
        //    _context.UserAccessibleInfo.Add(userAccessibleInfo);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUserAccessibleInfo", new { id = userAccessibleInfo.Id }, userAccessibleInfo);
        //}

        // DELETE: api/UserAccessibleInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserAccessibleInfo>> DeleteUserAccessibleInfo(string id)
        {
            var userAccessibleInfo = await _context.UserAccessibleInfo.FindAsync(id);
            if (userAccessibleInfo == null)
            {
                return NotFound();
            }

            _context.UserAccessibleInfo.Remove(userAccessibleInfo);
            await _context.SaveChangesAsync();

            return userAccessibleInfo;
        }

        private bool UserAccessibleInfoExists(string id)
        {
            return _context.UserAccessibleInfo.Any(e => e.Id == id);
        }
    }
}
