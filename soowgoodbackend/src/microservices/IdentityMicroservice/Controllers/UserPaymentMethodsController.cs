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
    public class UserPaymentMethodsController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public UserPaymentMethodsController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/UserPaymentMethods
        [HttpPost("AllUsersPaymentMethod")]
        public ActionResult<IEnumerable<UserPaymentMethodViewModel>> GetAllUserPaymentMethod()
         {

            var paymentMethodList = (from a in _context.UserPaymentMethod.Where(m => m.IsActive == true && m.IsDeleted == false)
                                     join b in _context.Users on a.UserId equals b.Id
                                     join c in _context.PaymentMethod on a.PaymentMethodId equals c.Id

                                     select new UserPaymentMethodViewModel
                                     {
                                         Id = a.Id,
                                         PaymentMethodId = c.Id,
                                         PaymentMethod = c.Name,
                                         UserId = b.Id,
                                         UserFullName = b.FullName,
                                         CardHolderName = a.CardHolderName,
                                         CardNumber = a.CardNumber,
                                         ExpiredDate = a.ExpiredDate,
                                         CVV = a.CVV,
                                         MobileNumber = a.MobileNumber
                                     }).ToList();

            if (paymentMethodList == null)
            {
                return NotFound();
            }

            return paymentMethodList;  
            ///return await _context.UserPaymentMethod.Where(m=> m.IsActive == true && m.IsDeleted == false).ToListAsync();
        }

        [HttpPost("UsersPaymentMethod")]
        public ActionResult<IEnumerable<UserPaymentMethodViewModel>> GetUsersPaymentMethod(UserPaymentMethodViewModel model)
        {

            var paymentMethodList = (from a in _context.UserPaymentMethod.Where(m => m.IsActive == true && m.IsDeleted == false)
                                     join b in _context.Users on a.UserId equals b.Id
                                     join c in _context.PaymentMethod on a.PaymentMethodId equals c.ParentId
                                     where a.UserId == model.UserId 

                                     select new UserPaymentMethodViewModel
                                     {
                                         Id = a.Id,
                                         PaymentMethodId = c.Id,
                                         ParentId = c.ParentId,
                                         PaymentMethod = c.Name,
                                         UserId = b.Id,
                                         UserFullName = b.FullName,
                                         CardHolderName = a.CardHolderName,
                                         CardNumber = a.CardNumber,
                                         ExpiredDate = a.ExpiredDate,
                                         CVV = a.CVV,
                                         MobileNumber = a.MobileNumber
                                     }).Distinct().ToList();

            paymentMethodList = paymentMethodList.Where(p => p.ParentId == model.ParentId).ToList();

            if (paymentMethodList == null)
            {
                return NotFound();
            }

            return paymentMethodList;
            ///return await _context.UserPaymentMethod.Where(m=> m.IsActive == true && m.IsDeleted == false).ToListAsync();
        }

        // GET: api/UserPaymentMethods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPaymentMethod>> GetUserPaymentMethod(string id)
        {
            var userPaymentMethod = await _context.UserPaymentMethod.FindAsync(id);

            if (userPaymentMethod == null)
            {
                return NotFound();
            }

            return userPaymentMethod;
        }

        // PUT: api/UserPaymentMethods/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserPaymentMethod(string id, UserPaymentMethod userPaymentMethod)
        {
            if (id != userPaymentMethod.Id)
            {
                return BadRequest();
            }

            _context.Entry(userPaymentMethod).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPaymentMethodExists(id))
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

        // POST: api/UserPaymentMethods
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("PaymentMethod")]
        public async Task<ActionResult<IEnumerable<UserPaymentMethodViewModel>>> PostUserPaymentMethod(UserPaymentMethod userPaymentMethod)
        {

            var info = _context.UserPaymentMethod.Any(m => m.UserId == userPaymentMethod.UserId && m.PaymentMethodId == userPaymentMethod.PaymentMethodId
                                && m.CardNumber == userPaymentMethod.CardNumber && m.MobileNumber == null && m.IsActive == true && m.IsDeleted == false);

            if(!info)
            {
                info = _context.UserPaymentMethod.Any(m => m.UserId == userPaymentMethod.UserId && m.PaymentMethodId == userPaymentMethod.PaymentMethodId
                                && m.MobileNumber == userPaymentMethod.MobileNumber && m.CardNumber == null && m.IsActive == true && m.IsDeleted == false);
            }

            if (info && userPaymentMethod.Id == null)
            {
                return Conflict();
            }

            else if (!info && userPaymentMethod.Id == null)
            {
                _context.UserPaymentMethod.Add(userPaymentMethod);
                await _context.SaveChangesAsync();
                return GetPaymentMethod(userPaymentMethod.UserId);
                //return CreatedAtAction("GetPaymentMethodInfo", new { id = userPaymentMethod.UserId }, userPaymentMethod);
            }

            else if (userPaymentMethod.Id != null)
            {
                var paymentInfo = _context.UserPaymentMethod.Any(m => m.Id == userPaymentMethod.Id);

                if (paymentInfo)
                {
                    _context.UserPaymentMethod.Update(userPaymentMethod);
                    await _context.SaveChangesAsync();
                    return GetPaymentMethod(userPaymentMethod.UserId);
                    //return CreatedAtAction("GetPaymentMethodInfo", new { id = userPaymentMethod.UserId }, userPaymentMethod);
                }
            }

            return NotFound();
        }

        public ActionResult<IEnumerable<UserPaymentMethodViewModel>> GetPaymentMethod(String UserId)
        {
            var paymentMethodList = (from a in _context.UserPaymentMethod.Where(m=> m.IsActive == true && m.IsDeleted == false)
                                   join b in _context.Users on a.UserId equals b.Id
                                   join c in _context.PaymentMethod on a.PaymentMethodId equals c.Id
                                   where a.UserId == UserId

                                   select new UserPaymentMethodViewModel
                                   {
                                       Id = a.Id,
                                       PaymentMethodId = c.Id,
                                       PaymentMethod = c.Name,
                                       UserId = b.Id,
                                       UserFullName = b.FullName,
                                       CardHolderName = a.CardHolderName,
                                       CardNumber = a.CardNumber,
                                       ExpiredDate = a.ExpiredDate,
                                       CVV = a.CVV,
                                       MobileNumber = a.MobileNumber
                                   }).ToList();

            if (paymentMethodList == null)
            {
                return NotFound();
            }

            return paymentMethodList;
        }

        [HttpPost("PaymentMethodInfo")]
        public ActionResult<IEnumerable<UserPaymentMethodViewModel>> GetPaymentMethodInfo(UserPaymentMethodViewModel model)
        {
            var paymentMethodList = (from a in _context.UserPaymentMethod.Where(m => m.IsActive == true && m.IsDeleted == false)
                                     join b in _context.Users on a.UserId equals b.Id
                                     join c in _context.PaymentMethod on a.PaymentMethodId equals c.Id
                                     where a.UserId == model.UserId

                                     select new UserPaymentMethodViewModel
                                     {
                                         Id = a.Id,
                                         PaymentMethodId = c.Id,
                                         PaymentMethod = c.Name,
                                         UserId = b.Id,
                                         UserFullName = b.FullName,
                                         CardHolderName = a.CardHolderName,
                                         CardNumber = a.CardNumber,
                                         ExpiredDate = a.ExpiredDate,
                                         CVV = a.CVV,
                                         MobileNumber = a.MobileNumber
                                     }).ToList();

            if (paymentMethodList == null)
            {
                return NotFound();
            }

            return paymentMethodList;
        }

        // DELETE: api/UserPaymentMethods/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserPaymentMethod>> DeleteUserPaymentMethod(string id)
        {
            var userPaymentMethod = await _context.UserPaymentMethod.FindAsync(id);
            if (userPaymentMethod == null)
            {
                return NotFound();
            }

            _context.UserPaymentMethod.Remove(userPaymentMethod);
            await _context.SaveChangesAsync();

            return userPaymentMethod;
        }

        private bool UserPaymentMethodExists(string id)
        {
            return _context.UserPaymentMethod.Any(e => e.Id == id);
        }
    }
}
