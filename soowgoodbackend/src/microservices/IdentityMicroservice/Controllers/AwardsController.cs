using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using IdentityMicroservice.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using IdentityMicroservice.ViewModels.ProviderViewModel;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwardsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IdentityMicroserviceContext _context;

        public AwardsController(
            IUserRepository userRepository,
            IdentityMicroserviceContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AwardsController> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/Awards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Awards>>> GetAwards()
        {
            return await _context.Awards.ToListAsync();
        }

        // GET: api/Awards/5
        [HttpPost("GetAward")]
        public async Task<ActionResult<IEnumerable<Awards>>> GetAwards(AwardsViewModel model)
        {
            var awards = await _context.Awards.Where(m => m.UserId == model.UserId && m.IsDeleted == false).ToListAsync();

            if (awards == null)
            {
                return NotFound();
            }

            return awards;
        }

        [HttpPost("Awards")]
        public async Task<ActionResult<Awards>> PostAward(Awards award)
        {
            try
            {
                //var awardInfo = _context.Awards.Any(m => m.Name == award.Name && m.URL == award.URL
                //                                                            && m.UserId == award.UserId && m.IsActive==true);
                //!awardInfo &&
                //if (award.Id != null)
                //else
                //    return Conflict();

                if (award.Id == null)
                {
                    _context.Awards.Add(award);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetAwards", new { id = award.Id }, award);
                }

                else 
                {
                    var Info = _context.Awards.Any(m => m.Id == award.Id);

                    if (Info)
                    {
                        await PostUpdateAward(award);
                        return CreatedAtAction("GetAwards", new { id = award.Id }, award);
                    }
                    else
                        return NotFound();
                }

               
            }
            catch (DbUpdateException ex)
            {
                return NotFound(); ;
            }
        }

        [HttpPost("UpdateAward")]
        public async Task<ActionResult<Awards>> PostUpdateAward(Awards award)
        {
            try
            {
                var awardInfo = _context.Awards.Any(m => m.Id == award.Id);

                if (awardInfo)
                {
                    _context.Awards.Update(award);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetAwards", new { id = award.Id }, award);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }


        }



        [HttpPost("DeleteAward")]
        public async Task<ActionResult<Awards>> DeleteAward(Awards award)
        {
            try
            {
                var awardInfo = _context.Awards.Any(m => m.Id == award.Id);
                if (awardInfo)
                {
                    award.IsActive = false;
                    award.IsDeleted = true;
                    _context.Awards.Update(award);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetAwards", new { id = award.Id }, award);
                }
                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }


        }

        // PUT: api/Awards/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAwards(string id, Awards awards)
        {
            if (id != awards.Id)
            {
                return BadRequest();
            }

            _context.Entry(awards).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AwardsExists(id))
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

        // POST: api/Awards
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Awards>> PostAwards(Awards awards)
        {
            _context.Awards.Add(awards);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AwardsExists(awards.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAwards", new { id = awards.Id }, awards);
        }

        // DELETE: api/Awards/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Awards>> DeleteAwards(string id)
        {
            var awards = await _context.Awards.FindAsync(id);
            if (awards == null)
            {
                return NotFound();
            }

            _context.Awards.Remove(awards);
            await _context.SaveChangesAsync();

            return awards;
        }

        private bool AwardsExists(string id)
        {
            return _context.Awards.Any(e => e.Id == id);
        }
    }
}
