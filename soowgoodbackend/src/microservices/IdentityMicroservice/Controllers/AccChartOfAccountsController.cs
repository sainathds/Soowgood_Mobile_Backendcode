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
    public class AccChartOfAccountsController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public AccChartOfAccountsController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/AccChatOfAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccChartOfAccount>>> GetAccChatOfAccount()
        {
            return await _context.AccChartOfAccount.ToListAsync();
        }

        // GET: api/AccChatOfAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccChartOfAccount>> GetAccChatOfAccount(string id)
        {
            var accChatOfAccount = await _context.AccChartOfAccount.FindAsync(id);

            if (accChatOfAccount == null)
            {
                return NotFound();
            }

            return accChatOfAccount;
        }

        // PUT: api/AccChatOfAccounts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccChatOfAccount(string id, AccChartOfAccount accChatOfAccount)
        {
            if (id != accChatOfAccount.Id)
            {
                return BadRequest();
            }

            _context.Entry(accChatOfAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccChatOfAccountExists(id))
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

        // POST: api/AccChatOfAccounts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AccChartOfAccount>> PostAccChatOfAccount(AccChartOfAccount accChatOfAccount)
        {
            _context.AccChartOfAccount.Add(accChatOfAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccChatOfAccount", new { id = accChatOfAccount.Id }, accChatOfAccount);
        }

        // DELETE: api/AccChatOfAccounts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AccChartOfAccount>> DeleteAccChatOfAccount(string id)
        {
            var accChatOfAccount = await _context.AccChartOfAccount.FindAsync(id);
            if (accChatOfAccount == null)
            {
                return NotFound();
            }

            _context.AccChartOfAccount.Remove(accChatOfAccount);
            await _context.SaveChangesAsync();

            return accChatOfAccount;
        }

        private bool AccChatOfAccountExists(string id)
        {
            return _context.AccChartOfAccount.Any(e => e.Id == id);
        }
    }
}
