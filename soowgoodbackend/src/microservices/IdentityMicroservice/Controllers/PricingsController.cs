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
using System.Transactions;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IdentityMicroserviceContext _context;

        public PricingsController(
            IUserRepository userRepository,
            IdentityMicroserviceContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<PricingsController> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/Pricings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pricing>>> GetPricing()
        {
            return await _context.Pricing.ToListAsync();
        }

        // GET: api/Pricings/5
        [HttpPost("DefaultPricingPlan")]
        public async Task<ActionResult<Pricing>> GetPricing(PricingViewModel model)
        {
            var pricing = await _context.Pricing.Where(m=>m.UserId == model.UserId && m.IsActive ==true).FirstOrDefaultAsync();

            if (pricing == null)
            {
                return NotFound();
            }

            return pricing;
        }

        [HttpPost("PricingPlanList")]
        public async Task<ActionResult<IEnumerable<Pricing>>> GetAllPricingPlanList(PricingViewModel model)
        {
            var pricing = await _context.Pricing.Where(m => m.UserTypeId == model.UserTypeId && m.IsActive == true).ToListAsync();

            if (pricing == null)
            {
                return NotFound();
            }

            return pricing.ToList();
        }

        [HttpPost("Pricing")]
        public async Task<ActionResult<Pricing>> PostExperience(Pricing pricing)
        {
            try
            {
                //var pricingInfo = _context.Pricing.Any(m => m.UserId == pricing.UserId && m.IsActive == true && m.IsDefault == false);

                if (pricing.Id == null)
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        ResetPreviousPricingDataByUser(pricing);
                        var _pricing = GetPricingPlanData(pricing.PlanId, pricing.UserId);
                        _context.Pricing.Add(_pricing);
                        await _context.SaveChangesAsync();
                        scope.Complete();
                        return CreatedAtAction("GetPricing", new { id = _pricing.Id }, _pricing);
                    }
                }
                else
                    return Conflict();
            }
            catch (DbUpdateException ex)
            {
                return NotFound(); ;
            }
        }

        [HttpPost("UpdatePricing")]
        public async Task<ActionResult<Pricing>> PostUpdatePricing(Pricing pricing)
        {
            try
            {
                var pricingInfo = _context.Pricing.Any(m => m.Id == pricing.Id);

                if (pricingInfo)
                {
                    _context.Pricing.Update(pricing);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetPricing", new { id = pricing.Id }, pricing);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }
        }

        public Pricing GetPricingPlanData(string pricingPlanId, string userId)
        {
            try
            {
                    var data = _context.PricingPlan.Where(p => p.IsActive == true && p.Id == pricingPlanId).FirstOrDefault();
                    var Data_Object = new Pricing
                    {
                        PlanId = data.Id,
                        NumberOfUsers = 10,
                        UserId = userId,
                        Name = data.Name,

                        Description = data.Description,
                        PlanStartingDate = DateTime.Now,
                        AmountPerUser = data.AmountPerUser,

                        NumberOfFreeUsers = data.NumberOfFreeUsers,
                        DiscountAmount = data.DiscountAmount,
                        IsDiscount = data.IsDiscount,
                        IsTrialPeriodOver = data.IsTrialPeriodOver,

                        IsOrganization = data.IsOrganization,
                        IsFree = data.IsFree,
                        IsMonthly = data.IsMonthly,
                        IsWeekly = data.IsWeekly,

                        IsCustomized = data.IsCustomized,
                        IsYearly = data.IsYearly,
                        IsDefault = data.IsDefault,
                        Currency = data.Currency,

                        CurrencySymbol = data.CurrencySymbol,
                        UserTypeId = data.UserTypeId,
                        TrialPeriodOver = data.PlanStartingDate.AddMonths(6),
                        Commission = data.Commission
                    };
                return Data_Object;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool ResetPreviousPricingDataByUser(Pricing pricing)
        {
            List<Pricing> results = (from p in _context.Pricing.Where(p=>p.UserId == pricing.UserId)
                                    select p).ToList();

            foreach (Pricing p in results)
            {
                p.IsActive = false;
            }

            _context.SaveChanges();
            return true;
        }

        // PUT: api/Pricings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPricing(string id, Pricing pricing)
        {
            if (id != pricing.Id)
            {
                return BadRequest();
            }

            _context.Entry(pricing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PricingExists(id))
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

        // POST: api/Pricings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
       
        // DELETE: api/Pricings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pricing>> DeletePricing(string id)
        {
            var pricing = await _context.Pricing.FindAsync(id);
            if (pricing == null)
            {
                return NotFound();
            }

            _context.Pricing.Remove(pricing);
            await _context.SaveChangesAsync();

            return pricing;
        }

        private bool PricingExists(string id)
        {
            return _context.Pricing.Any(e => e.Id == id);
        }
    }
}
