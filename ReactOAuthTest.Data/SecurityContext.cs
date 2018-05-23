using System.Linq;
using System.Threading.Tasks;
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

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public async Task InsertOrUpdateRefreshToken(RefreshToken token)
        {
            var existingTokens = RefreshTokens.Where(x => x.UserId == token.UserId);
            if (existingTokens.Any())
            {
                RefreshTokens.RemoveRange(existingTokens);
                await SaveChangesAsync();
            }

            await RefreshTokens.AddAsync(token);
            await SaveChangesAsync();
        }
    }
}