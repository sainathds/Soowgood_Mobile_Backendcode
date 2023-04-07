using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using IdentityMicroservice.StaticData;
using IdentityMicroservice.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using IdentityMicroservice.Interfaces;
using Microsoft.AspNetCore.Authorization;
using IdentityMicroservice.Extensions;
using System.Transactions;
using IdentityMicroservice.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.IO;
using IdentityMicroservice.Helpers;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using IdentityMicroservice.ViewModels.ProviderViewModel;
using System.Text.RegularExpressions;
using PhoneNumbers;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using HealthChecks.UI.Configuration;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly IdentityMicroserviceContext _context;
        private readonly IConfiguration _configuration;
        private String LogInURL { set; get; }
        private String PatientLogInURL { set; get; }
        private String linkinreturnurl { set; get; }
        private String adminemail { set; get; }

        private String otpmethod { set; get; }

        public UsersController(
            IUserRepository userRepository,
            IConfiguration configuration,
            IdentityMicroserviceContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender ssmSender,
            ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = ssmSender;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            LogInURL = configuration.GetSection("AppSettings").GetSection("LogInURL").Value;
            PatientLogInURL = configuration.GetSection("AppSettings").GetSection("PatientURL").Value;
            linkinreturnurl = configuration.GetSection("AppSettings").GetSection("linkinreturnurl").Value;
            adminemail = configuration.GetSection("AppSettings").GetSection("adminemail").Value;
            otpmethod = configuration.GetSection("AppSettings").GetSection("otpmethod").Value;
        }

        // GET: api/UserAddresses/5
        [HttpPost("GetUser")]
        public async Task<ActionResult<ApplicationUser>> GetUserInfo(UserViewModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                if (user == null)
                {
                    return NotFound();
                }

                else
                {
                    var userole = await _userManager.GetRolesAsync(user);
                    user.UserRole = userole[0];
                    return user;
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        [HttpPost("GetProvideReviewRating")]
        public List<Search> GetProvideReviewRating([FromBody] UserViewModel model)
        {
            //_smsSender.SendSms(model.Email, "SoowGood OTP. Code: 2345");
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                //Checking Exiting User
                var UserReviewRating = _userRepository.GetProvideReviewRating(model.Id);
                scope.Complete();
                return UserReviewRating;
            }
            return null;
        }
        [HttpPost("login")]
        public async Task<ApplicationUser> Login([FromBody] LoginViewModel model, [FromQuery(Name = "d")] string destination = "frontend")
        {
            var user = _userRepository.GetUserByEmail(model.Email);

            if (user != null && !user.IsConfirmedByAdmin)
            {
                var userole = await _userManager.GetRolesAsync(user);
                var res = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (res.Succeeded)
                {
                    if (string.IsNullOrEmpty(userole[0])) user.UserRole = userole[0];
                    user.Message = "Your profile is under apporval, please contact system Administrator.";
                    return user;
                }

                else if (!res.Succeeded)
                {
                    //if (string.IsNullOrEmpty(userole[0])) user.UserRole = userole[0];
                    //user.Message = "Wrong Credentials!";
                    //return user;

                    var tempUser = new ApplicationUser();
                    tempUser.UserRole = userole[0];
                    tempUser.Message = "Wrong Credentials!";
                    tempUser.Email = model.Email;
                    return tempUser;
                }
            }

            else if (user != null && user.IsConfirmedByAdmin)
            {
                var userole = await _userManager.GetRolesAsync(user);
                var res = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (res.Succeeded)
                {
                    if (!string.IsNullOrEmpty(userole[0])) user.UserRole = userole[0];
                    await RefreshAccessToken(user);
                    user.Message = "User Exists! Login Successful!";
                    return user;
                }

                else
                {
                    var tempUser = new ApplicationUser();
                    tempUser.UserRole = userole[0];
                    tempUser.Message = "Wrong Credentials!";
                    tempUser.Email = model.Email;
                    return tempUser;

                    //if (!string.IsNullOrEmpty(userole[0])) user.UserRole = userole[0];
                    //await RefreshAccessToken(user);
                    //user.Message = "User Exists! Login Successful!";
                    //return user;
                }
            }

            else
            {
                var tempUser = new ApplicationUser();
                tempUser.Message = "User does not Exist!";
                tempUser.Email = model.Email;
                return tempUser;
            }

            return null;
        }
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }
        public static bool validTelephoneNo(string telNo)
        {
            bool IsNumber = false;
            try
            {
                //number = PhoneNumberUtil.Instance.Parse(telNo, "US");  // Change to your default language, international numbers will still be recognised.
                IsNumber = PhoneNumberUtil.IsViablePhoneNumber(telNo);
            }
            catch (NumberParseException e)
            {
                return false;
            }

            return IsNumber;
        }
        private Random _random = new Random();
        public string GenerateRandomNo()
        {
            if (otpmethod == "live")
            {
                return _random.Next(0, 9999).ToString("D4");
            }
            else
            {
                return "1234";
            }
        }
        public static string cryptoservicemd5(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5 = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sb = new StringBuilder();
            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
                sb.Append(data[i].ToString("x2"));
            return sb.ToString();
        }
        [HttpPost("verifyuser")]
        public async Task<authentication> VerifyUserAsync([FromBody] RegisterViewModel model)
        {
            //_smsSender.SendSms(model.Email, "SoowGood OTP. Code: 2345");
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                authentication _userauth = new authentication();
                bool _signupwithemail = true;
                bool _IsPhoneNumber = validTelephoneNo(model.Email.Trim());
                if (_IsPhoneNumber)
                {
                    model.PhoneNumber = model.Email.Trim();
                    _signupwithemail = false;
                }


                //Checking Exiting User
                var ExistingUser = _userRepository.GetUserByEmail(model.Email.Trim());
                if (ExistingUser != null)
                {
                    var userole = await _userManager.GetRolesAsync(ExistingUser);
                    if (!String.IsNullOrEmpty(userole[0]))
                    {
                        scope.Complete();

                        _userauth.success = "0";
                        _userauth.message = "Email or Mobile No. already registered with us!";
                        _userauth.userName = ExistingUser.UserName.Trim();
                        _userauth.userRole = ExistingUser.UserRole.Trim();
                        _userauth.id = ExistingUser.Id;
                        _userauth.profilePicture = ExistingUser.ProfilePicture;
                        _userauth.concurrencyStamp = ExistingUser.ConcurrencyStamp;
                        return _userauth;
                    }
                }
                else
                {

                    string _verificationotp = GenerateRandomNo();
                    string _sendername = "";
                    if (model.FullName != null)
                    {
                        _sendername = model.FullName;
                    }
                    else
                    {
                        _sendername = model.UserRole;
                    }
                    if (_signupwithemail == true)
                    {
                        _emailSender.SendEmail(EmailTemplate.UserSignupSubject, EmailTemplate.GetUserSignupTemplate(_sendername, _verificationotp), new List<string> { model.Email }, "html");
                    }
                    else
                    {
                        _smsSender.SendSms(model.Email, "Your OTP to authenticate your Phone No is " + _verificationotp + ". Do not share this OTP with anyone.");
                    }
                    scope.Complete();
                    _userauth.email = model.Email;
                    _userauth.userRole = model.UserRole;
                    _userauth.success = "1";
                    _userauth.verificationotp = cryptoservicemd5(_verificationotp);
                    _userauth.message = "";
                    return _userauth;

                }
            }
            return null;
        }
        [HttpPost("confirmverification")]
        public async Task<authentication> ConfirmverificationAsync([FromBody] authentication model)
        {
            try
            {
                //_smsSender.SendSms(model.Email, "SoowGood OTP. Code: 2345");
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    authentication _userauth = new authentication();
                    string newuserid = "";
                    string phoneNumber = "";
                    string Email = "";
                    bool IsConfirmedByAdmin = false;
                    bool _IsPhoneNumber = validTelephoneNo(model.userName.Trim());
                    string currentotp = cryptoservicemd5(model.currentverificationotp);
                    if (currentotp == model.verificationotp)
                    {
                        _userauth.success = "1";
                        _userauth.message = "";

                        ApplicationUser _newuser = new ApplicationUser();
                        if (_IsPhoneNumber)
                        {
                            _newuser.PhoneNumber = model.userName.Trim();
                        }
                        else
                        {
                            _newuser.Email = model.userName.Trim();
                        }

                        if (model.userRole == "Beneficiar")
                        {
                            IsConfirmedByAdmin = true;
                        }


                        if (_newuser.PhoneNumber != null)
                        {
                            phoneNumber = _newuser.PhoneNumber.Trim();
                        }

                        if (_newuser.Email != null)
                        {
                            Email = _newuser.Email.Trim();
                        }

                        if (model.id == "")
                        {
                            var user = new ApplicationUser
                            {
                                Email = Email,
                                UserName = model.userName.Trim(),
                                RegistrationDate = DateTime.Now,
                                MemberSince = DateTime.Now,
                                PhoneNumber = phoneNumber,
                                UserRole = model.userRole,
                                IsConfirmedByAdmin = IsConfirmedByAdmin
                            };
                            var result = await _userManager.CreateAsync(user, "@Default123");
                            if (result.Succeeded)
                            {
                                await _userManager.AddToRoleAsync(user, model.userRole);
                            }
                            // Id _userManager
                            newuserid = user.Id;
                            await InsertDefaultData(newuserid);


                            notificationmaster objnotification = new notificationmaster();
                            objnotification.isactive = 1;
                            objnotification.isread = 0;
                            objnotification.showpopup = 0;
                            objnotification.isdeleted = 0;
                            if (model.userRole == "Provider")
                            {
                                objnotification.notificationtext = "Your profile is under approval, please contact system Administrator.";
                                objnotification.usertype = model.userRole;
                            }
                            else
                            {
                                objnotification.notificationtext = "We're glad that you've joined SoowGood.";
                                objnotification.usertype = model.userRole;
                            }

                            objnotification.notificationtype = "Profile";                           
                            objnotification.notificationdate = System.DateTime.Now;
                            objnotification.userid = user.Id;
                            _context.notificationmaster.Add(objnotification);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            newuserid = model.id;
                        }

                    }
                    else
                    {
                        _userauth.success = "0";
                        _userauth.message = "Please enter valid OTP";
                    }
                    scope.Complete();
                    _userauth.userName = model.userName.Trim();
                    _userauth.userRole = model.userRole;
                    _userauth.id = newuserid;
                    _userauth.concurrencyStamp = model.concurrencyStamp;

                    return _userauth;
                }
            }
            catch (Exception ex)
            {

            }

            return null;

        }
        [HttpPost("register")]
        public async Task<authentication> RegisterAsync([FromBody] RegisterViewModel model)
        {
            //_smsSender.SendSms(model.Email, "SoowGood OTP. Code: 2345");
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                authentication _userauth = new authentication();
                bool _signupwithemail = true;
                bool _IsPhoneNumber = validTelephoneNo(model.Email.Trim());
                if (_IsPhoneNumber)
                {
                    model.PhoneNumber = model.Email.Trim();
                    _signupwithemail = false;
                }


                //Checking Exiting User
                var ExistingUser = _userRepository.GetUserByEmail(model.Email.Trim());
                if (ExistingUser != null)
                {
                    var userole = await _userManager.GetRolesAsync(ExistingUser);
                    if (!String.IsNullOrEmpty(userole[0]))
                    {

                        _userauth.userRole = userole[0];
                        _userauth.success = "0";
                        _userauth.message = "Already Registered! Please Sign in!";
                        return _userauth;
                    }
                }
                else
                {

                    string _verificationotp = GenerateRandomNo();
                    string _sendername = "";
                    if (model.FullName != null)
                    {
                        _sendername = model.FullName;
                    }
                    else
                    {
                        _sendername = model.UserRole;
                    }
                    _emailSender.SendEmail(EmailTemplate.UserSignupSubject, EmailTemplate.GetUserSignupTemplate(_sendername, _verificationotp), new List<string> { model.Email }, "html");

                    _userauth.email = model.Email.Trim();
                    _userauth.userRole = model.UserRole;
                    _userauth.success = "1";
                    _userauth.verificationotp = cryptoservicemd5(_verificationotp);
                    _userauth.message = "";
                    return _userauth;

                }
            }
            return null;
        }
        [HttpPost("verifyuserusingsociallogin")]
        public async Task<authentication> VerifyUserUsingSocialLoginAsync([FromBody] RegisterViewModel model)
        {
            //_smsSender.SendSms(model.Email, "SoowGood OTP. Code: 2345");
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                authentication _userauth = new authentication();
                string username = "";
                bool _IsPhoneNumber = validTelephoneNo(model.Email.Trim());
                username = model.Email.Trim();

                //Checking Exiting User
                var ExistingUser = _userRepository.GetUserByEmail(username.Trim());
                if (ExistingUser != null)
                {
                    var userole = await _userManager.GetRolesAsync(ExistingUser);
                    if (!String.IsNullOrEmpty(userole[0]))
                    {
                        scope.Complete();
                        _userauth.success = "1";
                        _userauth.message = "";
                        _userauth.userName = ExistingUser.UserName.Trim();
                        _userauth.userRole = ExistingUser.UserRole;
                        _userauth.id = ExistingUser.Id;
                        _userauth.profilePicture = ExistingUser.ProfilePicture;
                        _userauth.concurrencyStamp = ExistingUser.ConcurrencyStamp;
                        return _userauth;
                    }
                }
                else
                {
                    scope.Complete();
                    _userauth.success = "0";
                    _userauth.message = "Looks like you are not have an account!";
                    _userauth.userName = username;
                    _userauth.userRole = "";
                    _userauth.id = "";
                    _userauth.concurrencyStamp = "";
                    return _userauth;

                }
            }
            return null;
        }
        [HttpPost("verifylinkedintoken")]
        public async Task<authentication> VerifyLinkedinToken([FromBody] authentication model)
        {

            //_smsSender.SendSms(model.Email, "SoowGood OTP. Code: 2345");
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                string loginuser = "";
                elements _elements = new elements();
                handle h1 = new handle();
                authentication _userauth = new authentication();
                string apiUrl = "https://www.linkedin.com/oauth/v2/accessToken?grant_type=authorization_code&client_id=86g8q46tvj2rzc&client_secret=5tXySSvq8652TcB7&code=" + model.code + "&redirect_uri=" + linkinreturnurl;
                HttpClient client = new HttpClient();
                authentication _userauthnew = new authentication();
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    _userauth = JsonConvert.DeserializeObject<authentication>(response.Content.ReadAsStringAsync().Result);

                    apiUrl = "https://api.linkedin.com/v2/emailAddress?q=members&projection=(elements*(handle~))";
                    HttpClient client1 = new HttpClient();
                    client1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userauth.access_token);
                    HttpResponseMessage response1 = client1.GetAsync(apiUrl).Result;
                    if (response1.IsSuccessStatusCode)
                    {
                        var data = response1.Content.ReadAsStringAsync().Result.Replace("handle~", "handle1");
                        _userauth = JsonConvert.DeserializeObject<authentication>(data);
                        if (_userauth.elements.Count > 0)
                        {
                            loginuser = _userauth.elements[0].handle1.emailAddress;
                        }
                        else
                        {
                            //apiUrl = "https://api.linkedin.com/v2/phoneNumber?q=members&projection=(elements*(handle~))";
                            //HttpClient client2 = new HttpClient();
                            //client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userauth.access_token);
                            //HttpResponseMessage response2 = client2.GetAsync(apiUrl).Result;
                            loginuser = "";
                        }
                    }
                    _userauthnew.success = "1";
                    _userauthnew.email = loginuser;

                }
                scope.Complete();

                return _userauthnew;
            }
            return null;



        }
        [HttpPost("bulkRegister")]
        public async Task<ApplicationUser> BulkRegisterAsync(List<RegisterViewModel> modelList)
        {
            foreach (var model in modelList)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //Checking Exiting User
                    var ExistingUser = _userRepository.GetUserByEmail(model.Email);

                    if (ExistingUser != null && !ExistingUser.EmailConfirmed)
                    {
                        var userole = await _userManager.GetRolesAsync(ExistingUser);

                        if (userole[0] == Role.OrganizationalAdmin)
                        {
                            await _userManager.AddToRoleAsync(ExistingUser, model.UserRole);

                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(ExistingUser);
                            var callbackUrl = Url.EmailConfirmationLink(ExistingUser.Id, code, Request.Scheme);

                            if (callbackUrl != null)
                            {
                                _emailSender.SendEmail(EmailTemplate.UserSignupSubject, EmailTemplate.GetUserSignupTemplate(model.FullName, callbackUrl), new List<string> { model.Email }, "html");
                            }

                            ExistingUser.UserRole = model.UserRole;
                            ExistingUser.Message = "Dear User! Looks like you already have an existing account! Please check your email to activate the account";
                            return ExistingUser;
                        }

                        else
                        {
                            await _userManager.AddToRoleAsync(ExistingUser, model.UserRole);

                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(ExistingUser);
                            var callbackUrl = Url.EmailConfirmationLink(ExistingUser.Id, code, Request.Scheme);

                            if (callbackUrl != null)
                            {
                                _emailSender.SendEmail(EmailTemplate.UserSignupSubject, EmailTemplate.GetUserSignupTemplate(model.FullName, callbackUrl), new List<string> { model.Email }, "html");
                            }

                            ExistingUser.UserRole = model.UserRole;
                            ExistingUser.Message = "Dear User! Looks like you already have an existing account! Please check your email to activate the account";
                            return ExistingUser;
                        }
                    }



                    else if (ExistingUser != null && ExistingUser.EmailConfirmed)
                    {
                        var userole = await _userManager.GetRolesAsync(ExistingUser);
                        if (!String.IsNullOrEmpty(userole[0]))
                        {
                            ExistingUser.UserRole = userole[0];
                            ExistingUser.Message = "Already Registered! Please Sign in!";
                            return ExistingUser;
                        }
                    }

                    // Add New User
                    if (!await _roleManager.RoleExistsAsync(model.UserRole))
                        await _roleManager.CreateAsync(new IdentityRole { Name = model.UserRole, NormalizedName = model.UserRole });

                    var _UserName = String.Empty;

                    if (!string.IsNullOrEmpty(model.Email) || !string.IsNullOrEmpty(model.PhoneNumber))
                    {
                        if (new EmailAddressAttribute().IsValid(model.Email))
                            _UserName = model.Email;
                        else
                            _UserName = model.PhoneNumber;
                    }

                    var user = new ApplicationUser
                    {
                        Email = model.Email,
                        UserName = _UserName,
                        RegistrationDate = DateTime.Now,
                        MemberSince = DateTime.Now,
                        DateOfBirth = model.DateOfBirth,
                        PhoneNumber = model.PhoneNumber,
                        IsOrganizational = true,
                        IsAssignedFromOrganization = true
                    };

                    model.Password = "@Default123";
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, model.UserRole);

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);

                        if (callbackUrl != null)
                        {
                            _emailSender.SendEmail(EmailTemplate.UserSignupSubject, EmailTemplate.GetUserSignupTemplate(model.FullName, callbackUrl), new List<string> { model.Email }, "html");
                        }

                        scope.Complete();
                        user.UserRole = model.UserRole;
                        user.Message = "Registration Successful! Please Check Your Email to Activate the Account!";
                    }
                }
            }

            //var successfulUserList = _userManager.Users.Where(m=> m.Email.Equals())
            return null;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(UsersController.Login), "Users");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                //using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _signInManager.SignInAsync(user, false);
                    var userole = await _userManager.GetRolesAsync(user);
                    await InsertDefaultData(userId);

                    if (userole[0] == Role.Doctor || userole[0] == Role.Provider)
                    {
                        LogInURL = LogInURL + "?userId=" + userId;
                        //scope.Complete();
                        return Redirect(LogInURL);
                    }

                    else if (userole[0] == Role.Patient || userole[0] == Role.Beneficiar)
                    {
                        PatientLogInURL = PatientLogInURL + "?userId=" + userId;
                        //scope.Complete();
                        return Redirect(PatientLogInURL);
                    }
                }
            }

            return null;
        }
        [HttpPost("forgotPassword")]
        public async Task<authentication> ForgotPassword(ForgotPasswordViewModel model)
        {
            authentication _userauth = new authentication();
            var user = _userRepository.GetUserByEmail(model.Email.Trim());
            if (user != null)
            {

                string _verificationotp = GenerateRandomNo();
                bool _IsPhoneNumber = validTelephoneNo(model.Email.Trim());
                if (_IsPhoneNumber)
                {
                    _smsSender.SendSms(model.Email, "Your OTP to verify your Phone No for reset password  is " + _verificationotp + ". Do not share this OTP with anyone.");
                }
                else
                {
                    string _sendername = "";
                    if (user.FullName != null)
                    {
                        _sendername = user.FullName;
                    }
                    else
                    {
                        _sendername = user.UserRole;
                    }
                    _emailSender.SendEmail(EmailTemplate.UserForgotPassword, EmailTemplate.GetUserForgotPasswordTemplate(_sendername, _verificationotp), new List<string> { model.Email }, "html");
                }

                _userauth.email = user.UserName.Trim();
                _userauth.userRole = user.UserRole;
                _userauth.id = user.Id;
                _userauth.success = "1";
                _userauth.verificationotp = cryptoservicemd5(_verificationotp);
                _userauth.message = "";
            }
            else
            {
                _userauth.success = "0";
                _userauth.message = "Email or Phone No not registered with us! Please Sign Up!";
            }

            return _userauth;
        }
        [HttpPost("updateBeneficiaryProfile")]
        public async Task<ActionResult<ApplicationUser>> updateBeneficiaryProfile(UserViewModel model)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //Checking Exiting User
                    var ExistingUser = await _userManager.FindByIdAsync(model.Id);

                    if (ExistingUser != null)
                    {

                        var _user = await _userManager.FindByIdAsync(ExistingUser.Id);
                        //_user.Country = string.IsNullOrEmpty(model.Country) ? _user.Country : model.Country;
                        _user.DateOfBirth = model.DateOfBirth == null ? _user.DateOfBirth : model.DateOfBirth;
                        _user.DateOfBirth = _user.DateOfBirth.Date;

                        _user.Designation = model.Designation;
                        _user.Email = model.Email;
                        _user.FirstName = model.FirstName;
                        _user.LastName = model.LastName;
                        _user.FullName = model.FullName;
                        _user.UserName = model.UserName;
                        _user.UserName = string.IsNullOrEmpty(model.UserName) ? _user.UserName : model.UserName;
                        _user.BloodGroup = model.BloodGroup;
                        // _user.Gender = (Gender)model?.Gender;

                        //_user.Interests = string.IsNullOrEmpty(model.Interests) ? _user.Interests : model.Interests;
                        _user.PhoneNumber = model.PhoneNumber;
                        _user.Twitter = model.Twitter;
                        _user.Website = model.Website;
                        //_user.MaritalStatus = model?.MaritalStatus;

                        var result = await _userManager.UpdateAsync(_user);
                        scope.Complete();
                        return _user;
                    }

                    else if (ExistingUser == null)
                        return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return NotFound();
        }
        [HttpPost("changePassword")]
        public async Task<ApplicationUser> ChangePassword(UserViewModel model)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                //Checking Exiting User
                var ExistingUser = await _userManager.FindByIdAsync(model.Id);

                if (ExistingUser != null)
                {
                    var res1 = await _signInManager.PasswordSignInAsync(ExistingUser.UserName, model.OldPassword, false, lockoutOnFailure: false);
                    if (!res1.Succeeded)
                    {
                        //if (string.IsNullOrEmpty(userole[0])) user.UserRole = userole[0];
                        //user.Message = "Wrong Credentials!";
                        //return user;


                        var _user = await _userManager.FindByIdAsync(ExistingUser.Id);
                        var userole = await _userManager.GetRolesAsync(_user);
                        var tempUser = new ApplicationUser();
                        tempUser.UserRole = userole[0];
                        tempUser.Message = "Current password is wrong!";                        
                        return tempUser;
                    }
                    else
                    {
                        var _user = await _userManager.FindByIdAsync(ExistingUser.Id);

                        if (model.Password == model.ConfirmPassword && !string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.ConfirmPassword))
                        {
                            var token = await _userManager.GeneratePasswordResetTokenAsync(_user);
                            var res = await _userManager.ResetPasswordAsync(_user, token, model.Password);
                            if (res.Succeeded)
                            {

                                var userole = await _userManager.GetRolesAsync(_user);
                                scope.Complete();
                                _user.UserRole = userole[0];
                                _user.Message = "Password Changed!";
                                return _user;
                            }
                        }
                    }
                }
            }
            return null;
        }
        [HttpPost("GetUserById")]
        public async Task<ApplicationUser> GetUserById(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return null;
            }
            return user;
        }
        [HttpPost("GetUserList")]
        public IEnumerable<UserViewModel> GetUserList(UserViewModel model)
        {
            if (model.UserRole == Role.Provider)
            {
                var users = (from a in _context.Users
                             join b in _context.UserRoles on a.Id equals b.UserId
                             join c in _context.Roles on b.RoleId equals c.Id
                             join d in _context.UserAddress on a.Id equals d.UserId
                             join e in _context.Specialization on a.Id equals e.UserId
                             join f in _context.AppointmentSettings on a.Id equals f.ServiceProviderId
                             where c.Name == model.UserRole && f.Id != null
                             select new UserViewModel
                             {
                                 Id = a.Id,
                                 UserName = a.UserName,
                                 Email = a.Email,
                                 PhoneNumber = a.PhoneNumber,
                                 FullName = a.FullName,
                                 ProfilePicture = a.ProfilePicture,
                                 UserRole = a.UserRole,
                                 CurrentAddress = d.CurrentAddress,
                                 City = d.City,
                                 State = d.State,
                                 Country = d.Country,
                                 PostalCode = d.PostalCode,
                                 Service = e.ServiceName,
                                 Specialization = e.SpecializationName,
                                 Gender = (a.Gender == 0) ? Gender.Male.ToString() : Gender.Female.ToString()
                             }).ToList();

                return users.ToList();
            }

            else if (model.UserRole == Role.Beneficiar)
            {
                var users = (from a in _context.Users
                             join b in _context.UserRoles on a.Id equals b.UserId
                             join c in _context.Roles on b.RoleId equals c.Id
                             //join d in _context.UserAddress on a.Id equals d.UserId
                             where c.Name == model.UserRole
                             select new UserViewModel
                             {
                                 Id = a.Id,
                                 UserName = a.UserName,
                                 Email = a.Email,
                                 PhoneNumber = a.PhoneNumber,
                                 FullName = a.FullName,
                                 ProfilePicture = a.ProfilePicture,
                                 UserRole = a.UserRole,
                                 Gender = (a.Gender == 0) ? Gender.Male.ToString() : Gender.Female.ToString()
                                 //CurrentAddress = d.CurrentAddress,
                                 //City = d.City,
                                 //State =d.State,
                                 //Country = d.Country,
                                 //PostalCode = d.PostalCode,

                             }).ToList();

                return users.ToList();
            }

            else if (model.UserRole == Role.Admin)
            {
                var users = (from a in _context.Users
                             join b in _context.UserRoles on a.Id equals b.UserId
                             join c in _context.Roles on b.RoleId equals c.Id
                             join d in _context.UserAddress on a.Id equals d.UserId
                             where c.Name == model.UserRole && d.IsActive == true
                             select new UserViewModel
                             {
                                 Id = a.Id,
                                 UserName = a.UserName,
                                 Email = a.Email,
                                 PhoneNumber = a.PhoneNumber,
                                 FullName = a.FullName,
                                 ProfilePicture = a.ProfilePicture,
                                 UserRole = a.UserRole,
                                 Gender = (a.Gender == 0) ? Gender.Male.ToString() : Gender.Female.ToString()
                                 //CurrentAddress = d.CurrentAddress,
                                 //City = d.City,
                                 //State =d.State,
                                 //Country = d.Country,
                                 //PostalCode = d.PostalCode,

                             }).ToList();

                return users.ToList();
            }

            return null;
        }
        [HttpPost("ProfilePicture")]
        public async Task<ApplicationUser> UploadProfilePicture([FromForm] UserViewModel model)
        {
            string imagename = "";
            try
            {
                if (model.File == null || model.File.Length == 0)
                {
                    return null;
                }

                var user = await _userManager.FindByIdAsync(model.Id);
                //var user = _userManager.GetUserAsync(User).Result;
                var file = Request.Form.Files[0];
                //var file = model.File;
                var uploadFolderUrl = Directory.GetCurrentDirectory() + "\\Data\\ProfilePic\\img";
                string imagePath = string.Empty;
                if (file != null)
                {
                    var fileextension = System.IO.Path.GetExtension(file.FileName);
                    if (!Directory.Exists(uploadFolderUrl))
                        Directory.CreateDirectory(uploadFolderUrl);

                    var ExistingUser = await _userManager.FindByIdAsync(model.Id);

                    if (ExistingUser != null)
                    {
                        if (ExistingUser.ProfilePicture != null)
                        {
                            imagePath = Path.Combine(uploadFolderUrl, ExistingUser.ProfilePicture);
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }
                        }
                    }
                    imagePath = "";
                    imagePath = Path.Combine(uploadFolderUrl, $"{user.Id}" + fileextension);
                    imagename = $"{user.Id}" + fileextension;


                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    var imageByte = Helper.GetImage(imagePath);
                    user.ProfilePicture = imagename;
                    await _userManager.UpdateAsync(user);
                    return user;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("updateProviderProfile")]
        public async Task<ActionResult<authentication>> updateProviderProfile(UserViewModel model)
        {
            authentication objauth = new authentication();
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //Checking Exiting User

                    var ExistingUser = await _userManager.FindByIdAsync(model.Id);

                    if (ExistingUser != null)
                    {
                        var _user = await _userManager.FindByIdAsync(ExistingUser.Id);
                        //_user.Country = string.IsNullOrEmpty(model.Country) ? _user.Country : model.Country;
                        _user.DateOfBirth = model.DateOfBirth == null ? _user.DateOfBirth : model.DateOfBirth;
                        _user.DateOfBirth = _user.DateOfBirth.Date;

                        _user.Designation = model.Designation;
                        _user.Email = model.Email;
                        _user.FirstName = model.FirstName;
                        _user.LastName = model.LastName;
                        _user.FullName = model.FullName;
                        _user.UserName = string.IsNullOrEmpty(model.UserName) ? _user.UserName : model.UserName;
                        _user.BloodGroup = model.BloodGroup;
                        _user.Gender = (Gender)Convert.ToInt32(model.Gender);

                        //_user.Interests = string.IsNullOrEmpty(model.Interests) ? _user.Interests : model.Interests;
                        _user.PhoneNumber = model.PhoneNumber;
                        _user.Twitter = model.Twitter;
                        _user.Website = model.Website;
                        //_user.MaritalStatus = model?.MaritalStatus;

                        var result = await _userManager.UpdateAsync(_user);
                        var _updatedUser = await _userManager.FindByIdAsync(_user.Id);
                        scope.Complete();

                        objauth.success = "1";
                        objauth.message = "Profile Information Update Successfully.";

                    }
                    else if (ExistingUser == null)
                    {
                        objauth.success = "1";
                        objauth.message = "No Profile Information found.";
                    }

                }
            }
            catch (Exception ex)
            {
                objauth.success = "1";
                objauth.message = "Opps! Something went wrong please try after sometime;";
            }
            return objauth;
        }

        [HttpPost("updateUserProfile")]
        public async Task<ActionResult<authentication>> updateUserProfile(userdata model)
        {
            authentication objauth = new authentication();
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    //Checking Exiting User

                    var ExistingUser = await _userManager.FindByIdAsync(model.Id);
                    if (ExistingUser != null)
                    {
                        var _user = await _userManager.FindByIdAsync(ExistingUser.Id);
                        //_user.Country = string.IsNullOrEmpty(model.Country) ? _user.Country : model.Country;
                        _user.DateOfBirth = model.dateofbirth == null ? _user.DateOfBirth : model.dateofbirth;
                         
                        _user.Email = model.email;
                        
                        _user.FullName = model.fullname;
                        _user.UserName = string.IsNullOrEmpty(model.username) ? _user.UserName : model.username;
                        _user.BloodGroup = model.bloodgroup;
                        _user.Gender = (Gender)Convert.ToInt32(model.gender); 
                        _user.PhoneNumber = model.phonenumber;
                        var result = await _userManager.UpdateAsync(_user);
                        
                        

                        var addressInfo = _context.UserAddress.Where(m => m.UserId == model.Id).FirstOrDefault();                                        
                        if (addressInfo == null)
                        {
                            UserAddress objAddress = new UserAddress();
                            objAddress.CurrentAddress = model.currentaddress;
                            objAddress.City = model.city;
                            objAddress.State = model.state;
                            objAddress.PostalCode = model.postalcode;
                            objAddress.Country = model.country;
                            objAddress.UserId = model.Id;
                            _context.UserAddress.Add(objAddress);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            addressInfo.CurrentAddress = model.currentaddress;
                            addressInfo.City = model.city;
                            addressInfo.State = model.state;
                            addressInfo.PostalCode = model.postalcode;
                            addressInfo.Country = model.country;
                            _context.UserAddress.Update(addressInfo);
                            await _context.SaveChangesAsync();
                        }
                        if (model.usertype == "Provider")
                        {
                            var aboutmeInfo = _context.UserAboutMe.Where(m => m.UserId == model.Id).FirstOrDefault();
                            if (aboutmeInfo == null)
                            {
                                UserAboutMe objAboutme = new UserAboutMe();
                                objAboutme.AboutMe = model.aboutme;
                                objAboutme.UserId = model.Id;
                                _context.UserAboutMe.Add(objAboutme);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                aboutmeInfo.AboutMe = model.aboutme;
                                _context.UserAboutMe.Update(aboutmeInfo);
                                await _context.SaveChangesAsync();
                            }
                        }

                        var _updatedUser = await _userManager.FindByIdAsync(_user.Id);
                        scope.Complete();
                        objauth.success = "1";
                        objauth.message = "Profile Information Update Successfully.";

                    }
                    else if (ExistingUser == null)
                    {
                        objauth.success = "1";
                        objauth.message = "No Profile Information found.";
                    }

                }
            }
            catch (Exception ex)
            {
                objauth.success = "1";
                objauth.message = "Opps! Something went wrong please try after sometime;";
            }
            return objauth;
        }





        [HttpPost("getuserdatabyid")]
        public List<appuserprofiledata> getuserdatabyid(UserViewModel model)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                //Checking Exiting User
                var existinguserdata = _userRepository.getuserdatabyid(model.Id);
                scope.Complete();
                return existinguserdata;
            }
        }
        public async Task<bool> RefreshAccessToken(ApplicationUser user)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _userManager.RemoveAuthenticationTokenAsync(user, "SoowGood", "RefreshToken");
                    var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "SoowGood", "RefreshToken");
                    var result = await _userManager.SetAuthenticationTokenAsync(user, "SoowGood", "RefreshToken", newRefreshToken);

                    scope.Complete();
                    return result.Succeeded;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public async Task InsertDefaultData(string userId)
        {
            try
            {
                //using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var data = _context.PricingPlan.Where(p => p.IsActive == true && p.IsDefault == true).FirstOrDefault();
                    var userInfo = _context.Pricing.Where(p => p.IsActive == true && p.IsDefault == true && p.UserId == userId).ToList();

                    if (userInfo.Count() == 0)
                    {
                        var Data_Object = new Pricing
                        {
                            PlanId = data.Id,
                            NumberOfUsers = 10,
                            UserId = userId,
                            Name = data.Name,

                            Description = data.Description,
                            AmountPerUser = data.AmountPerUser,
                            RegistrationFees = data.RegistrationFees,
                            YearlyFees = data.YearlyFees,

                            NumberOfFreeUsers = data.NumberOfFreeUsers,
                            DiscountAmount = data.DiscountAmount,
                            IsDiscount = data.IsDiscount,
                            IsTrialPeriodOver = data.IsTrialPeriodOver,

                            IsOrganization = data.IsOrganization,
                            IsFree = data.IsFree,
                            IsMonthly = data.IsMonthly,
                            IsWeekly = data.IsWeekly,

                            IsCustomized = data.IsCustomized,
                            IsYearly = data.IsYearly,
                            IsDefault = data.IsDefault,
                            Currency = data.Currency,

                            CurrencySymbol = data.CurrencySymbol,
                            UserTypeId = data.UserTypeId,
                            PlanStartingDate = DateTime.Now,
                            TrialPeriodOver = data.PlanStartingDate.AddMonths(6),
                            Commission = data.Commission
                        };

                        _context.Pricing.Add(Data_Object);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        [HttpPost("getUserProfileCompletionStatus")]
        public List<ProfileStatus> GetUserProfileCompletionStatus([FromBody] UserViewModel model)
        {
            //_smsSender.SendSms(model.Email, "SoowGood OTP. Code: 2345");
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                //Checking Exiting User
                var ExistingUser = _userRepository.GetUserProfileCompletionStatus(model.Id);
                scope.Complete();
                return ExistingUser;
            }
            return null;
        }


        [HttpPost("sendcontactusemail")]
        public async Task<authentication> sendcontactusemail(contactus model)
        {             
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _emailSender.SendEmail(EmailTemplate.NewEnquiry, EmailTemplate.SendNewEnquiryEmailToAdmin(model.name, model.email,model.mobileno,model.message,model.address), new List<string> { adminemail }, "html");
                _emailSender.SendEmail(EmailTemplate.EnquiryConfirmation, EmailTemplate.SendNewEnquiryConfirmation(model.name), new List<string> { model.email }, "html");
                authentication objauth = new authentication();
                objauth.message = "Email send successfully.";
                objauth.success = "1";
                return objauth;

            }
            return null;
        }


        [HttpPost("AddUpdateDeviceInformation")]
        public ActionResult<userdeviceinfo> PostDeviceInformation(userdeviceinfo deviceinfo)
        {
            try
            {
                var deviceinfoInfo = _context.userdeviceinfo.Any(m => m.UserId == deviceinfo.UserId);

                if (deviceinfoInfo == false)
                {
                    _context.userdeviceinfo.Add(deviceinfo);
                    _context.SaveChanges();
                }
                else { 
                    _context.userdeviceinfo.Update(deviceinfo);
                    _context.SaveChanges();                    
                }
                return CreatedAtAction("GetUserDeviceInfo", new { id = deviceinfo.UserId }, deviceinfo);

            }
            catch (DbUpdateException ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("GetUserDeviceInfo")]
        public async Task<ActionResult<IEnumerable<userdeviceinfo>>> GetUserDeviceInfo(userdeviceinfo model)
        {

            var userdeviceinfo = await _context.userdeviceinfo.Where(m => m.UserId == model.UserId).ToListAsync();

            if (userdeviceinfo == null)
            {
                return NotFound();
            }
            else
            {
                return userdeviceinfo;
            }
        }


        [HttpPost("DeleteUserDeviceInfo")]
        public async Task<authentication> DeleteUserDeviceInfo(userdeviceinfo model)
        {
            authentication objauth = new authentication();
            var objSetting = _context.userdeviceinfo.Where(a => a.UserId == model.UserId).ToList();
            if (objSetting == null)
            {
                objauth.success = "1";
                objauth.message = "Device key information deleted Successfully!";
            }
            else
            {
                objSetting.ForEach(setting =>
                {
                    _context.userdeviceinfo.Remove(setting);
                });
                await _context.SaveChangesAsync();
                objauth.success = "1";
                objauth.message = "Device key information deleted Successfully!";
            }           
            return objauth;
        }



        [HttpPost("deleteuserprofiledata")]
        public List<authentication> deleteuserprofiledata(userdeviceinfo model)
        {
            var   existinguserdata = _userRepository.deleteuserprofiledata(model.Id);
            //objauth.success = "1";
            //objauth.message = "User deleted Successfully!";
            return existinguserdata;
        }


    }
}


