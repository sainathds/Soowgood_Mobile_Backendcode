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
using IdentityMicroservice.ViewModels.ManageViewModels;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderCategoryInfoesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public ProviderCategoryInfoesController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/ProviderCategoryInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderCategoryInfo>>> GetProviderCategoryInfo()
        {
            return await _context.ProviderCategoryInfo.ToListAsync();
        }

        [HttpPost("GetProviderType")]
        public List<ProviderCategoryInfoViewModel> GetProviderType(string id)
        {
            //var providerCategoryInfo = (from a in _context.ProviderCategoryInfo
            //                            select new ProviderCategoryInfo
            //                            {
            //                                Id = a.Id,
            //                                Provider = a.Provider,
            //                                MedicalCareType = a.MedicalCareType,
            //                                ProviderType = a.ProviderType

            //                            }).ToList();

            //return providerCategoryInfo;

            var results = (from a in _context.ProviderInfo.Where(p=>p.IsActive == true)
                           join b in _context.ProviderCategoryInfo.Where(p=> p.IsMedicalCareType == false && p.IsActive == true) on a.ProviderCategoryInfoId equals b.Id
                           join c in _context.ProviderCategoryInfo.Where(p => p.IsMedicalCareType == true && p.IsActive == true) on b.ParentId equals c.Id
                           select new ProviderCategoryInfoViewModel
                           {
                               Id = a.Id,
                               Provider = a.Provider,
                               MedicalCareType = c.CategoryName,
                               ProviderType = b.CategoryName

                           }).Distinct().OrderBy(p=>p.Provider).ToList();
            return results;

        }


        [HttpPost("MedicalCareType")]
        public IEnumerable<string> GetMedicalCareTypeList(string id)
        {

            var rowsToReturn = _context.ProviderCategoryInfo
               .OrderBy(p=>p.SequenceNo)
               .Where(p=> p.IsActive == true && p.IsMedicalCareType == true)
               .Select(d => d.CategoryName)
               .ToList();
            return rowsToReturn;
        }

        [HttpPost("ProviderType")]
        public IEnumerable<ProviderTypeCategory> GetProviderTypeList(string id)
        {

            List<ProviderTypeCategory> result = new List<ProviderTypeCategory>();

            var providerTypeResults = _context.ProviderCategoryInfo
                .Where(p => p.IsActive == true && p.IsMedicalCareType == false && p.IsDefault == true)
                .OrderBy(p=>p.SequenceNo)
                .Select(x => new ProviderTypeCategory()
            {
                ProviderType = x.CategoryName,
                ImageURL = x.ImageURL

            }).ToList();

            result.AddRange(providerTypeResults);

            var providerResults = _context.ProviderInfo
               .Where(p => p.IsActive == true && p.IsDefault == true)
               .OrderBy(p => p.SequenceNo)
               .Select(x => new ProviderTypeCategory()
               {
                   ProviderType = x.Provider,
                   ImageURL = x.ImageURL

               }).ToList();

            result.AddRange(providerResults);

            return result;
        }

        [HttpPost("LoadOnProviderList")]
        public IEnumerable<ProviderTypeCategory> GetLoadOnProviderList(ProviderCategoryInfo model)
        {
            var results = (from b in _context.ProviderCategoryInfo.Where(p => p.IsMedicalCareType == false && p.IsActive == true)
                           join c in _context.ProviderCategoryInfo.Where(p => p.IsMedicalCareType == true && p.IsActive == true) on b.ParentId equals c.Id
                           where c.CategoryName.Equals(model.CategoryName)
                           select new ProviderTypeCategory
                           {
                               ProviderType = b.CategoryName

                           }).Distinct().ToList();
            return results;
        }

        [HttpPost("Provider")]
        public List<ProviderTypeCategory> GetProviderList(string id)
        {
            var results = (from a in _context.ProviderCategoryInfo
                                   join b in _context.ProviderInfo on a.Id equals b.ProviderCategoryInfoId
                                   where a.IsActive == true && b.IsActive == true
                                   select new ProviderTypeCategory
                                   {
                                       ProviderType = a.CategoryName,
                                       Provider = b.Provider,
                                       ImageURL = a.ImageURL
                                   }).Distinct().OrderBy(p => p.Provider).ToList();
            return results;
        }

        [HttpPost("AllServices")]
        public List<ServiceCategory> GetAllServicesList(string name)
        {
            var results = (from a in _context.ProviderCategoryInfo
                           join b in _context.ProviderInfo on a.Id equals b.ProviderCategoryInfoId
                           where a.IsActive == true && b.IsActive == true
                           select new ServiceCategory
                           {
                               ProviderType = a.CategoryName,
                               Provider = b.Provider

                           }).Distinct().OrderBy(p => p.ProviderType).ThenBy(p=>p.Provider).ToList();

            return results;
        }

        // GET: api/ProviderCategoryInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProviderCategoryInfo>> GetProviderCategoryInfo(string id)
        {
            var providerCategoryInfo = await _context.ProviderCategoryInfo.FindAsync(id);

            if (providerCategoryInfo == null)
            {
                return NotFound();
            }

            return providerCategoryInfo;
        }

        // PUT: api/ProviderCategoryInfoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProviderCategoryInfo(string id, ProviderCategoryInfo providerCategoryInfo)
        {
            if (id != providerCategoryInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(providerCategoryInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProviderCategoryInfoExists(id))
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

        // POST: api/ProviderCategoryInfoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ProviderCategoryInfo>> PostProviderCategoryInfo(ProviderCategoryInfo providerCategoryInfo)
        {
            _context.ProviderCategoryInfo.Add(providerCategoryInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProviderCategoryInfo", new { id = providerCategoryInfo.Id }, providerCategoryInfo);
        }

        // DELETE: api/ProviderCategoryInfoes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProviderCategoryInfo>> DeleteProviderCategoryInfo(string id)
        {
            var providerCategoryInfo = await _context.ProviderCategoryInfo.FindAsync(id);
            if (providerCategoryInfo == null)
            {
                return NotFound();
            }

            _context.ProviderCategoryInfo.Remove(providerCategoryInfo);
            await _context.SaveChangesAsync();

            return providerCategoryInfo;
        }

        private bool ProviderCategoryInfoExists(string id)
        {
            return _context.ProviderCategoryInfo.Any(e => e.Id == id);
        }
    }
}
