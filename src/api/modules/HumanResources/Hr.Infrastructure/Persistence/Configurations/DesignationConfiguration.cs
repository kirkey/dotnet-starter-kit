using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Designation (Job Title).
/// </summary>
public class DesignationConfiguration : IEntityTypeConfiguration<Designation>
{
    public void Configure(EntityTypeBuilder<Designation> builder)
    {
        builder.IsMultiTenant();
        builder.ToTable("Designations", SchemaNames.HumanResources);

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(d => d.Code)
            .IsUnique()
            .HasDatabaseName("IX_Designations_Code");

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(d => d.Area)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("National");

        builder.Property(d => d.SalaryGrade)
            .HasMaxLength(50);

        builder.Property(d => d.Description)
            .HasMaxLength(2000);

        builder.Property(d => d.MinimumSalary)
            .HasPrecision(16, 2);

        builder.Property(d => d.MaximumSalary)
            .HasPrecision(16, 2);

        builder.Property(d => d.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(d => d.IsManagerial)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes for common queries
        builder.HasIndex(d => d.IsActive)
            .HasDatabaseName("IX_Designations_IsActive");

        builder.HasIndex(d => d.Area)
            .HasDatabaseName("IX_Designations_Area");

        builder.HasIndex(d => d.SalaryGrade)
            .HasDatabaseName("IX_Designations_SalaryGrade");

        builder.HasIndex(d => d.IsManagerial)
            .HasDatabaseName("IX_Designations_IsManagerial");

        // Optimized for manager role identification
        builder.HasIndex(d => new { d.IsManagerial, d.IsActive })
            .HasDatabaseName("IX_Designations_Manager_Active");

        // Salary grade + area lookups
        builder.HasIndex(d => new { d.SalaryGrade, d.Area, d.IsActive })
            .HasDatabaseName("IX_Designations_Grade_Area_Active");

        // Title-based searches (covering index would benefit lookup)
        builder.HasIndex(d => new { d.Title, d.IsActive })
            .HasDatabaseName("IX_Designations_Title_Active");

        // Area filtering
        builder.HasIndex(d => new { d.Area, d.IsActive })
            .HasDatabaseName("IX_Designations_Area_Active");

        // Audit fields
        builder.Property(d => d.CreatedBy)
            .HasMaxLength(256);

        builder.Property(d => d.LastModifiedBy)
            .HasMaxLength(256);
    }
}

