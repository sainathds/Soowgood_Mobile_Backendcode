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
    public class UserLifeStyleInfoesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public UserLifeStyleInfoesController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/UserLifeStyleInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLifeStyleInfo>>> GetUserLifeStyleInfo()
        {
            return await _context.UserLifeStyleInfo.ToListAsync();
        }

        // GET: api/UserLifeStyleInfoes/5
        [HttpPost("GetUserLifeStyleInfo")]
        public async Task<ActionResult<IEnumerable<UserLifeStyleInfo>>> GetUserLifeStyleInfo(UserLifeStyleInfoViewModel model)
        {

            var accessibleInfo = await _context.UserLifeStyleInfo.Where(m => m.IsDeleted == false && m.UserId == model.UserId).ToListAsync();

            if (accessibleInfo == null)
            {
                return NotFound();
            }
            else
            {
                return accessibleInfo;
            }
        }

        [HttpPost("UserLifeStyleInfo")]
        public async Task<ActionResult<IEnumerable<UserLifeStyleInfoViewModel>>> PostUserLifeStyleInfo(UserLifeStyleInfo accessible)
        {
            try
            {
                if (!String.IsNullOrEmpty(accessible.LifeStyleName))
                {
                    string[] arrItemsPlanner = accessible.LifeStyleName.Split(",");

                    foreach (string name in arrItemsPlanner)
                    {
                        if (!String.IsNullOrEmpty(name))
                        {
                            var accessibleMasterInfo = _context.LifeStyle.Any(p => p.Name.Equals(name.Trim()));

                            if (!accessibleMasterInfo && !String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(name.Trim()))
                            {
                                var item = new LifeStyle();
                                item.Name = name.Trim();
                                _context.LifeStyle.Add(item);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }

                var accessibleInfo = _context.UserLifeStyleInfo.Any(m => m.LifeStyleName == accessible.LifeStyleName && m.Note == accessible.Note
                                                                             && m.IsActive == true && m.UserId == accessible.UserId);

                if (!accessibleInfo && accessible.Id == null)
                {
                    _context.UserLifeStyleInfo.Add(accessible);
                    await _context.SaveChangesAsync();
                    return GetUserLifeStyleInfo(accessible.UserId);
                }

                else if (accessible.Id != null)
                {
                    var Info = _context.UserLifeStyleInfo.Any(m => m.Id == accessible.Id);

                    if (Info)
                    {
                        await PostUpdateUserLifeStyleInfo(accessible);
                        return GetUserLifeStyleInfo(accessible.UserId);
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

        [HttpPost("UpdateUserLifeStyleInfo")]
        public async Task<ActionResult<UserLifeStyleInfo>> PostUpdateUserLifeStyleInfo(UserLifeStyleInfo accessible)
        {
            try
            {
                var accessibleInfo = _context.UserLifeStyleInfo.Any(m => m.Id == accessible.Id);

                if (accessibleInfo)
                {
                    _context.UserLifeStyleInfo.Update(accessible);
                    await _context.SaveChangesAsync();
                    //return GetUserLifeStyleInfo(accessible.UserId);
                    return CreatedAtAction("GetUserLifeStyleInfo", new { id = accessible.Id }, accessible);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }
        }


        private ActionResult<IEnumerable<UserLifeStyleInfoViewModel>> GetUserLifeStyleInfo(string userId)
        {

            var accessibleInfoList = (from a in _context.UserLifeStyleInfo
                                      where a.UserId == userId
                                      select new UserLifeStyleInfoViewModel
                                      {
                                          Id = a.Id,
                                          LifeStyleName = a.LifeStyleName,
                                          Note = a.Note,
                                          UserId = a.UserId,
                                          CurrentStatus = a.CurrentStatus,
                                          SinceWhen = a.SinceWhen,
                                          TillWhen = a.TillWhen,
                                          QuantityPerDay = a.QuantityPerDay
                                      }).ToList();

            if (accessibleInfoList == null)
            {
                return NotFound();
            }

            return accessibleInfoList;

        }

        // PUT: api/UserLifeStyleInfoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUserLifeStyleInfo(string id, UserLifeStyleInfo userLifeStyleInfo)
        //{
        //    if (id != userLifeStyleInfo.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(userLifeStyleInfo).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserLifeStyleInfoExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/UserLifeStyleInfoes
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<UserLifeStyleInfo>> PostUserLifeStyleInfo(UserLifeStyleInfo userLifeStyleInfo)
        //{
        //    _context.UserLifeStyleInfo.Add(userLifeStyleInfo);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUserLifeStyleInfo", new { id = userLifeStyleInfo.Id }, userLifeStyleInfo);
        //}

        // DELETE: api/UserLifeStyleInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserLifeStyleInfo>> DeleteUserLifeStyleInfo(string id)
        {
            var userLifeStyleInfo = await _context.UserLifeStyleInfo.FindAsync(id);
            if (userLifeStyleInfo == null)
            {
                return NotFound();
            }

            _context.UserLifeStyleInfo.Remove(userLifeStyleInfo);
            await _context.SaveChangesAsync();

            return userLifeStyleInfo;
        }

        private bool UserLifeStyleInfoExists(string id)
        {
            return _context.UserLifeStyleInfo.Any(e => e.Id == id);
        }
    }
}
