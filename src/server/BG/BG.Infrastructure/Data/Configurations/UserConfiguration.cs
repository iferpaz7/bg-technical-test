namespace BG.Infrastructure.Data.Configurations;

public class UserConfiguration : BaseEntityTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder
            .HasIndex(x => x.Username)
            .HasDatabaseName("UX_Username")
            .IsUnique();
    }
}