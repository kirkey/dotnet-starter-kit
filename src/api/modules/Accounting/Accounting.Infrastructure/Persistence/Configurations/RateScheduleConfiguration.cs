namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for RateSchedule entity.
/// Configures database mapping, indexes, and relationships for utility rate schedules.
/// </summary>
public class RateScheduleConfiguration : IEntityTypeConfiguration<RateSchedule>
{
    /// <summary>
    /// Configures the RateSchedule entity mapping.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<RateSchedule> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(RateSchedule), SchemaNames.Accounting);

        builder.HasKey(rs => rs.Id);

        builder.Property(rs => rs.RateCode)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Unique rate code identifier (e.g., RES-1, COM-2)");

        builder.Property(rs => rs.RateName)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Display name for the rate schedule");

        builder.Property(rs => rs.EffectiveDate)
            .IsRequired()
            .HasComment("Date when the rate becomes effective");

        builder.Property(rs => rs.ExpirationDate)
            .HasComment("Optional expiration date for the rate");

        builder.Property(rs => rs.EnergyRatePerKwh)
            .IsRequired()
            .HasPrecision(16, 6)
            .HasComment("Energy charge per kWh");

        builder.Property(rs => rs.DemandRatePerKw)
            .HasPrecision(16, 6)
            .HasComment("Optional demand charge per kW for demand-billed customers");

        builder.Property(rs => rs.FixedMonthlyCharge)
            .IsRequired()
            .HasPrecision(16, 2)
            .HasComment("Fixed monthly customer charge");

        builder.Property(rs => rs.IsTimeOfUse)
            .IsRequired()
            .HasComment("Whether the rate uses time-of-use periods");

        builder.Property(rs => rs.Description)
            .HasMaxLength(500)
            .HasComment("Description of the rate schedule");

        builder.Property(rs => rs.Notes)
            .HasMaxLength(2000)
            .HasComment("Additional notes");

        // Indexes for performance
        builder.HasIndex(rs => rs.RateCode)
            .IsUnique()
            .HasDatabaseName("IX_RateSchedule_RateCode");

        builder.HasIndex(rs => rs.EffectiveDate)
            .HasDatabaseName("IX_RateSchedule_EffectiveDate");

        builder.HasIndex(rs => rs.IsTimeOfUse)
            .HasDatabaseName("IX_RateSchedule_IsTimeOfUse");
    }
}

