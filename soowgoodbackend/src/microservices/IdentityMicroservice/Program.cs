using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ProfileManagement.Models;
using Microsoft.AspNetCore.Identity;
using IdentityMicroservice.Model;
using IdentityMicroservice.Data;

namespace IdentityMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // Since we want to use scaffolding, we can't use an async Main method so we 
                    // just wait for the task to finish in the old fashioned way
                    SeedData<ApplicationUser, IdentityRole>.SeedDataAsync(services).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseUrls("http://localhost:5001")
            .UseStartup<Startup>()
            .Build();
    }
}
