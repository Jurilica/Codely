using Codely.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Data;

public class CodelyContext : DbContext
{
    public CodelyContext(DbContextOptions<CodelyContext> options) 
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
}