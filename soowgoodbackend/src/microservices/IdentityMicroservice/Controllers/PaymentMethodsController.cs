using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using IdentityMicroservice.ViewModels.ManageViewModels;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public PaymentMethodsController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/PaymentMethods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetPaymentMethod()
        {
            return await _context.PaymentMethod.ToListAsync();
        }

        // GET: api/Awards/5
        [HttpPost("GetPaymentSystem")]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetPaymentSystem(PaymentMethodViewModel model)
        {
            var paymentMethod = await _context.PaymentMethod.Where(m => m.IsActive == true && m.IsParent == true).ToListAsync();

            if (paymentMethod == null)
            {
                return NotFound();
            }

            return paymentMethod;
        }

        [HttpPost("GetPaymentMethod")]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetPaymentMethod(PaymentMethodViewModel model)
        {
            var paymentMethod = await _context.PaymentMethod.Where(m => m.IsActive == true && m.IsParent == false && m.ParentId == model.ParentId).ToListAsync();

            if (paymentMethod == null)
            {
                return NotFound();
            }

            return paymentMethod;
        }

        [HttpPost("PaymentMethod")]
        public async Task<ActionResult<PaymentMethod>> PostPaymentMethod(PaymentMethod model)
        {
            try
            {
                var PaymentMethodInfo = _context.PaymentMethod.Any(m => m.Name == model.Name && m.IsActive == true);

                if (!PaymentMethodInfo && model.Id == null)
                {
                    _context.PaymentMethod.Add(model);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetPaymentMethod", new { id = model.Id }, model);
                }

                else if (model.Id != null)
                {
                    var Info = _context.PaymentMethod.Any(m => m.Id == model.Id);

                    if (Info)
                    {
                        await UpdatePaymentMethod(model);
                        return CreatedAtAction("GetPaymentMethod", new { id = model.Id }, model);
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

        [HttpPost("UpdatePaymentMethod")]
        public async Task<ActionResult<PaymentMethod>> UpdatePaymentMethod(PaymentMethod model)
        {
            try
            {
                var PaymentMethodInfo = _context.PaymentMethod.Any(m => m.Id == model.Id);

                if (PaymentMethodInfo)
                {
                    _context.PaymentMethod.Update(model);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetPaymentMethod", new { id = model.Id }, model);
                }

                else
                    return NotFound();

            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }


        }

        // GET: api/PaymentMethods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethod>> GetPaymentMethod(string id)
        {
            var paymentMethod = await _context.PaymentMethod.FindAsync(id);

            if (paymentMethod == null)
            {
                return NotFound();
            }

            return paymentMethod;
        }

        // PUT: api/PaymentMethods/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentMethod(string id, PaymentMethod paymentMethod)
        {
            if (id != paymentMethod.Id)
            {
                return BadRequest();
            }

            _context.Entry(paymentMethod).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentMethodExists(id))
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

        // POST: api/PaymentMethods
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        // DELETE: api/PaymentMethods/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentMethod>> DeletePaymentMethod(string id)
        {
            var paymentMethod = await _context.PaymentMethod.FindAsync(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            _context.PaymentMethod.Remove(paymentMethod);
            await _context.SaveChangesAsync();

            return paymentMethod;
        }

        private bool PaymentMethodExists(string id)
        {
            return _context.PaymentMethod.Any(e => e.Id == id);
        }
    }
}
