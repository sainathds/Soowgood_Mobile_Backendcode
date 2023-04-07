using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentTypesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public AppointmentTypesController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/AppointmentTypes
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<AppointmentType>>> GetAppointmentType()
        //{
        //    return await _context.AppointmentType.ToListAsync();
        //}

        //// GET: api/AppointmentTypes/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<AppointmentType>> GetAppointmentType(string id)
        //{
        //    var appointmentType = await _context.AppointmentType.FindAsync(id);

        //    if (appointmentType == null)
        //    {
        //        return NotFound();
        //    }

        //    return appointmentType;
        //}

        // GET: api/Awards/5
        [HttpPost("AppointmentType")]
        public async Task<ActionResult<IEnumerable<AppointmentType>>> GetAppointmentType(String Id)
        {
            var appointmentType = await _context.AppointmentType.Where(m => m.IsActive == true).ToListAsync();

            if (appointmentType == null)
            {
                return NotFound();
            }

            return appointmentType;
        }

        // PUT: api/AppointmentTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointmentType(string id, AppointmentType appointmentType)
        {
            if (id != appointmentType.Id)
            {
                return BadRequest();
            }

            _context.Entry(appointmentType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentTypeExists(id))
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

        // POST: api/AppointmentTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AppointmentType>> PostAppointmentType(AppointmentType appointmentType)
        {
            _context.AppointmentType.Add(appointmentType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointmentType", new { id = appointmentType.Id }, appointmentType);
        }

        // DELETE: api/AppointmentTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AppointmentType>> DeleteAppointmentType(string id)
        {
            var appointmentType = await _context.AppointmentType.FindAsync(id);
            if (appointmentType == null)
            {
                return NotFound();
            }

            _context.AppointmentType.Remove(appointmentType);
            await _context.SaveChangesAsync();

            return appointmentType;
        }

        private bool AppointmentTypeExists(string id)
        {
            return _context.AppointmentType.Any(e => e.Id == id);
        }
    }
}
