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
    public class MembershipsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IdentityMicroserviceContext _context;


        public MembershipsController(
            IUserRepository userRepository,
            IdentityMicroserviceContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<MembershipsController> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/Memberships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Membership>>> GetMembership()
        {
            return await _context.Membership.ToListAsync();
        }

        // GET: api/Memberships/5
        [HttpPost("GetMembership")]
        public async Task<ActionResult<IEnumerable<Membership>>> GetMembership(MembershipViewModel model)
        {
            var membership = await _context.Membership.Where(m => m.UserId == model.UserId && m.IsActive==true).ToListAsync();

            if (membership == null)
            {
                return NotFound();
            }

            return membership;
        }

        [HttpPost("Membership")]
        public async Task<ActionResult<Membership>> PostMembership(Membership membership)
        {
            try
            {
                //var membershipInfo = _context.Membership.Any(m => m.MembershipName == membership.MembershipName && m.UserId == membership.UserId);
                //!membershipInfo &&
                //if (membership.Id != null)
                // else
                //return Conflict();
                if ( membership.Id == null)
                {
                    _context.Membership.Add(membership);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetMembership", new { id = membership.Id }, membership);
                }

                else 
                {
                    var Info = _context.Membership.Any(m => m.Id == membership.Id);

                    if (Info)
                    {
                        await PostUpdateMembership(membership);
                        return CreatedAtAction("GetMembership", new { id = membership.Id }, membership);
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

        [HttpPost("UpdateMembership")]
        public async Task<ActionResult<Membership>> PostUpdateMembership(Membership membership)
        {
            try
            {
                var membershipInfo = _context.Membership.Any(m => m.Id == membership.Id);

                if (membershipInfo)
                {
                    _context.Membership.Update(membership);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetMembership", new { id = membership.Id }, membership);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }
        }




        [HttpPost("DeleteMembership")]
        public async Task<ActionResult<Membership>> PostDeleteMembership(Membership membership)
        {
            try
            {
                var membershipInfo = _context.Membership.Any(m => m.Id == membership.Id);

                if (membershipInfo)
                {
                    membership.IsActive = false;
                    membership.IsDeleted = true;
                    _context.Membership.Update(membership);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetMembership", new { id = membership.Id }, membership);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }
        }

        // PUT: api/Memberships/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMembership(string id, Membership membership)
        {
            if (id != membership.Id)
            {
                return BadRequest();
            }

            _context.Entry(membership).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembershipExists(id))
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

        // DELETE: api/Memberships/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Membership>> DeleteMembership(string id)
        {
            var membership = await _context.Membership.FindAsync(id);
            if (membership == null)
            {
                return NotFound();
            }

            _context.Membership.Remove(membership);
            await _context.SaveChangesAsync();

            return membership;
        }

        private bool MembershipExists(string id)
        {
            return _context.Membership.Any(e => e.Id == id);
        }
    }
}
