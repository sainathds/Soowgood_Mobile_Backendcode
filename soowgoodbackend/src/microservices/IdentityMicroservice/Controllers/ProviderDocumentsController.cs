using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using IdentityMicroservice.ViewModels;
using IdentityMicroservice.ViewModels.ProviderViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderDocumentsController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;

        public ProviderDocumentsController(IdentityMicroserviceContext context)
        {
            _context = context;
        }


        [HttpPost("GetProviderDocument")]
        public async Task<ActionResult<IEnumerable<providerdocument>>> GetProviderDocumentAsync(ProviderDocumentViewModel model)
        {

            var providerdocument = await _context.ProviderDocument.Where(m => m.UserId == model.UserId && m.IsDeleted==false).ToListAsync();

            if (providerdocument == null)
            {
                return NotFound();
            }
            else
            {
                return providerdocument;
            }
        }


        [HttpPost("saveProviderDocument")]
        public async Task<providerdocument> SaveProviderDocument([FromForm] ProviderDocumentViewModel model)
        {
            try
            {
                var uploadFolderUrl = Directory.GetCurrentDirectory() + "\\Data\\ProviderDocument";
                string filename = string.Empty;
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var file = model.File;

                    //var file = model.File;
                    if (file != null)
                    {
                        if (!Directory.Exists(uploadFolderUrl))
                            Directory.CreateDirectory(uploadFolderUrl);


                        providerdocument _providerdocument = new providerdocument();
                        _providerdocument.documentname = model.documentname;
                        filename = file.FileName.ToLower();
                        string fullpath = Path.Combine(uploadFolderUrl, file.FileName.ToLower());
                        bool isexists = false;
                        int j = 1;
                        while (isexists == false)
                        {
                            fullpath = Path.Combine(uploadFolderUrl, filename);
                            if (System.IO.File.Exists(fullpath))
                            {
                                string filewithouthext = Path.GetFileNameWithoutExtension(fullpath);
                                string fileextension = Path.GetExtension(fullpath);
                                filename = filewithouthext + "_" + j.ToString() + fileextension.ToLower();
                                j = j + 1;
                            }
                            else
                            {
                                isexists = true;
                            }
                        }
                        using (var fileStream = new FileStream(fullpath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        _providerdocument.documentfilename = filename;
                        _providerdocument.UserId = model.UserId;
                        _context.ProviderDocument.Add(_providerdocument);
                        await _context.SaveChangesAsync();
                        scope.Complete();
                        return _providerdocument;
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        [HttpPost("updateProviderDocument")]
        public async Task<ActionResult<providerdocument>> UpdateProviderDocument([FromForm] ProviderDocumentViewModel model)
        {
            try
            {


                string filename = string.Empty;
                //var file = model.File;
                var uploadFolderUrl = Directory.GetCurrentDirectory() + "\\Data\\ProviderDocument";
                string fullpath = string.Empty;


                if (!Directory.Exists(uploadFolderUrl))
                    Directory.CreateDirectory(uploadFolderUrl);

                var providerdocumentinfo = _context.ProviderDocument.Any(m => m.Id == model.Id);

                if (providerdocumentinfo)
                {

                    var _providerdoc = await _context.ProviderDocument.FindAsync(model.Id); 
                    _providerdoc.Id = model.Id;
                    _providerdoc.UserId = model.UserId;
                    _providerdoc.documentname = model.documentname;
                    if (model.filetype == "noturl")
                    {
                                               
                        fullpath = Path.Combine(uploadFolderUrl, _providerdoc.documentfilename);
                        if (System.IO.File.Exists(fullpath))
                        {
                            System.IO.File.Delete(fullpath);

                        }

                        var file = model.File;
                        if (file != null)
                        {

                            bool isexists = false;
                            int j = 1;
                            filename = file.FileName.ToLower();
                            while (isexists == false)
                            {
                                fullpath = Path.Combine(uploadFolderUrl, filename);
                                if (System.IO.File.Exists(fullpath))
                                {
                                    string filewithouthext = Path.GetFileNameWithoutExtension(fullpath);
                                    string fileextension = Path.GetExtension(fullpath);
                                    filename = filewithouthext + "_" + j.ToString() + fileextension.ToLower();
                                    j = j + 1;
                                }
                                else
                                {
                                    isexists = true;
                                }
                            }
                            using (var fileStream = new FileStream(fullpath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                        }
                    }
                    else
                    {
                        filename = model.documentfilename;
                    }
                    _providerdoc.documentfilename = filename;
                    _context.ProviderDocument.Update(_providerdoc);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetProviderDocument", new { id = _providerdoc.Id }, _providerdoc);
                }

                else
                    return NotFound();


            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost("deleteProviderDocumentDetails")]
        public async Task<ActionResult<providerdocument>> DelteProviderDocumentDetails(ProviderDocumentViewModel model)
        {
            try
            {
                var providerdocument = await _context.ProviderDocument.FindAsync(model.Id);
                if (providerdocument == null)
                {
                    return NotFound();
                }
                providerdocument.IsActive = false;
                providerdocument.IsDeleted = true;
                _context.ProviderDocument.Update(providerdocument);
                await _context.SaveChangesAsync();
                return providerdocument;


            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
