using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CmnUserPermissionToGroupsController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public CmnUserPermissionToGroupsController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/CmnUserPermissionToGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CmnUserPermissionToGroup>>> GetCmnUserPermissionToGroup()
        {
            return await _context.CmnUserPermissionToGroup.ToListAsync();
        }

        // GET: api/CmnUserPermissionToGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CmnUserPermissionToGroup>> GetCmnUserPermissionToGroup(string id)
        {
            var cmnUserPermissionToGroup = await _context.CmnUserPermissionToGroup.FindAsync(id);

            if (cmnUserPermissionToGroup == null)
            {
                return NotFound();
            }

            return cmnUserPermissionToGroup;
        }

        // PUT: api/CmnUserPermissionToGroups/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCmnUserPermissionToGroup(string id, CmnUserPermissionToGroup cmnUserPermissionToGroup)
        {
            if (id != cmnUserPermissionToGroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(cmnUserPermissionToGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CmnUserPermissionToGroupExists(id))
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

        // POST: api/CmnUserPermissionToGroups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CmnUserPermissionToGroup>> PostCmnUserPermissionToGroup(CmnUserPermissionToGroup cmnUserPermissionToGroup)
        {
            _context.CmnUserPermissionToGroup.Add(cmnUserPermissionToGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCmnUserPermissionToGroup", new { id = cmnUserPermissionToGroup.Id }, cmnUserPermissionToGroup);
        }

        // DELETE: api/CmnUserPermissionToGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CmnUserPermissionToGroup>> DeleteCmnUserPermissionToGroup(string id)
        {
            var cmnUserPermissionToGroup = await _context.CmnUserPermissionToGroup.FindAsync(id);
            if (cmnUserPermissionToGroup == null)
            {
                return NotFound();
            }

            _context.CmnUserPermissionToGroup.Remove(cmnUserPermissionToGroup);
            await _context.SaveChangesAsync();

            return cmnUserPermissionToGroup;
        }

        private bool CmnUserPermissionToGroupExists(string id)
        {
            return _context.CmnUserPermissionToGroup.Any(e => e.Id == id);
        }
    }
}
