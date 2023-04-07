using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using IdentityMicroservice.ViewModels.ProviderViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderBillInformationController : Controller
    {
        private readonly IdentityMicroserviceContext _context;

        public ProviderBillInformationController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        [HttpPost("saveProviderBillInformation")]
        public async Task<ProviderBillDetails> SaveProviderBillInformation(ProviderBillInformation model)
        {
            try
            {                 
                string filename = string.Empty;
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ProviderBillDetails _providerbilldata = new ProviderBillDetails();
                    _providerbilldata.accountname = model.accountname;
                    _providerbilldata.accountno = model.accountno;
                    _providerbilldata.accounttype = model.accounttype;
                    _providerbilldata.bankname = model.bankname;
                    _providerbilldata.branchname = model.branchname;
                    _providerbilldata.UserId = model.UserId;
                    _providerbilldata.IsActive = true;
                    _providerbilldata.IsDeleted = false;
                    _context.ProviderBillDetails.Add(_providerbilldata);
                    await _context.SaveChangesAsync();
                    scope.Complete();
                    return _providerbilldata;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost("getProviderBillInformation")]
        public async Task<ActionResult<IEnumerable<ProviderBillDetails>>> getProviderBillInformationAsync(ProviderBillInformation model)
        {

            var providerbilldetails = await _context.ProviderBillDetails.Where(m => m.UserId == model.UserId && m.IsActive==true && m.IsDeleted==false).ToListAsync();

            if (providerbilldetails == null)
            {
                return NotFound();
            }
            else
            {
                return providerbilldetails;
            }
        }


        [HttpPost("deleteProviderBillInformation")]
        public async Task<ActionResult<ProviderBillDetails>> DeleteProviderBillInformation(ProviderBillInformation model)
        {

            var providerbilldata = await _context.ProviderBillDetails.FindAsync(model.Id);
            if (providerbilldata == null)
            {
                return NotFound();
            }
            providerbilldata.IsActive = false;
            providerbilldata.IsDeleted = true;
            _context.ProviderBillDetails.Update(providerbilldata);
            await _context.SaveChangesAsync();
            return providerbilldata;
        }

        

    }
}
