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
    public class TaskChargeSettingsController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public TaskChargeSettingsController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/TaskChargeSettings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskChargeSetting>>> GetTaskChargeSetting()
        {
            return await _context.TaskChargeSetting.ToListAsync();
        }

        // GET: api/TaskChargeSettings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskChargeSetting>> GetTaskChargeSetting(string id)
        {
            var taskChargeSetting = await _context.TaskChargeSetting.FindAsync(id);

            if (taskChargeSetting == null)
            {
                return NotFound();
            }

            return taskChargeSetting;
        }

        // PUT: api/TaskChargeSettings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskChargeSetting(string id, TaskChargeSetting taskChargeSetting)
        {
            if (id != taskChargeSetting.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskChargeSetting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskChargeSettingExists(id))
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

        // POST: api/TaskChargeSettings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TaskChargeSetting>> PostTaskChargeSetting(TaskChargeSetting taskChargeSetting)
        {
            _context.TaskChargeSetting.Add(taskChargeSetting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskChargeSetting", new { id = taskChargeSetting.Id }, taskChargeSetting);
        }

        // DELETE: api/TaskChargeSettings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TaskChargeSetting>> DeleteTaskChargeSetting(string id)
        {
            var taskChargeSetting = await _context.TaskChargeSetting.FindAsync(id);
            if (taskChargeSetting == null)
            {
                return NotFound();
            }

            _context.TaskChargeSetting.Remove(taskChargeSetting);
            await _context.SaveChangesAsync();

            return taskChargeSetting;
        }

        private bool TaskChargeSettingExists(string id)
        {
            return _context.TaskChargeSetting.Any(e => e.Id == id);
        }
    }
}
