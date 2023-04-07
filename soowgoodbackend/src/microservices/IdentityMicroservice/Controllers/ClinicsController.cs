using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using IdentityMicroservice.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using IdentityMicroservice.ViewModels.ProviderViewModel;
using System.IO;
using IdentityMicroservice.Helpers;
using System.Transactions;
using IdentityMicroservice.ViewModels.ManageViewModels;
using IdentityMicroservice.ViewModels.Search;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IdentityMicroserviceContext _context;

        public ClinicsController(
            IUserRepository userRepository,
            IdentityMicroserviceContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<ClinicsController> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: api/Clinics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clinic>>> GetClinic()
        {
            return await _context.Clinic.Where(m => m.IsDeleted == false && m.IsActive == true).ToListAsync();
        }

        // GET: api/Clinics/5
        [HttpPost("GetClinic")]
        public async Task<ActionResult<IEnumerable<Clinic>>> GetClinic(ClinicViewModel model)
        {
            var clinic = await _context.Clinic.Where(m => m.UserId == model.UserId && m.IsDeleted == false && m.IsActive == true).ToListAsync();

            if (clinic == null)
            {
                return NotFound();
            }
            else
            {
                return clinic;
            }
        }

        // PUT: api/Clinics/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClinic(string id, Clinic clinic)
        {
            if (id != clinic.Id)
            {
                return BadRequest();
            }

            _context.Entry(clinic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClinicExists(id))
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

        // POST: api/Clinics
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.



        [HttpPost("getClinicImages")]
        public List<Clinic> GetClinicimages(SearchParameterViewModel _objclinicmodel)
        {
            var cliniclist = _userRepository.getClinicImages(_objclinicmodel.ServiceProviderId);
            return cliniclist;
        }

        [HttpPost("saveClinicInformation")]
        public async Task<ActionResult<Clinic>> PostClinic([FromForm] ClinicViewModel _objclinicmodel)
        {
            try
            {
                var uploadFolderUrl = Directory.GetCurrentDirectory() + "\\Data\\ClinicPic";
                string filename = string.Empty;


                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var clinicInfo = _context.Clinic.Any(m => m.Name == _objclinicmodel.Name && m.CurrentAddress == _objclinicmodel.CurrentAddress
                                                                             && m.PostalCode == _objclinicmodel.PostalCode
                                                                             && m.State == _objclinicmodel.State
                                                                             && m.Country == _objclinicmodel.Country
                                                                             && m.UserId == _objclinicmodel.UserId
                                                                             && m.IsActive==true
                                                                             && m.IsDeleted== true);
                    Clinic _objclinic = new Clinic();
                    _objclinic.Name = _objclinicmodel.Name;
                    _objclinic.CurrentAddress = _objclinicmodel.CurrentAddress;
                    _objclinic.City = _objclinicmodel.City;
                    _objclinic.PostalCode = _objclinicmodel.PostalCode;
                    _objclinic.State = _objclinicmodel.State;
                    _objclinic.Country = _objclinicmodel.Country;
                    _objclinic.UserId = _objclinicmodel.UserId;
                    _objclinic.IsActive = true;
                    _objclinic.IsDeleted = false;
                    if (!clinicInfo && _objclinicmodel.Id == null)
                    {   
                        var file = _objclinicmodel.File1;
                        if (file != null)
                        {
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
                            _objclinic.ImageURL = filename;
                        }
                        filename = "";
                        var file1 = _objclinicmodel.File2;
                        if (file1 != null)
                        {
                            filename = file1.FileName.ToLower();
                            string fullpath = Path.Combine(uploadFolderUrl, file1.FileName.ToLower());
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
                                await file1.CopyToAsync(fileStream);
                            }
                            _objclinic.ImageURLOptionalTwo = filename;
                        }
                        filename = "";
                        var file2 = _objclinicmodel.File3;
                        if (file2 != null)
                        {
                            filename = file2.FileName.ToLower();
                            string fullpath = Path.Combine(uploadFolderUrl, file2.FileName.ToLower());
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
                                await file2.CopyToAsync(fileStream);
                            }
                            _objclinic.ImageURLOptionalThree = filename;
                        }                        
                        _context.Clinic.Add(_objclinic);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return Conflict();

                    }
                    scope.Complete();
                    return _objclinic;
                }
                return null;
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost("updateClinicInformation")]
        public async Task<ActionResult<Clinic>> PostUpdateClinic([FromForm] ClinicViewModel _objclinicmodel)
        {
            try
            {
                var uploadFolderUrl = Directory.GetCurrentDirectory() + "\\Data\\ClinicPic";
                string filename = string.Empty;
                string Id = "";
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                     
                    var clinicobj = await _context.Clinic.FindAsync(_objclinicmodel.Id);
                    if (clinicobj != null)
                    {

                        clinicobj.Name = _objclinicmodel.Name;
                        clinicobj.CurrentAddress = _objclinicmodel.CurrentAddress;
                        clinicobj.City = _objclinicmodel.City;
                        clinicobj.PostalCode = _objclinicmodel.PostalCode;
                        clinicobj.State = _objclinicmodel.State;
                        clinicobj.Country = _objclinicmodel.Country;
                        clinicobj.UserId = _objclinicmodel.UserId;
                        
                        if (_objclinicmodel.IsImageURLOptionalOne == "1")
                        {
                            if (clinicobj.ImageURL != null)
                            {
                                string fullpath = Path.Combine(uploadFolderUrl, clinicobj.ImageURL);
                                if (System.IO.File.Exists(fullpath))
                                {
                                    System.IO.File.Delete(fullpath);

                                }
                            }
                            clinicobj.ImageURL = "";
                        }

                        if (_objclinicmodel.IsImageURLOptionalTwo == "1")
                        {
                            
                            if (clinicobj.ImageURLOptionalTwo != null)
                            {
                                string fullpath = Path.Combine(uploadFolderUrl, clinicobj.ImageURLOptionalTwo);
                                if (System.IO.File.Exists(fullpath))
                                {
                                    System.IO.File.Delete(fullpath);

                                }
                            }
                            clinicobj.ImageURLOptionalTwo = "";
                        }

                        if (_objclinicmodel.IsImageURLOptionalThree == "1")
                        {
                            
                            if (clinicobj.ImageURLOptionalThree != null)
                            {
                                string fullpath = Path.Combine(uploadFolderUrl, clinicobj.ImageURLOptionalThree);
                                if (System.IO.File.Exists(fullpath))
                                {
                                    System.IO.File.Delete(fullpath);

                                }
                            }
                            clinicobj.ImageURLOptionalThree = "";
                        }


                        var file = _objclinicmodel.File1;
                        if (file != null)
                        {
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
                            clinicobj.ImageURL = filename;
                        }
                        filename = "";
                        var file1 = _objclinicmodel.File2;
                        if (file1 != null)
                        {
                            filename = file1.FileName.ToLower();
                            string fullpath = Path.Combine(uploadFolderUrl, file1.FileName.ToLower());
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
                                await file1.CopyToAsync(fileStream);
                            }
                            clinicobj.ImageURLOptionalTwo = filename;
                        }
                        filename = "";
                        var file2 = _objclinicmodel.File3;
                        if (file2 != null)
                        {
                            filename = file2.FileName.ToLower();
                            string fullpath = Path.Combine(uploadFolderUrl, file2.FileName.ToLower());
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
                                await file2.CopyToAsync(fileStream);
                            }
                            clinicobj.ImageURLOptionalThree = filename;
                        }
                        _context.Clinic.Update(clinicobj);
                        await _context.SaveChangesAsync();
                        scope.Complete();
                        return clinicobj;
                    }
                    else
                    {
                        scope.Complete();
                        return NotFound();
                    }
                   
                }

            }
            catch (DbUpdateException ex)
            {
                return NotFound();
            }


        }

        // DELETE: api/Clinics/5
        [HttpPost("{deleteProviderClinicInfo}")]
        public async Task<ActionResult<Clinic>> DeleteClinic(ClinicViewModel _objclinicmodel)
        {
            string filename = string.Empty;
            //var file = model.File;
            var uploadFolderUrl = Directory.GetCurrentDirectory() + "\\Data\\ClinicPic";
            string fullpath = string.Empty;

            var clinic = await _context.Clinic.FindAsync(_objclinicmodel.Id);
            if (clinic == null)
            {
                return NotFound();
            }

            clinic.IsActive = false;
            clinic.IsDeleted = true;
            _context.Clinic.Update(clinic);
            await _context.SaveChangesAsync();
            return clinic;
        }

        private bool ClinicExists(string id)
        {
            return _context.Clinic.Any(e => e.Id == id);
        }




        //[HttpPost("ClinicImage")]
        //public async Task<Clinic> UploadProfilePicture([FromForm] ClinicViewModel model)
        //{
        //    try
        //    {
        //        if (model.File == null || model.File.Length == 0)
        //        {
        //            return null;
        //        }

        //        var _clinic = _context.Clinic.Where(p => p.Id == model.Id).FirstOrDefault();
        //        var file = Request.Form.Files[0];
        //        var uploadFolderUrl = Directory.GetCurrentDirectory() + "\\Data\\ClinicImage";
        //        string imagePath = string.Empty;
        //        if (file != null)
        //        {
        //            if (!Directory.Exists(uploadFolderUrl))
        //                Directory.CreateDirectory(uploadFolderUrl);
        //            imagePath = Path.Combine(uploadFolderUrl, $"{_clinic.Id}.jpg");
        //            using (var fileStream = new FileStream(imagePath, FileMode.Create))
        //            {
        //                await file.CopyToAsync(fileStream);
        //            }
        //            var imageByte = Helper.GetImage(imagePath);

        //            if (model.IsImageURL == true && model.IsImageURLOptionalOne == false
        //                && model.IsImageURLOptionalTwo == false && model.IsImageURLOptionalThree == false)
        //            {
        //                _clinic.ImageURL = model.ImageURL;
        //            }

        //            else if (model.IsImageURL == false && model.IsImageURLOptionalOne == true
        //                && model.IsImageURLOptionalTwo == false && model.IsImageURLOptionalThree == false)
        //            {
        //                _clinic.ImageURLOptionalOne = model.ImageURLOptionalOne;
        //            }

        //            else if (model.IsImageURL == false && model.IsImageURLOptionalOne == false
        //               && model.IsImageURLOptionalTwo == true && model.IsImageURLOptionalThree == false)
        //            {
        //                _clinic.ImageURLOptionalTwo = model.ImageURLOptionalTwo;
        //            }

        //            else if (model.IsImageURL == false && model.IsImageURLOptionalOne == false
        //               && model.IsImageURLOptionalTwo == false && model.IsImageURLOptionalThree == true)
        //            {
        //                _clinic.ImageURLOptionalTwo = model.ImageURLOptionalThree;
        //            }

        //            _context.Clinic.Update(_clinic);
        //            return _clinic;
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}
