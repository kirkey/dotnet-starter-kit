using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

internal sealed class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
{
    public void Configure(EntityTypeBuilder<Holiday> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(h => h.Id);

        builder.Property(h => h.HolidayName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(h => h.Description)
            .HasMaxLength(500);

        builder.HasIndex(h => h.HolidayDate)
            .HasDatabaseName("IX_Holiday_HolidayDate");

        builder.HasIndex(h => h.IsActive)
            .HasDatabaseName("IX_Holiday_IsActive");
    }
}

