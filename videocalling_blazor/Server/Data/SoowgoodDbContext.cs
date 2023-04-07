using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using videocalling_blazor.Server.Model;

namespace videocalling_blazor.Server.Data
{
    public class SoowgoodDbContext : IdentityDbContext
    {
        public SoowgoodDbContext(DbContextOptions<SoowgoodDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            
        }
    }
}
