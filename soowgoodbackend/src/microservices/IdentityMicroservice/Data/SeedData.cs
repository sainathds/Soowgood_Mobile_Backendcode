using IdentityMicroservice.Model;
using IdentityMicroservice.StaticData;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicroservice.Data
{
    public static class SeedData<TIdentityUser, TIdentityRole>
         where TIdentityUser : ApplicationUser, new()
         where TIdentityRole : IdentityRole, new()
    {
        private const string DefaultAdminRoleName = Role.SuperAdmin;
        private const string DefaultUserRoleName = Role.User;
        private const string DefaultAdminUserEmail = "admin@gmail.com";
        private const string DefaultUserEmail = "ashik@gmail.com";
        private const string DefaultPassword = "123456aA@";

        private static readonly string[] DefaultRoles = { Role.SuperAdmin, Role.Admin, Role.User, Role.OrganizationalAdmin, Role.Doctor, Role.Patient };

        private static async Task CreateDefaultRoles(RoleManager<TIdentityRole> roleManager)
        {
            foreach (string defaultRole in DefaultRoles)
            {
                // Make sure we have an Administrator role
                try
                {
                    if (!await roleManager.RoleExistsAsync(defaultRole))
                    {
                        var role = new TIdentityRole
                        {
                            Name = defaultRole
                        };

                        var roleResult = await roleManager.CreateAsync(role);
                        if (!roleResult.Succeeded)
                        {
                            throw new ApplicationException($"Could not create '{defaultRole}' role");
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        private static async Task<TIdentityUser> CreateDefaultAdminUser(UserManager<TIdentityUser> userManager)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(DefaultAdminUserEmail);
                if (user == null)
                {
                    user = new TIdentityUser
                    {
                        //UserNumber = 1,
                        UserName = "admin",
                        Email = DefaultAdminUserEmail,
                        EmailConfirmed = true,
                        Designation = "SuperAdmin",
                        FullName = "Super Admin",
                        Status = "Active",
                        Website = "https://www.google.com/",
                        Facebook = "https://www.facebook.com/",
                        Twitter = "https://www.twitter.com/",
                    };
                    var userResult = await userManager.CreateAsync(user, DefaultPassword);

                    if (!userResult.Succeeded)
                    {
                        throw new ApplicationException($"Could not create '{DefaultAdminUserEmail}' user");
                    }
                }
                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private static async Task AddDefaultAdminRoleToDefaultAdminUser(
            UserManager<TIdentityUser> userManager,
            TIdentityUser user)
        {
            // Add user to Administrator role if it's not already associated
            if (!(await userManager.GetRolesAsync(user)).Contains(DefaultAdminRoleName))
            {
                var addToRoleResult = await userManager.AddToRoleAsync(user, DefaultAdminRoleName);
                if (!addToRoleResult.Succeeded)
                {
                    throw new ApplicationException(
                        $"Could not add user '{DefaultAdminUserEmail}' to '{DefaultAdminRoleName}' role");
                }
            }
        }

        public static async Task SeedDataAsync(IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<IdentityMicroserviceContext>();
                var userManager = services.GetRequiredService<UserManager<TIdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<TIdentityRole>>();

                await context.Database.EnsureCreatedAsync();

                await CreateDefaultRoles(roleManager);
                var defaultAdminUser = await CreateDefaultAdminUser(userManager);
                await AddDefaultAdminRoleToDefaultAdminUser(userManager, defaultAdminUser);
                UpdateDBScripts(services);
                await context.Database.MigrateAsync();
            }catch(Exception ex)
            {
                throw;
            }
        }

        public static void UpdateDBScripts(IServiceProvider services)
        {
            var _configuration = services.GetRequiredService<IConfiguration>();
            string sqlConnectionString = _configuration.GetConnectionString("IdentityMicroserviceContext");
            InsertScript(sqlConnectionString, _configuration);
        }

        private static async Task<TIdentityUser> CreateDefaultUser(UserManager<TIdentityUser> userManager, TIdentityUser identityUser)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(identityUser.Email);
                if (user == null)
                {
                    user = identityUser;
                    var userResult = await userManager.CreateAsync(user, DefaultPassword);

                    if (!userResult.Succeeded)
                    {
                        throw new ApplicationException($"Could not create '{DefaultUserEmail}' user");
                    }
                }
                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private static async Task AddDefaultUserRoleToDefaultUser(
            UserManager<TIdentityUser> userManager,
            TIdentityUser user)
        {
            // Add user to Administrator role if it's not already associated
            if (!(await userManager.GetRolesAsync(user)).Contains(DefaultUserRoleName))
            {
                var addToRoleResult = await userManager.AddToRoleAsync(user, DefaultUserRoleName);
                if (!addToRoleResult.Succeeded)
                {
                    throw new ApplicationException(
                        $"Could not add user '{DefaultUserEmail}' to '{DefaultUserRoleName}' role");
                }
            }
        }

        public static void InsertScript(String conn, IConfiguration _configuration)
        {
            var path = string.Concat(Environment.CurrentDirectory, @"\Database\Script");
            string[] files = Directory.GetFiles(path);

            foreach (var singleFile in files)
            {
                try
                {
                    using (SqlConnection objConn = new SqlConnection(conn))
                    {
                        FileInfo file = new FileInfo(singleFile);
                        string script = file.OpenText().ReadToEnd();

                        objConn.Open();
                        SqlCommand comnd = new SqlCommand();
                        comnd.CommandType = CommandType.Text;

                        script = script.Replace("go", "GO");
                        script = script.Replace("Go", "GO");
                        script = script.Replace("gO", "GO");
                        comnd.Connection = objConn;

                        foreach (var sqlBatch in script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            comnd.CommandText = sqlBatch;
                            comnd.ExecuteNonQuery();
                        }

                        objConn.Close();

                    }
                }
                catch (Exception ex)
                {

                }
            }

        }
    }
}
