using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using Microsoft.OpenApi.Models;
using IdentityMicroservice.Repository;
using IdentityMicroservice.Interfaces;
using IdentityMicroservice.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Nager.Country;
using IdentityMicroservice.StaticData;
using Microsoft.Extensions.FileProviders;
using System.IO;
using IdentityMicroservice.Options;
using IdentityMicroservice.Hubs;

namespace IdentityMicroservice
{
    public class Startup
    {
        
        public static string ConnectionString { get; private set; }
        public static string ResourceConnectionString { get; private set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
           .SetBasePath(env.ContentRootPath)
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            var mvcBuilder = serviceProvider.GetService<IMvcBuilder>();
            new MvcConfiguration().ConfigureMvc(mvcBuilder);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConnectionString = Configuration.GetConnectionString("IdentityMicroserviceContext");
            ResourceConnectionString = Configuration.GetConnectionString("IdentityMicroserviceContext");
            //services.AddControllers();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                //options.SerializerSettings.DateFormatString = "dd/MM/yyyy hh:mm tt";
                options.SerializerSettings.DateFormatString = "yyyy/MM/dd hh:mm tt";
                //options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";
                //options.SerializerSettings.Converters.Add(new JsonTimeSpanConverter());
            });

            services.Configure<PasswordHasherOptions>(options =>
                options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
            );

            services.AddDbContext<IdentityMicroserviceContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("IdentityMicroserviceContext")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);

            //services.AddDbContext<IdentityMicroserviceContext>(options =>
            //   options.UseNpgsql(Configuration.GetConnectionString("IdentityMicroserviceContext")), ServiceLifetime.Transient);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityMicroserviceContext>()
                .AddTokenProvider("SoowGood", typeof(DataProtectorTokenProvider<ApplicationUser>))
                .AddDefaultTokenProviders();

            services.AddControllersWithViews();

            services.Configure<TwilioSettings>(
                settings =>
                {
                    settings.AccountSid = Configuration.GetSection("TWILIO_ACCOUNT_SID").Value;
                    settings.ApiSecret = Configuration.GetSection("TWILIO_API_SECRET").Value;
                    settings.ApiKey = Configuration.GetSection("TWILIO_API_KEY").Value;
                })
                .AddTransient<IVideoService, VideoService>();
                 

            services.AddSignalR();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;

                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false; 
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "SooGood.UserIdentity";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

                //options.Cookie.SameSite = SameSiteMode.None;
                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                //setting the inactivity timeout to 3 days
                options.ExpireTimeSpan = TimeSpan.FromDays(3);
                options.SlidingExpiration = true;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });

            services.AddCors(options => options.AddPolicy("CORS", x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyOrigin();
                x.AllowAnyMethod();

            }));

            services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddGoogle(options => {
                    options.ClientId = Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.SaveTokens = true;
                    options.UserInformationEndpoint = "https://openidconnect.googleapis.com/v1/userinfo";
                    options.ClaimActions.Clear();
                    //options.ClaimActions.MapJsonKey(ClaimTypes.PPID, "ppid");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "email");
                });

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["FacebookLogin:AppId"];
                facebookOptions.AppSecret = Configuration["FacebookLogin:AppSecret"];
            });

            services.AddAuthentication().AddTwitter(twitterOptions =>
            {
                twitterOptions.ConsumerKey = Configuration["TwitterLogin:ConsumerKey"];
                twitterOptions.ConsumerSecret = Configuration["TwitterLogin:ConsumerSecret"];
            });

            services.AddSession();
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc(options => options.EnableEndpointRouting = false);
            
            RegisterServices(services);
        }
        private void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserActivityRepository, UserActivityRepository>();
            services.AddScoped<ISearchService, SearchService>(); 
            services.AddScoped<IProfileStatusService, ProfileStatusService>();
            services.AddScoped<ICountryProvider, CountryProvider>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            builder.WithOrigins("*")
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), @"Data\ProfilePic\img")),
                RequestPath = "/img"
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), @"Data\ProviderDocument")),
                RequestPath = "/ProviderDocument"
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Data\appointmentdoc")),
                RequestPath = "/appointmentdoc"
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), @"Data\prescriptionpdf")),
                RequestPath = "/prescriptionpdf"
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Data\ClinicPic")),
                RequestPath = "/clinicpic"
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), @"Data\DoctorSignature")),
                RequestPath = "/doctorsignature"
            });




            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });
        }
    }
}
