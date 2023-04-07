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
using IdentityMicroservice.StaticData;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IdentityMicroserviceContext _context;

        public SpecializationsController(
            IUserRepository userRepository,
            IdentityMicroserviceContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<SpecializationsController> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/Specializations
        [HttpPost("SpecializationList")]
        public IEnumerable<string> GetSpecialization(SpecializationViewModel model)
        {
            if (String.IsNullOrEmpty(model.SearchKeyword))
            {
                var rowsToReturn = _context.Specialization
                .Select(d => d.SpecializationName)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

                var result = string.Join(",", rowsToReturn.ToArray());
                string[] values = result.Split(',');
                var filtered = values.Where(s => !string.IsNullOrWhiteSpace(s)).ToList().Distinct();
                return filtered;
            }

            else
            {

                var rowsToReturn = _context.Specialization
                .Where(d => d.SpecializationName.Contains(model.SearchKeyword))
               .Select(d => d.SpecializationName)
               .Distinct()
               .OrderBy(d => d)
               .ToList();

                var result = string.Join(",", rowsToReturn.ToArray());
                string[] values = result.Split(',');
                var filtered = values.Where(s => !string.IsNullOrWhiteSpace(s)).ToList().Distinct();
                return filtered;
            }
        }

        // GET: api/Specializations
        [HttpPost("ServiceList")]
        public IEnumerable<string> GetServiceListAsync(SpecializationViewModel model)
        {
            if (String.IsNullOrEmpty(model.SearchKeyword))
            {
                var rowsToReturn = _context.Specialization
                .Select(d => d.ServiceName)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

                var result = string.Join(",", rowsToReturn.ToArray());
                string[] values = result.Split(',');
                var filtered = values.Where(s => !string.IsNullOrWhiteSpace(s)).ToList().Distinct();
                return filtered;
            }

            else
            {

                var rowsToReturn = _context.Specialization
                .Where(d => d.ServiceName.Contains(model.SearchKeyword))
               .Select(d => d.ServiceName)
               .Distinct()
               .OrderBy(d => d)
               .ToList();

                var result = string.Join(",", rowsToReturn.ToArray());
                string[] values = result.Split(',');
                var filtered = values.Where(s => !string.IsNullOrWhiteSpace(s)).ToList().Distinct();
                return filtered;
            }
        }


        
        [HttpPost("getSpecializationdata")]
        public async Task<ActionResult<Specialization>> GetSpecializationData(SpecializationViewModel model)
        {
            var specialization = await _context.Specialization.FindAsync(model.Id);

            if (specialization == null)
            {
                return NotFound();
            }

            return specialization;
        }

        // GET: api/Specializations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Specialization>> GetSpecialization(string id)
        {
            var specialization = await _context.Specialization.FindAsync(id);

            if (specialization == null)
            {
                return NotFound();
            }

            return specialization;
        }

        [HttpPost("GetSpecialization")]
        public ActionResult<IEnumerable<SpecializationViewModel>> GetSpecializationList(SpecializationViewModel model)
        {
            //var specialization = await _context.Specialization.Where(m => m.UserId == model.UserId).FirstOrDefaultAsync();

            var specialization = (from a in _context.Specialization
                                  join b in _context.ProviderInfo.Where(p => p.IsActive == true) on a.TypeId equals b.Id
                                  where a.UserId == model.UserId && a.IsDeleted == false
                                  select new SpecializationViewModel
                                  {
                                      Id = a.Id,
                                      SpecializationName = a.SpecializationName,
                                      ServiceName = a.ServiceName,
                                      Description = a.Description,
                                      UserId = a.UserId,
                                      TypeId = b.Id,
                                      Type = b.Provider
                                  }).ToList();


            if (specialization == null)
            {
                return NotFound();
            }
            else
            {
                return specialization.ToList();
            }
        }

        // PUT: api/Specializations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecialization(string id, Specialization specialization)
        {
            if (id != specialization.Id)
            {
                return BadRequest();
            }

            _context.Entry(specialization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecializationExists(id))
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

        // POST: api/Specializations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("Information")]
        public async Task<authentication> PostSpecialization(Specialization model)
        {
            authentication _userauth = new authentication();
            try
            {
                var spec = _context.Specialization.Where(m => m.UserId == model.UserId && m.IsDeleted == false).FirstOrDefault();
               
                if (spec == null)
                {
                    _context.Specialization.Add(model);
                    await _context.SaveChangesAsync();

                    _userauth.success = "1";
                    _userauth.message = "Skill information saved successfully.";

                }
                else
                {
                    _userauth.success = "0";
                    _userauth.message = "Skill information already present.";
                }
            }
            catch (DbUpdateException ex)
            {
                _userauth.success = "0";
                _userauth.message = ex.Message;
            }
            return _userauth;
        }

        [HttpPost("UpdateInformation")]
        public async Task<ActionResult<Specialization>> PostUpdateSpecialization(Specialization specialization)
        {
            try
            {
                var specializationInfo = _context.Specialization.Where(m => m.Id == specialization.Id).FirstOrDefault();

                if (specializationInfo != null)
                {
                    specializationInfo.Id = specialization.Id;
                    specializationInfo.SpecializationName = specialization.SpecializationName;
                    specializationInfo.ServiceName = specialization.ServiceName;
                    specializationInfo.TypeId = specialization.TypeId;
                    specializationInfo.UserId = specialization.UserId;

                    _context.Specialization.Update(specializationInfo);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetSpecialization", new { id = specialization.Id }, specialization);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }
        }


        [HttpPost("deleteSpecialization")]
        public async Task<ActionResult<Specialization>> DeleteSpecialization(Specialization specialization)
        {
            try
            {
                var specializationInfo = _context.Specialization.Where(m => m.Id == specialization.Id).FirstOrDefault();

                if (specializationInfo != null)
                {
                    specializationInfo.Id = specialization.Id;
                    specializationInfo.IsActive = false;
                    specializationInfo.IsDeleted = true;

                    _context.Specialization.Update(specializationInfo);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetSpecialization", new { id = specialization.Id }, specialization);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }
        }

        // DELETE: api/Specializations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Specialization>> DeleteSpecialization(string id)
        {
            var specialization = await _context.Specialization.FindAsync(id);
            if (specialization == null)
            {
                return NotFound();
            }

            _context.Specialization.Remove(specialization);
            await _context.SaveChangesAsync();

            return specialization;
        }

        private bool SpecializationExists(string id)
        {
            return _context.Specialization.Any(e => e.Id == id);
        }
    }
}
