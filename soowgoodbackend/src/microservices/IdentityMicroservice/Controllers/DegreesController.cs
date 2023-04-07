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
    public class DegreesController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IdentityMicroserviceContext _context;

        public DegreesController(
            IUserRepository userRepository,
            IdentityMicroserviceContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<DegreesController> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/Degrees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Degree>>> GetDegree()
        {
            return await _context.Degree.ToListAsync();
        }

        [HttpGet("mylist")]
        public string mylist()
        {
            string myname = "Nitin";
            return myname;
        }

        // GET: api/Degrees/5
        [HttpPost("GetDegree")]
        public async Task<ActionResult<IEnumerable<Degree>>> GetDegree(DegreeViewModel model)
        {
            var degree = await _context.Degree.Where(m => m.UserId == model.UserId && m.IsActive==true).ToListAsync();

            if (degree == null)
            {
                return NotFound();
            }

            return degree;
        }


        [HttpPost("Degree")]
        public async Task<ActionResult<Degree>> PostExperience(Degree degree)
        {
            try
            {
                //var degreeInfo = _context.Degree.Any(m => m.Name == degree.Name && m.Institution == degree.Institution
                //                                                            && m.UserId == degree.UserId);
                //!degreeInfo &&
                //else
                //    return Conflict();

                if ( degree.Id == null)
                {
                    _context.Degree.Add(degree);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetDegree", new { id = degree.Id }, degree);
                }
                else
                {
                    var Info = _context.Degree.Any(m => m.Id == degree.Id);

                    if (Info)
                    {
                        await PostUpdateDegree(degree);
                        return CreatedAtAction("GetDegree", new { id = degree.Id }, degree);
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

        [HttpPost("UpdateDegree")]
        public async Task<ActionResult<Degree>> PostUpdateDegree(Degree degree)
        {
            try
            {
                var degreeInfo = _context.Degree.Any(m => m.Id == degree.Id);

                if (degreeInfo)
                {
                    _context.Degree.Update(degree);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetDegree", new { id = degree.Id }, degree);
                }

                else
                    return NotFound();

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



        [HttpPost("DeleteDegree")]
        public async Task<ActionResult<Degree>> DeleteDegree(Degree degree)
        {
            try
            {
                var degreeInfo = _context.Degree.Any(m => m.Id == degree.Id);

                if (degreeInfo)
                {
                    degree.IsActive = false;
                    degree.IsDeleted = true;
                    _context.Degree.Update(degree);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetDegree", new { id = degree.Id }, degree);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }


        }

        // PUT: api/Degrees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDegree(string id, Degree degree)
        {
            if (id != degree.Id)
            {
                return BadRequest();
            }

            _context.Entry(degree).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DegreeExists(id))
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

        // DELETE: api/Degrees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Degree>> DeleteDegree(string id)
        {
            var degree = await _context.Degree.FindAsync(id);
            if (degree == null)
            {
                return NotFound();
            }

            _context.Degree.Remove(degree);
            await _context.SaveChangesAsync();

            return degree;
        }

        private bool DegreeExists(string id)
        {
            return _context.Degree.Any(e => e.Id == id);
        }
    }
}
