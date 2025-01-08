using System.Reflection;

namespace BG.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<IdentificationType> IdentificationTypes { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    //SET TO VARCHAR STRING COLUMNS
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().AreUnicode(false);
    }

    //SET VALUES BY DEFAULT IN UPDATEDAT
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is { Entity: BaseEntity, State: EntityState.Added or EntityState.Modified });

        foreach (var entityEntry in entries)
            if (entityEntry.State == EntityState.Modified)
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.Now;
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}