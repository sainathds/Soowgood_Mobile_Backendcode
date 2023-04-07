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
    public class PricingPlansController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public PricingPlansController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/PricingPlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PricingPlan>>> GetPricingPlan()
        {
            return await _context.PricingPlan.ToListAsync();
        }

        // GET: api/PricingPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PricingPlan>> GetPricingPlan(string id)
        {
            var pricingPlan = await _context.PricingPlan.FindAsync(id);

            if (pricingPlan == null)
            {
                return NotFound();
            }

            return pricingPlan;
        }

        // PUT: api/PricingPlans/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPricingPlan(string id, PricingPlan pricingPlan)
        {
            if (id != pricingPlan.Id)
            {
                return BadRequest();
            }

            _context.Entry(pricingPlan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PricingPlanExists(id))
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

        // POST: api/PricingPlans
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PricingPlan>> PostPricingPlan(PricingPlan pricingPlan)
        {
            _context.PricingPlan.Add(pricingPlan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPricingPlan", new { id = pricingPlan.Id }, pricingPlan);
        }

        // DELETE: api/PricingPlans/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PricingPlan>> DeletePricingPlan(string id)
        {
            var pricingPlan = await _context.PricingPlan.FindAsync(id);
            if (pricingPlan == null)
            {
                return NotFound();
            }

            _context.PricingPlan.Remove(pricingPlan);
            await _context.SaveChangesAsync();

            return pricingPlan;
        }

        private bool PricingPlanExists(string id)
        {
            return _context.PricingPlan.Any(e => e.Id == id);
        }
    }
}
