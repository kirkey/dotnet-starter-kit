using Shared.Constants;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Company.
/// </summary>
public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.IsMultiTenant();
        builder.ToTable("Companies", SchemaNames.HumanResources);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CompanyCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.CompanyCode)
            .IsUnique()
            .HasDatabaseName("IX_Companies_CompanyCode");

        builder.Property(c => c.Tin)
            .HasMaxLength(50);

        builder.Property(c => c.Address)
            .HasMaxLength(500);

        builder.Property(c => c.ZipCode)
            .HasMaxLength(20);
        
        builder.Property(c => c.Phone)
            .HasMaxLength(50);

        builder.Property(c => c.Email)
            .HasMaxLength(256);

        builder.Property(c => c.Website)
            .HasMaxLength(500);

        builder.Property(c => c.LogoUrl)
            .HasMaxLength(500);

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Indexes for performance
        builder.HasIndex(c => c.IsActive)
            .HasDatabaseName("IX_Companies_IsActive");
        
        // Audit fields
        builder.Property(c => c.CreatedBy)
            .HasMaxLength(256);

        builder.Property(c => c.LastModifiedBy)
            .HasMaxLength(256);
    }
}
