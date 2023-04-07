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
    public class CmnMenuListsController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public CmnMenuListsController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/CmnMenuLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CmnMenuList>>> GetCmnMenuList()
        {
            return await _context.CmnMenuList.ToListAsync();
        }

        // GET: api/CmnMenuLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CmnMenuList>> GetCmnMenuList(long id)
        {
            var cmnMenuList = await _context.CmnMenuList.FindAsync(id);

            if (cmnMenuList == null)
            {
                return NotFound();
            }

            return cmnMenuList;
        }

        // PUT: api/CmnMenuLists/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCmnMenuList(long id, CmnMenuList cmnMenuList)
        {
            if (id != cmnMenuList.MenuId)
            {
                return BadRequest();
            }

            _context.Entry(cmnMenuList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CmnMenuListExists(id))
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

        // POST: api/CmnMenuLists
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CmnMenuList>> PostCmnMenuList(CmnMenuList cmnMenuList)
        {
            _context.CmnMenuList.Add(cmnMenuList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCmnMenuList", new { id = cmnMenuList.MenuId }, cmnMenuList);
        }

        // DELETE: api/CmnMenuLists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CmnMenuList>> DeleteCmnMenuList(long id)
        {
            var cmnMenuList = await _context.CmnMenuList.FindAsync(id);
            if (cmnMenuList == null)
            {
                return NotFound();
            }

            _context.CmnMenuList.Remove(cmnMenuList);
            await _context.SaveChangesAsync();

            return cmnMenuList;
        }

        private bool CmnMenuListExists(long id)
        {
            return _context.CmnMenuList.Any(e => e.MenuId == id);
        }
    }
}
