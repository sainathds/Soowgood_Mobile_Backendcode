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
    public class LifeStylesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public LifeStylesController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/LifeStyles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LifeStyle>>> GetLifeStyle()
        {
            return await _context.LifeStyle.ToListAsync();
        }

        // GET: api/LifeStyles/5
        [HttpPost("GetLifeStyles")]
        public async Task<ActionResult<IEnumerable<LifeStyle>>> GetAccessibleInfo(LifeStyleViewModel model)
        {

            var accessibleInfo = await _context.LifeStyle.Where(m => m.IsDeleted == false).ToListAsync();

            if (accessibleInfo == null)
            {
                return NotFound();
            }
            else
            {
                return accessibleInfo;
            }
        }

        // PUT: api/LifeStyles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("LifeStyles")]
        public async Task<ActionResult<LifeStyle>> PostLifeStyles(LifeStyle accessible)
        {
            try
            {
                var accessibleInfo = _context.LifeStyle.Any(m => m.Name == accessible.Name
                                                                             && m.IsActive == true);

                if (!accessibleInfo && accessible.Id == null)
                {
                    _context.LifeStyle.Add(accessible);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetLifeStyle", new { id = accessible.Id }, accessible);
                }

                else if (accessible.Id != null)
                {
                    var Info = _context.LifeStyle.Any(m => m.Id == accessible.Id);

                    if (Info)
                    {
                        await PostUpdateLifeStyles(accessible);
                        return CreatedAtAction("GetLifeStyle", new { id = accessible.Id }, accessible);
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

        [HttpPost("UpdateLifeStyles")]
        public async Task<ActionResult<LifeStyle>> PostUpdateLifeStyles(LifeStyle accessible)
        {
            try
            {
                var accessibleInfo = _context.LifeStyle.Any(m => m.Id == accessible.Id);

                if (accessibleInfo)
                {
                    _context.LifeStyle.Update(accessible);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetLifeStyle", new { id = accessible.Id }, accessible);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }
        }

        // POST: api/LifeStyles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<LifeStyle>> PostLifeStyle(LifeStyle lifeStyle)
        {
            _context.LifeStyle.Add(lifeStyle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLifeStyle", new { id = lifeStyle.Id }, lifeStyle);
        }

        // DELETE: api/LifeStyles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LifeStyle>> DeleteLifeStyle(string id)
        {
            var lifeStyle = await _context.LifeStyle.FindAsync(id);
            if (lifeStyle == null)
            {
                return NotFound();
            }

            _context.LifeStyle.Remove(lifeStyle);
            await _context.SaveChangesAsync();

            return lifeStyle;
        }

        private bool LifeStyleExists(string id)
        {
            return _context.LifeStyle.Any(e => e.Id == id);
        }
    }
}
