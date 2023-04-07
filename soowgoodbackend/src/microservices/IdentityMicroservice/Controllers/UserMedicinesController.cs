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
    public class UserMedicinesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public UserMedicinesController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/UserMedicines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserMedicine>>> GetUserMedicine()
        {
            return await _context.UserMedicine.ToListAsync();
        }

        // GET: api/UserMedicines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserMedicine>> GetUserMedicine(string id)
        {
            var userMedicine = await _context.UserMedicine.FindAsync(id);

            if (userMedicine == null)
            {
                return NotFound();
            }

            return userMedicine;
        }

        [HttpPost("UserMedicineInfo")]
        public async Task<ActionResult<IEnumerable<UserMedicineViewModel>>> PostUserMedicineInfo(UserMedicine accessible)
        {
            try
            {
                if (!String.IsNullOrEmpty(accessible.Name))
                {
                    string[] arrItemsPlanner = accessible.Name.Split(",");

                    foreach (string name in arrItemsPlanner)
                    {
                        if (!String.IsNullOrEmpty(name))
                        {
                            var accessibleMasterInfo = _context.UserMedicine.Any(p => p.Name.Equals(name.Trim()));

                            if (!accessibleMasterInfo && !String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(name.Trim()))
                            {
                                var item = new Medicine();
                                item.Name = name.Trim();
                                _context.Medicine.Add(item);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }

                var accessibleInfo = _context.UserMedicine.Any(m => m.Name == accessible.Name && m.Note == accessible.Note
                                                                             && m.IsActive == true && m.UserId == accessible.UserId);

                if (!accessibleInfo && accessible.Id == null)
                {
                    _context.UserMedicine.Add(accessible);
                    await _context.SaveChangesAsync();
                    return GetUserMedicineInfo(accessible.UserId);
                }

                else if (accessible.Id != null)
                {
                    var Info = _context.UserMedicine.Any(m => m.Id == accessible.Id);

                    if (Info)
                    {
                        await PostUpdateUserMedicineInfo(accessible);
                        return GetUserMedicineInfo(accessible.UserId);
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

        [HttpPost("UpdateUserMedicineInfo")]
        public async Task<ActionResult<UserMedicine>> PostUpdateUserMedicineInfo(UserMedicine accessible)
        {
            try
            {
                var accessibleInfo = _context.UserMedicine.Any(m => m.Id == accessible.Id);

                if (accessibleInfo)
                {
                    _context.UserMedicine.Update(accessible);
                    await _context.SaveChangesAsync();
                    //return GetUserLifeStyleInfo(accessible.UserId);
                    return CreatedAtAction("GetUserMedicineInfo", new { id = accessible.Id }, accessible);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }
        }

        private ActionResult<IEnumerable<UserMedicineViewModel>> GetUserMedicineInfo(string userId)
        {

            var accessibleInfoList = (from a in _context.UserMedicine
                                      where a.UserId == userId
                                      select new UserMedicineViewModel
                                      {
                                          Id = a.Id,
                                          Name = a.Name,
                                          GenericName = a.GenericName,
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

        // GET: api/UserLifeStyleInfoes/5
        [HttpPost("GetUserMedicineInfo")]
        public async Task<ActionResult<IEnumerable<UserMedicine>>> GetUserLifeStyleInfo(UserMedicineViewModel model)
        {

            var accessibleInfo = await _context.UserMedicine.Where(m => m.IsDeleted == false && m.UserId == model.UserId).ToListAsync();

            if (accessibleInfo == null)
            {
                return NotFound();
            }
            else
            {
                return accessibleInfo;
            }
        }

        // PUT: api/UserMedicines/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserMedicine(string id, UserMedicine userMedicine)
        {
            if (id != userMedicine.Id)
            {
                return BadRequest();
            }

            _context.Entry(userMedicine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserMedicineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserMedicines
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserMedicine>> PostUserMedicine(UserMedicine userMedicine)
        {
            _context.UserMedicine.Add(userMedicine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserMedicine", new { id = userMedicine.Id }, userMedicine);
        }

        // DELETE: api/UserMedicines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserMedicine>> DeleteUserMedicine(string id)
        {
            var userMedicine = await _context.UserMedicine.FindAsync(id);
            if (userMedicine == null)
            {
                return NotFound();
            }

            _context.UserMedicine.Remove(userMedicine);
            await _context.SaveChangesAsync();

            return userMedicine;
        }

        private bool UserMedicineExists(string id)
        {
            return _context.UserMedicine.Any(e => e.Id == id);
        }
    }
}
