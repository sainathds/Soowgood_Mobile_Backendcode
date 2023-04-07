using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System;
using videocalling_blazor.Server.Options;
using videocalling_blazor.Server.Services;
using Microsoft.AspNetCore.Http.Features;
using videocalling_blazor.Server.Hubs;
using videocalling_blazor.Shared;
using Microsoft.Net.Http.Headers;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using videocalling_blazor.Server.Data;
using videocalling_blazor.Server.Interfaces;
using videocalling_blazor.Server.Repository;

namespace videocalling_blazor.Server
{
    public class Startup
    {
        public static string ConnectionString { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConnectionString = Configuration.GetConnectionString("SoowgoodDbContext");
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddDbContext<SoowgoodDbContext>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("SoowgoodDbContext")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddCors(options => options.AddPolicy("CORS", x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyOrigin();
                x.AllowAnyMethod();

            }));

            services.AddSignalR(options => options.EnableDetailedErrors = true)
        .AddMessagePackProtocol();
            services.Configure<TwilioSettings>(settings =>
            {
                settings.AccountSid = Configuration.GetSection("TWILIO_ACCOUNT_SID").Value;
                settings.ApiSecret = Configuration.GetSection("TWILIO_API_SECRET").Value;
                settings.ApiKey = Configuration.GetSection("TWILIO_API_KEY").Value;
            });
            services.AddSingleton<TwilioService>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddResponseCompression(opts =>
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" }));

            RegisterServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IBookingRepository, BookingRepository>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                HttpsCompression = HttpsCompressionMode.Compress,
                OnPrepareResponse = context =>
                    context.Context.Response.Headers[HeaderNames.CacheControl] =
                        $"public,max-age={86_400}"
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //endpoints.MapHub<NotificationHub>(HubEndpoints.NotificationHub);
                endpoints.MapFallbackToFile("index.html");
            });




        }
    }
}
