namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for SecurityDeposit entity.
/// Configures database mapping, indexes, and relationships for customer security deposits.
/// </summary>
public class SecurityDepositConfiguration : IEntityTypeConfiguration<SecurityDeposit>
{
    /// <summary>
    /// Configures the SecurityDeposit entity mapping.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<SecurityDeposit> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable(nameof(SecurityDeposit), SchemaNames.Accounting);

        builder.HasKey(sd => sd.Id);

        builder.Property(sd => sd.MemberId)
            .IsRequired()
            .HasComment("Member who paid the security deposit");

        builder.Property(sd => sd.DepositAmount)
            .IsRequired()
            .HasPrecision(16, 2)
            .HasComment("Amount deposited; must be positive");

        builder.Property(sd => sd.DepositDate)
            .IsRequired()
            .HasComment("Date the deposit was received");

        builder.Property(sd => sd.IsRefunded)
            .IsRequired()
            .HasComment("Whether the deposit has been refunded");

        builder.Property(sd => sd.RefundedDate)
            .HasComment("Date of refund, when applicable");

        builder.Property(sd => sd.RefundReference)
            .HasMaxLength(100)
            .HasComment("External reference for the refund (e.g., check number)");

        builder.Property(sd => sd.Description)
            .HasMaxLength(500)
            .HasComment("Description of the deposit");

        builder.Property(sd => sd.Notes)
            .HasMaxLength(2000)
            .HasComment("Additional notes");

        // Indexes for performance
        builder.HasIndex(sd => sd.MemberId)
            .HasDatabaseName("IX_SecurityDeposit_MemberId");

        builder.HasIndex(sd => sd.DepositDate)
            .HasDatabaseName("IX_SecurityDeposit_DepositDate");

        builder.HasIndex(sd => sd.IsRefunded)
            .HasDatabaseName("IX_SecurityDeposit_IsRefunded");

        builder.HasIndex(sd => new { sd.MemberId, sd.IsRefunded })
            .HasDatabaseName("IX_SecurityDeposit_Member_IsRefunded");
    }
}

