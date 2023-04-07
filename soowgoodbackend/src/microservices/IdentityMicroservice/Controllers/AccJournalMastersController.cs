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
    public class AccJournalMastersController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public AccJournalMastersController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/AccJournalMasters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccJournalMaster>>> GetAccJournalMaster()
        {
            return await _context.AccJournalMaster.ToListAsync();
        }

        // GET: api/AccJournalMasters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccJournalMaster>> GetAccJournalMaster(string id)
        {
            var accJournalMaster = await _context.AccJournalMaster.FindAsync(id);

            if (accJournalMaster == null)
            {
                return NotFound();
            }

            return accJournalMaster;
        }

        // PUT: api/AccJournalMasters/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccJournalMaster(string id, AccJournalMaster accJournalMaster)
        {
            if (id != accJournalMaster.Id)
            {
                return BadRequest();
            }

            _context.Entry(accJournalMaster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccJournalMasterExists(id))
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

        // POST: api/AccJournalMasters
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AccJournalMaster>> PostAccJournalMaster(AccJournalMaster accJournalMaster)
        {
            _context.AccJournalMaster.Add(accJournalMaster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccJournalMaster", new { id = accJournalMaster.Id }, accJournalMaster);
        }

        // DELETE: api/AccJournalMasters/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AccJournalMaster>> DeleteAccJournalMaster(string id)
        {
            var accJournalMaster = await _context.AccJournalMaster.FindAsync(id);
            if (accJournalMaster == null)
            {
                return NotFound();
            }

            _context.AccJournalMaster.Remove(accJournalMaster);
            await _context.SaveChangesAsync();

            return accJournalMaster;
        }

        private bool AccJournalMasterExists(string id)
        {
            return _context.AccJournalMaster.Any(e => e.Id == id);
        }
    }
}
