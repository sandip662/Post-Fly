using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{



    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Roles (User, Admin, SuperAdmin)

            var adminRoleId = "4848f8bd-8943-4e3e-8a77-0f245be64e3b";
            var superAdminRoleId = "ce4794f3-d9a7-40bf-b85d-a15ed5f7cac9";
            var userRoleId = "aff893dd-4a0a-4fbb-9ecb-aed8e29e1021";


            var roles = new List<IdentityRole>
            {
              new IdentityRole
              {
                 Name = "Admin",
                 NormalizedName = "Admin",
                 Id = adminRoleId,
                 ConcurrencyStamp = adminRoleId
              },

              new IdentityRole
              {
                 Name = "SuperAdmin",
                 NormalizedName = "SuperAdmin",
                 Id = superAdminRoleId,
                 ConcurrencyStamp = superAdminRoleId
              },

              new IdentityRole
              {
                 Name = "User",
                 NormalizedName = "User",
                 Id = userRoleId,
                 ConcurrencyStamp = userRoleId
              }


            };

            builder.Entity<IdentityRole>().HasData(roles);


            // Seed SuperAdminUser

            var superAdminId = "e57531ab-259a-4d31-a235-f67ac00dfff4";
            var superAdminUser = new IdentityUser
            {
               UserName = "superadmin",
               Email = "superadmin@bloggie.com",
               NormalizedUserName = "superadmin".ToUpper(),
               NormalizedEmail = "superadmin@bloggie.com" .ToUpper(),
               Id = superAdminId,
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "superAdmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);



            // Add All roles to SuperAdminUser

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                 RoleId = adminRoleId,
                 UserId = superAdminId,
                },

                 new IdentityUserRole<string>
                {
                 RoleId = superAdminRoleId,
                 UserId = superAdminId,
                },

                  new IdentityUserRole<string>
                {
                 RoleId = userRoleId,
                 UserId = superAdminId,
                }



            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);

        }


    }
}
