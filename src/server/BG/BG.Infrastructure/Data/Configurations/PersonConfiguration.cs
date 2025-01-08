namespace BG.Infrastructure.Data.Configurations;

public class PersonConfiguration : BaseEntityTypeConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);

        builder
            .HasIndex(x => x.Code)
            .HasDatabaseName("UX_PersonCode")
            .IsUnique();

        builder
            .HasIndex(x => x.IdCard)
            .HasDatabaseName("UX_PersonIdCard")
            .IsUnique();
    }
}