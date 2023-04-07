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
using IdentityMicroservice.ViewModels.UserViewModels;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAboutMeController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IdentityMicroserviceContext _context;

        public UserAboutMeController(
            IUserRepository userRepository,
            IdentityMicroserviceContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<UserAboutMeController> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/UserAboutMes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAboutMe>>> GetUserAboutMe()
        {
            return await _context.UserAboutMe.ToListAsync();
        }

        // GET: api/UserAboutMes/5
        [HttpPost("GetAboutMe")]
        public async Task<ActionResult<UserAboutMe>> GetUserAboutMe(UserAboutMeViewModel model)
        {
            var userAboutMe = await _context.UserAboutMe.Where(m => m.UserId == model.UserId).FirstOrDefaultAsync();
            return userAboutMe;
        }

        // PUT: api/UserAboutMes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAboutMe(string id, UserAboutMe userAboutMe)
        {
            if (id != userAboutMe.Id)
            {
                return BadRequest();
            }

            _context.Entry(userAboutMe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAboutMeExists(id))
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

        // POST: api/UserAboutMes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [HttpPost("AboutMe")]
        public async Task<ActionResult<UserAboutMe>> PostUserAboutMe(UserAboutMe aboutMe)
        {
            try
            {
                var aboutMeInfo = _context.UserAboutMe.Any(m => m.UserId == aboutMe.UserId);

                if (!aboutMeInfo && aboutMe.Id == null)
                {
                    _context.UserAboutMe.Add(aboutMe);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetUserAboutMe", new { id = aboutMe.Id }, aboutMe);
                }

                else if (aboutMe.Id != null)
                {
                    var Info = _context.UserAboutMe.Any(m => m.Id == aboutMe.Id);

                    if (Info)
                    {
                        await PostUpdateUserAboutMe(aboutMe);
                        return CreatedAtAction("GetUserAboutMe", new { id = aboutMe.Id }, aboutMe);
                    }
                    else
                        return NotFound();
                }

                else
                    return Conflict();
            }
            catch (DbUpdateException ex)
            {
                return NotFound(); ;
            }
        }

        [HttpPost("UpdateAboutMe")]
        public async Task<ActionResult<UserAboutMe>> PostUpdateUserAboutMe(UserAboutMe aboutMe)
        {
            var _userAboutMe = _context.UserAboutMe.Any(m => m.Id == aboutMe.Id);

            if (_userAboutMe)
            {
                _context.UserAboutMe.Update(aboutMe);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUserAboutMe", new { id = aboutMe.Id }, aboutMe);
            }

            else
                return NotFound();
        }

        // DELETE: api/UserAboutMes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserAboutMe>> DeleteUserAboutMe(string id)
        {
            var userAboutMe = await _context.UserAboutMe.FindAsync(id);
            if (userAboutMe == null)
            {
                return NotFound();
            }

            _context.UserAboutMe.Remove(userAboutMe);
            await _context.SaveChangesAsync();

            return userAboutMe;
        }

        private bool UserAboutMeExists(string id)
        {
            return _context.UserAboutMe.Any(e => e.Id == id);
        }
    }
}
