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
    public class CmnMenuPermissionToGroupsController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public CmnMenuPermissionToGroupsController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/CmnMenuPermissionToGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CmnMenuPermissionToGroup>>> GetCmnMenuPermissionToGroup()
        {
            return await _context.CmnMenuPermissionToGroup.ToListAsync();
        }

        // GET: api/CmnMenuPermissionToGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CmnMenuPermissionToGroup>> GetCmnMenuPermissionToGroup(string id)
        {
            var cmnMenuPermissionToGroup = await _context.CmnMenuPermissionToGroup.FindAsync(id);

            if (cmnMenuPermissionToGroup == null)
            {
                return NotFound();
            }

            return cmnMenuPermissionToGroup;
        }

        // PUT: api/CmnMenuPermissionToGroups/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCmnMenuPermissionToGroup(string id, CmnMenuPermissionToGroup cmnMenuPermissionToGroup)
        {
            if (id != cmnMenuPermissionToGroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(cmnMenuPermissionToGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CmnMenuPermissionToGroupExists(id))
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

        // POST: api/CmnMenuPermissionToGroups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CmnMenuPermissionToGroup>> PostCmnMenuPermissionToGroup(CmnMenuPermissionToGroup cmnMenuPermissionToGroup)
        {
            _context.CmnMenuPermissionToGroup.Add(cmnMenuPermissionToGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCmnMenuPermissionToGroup", new { id = cmnMenuPermissionToGroup.Id }, cmnMenuPermissionToGroup);
        }

        // DELETE: api/CmnMenuPermissionToGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CmnMenuPermissionToGroup>> DeleteCmnMenuPermissionToGroup(string id)
        {
            var cmnMenuPermissionToGroup = await _context.CmnMenuPermissionToGroup.FindAsync(id);
            if (cmnMenuPermissionToGroup == null)
            {
                return NotFound();
            }

            _context.CmnMenuPermissionToGroup.Remove(cmnMenuPermissionToGroup);
            await _context.SaveChangesAsync();

            return cmnMenuPermissionToGroup;
        }

        private bool CmnMenuPermissionToGroupExists(string id)
        {
            return _context.CmnMenuPermissionToGroup.Any(e => e.Id == id);
        }
    }
}
