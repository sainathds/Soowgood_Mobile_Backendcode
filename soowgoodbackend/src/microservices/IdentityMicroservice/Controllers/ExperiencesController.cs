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
    public class ExperiencesController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IdentityMicroserviceContext _context;

        public ExperiencesController(
           IUserRepository userRepository,
           IdentityMicroserviceContext context,
           RoleManager<IdentityRole> roleManager,
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           IEmailSender emailSender,
           ILogger<ExperiencesController> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/Experiences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Experience>>> GetExperience()
        {
            return await _context.Experience.ToListAsync();
        }

        // GET: api/Experiences/5
        [HttpPost("GetExperience")]
        public async Task<ActionResult<IEnumerable<Experience>>> GetExperience(ExperienceViewModel model)
        {

            var experience = await _context.Experience.Where(m => m.UserId == model.UserId && m.IsActive == true).ToListAsync();

            if (experience == null)
            {
                return NotFound();
            }
            else
            {
                return experience;
            }
        }

        [HttpPost("Experience")]
        public async Task<ActionResult<Experience>> PostExperience(Experience experience)
        {
            try
            {
                var experienceInfo = _context.Experience.Any(m => m.HospitalName == experience.HospitalName && m.Designation == experience.Designation
                                                                             && m.UserId == experience.UserId);

                if (experience.Id == null)
                {
                    _context.Experience.Add(experience);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetExperience", new { id = experience.Id }, experience);
                }

                else
                {
                    var Info = _context.Experience.Any(m => m.Id == experience.Id);

                    if (Info)
                    {
                        await PostUpdateExperience(experience);
                        return CreatedAtAction("GetExperience", new { id = experience.Id }, experience);
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

        [HttpPost("UpdateExperience")]
        public async Task<ActionResult<Experience>> PostUpdateExperience(Experience experience)
        {
            try
            {
                var experienceInfo = _context.Experience.Any(m => m.Id == experience.Id);

                if (experienceInfo)
                {
                    _context.Experience.Update(experience);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetExperience", new { id = experience.Id }, experience);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }
        }




        [HttpPost("DeleteExperience")]
        public async Task<ActionResult<Experience>> DeleteExperience(Experience experience)
        {
            try
            {
                var experienceInfo = _context.Experience.Any(m => m.Id == experience.Id);

                if (experienceInfo)
                {
                    experience.IsActive = false;
                    experience.IsDeleted = true;
                    _context.Experience.Update(experience);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetExperience", new { id = experience.Id }, experience);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }
        }

        // PUT: api/Experiences/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExperience(string id, Experience experience)
        {
            if (id != experience.Id)
            {
                return BadRequest();
            }

            _context.Entry(experience).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExperienceExists(id))
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

        // POST: api/Experiences
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        // DELETE: api/Experiences/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Experience>> DeleteExperience(string id)
        {
            var experience = await _context.Experience.FindAsync(id);
            if (experience == null)
            {
                return NotFound();
            }

            _context.Experience.Remove(experience);
            await _context.SaveChangesAsync();

            return experience;
        }

        private bool ExperienceExists(string id)
        {
            return _context.Experience.Any(e => e.Id == id);
        }
    }
}
