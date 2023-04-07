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
    public class AccJournalDetailsController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public AccJournalDetailsController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/AccJournalDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccJournalDetail>>> GetAccJournalDetail()
        {
            return await _context.AccJournalDetail.ToListAsync();
        }

        // GET: api/AccJournalDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccJournalDetail>> GetAccJournalDetail(string id)
        {
            var accJournalDetail = await _context.AccJournalDetail.FindAsync(id);

            if (accJournalDetail == null)
            {
                return NotFound();
            }

            return accJournalDetail;
        }

        // PUT: api/AccJournalDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccJournalDetail(string id, AccJournalDetail accJournalDetail)
        {
            if (id != accJournalDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(accJournalDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccJournalDetailExists(id))
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

        // POST: api/AccJournalDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AccJournalDetail>> PostAccJournalDetail(AccJournalDetail accJournalDetail)
        {
            _context.AccJournalDetail.Add(accJournalDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccJournalDetail", new { id = accJournalDetail.Id }, accJournalDetail);
        }

        // DELETE: api/AccJournalDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AccJournalDetail>> DeleteAccJournalDetail(string id)
        {
            var accJournalDetail = await _context.AccJournalDetail.FindAsync(id);
            if (accJournalDetail == null)
            {
                return NotFound();
            }

            _context.AccJournalDetail.Remove(accJournalDetail);
            await _context.SaveChangesAsync();

            return accJournalDetail;
        }

        private bool AccJournalDetailExists(string id)
        {
            return _context.AccJournalDetail.Any(e => e.Id == id);
        }
    }
}
