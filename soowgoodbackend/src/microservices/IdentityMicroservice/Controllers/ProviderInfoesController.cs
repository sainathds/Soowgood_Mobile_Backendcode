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
    public class ProviderInfoesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public ProviderInfoesController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/ProviderInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderInfo>>> GetProviderInfo()
        {
            return await _context.ProviderInfo.ToListAsync();
        }

        // GET: api/ProviderInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProviderInfo>> GetProviderInfo(string id)
        {
            var providerInfo = await _context.ProviderInfo.FindAsync(id);

            if (providerInfo == null)
            {
                return NotFound();
            }

            return providerInfo;
        }

        // PUT: api/ProviderInfoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProviderInfo(string id, ProviderInfo providerInfo)
        {
            if (id != providerInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(providerInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProviderInfoExists(id))
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

        // POST: api/ProviderInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProviderInfo>> PostProviderInfo(ProviderInfo providerInfo)
        {
            _context.ProviderInfo.Add(providerInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProviderInfo", new { id = providerInfo.Id }, providerInfo);
        }

        // DELETE: api/ProviderInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProviderInfo>> DeleteProviderInfo(string id)
        {
            var providerInfo = await _context.ProviderInfo.FindAsync(id);
            if (providerInfo == null)
            {
                return NotFound();
            }

            _context.ProviderInfo.Remove(providerInfo);
            await _context.SaveChangesAsync();

            return providerInfo;
        }

        private bool ProviderInfoExists(string id)
        {
            return _context.ProviderInfo.Any(e => e.Id == id);
        }
    }
}
