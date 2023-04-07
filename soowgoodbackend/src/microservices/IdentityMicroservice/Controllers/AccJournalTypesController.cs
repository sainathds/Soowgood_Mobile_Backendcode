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
    public class AccJournalTypesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public AccJournalTypesController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/AccJournalTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccJournalType>>> GetAccJournalType()
        {
            return await _context.AccJournalType.ToListAsync();
        }

        // GET: api/AccJournalTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccJournalType>> GetAccJournalType(string id)
        {
            var accJournalType = await _context.AccJournalType.FindAsync(id);

            if (accJournalType == null)
            {
                return NotFound();
            }

            return accJournalType;
        }

        // PUT: api/AccJournalTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccJournalType(string id, AccJournalType accJournalType)
        {
            if (id != accJournalType.Id)
            {
                return BadRequest();
            }

            _context.Entry(accJournalType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccJournalTypeExists(id))
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

        // POST: api/AccJournalTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AccJournalType>> PostAccJournalType(AccJournalType accJournalType)
        {
            _context.AccJournalType.Add(accJournalType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccJournalType", new { id = accJournalType.Id }, accJournalType);
        }

        // DELETE: api/AccJournalTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AccJournalType>> DeleteAccJournalType(string id)
        {
            var accJournalType = await _context.AccJournalType.FindAsync(id);
            if (accJournalType == null)
            {
                return NotFound();
            }

            _context.AccJournalType.Remove(accJournalType);
            await _context.SaveChangesAsync();

            return accJournalType;
        }

        private bool AccJournalTypeExists(string id)
        {
            return _context.AccJournalType.Any(e => e.Id == id);
        }
    }
}
