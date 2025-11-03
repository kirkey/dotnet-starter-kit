namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for DeferredRevenue entity.
/// Configures database mapping, indexes, and relationships for deferred revenue tracking.
/// </summary>
public class DeferredRevenueConfiguration : IEntityTypeConfiguration<DeferredRevenue>
{
    /// <summary>
    /// Configures the DeferredRevenue entity mapping.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<DeferredRevenue> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(DeferredRevenue), SchemaNames.Accounting);

        builder.HasKey(dr => dr.Id);

        builder.Property(dr => dr.DeferredRevenueNumber)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Unique identifier for the deferred revenue entry");

        builder.Property(dr => dr.RecognitionDate)
            .IsRequired()
            .HasComment("Date when the deferred revenue should be recognized");

        builder.Property(dr => dr.Amount)
            .IsRequired()
            .HasPrecision(16, 2)
            .HasComment("Deferred amount to be recognized; must be positive");

        builder.Property(dr => dr.Description)
            .HasMaxLength(500)
            .HasComment("Description of the deferred revenue");

        builder.Property(dr => dr.IsRecognized)
            .IsRequired()
            .HasComment("Whether the deferred revenue has been recognized");

        builder.Property(dr => dr.RecognizedDate)
            .HasComment("When the deferred revenue was recognized, if applicable");

        builder.Property(dr => dr.Notes)
            .HasMaxLength(2000)
            .HasComment("Additional notes");

        // Indexes for performance
        builder.HasIndex(dr => dr.DeferredRevenueNumber)
            .IsUnique()
            .HasDatabaseName("IX_DeferredRevenue_Number");

        builder.HasIndex(dr => dr.RecognitionDate)
            .HasDatabaseName("IX_DeferredRevenue_RecognitionDate");

        builder.HasIndex(dr => dr.IsRecognized)
            .HasDatabaseName("IX_DeferredRevenue_IsRecognized");

        builder.HasIndex(dr => new { dr.IsRecognized, dr.RecognitionDate })
            .HasDatabaseName("IX_DeferredRevenue_IsRecognized_RecognitionDate");
    }
}

