using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ReactOAuthTest.Data;
using ReactOAuthTest.Data.Entities;

namespace ReactOAuthTest.Api.Helpers
{
    public class SecurityContextSeedData
    {
        private readonly UserManager<User> userManager;

        public SecurityContextSeedData(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async void SeedUser()
        {
            var user = new User
            {
                UserName = "test@test.com",
                NormalizedUserName = "test@test.com",
                Email = "test@test.com",
                NormalizedEmail = "test@test.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = "Testy",
                LastName = "McTest"
            };

            await userManager.CreateAsync(user, "Test123!");
        }
    }
}