using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using Nager.Country;
using IdentityMicroservice.ViewModels.Address;
using IdentityMicroservice.Interfaces;
using Microsoft.AspNetCore.Identity;
using AutoMapper.Configuration;
using Microsoft.Extensions.Logging;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressesController : ControllerBase
    {
        private readonly ICountryProvider _countryProvider;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IdentityMicroserviceContext _context;
        private readonly IConfiguration _configuration;

        public UserAddressesController(
            ICountryProvider countryProvider,
            IUserRepository userRepository,
            IdentityMicroserviceContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<UserAddressesController> logger)
        {
            _countryProvider = countryProvider;
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/UserAddresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAddress>>> GetUserAddress()
        {
            return await _context.UserAddress.ToListAsync();
        }

        // GET: api/UserAddresses/5
        [HttpPost("GetAddress")]
        public async Task<ActionResult<IEnumerable<UserAddress>>> GetUserAddress(UserAddress model)
        {
            var userAddress = await _context.UserAddress.Where(m=>m.UserId == model.UserId).ToListAsync();

            if (userAddress == null)
            {
                return NotFound();
            }

            return userAddress;
        }

        // PUT: api/UserAddresses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAddress(string id, UserAddress userAddress)
        {
            if (id != userAddress.Id)
            {
                return BadRequest();
            }

            _context.Entry(userAddress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAddressExists(id))
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

        // POST: api/UserAddresses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        
        [HttpPost("Address")]
        public async Task<ActionResult<UserAddress>> PostExperience(UserAddress userAddress)
        {
            try
            {
                var addressInfo = _context.UserAddress.Any(m => m.City == userAddress.City && m.Country == userAddress.Country
                                                                            && m.CurrentAddress == userAddress.CurrentAddress && m.OptionalAddress == userAddress.OptionalAddress
                                                                            && m.PostalCode == userAddress.PostalCode && m.PreferableAddress == userAddress.PreferableAddress
                                                                            && m.State == userAddress.State && m.UserId == userAddress.UserId);

                if (!addressInfo && userAddress.Id == null)
                {
                    _context.UserAddress.Add(userAddress);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetUserAddress", new { id = userAddress.Id }, userAddress);
                }

                else if (userAddress.Id != null)
                {
                    var Info = _context.UserAddress.Any(m => m.Id == userAddress.Id);

                    if (Info)
                    {
                        await PostUpdateUserAddress(userAddress);
                        return CreatedAtAction("GetUserAddress", new { id = userAddress.Id }, userAddress);
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

        [HttpPost("UpdateAddress")]
        public async Task<ActionResult<UserAddress>> PostUpdateUserAddress(UserAddress userAddress)
        {
            try
            {
                var address = _context.UserAddress.Any(m => m.Id == userAddress.Id);

                if (address)
                {
                    _context.UserAddress.Update(userAddress);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetUserAddress", new { id = userAddress.Id }, userAddress);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return null;
            }

            
        }

        // DELETE: api/UserAddresses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserAddress>> DeleteUserAddress(string id)
        {
            var userAddress = await _context.UserAddress.FindAsync(id);
            if (userAddress == null)
            {
                return NotFound();
            }

            _context.UserAddress.Remove(userAddress);
            await _context.SaveChangesAsync();

            return userAddress;
        }

        private bool UserAddressExists(string id)
        {
            return _context.UserAddress.Any(e => e.Id == id);
        }

        [HttpGet("CountryList")]
        public List<string> GetCountryList()
        {
            var name = _countryProvider.GetCountries().Select(m=>m.CommonName);
            return name.ToList();
        }
    }
}
