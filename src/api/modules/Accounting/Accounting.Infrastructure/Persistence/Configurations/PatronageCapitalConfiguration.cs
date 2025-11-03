namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for PatronageCapital entity.
/// Configures database mapping, indexes, and relationships for patronage capital allocations and retirements.
/// </summary>
public class PatronageCapitalConfiguration : IEntityTypeConfiguration<PatronageCapital>
{
    /// <summary>
    /// Configures the PatronageCapital entity mapping.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<PatronageCapital> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(PatronageCapital), SchemaNames.Accounting);

        builder.HasKey(pc => pc.Id);

        builder.Property(pc => pc.MemberId)
            .IsRequired()
            .HasComment("Member receiving the patronage capital allocation");

        builder.Property(pc => pc.FiscalYear)
            .IsRequired()
            .HasComment("Fiscal year of the allocation (e.g., 2025)");

        builder.Property(pc => pc.AmountAllocated)
            .IsRequired()
            .HasPrecision(16, 2)
            .HasComment("Total capital amount allocated for the year");

        builder.Property(pc => pc.AmountRetired)
            .IsRequired()
            .HasPrecision(16, 2)
            .HasComment("Cumulative amount retired from the allocation");

        builder.Property(pc => pc.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Status: Allocated, Retired, PartiallyRetired");

        builder.Property(pc => pc.Description)
            .HasMaxLength(500)
            .HasComment("Description of the allocation");

        builder.Property(pc => pc.Notes)
            .HasMaxLength(2000)
            .HasComment("Additional notes");

        // Indexes for performance
        builder.HasIndex(pc => pc.MemberId)
            .HasDatabaseName("IX_PatronageCapital_MemberId");

        builder.HasIndex(pc => pc.FiscalYear)
            .HasDatabaseName("IX_PatronageCapital_FiscalYear");

        builder.HasIndex(pc => pc.Status)
            .HasDatabaseName("IX_PatronageCapital_Status");

        builder.HasIndex(pc => new { pc.MemberId, pc.FiscalYear })
            .HasDatabaseName("IX_PatronageCapital_Member_FiscalYear");
    }
}

