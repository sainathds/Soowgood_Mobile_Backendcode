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
    public class AccessibleInfoesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public AccessibleInfoesController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/AccessibleInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccessibleInfo>>> GetAccessibleInfo()
        {
            return await _context.AccessibleInfo.ToListAsync();
        }

        // GET: api/AccessibleInfoes/5
        [HttpPost("GetAccessibleInfo")]
        public async Task<ActionResult<IEnumerable<AccessibleInfo>>> GetAccessibleInfo(AccessibleInfoViewModel model)
        {

            var accessibleInfo = await _context.AccessibleInfo.Where(m => m.IsDeleted == false).ToListAsync();

            if (accessibleInfo == null)
            {
                return NotFound();
            }
            else
            {
                return accessibleInfo;
            }
        }

        // PUT: api/AccessibleInfoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("AccessibleInfo")]
        public async Task<ActionResult<AccessibleInfo>> PostExperience(AccessibleInfo accessible)
        {
            try
            {
                var accessibleInfo = _context.AccessibleInfo.Any(m => m.Name == accessible.Name && m.Note == accessible.Note
                                                                             && m.IsActive == true);

                if (!accessibleInfo && accessible.Id == null)
                {
                    _context.AccessibleInfo.Add(accessible);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetAccessibleInfo", new { id = accessible.Id }, accessible);
                }

                else if (accessible.Id != null)
                {
                    var Info = _context.AccessibleInfo.Any(m => m.Id == accessible.Id);

                    if (Info)
                    {
                        await PostUpdateAccessibleInfo(accessible);
                        return CreatedAtAction("GetAccessibleInfo", new { id = accessible.Id }, accessible);
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

        [HttpPost("UpdateAccessibleInfo")]
        public async Task<ActionResult<AccessibleInfo>> PostUpdateAccessibleInfo(AccessibleInfo accessible)
        {
            try
            {
                var accessibleInfo = _context.AccessibleInfo.Any(m => m.Id == accessible.Id);

                if (accessibleInfo)
                {
                    _context.AccessibleInfo.Update(accessible);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetAccessibleInfo", new { id = accessible.Id }, accessible);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }
        }

        // POST: api/AccessibleInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AccessibleInfo>> PostAccessibleInfo(AccessibleInfo accessibleInfo)
        {
            _context.AccessibleInfo.Add(accessibleInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccessibleInfo", new { id = accessibleInfo.Id }, accessibleInfo);
        }

        // DELETE: api/AccessibleInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AccessibleInfo>> DeleteAccessibleInfo(string id)
        {
            var accessibleInfo = await _context.AccessibleInfo.FindAsync(id);
            if (accessibleInfo == null)
            {
                return NotFound();
            }

            _context.AccessibleInfo.Remove(accessibleInfo);
            await _context.SaveChangesAsync();

            return accessibleInfo;
        }

        private bool AccessibleInfoExists(string id)
        {
            return _context.AccessibleInfo.Any(e => e.Id == id);
        }
    }
}
