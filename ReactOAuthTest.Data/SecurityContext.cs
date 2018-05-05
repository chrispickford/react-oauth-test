using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReactOAuthTest.Data.Entities;

namespace ReactOAuthTest.Data
{
    public class SecurityContext : IdentityDbContext<User>
    {
        public SecurityContext(DbContextOptions options) : base(options)
        {
        }
    }
}