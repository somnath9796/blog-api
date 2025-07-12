using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Create Reader & Writer role

            var readerRoleId = "5fdc6be6-e5b6-4dc8-9bba-9beddcc6748b";
            var writerRoleId = "d41fa7b4-8326-418a-a6cf-f67b589d8d9e";

            var Role = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            //Seed Data
            builder.Entity<IdentityRole>().HasData(Role);

            //Create a Admin User

            var UserId = "ec5acc7a-3df5-48c8-8239-033fb68d5790";



            var userAdmin = new IdentityUser()
            {
                Id = UserId,
                UserName = "admin@codepulse.com",
                Email = "admin@codepulse.com",
                NormalizedUserName = "admin@codepulse.com".ToUpper(),
                NormalizedEmail = "admin@codepulse.com".ToUpper(),
            };

            userAdmin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(userAdmin, "Admin@123");

            //Seed Admin User
            builder.Entity<IdentityUser>().HasData(userAdmin);


            //Assign Roles to AdminUser It will have both reader & write
            var AdminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId=UserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId=UserId,
                    RoleId = writerRoleId
                },
            };

            //Seed Admin User
            builder.Entity<IdentityUserRole<string>>().HasData(AdminRoles);

        }
    }
}
