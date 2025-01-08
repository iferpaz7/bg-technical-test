namespace BG.Infrastructure.Data.Configurations;

public class IdentificationTypeConfiguration : BaseEntityTypeConfiguration<IdentificationType>
{
    public override void Configure(EntityTypeBuilder<IdentificationType> builder)
    {
        base.Configure(builder);

        builder
            .HasIndex(x => x.Code)
            .HasDatabaseName("UX_IdentificationTypeCode")
            .IsUnique();
    }
}