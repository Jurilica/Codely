using Codely.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Data;

public class CodelyContext : DbContext
{
    public CodelyContext(DbContextOptions<CodelyContext> options) 
        : base(options)
    {
    }
    
    public DbSet<Example> Examples => Set<Example>();
    
    public DbSet<Problem> Problems => Set<Problem>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    
    public DbSet<Submission> Submissions => Set<Submission>();
    
    public DbSet<TestCase> TestCases => Set<TestCase>();
    
    public DbSet<User> Users => Set<User>();
}