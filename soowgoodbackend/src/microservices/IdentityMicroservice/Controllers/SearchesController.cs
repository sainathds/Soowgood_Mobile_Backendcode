using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using IdentityMicroservice.Services;
using IdentityMicroservice.ViewModels.Search;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchesController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;
        private readonly ISearchService _searchService;

        public SearchesController(IdentityMicroserviceContext context, ISearchService searchService)
        {
            _context = context;
            _searchService = searchService;
        }

        // GET: api/Searches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Search>>> GetSearch()
        {
            return await _context.Search.ToListAsync();
        }

        // GET: api/Searches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Search>> GetSearch(string id)
        {
            var search = await _context.Search.FindAsync(id);

            if (search == null)
            {
                return NotFound();
            }

            return search;
        }

        // PUT: api/Searches/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSearch(string id, Search search)
        {
            if (id != search.Id)
            {
                return BadRequest();
            }

            _context.Entry(search).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SearchExists(id))
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

        // POST: api/Searches
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("GlobalSearch")]
        public ActionResult<IEnumerable<Search>> GetGlobalSearchResult(SearchParameterViewModel model)
        {
            
            List<Search> result = _searchService.GlobalSearch(model);
            result = result.Skip(model.PageSize * (model.PageNumber)).Take((model.PageSize)).ToList();
            return result.ToList();
        }


        [HttpPost("Provider")]
        public ActionResult<IEnumerable<Search>> GetProviderInfo(SearchParameterViewModel model)
        {
            List<Search> result = _searchService.GetProviderInfo(model);
            result = result.Skip(model.PageSize * (model.PageNumber)).Take((model.PageSize)).ToList();
            return result.ToList();
        }

        // DELETE: api/Searches/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Search>> DeleteSearch(string id)
        {
            var search = await _context.Search.FindAsync(id);
            if (search == null)
            {
                return NotFound();
            }

            _context.Search.Remove(search);
            await _context.SaveChangesAsync();

            return search;
        }

        private bool SearchExists(string id)
        {
            return _context.Search.Any(e => e.Id == id);
        }
    }
}
