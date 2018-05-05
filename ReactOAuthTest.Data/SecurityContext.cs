using Microsoft.EntityFrameworkCore;
using ReactOAuthTest.Data.Entities;

namespace ReactOAuthTest.Data
{
    public class SecurityContext : DbContext
    {
        public SecurityContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}