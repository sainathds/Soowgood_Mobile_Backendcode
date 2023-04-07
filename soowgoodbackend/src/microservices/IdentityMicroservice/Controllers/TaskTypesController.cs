using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using IdentityMicroservice.ViewModels.ProviderViewModel;
using Microsoft.AspNetCore.Identity;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTypesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskTypesController(IdentityMicroserviceContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/TaskTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskType>>> GetTaskType()
        {
            return await _context.TaskType.ToListAsync();
        }

        // GET: api/TaskTypes/5
        [HttpPost("TaskType")]
        public async Task<ActionResult<IEnumerable<TaskType>>> GetTaskType(TaskTypeViewModel model)
        {
            var roleId = _context.UserRoles.Where(u => u.UserId == model.UserId).Select(r => r.RoleId).FirstOrDefault();

            var taskType = await _context.TaskType.Where(p=>p.UserTypeId == roleId && p.IsActive == true).ToListAsync();

            if (taskType == null)
            {
                return NotFound();
            }
            
            return taskType;
        }

        // PUT: api/TaskTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskType(string id, TaskType taskType)
        {
            if (id != taskType.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskTypeExists(id))
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

        // POST: api/TaskTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TaskType>> PostTaskType(TaskType taskType)
        {
            _context.TaskType.Add(taskType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskType", new { id = taskType.Id }, taskType);
        }

        // DELETE: api/TaskTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TaskType>> DeleteTaskType(string id)
        {
            var taskType = await _context.TaskType.FindAsync(id);
            if (taskType == null)
            {
                return NotFound();
            }

            _context.TaskType.Remove(taskType);
            await _context.SaveChangesAsync();

            return taskType;
        }

        private bool TaskTypeExists(string id)
        {
            return _context.TaskType.Any(e => e.Id == id);
        }
    }
}
