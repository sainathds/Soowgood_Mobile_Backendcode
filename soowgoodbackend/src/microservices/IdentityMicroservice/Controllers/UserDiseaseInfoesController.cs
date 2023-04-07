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
    public class UserDiseaseInfoesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public UserDiseaseInfoesController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/UserDiseaseInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDiseaseInfo>>> GetUserDiseaseInfo()
        {
            return await _context.UserDiseaseInfo.ToListAsync();
        }

        // GET: api/UserDiseaseInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDiseaseInfo>> GetUserDiseaseInfo(string id)
        {
            var userDiseaseInfo = await _context.UserDiseaseInfo.FindAsync(id);

            if (userDiseaseInfo == null)
            {
                return NotFound();
            }

            return userDiseaseInfo;
        }

        // PUT: api/UserDiseaseInfoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDiseaseInfo(string id, UserDiseaseInfo userDiseaseInfo)
        {
            if (id != userDiseaseInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(userDiseaseInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDiseaseInfoExists(id))
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

        // POST: api/UserDiseaseInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserDiseaseInfo>> PostUserDiseaseInfo(UserDiseaseInfo userDiseaseInfo)
        {
            _context.UserDiseaseInfo.Add(userDiseaseInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserDiseaseInfo", new { id = userDiseaseInfo.Id }, userDiseaseInfo);
        }

        // DELETE: api/UserDiseaseInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDiseaseInfo>> DeleteUserDiseaseInfo(string id)
        {
            var userDiseaseInfo = await _context.UserDiseaseInfo.FindAsync(id);
            if (userDiseaseInfo == null)
            {
                return NotFound();
            }

            _context.UserDiseaseInfo.Remove(userDiseaseInfo);
            await _context.SaveChangesAsync();

            return userDiseaseInfo;
        }

        private bool UserDiseaseInfoExists(string id)
        {
            return _context.UserDiseaseInfo.Any(e => e.Id == id);
        }
    }
}
