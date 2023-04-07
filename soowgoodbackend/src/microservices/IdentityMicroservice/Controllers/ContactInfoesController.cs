using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using IdentityMicroservice.ViewModels.Address;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfoesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public ContactInfoesController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/ContactInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactInfo>>> GetContactInfo()
        {
            return await _context.ContactInfo.ToListAsync();
        }

        // GET: api/ContactInfoes/5
        [HttpPost("DefaultInfo")]
        public IEnumerable<ContactInfoViewModel> GetContactInfo(ContactInfoViewModel model)
        {
            var contactInfo = (from a in _context.ContactInfo.Where(p=> p.Country.Equals(model.Country))
                              select new ContactInfoViewModel
                              {
                                  Address = a.Address,
                                  Country = a.Country,
                                  CountryCode = a.CountryCode,
                                  Email = a.Email,
                                  Phone = a.Phone,
                                  ImageURL = a.ImageURL,
                                  IsDefault = a.IsDefault
                              }).ToList();

            if (contactInfo == null)
            {
                contactInfo = (from a in _context.ContactInfo.Where(p => p.IsDefault == true)
                               select new ContactInfoViewModel
                               {
                                   Address = a.Address,
                                   Country = a.Country,
                                   CountryCode = a.CountryCode,
                                   Email = a.Email,
                                   Phone = a.Phone,
                                   ImageURL = a.ImageURL,
                                   IsDefault = a.IsDefault
                               }).ToList();

            }

            return contactInfo;
        }

        // PUT: api/ContactInfoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactInfo(string id, ContactInfo contactInfo)
        {
            if (id != contactInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(contactInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactInfoExists(id))
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

        // POST: api/ContactInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ContactInfo>> PostContactInfo(ContactInfo contactInfo)
        {
            _context.ContactInfo.Add(contactInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactInfo", new { id = contactInfo.Id }, contactInfo);
        }

        // DELETE: api/ContactInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactInfo>> DeleteContactInfo(string id)
        {
            var contactInfo = await _context.ContactInfo.FindAsync(id);
            if (contactInfo == null)
            {
                return NotFound();
            }

            _context.ContactInfo.Remove(contactInfo);
            await _context.SaveChangesAsync();

            return contactInfo;
        }

        private bool ContactInfoExists(string id)
        {
            return _context.ContactInfo.Any(e => e.Id == id);
        }
    }
}
