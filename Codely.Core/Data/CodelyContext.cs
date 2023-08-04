using System.Linq.Expressions;
using Codely.Core.Data.Entities;
using Codely.Core.Data.Entities.Base;
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

    public DbSet<ProgrammingLanguageVersion> ProgrammingLanguageVersions => Set<ProgrammingLanguageVersion>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Submission> Submissions => Set<Submission>();

    public DbSet<SubmissionTestCase> SubmissionTestCases => Set<SubmissionTestCase>();

    public DbSet<TestCase> TestCases => Set<TestCase>();

    public DbSet<User> Users => Set<User>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var softDeleteEntities = typeof(BaseEntity).Assembly.GetTypes()
            .Where(x => typeof(BaseEntity).IsAssignableFrom(x))
            .Where(x => x.IsClass)
            .Where(x => !x.IsAbstract);
        
        foreach (var softDeleteEntity in softDeleteEntities)
        {
            modelBuilder.Entity(softDeleteEntity)
                .HasQueryFilter(GenerateQueryFilterLambdaExpression(softDeleteEntity));
        }

        base.OnModelCreating(modelBuilder);
    }
    
    private static LambdaExpression GenerateQueryFilterLambdaExpression(Type type)
    {
        // we generate:  x => x.ArchivedTime == null
        // x =>
        var parameter = Expression.Parameter(type, "x");
        // null
        var falseConstant = Expression.Constant(null);
        // x.ArchiveDate
        var propertyAccess = Expression.PropertyOrField(parameter, nameof(BaseEntity.Archived));
        // e.ArchiveDate == null
        var equalExpression = Expression.Equal(propertyAccess, falseConstant);
        // x => e.ArchiveDate == null
        var lambda = Expression.Lambda(equalExpression, parameter);
        
        return lambda;
    }
}