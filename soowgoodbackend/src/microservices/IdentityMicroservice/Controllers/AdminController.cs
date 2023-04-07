using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using IdentityMicroservice.Data;
using IdentityMicroservice.Interfaces;
using IdentityMicroservice.Model;
using IdentityMicroservice.StaticData;
using IdentityMicroservice.ViewModels.ManageViewModels;
using IdentityMicroservice.ViewModels.ProviderViewModel;
using IdentityMicroservice.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IdentityMicroserviceContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISmsSender _smsSender;
        public AdminController(IdentityMicroserviceContext context, IUserRepository userRepository,  UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ISmsSender ssmSender, IAdminRepository adminRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _adminRepository = adminRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = ssmSender;
        }




        [HttpPost("login")]
        public async Task<authentication> Login([FromBody] LoginViewModel model)
        {
            authentication _userauth = new authentication();
            var user = _userRepository.GetUserByEmail(model.Email);
            var res = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (res.Succeeded && user.UserRole.ToLower()=="admin")
            {
                _userauth.message = "Login is successfully.";
                _userauth.success = "1";
                _userauth.userName = model.Email;
                _userauth.userRole = "Admin";
                _userauth.id = user.Id;

            }
            else if (!res.Succeeded)
            {
                _userauth.message = "Email or password is wrong. Please check you.";
                _userauth.success = "0";
                _userauth.userName = model.Email;

            }
            else  
            {
                _userauth.message = "Email or password is wrong. Please check you.";
                _userauth.success = "0";
                _userauth.userName = model.Email;

            }
            return _userauth;
        }


        [HttpPost("getBeneficialList")]
        public ActionResult<IEnumerable<UserList>> GetBeneficialListAsync()
        {
            int count = 0;
            var _beneficialList = (from a in _context.Users
                                   join b in _context.UserAddress on a.Id equals b.UserId
                                   into add
                                   from address in add.DefaultIfEmpty()
                                   where a.UserRole == "Beneficiar"
                                   orderby a.FullName
                                   select new UserList
                                   {
                                       srno = count + 1,
                                       Id = a.Id,
                                       CurrentAddress = (address == null ? String.Empty : address.CurrentAddress),
                                       City = (address == null ? String.Empty : address.City),
                                       DateOfBirth = a.DateOfBirth.ToShortDateString(),
                                       FullName = a.FullName,
                                       BloodGroup = a.BloodGroup,
                                       Email = a.Email,
                                       PhoneNumber = a.PhoneNumber,
                                       Gender = (a.Gender.ToString() == "0" ? string.Empty : a.Gender.ToString())
                                   }).ToList();

            _beneficialList = _beneficialList.ToList();
            return _beneficialList;


        }


        [HttpPost("getProviderList")]
        public ActionResult<IEnumerable<UserList>> GetProviderListAsync()
        {
            int count = 1;
            var _providerlistlist = (from a in _context.Users
                                     join b in _context.UserAddress on a.Id equals b.UserId into add
                                     from address in add.DefaultIfEmpty()
                                     where a.UserRole == "Provider" && a.IsConfirmedByAdmin == true
                                     orderby a.FullName
                                     select new UserList
                                     {
                                         srno = count + 1,
                                         Id = a.Id,
                                         CurrentAddress = (address == null ? String.Empty : address.CurrentAddress),
                                         City = (address == null ? String.Empty : address.City),
                                         DateOfBirth = a.DateOfBirth.ToShortDateString(),
                                         FullName = a.FullName,
                                         BloodGroup = a.BloodGroup,
                                         Email = a.Email,
                                         PhoneNumber = a.PhoneNumber,
                                         Gender = (a.Gender.ToString() == "0" ? string.Empty : a.Gender.ToString())
                                     }).ToList();

            _providerlistlist = _providerlistlist.ToList();
            return _providerlistlist;


        }

        [HttpPost("getproviderprofiletoapprove")]
        public List<UserList> GetProviderProfileToApprove()
        {
            //_smsSender.SendSms(model.Email, "SoowGood OTP. Code: 2345");
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                //Checking Exiting User
                var providerUser = _userRepository.GetProviderProfileToApprove();
                scope.Complete();
                return providerUser;
            }
            return null;
        }



        [HttpPost("approvalProviderProfile")]
        public async Task<authentication> approvalProviderProfile(ProviderDocumentViewModel model)
        {
            //_smsSender.SendSms(model.Email, "SoowGood OTP. Code: 2345");
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                authentication _userauth = new authentication();
                var ExistingUser = await _userManager.FindByIdAsync(model.UserId);
                string callbackUrl = "";
                if (ExistingUser != null)
                {
                    var _user = await _userManager.FindByIdAsync(ExistingUser.Id);
                    _user.IsConfirmedByAdmin = true;
                    var result = await _userManager.UpdateAsync(_user);
                    if (_user.Email.Length > 0)
                    {
                        _emailSender.SendEmail(EmailTemplate.UserConfirmation, EmailTemplate.GetUserConfirmationTemplate(_user.FullName, callbackUrl), new List<string> { _user.Email }, "html");
                    }
                    if (_user.PhoneNumber.Length > 0)
                    {
                        _smsSender.SendSms(_user.PhoneNumber, "Your profile has been confirmed by Team SoowGood.");
                    }


                    notificationmaster objnotification = new notificationmaster();
                    objnotification.isactive = 1;
                    objnotification.isread = 0;
                    objnotification.showpopup = 0;
                    objnotification.isdeleted = 0;
                    objnotification.notificationtext = "Your profile has been confirmed by Team SoowGood.";
                    objnotification.userid = ExistingUser.Id;
                    objnotification.notificationtype = "Profile";
                    objnotification.usertype = "Provider";
                    objnotification.notificationdate = System.DateTime.Now;
                    _context.notificationmaster.Add(objnotification);
                    await _context.SaveChangesAsync();

                    _userauth.success = "1";
                }
                else
                {
                    _userauth.success = "0";
                    return null;
                }
                scope.Complete();
                return _userauth;
            }

        }


        [HttpPost("GetApoointmentBookingList")]
        public List<BookingViewModel> GetApoointmentBookingList(BookingViewModel model)
        {             
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                //Checking Exiting User
                var apoointmentBooking = _adminRepository.GetApoointmentBookingList(model);
                scope.Complete();
                return apoointmentBooking;
            }
            return null;
        }


        [HttpPost("getPendingPaymentRequest")]
        public ActionResult<IEnumerable<BookingViewModel>> GetProviderBookingBillDetails(BookingViewModel model)
        {
            List<BookingViewModel> appointmentList = _adminRepository.getPendingPaymentRequest(model);
            return appointmentList;
        }

    }
}
