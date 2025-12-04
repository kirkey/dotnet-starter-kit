namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the FeeCharge entity.
/// </summary>
internal sealed class FeeChargeConfiguration : IEntityTypeConfiguration<FeeCharge>
{
    public void Configure(EntityTypeBuilder<FeeCharge> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Reference)
            .IsRequired()
            .HasMaxLength(FeeCharge.ReferenceMaxLength);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(FeeCharge.StatusMaxLength);

        builder.Property(x => x.Notes)
            .HasMaxLength(FeeCharge.NotesMaxLength);

        builder.Property(x => x.Amount).HasPrecision(18, 2);
        builder.Property(x => x.AmountPaid).HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.FeeDefinition)
            .WithMany()
            .HasForeignKey(x => x.FeeDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Loan)
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.SavingsAccount)
            .WithMany()
            .HasForeignKey(x => x.SavingsAccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.ShareAccount)
            .WithMany()
            .HasForeignKey(x => x.ShareAccountId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.Reference)
            .IsUnique()
            .HasDatabaseName("IX_FeeCharges_Reference");

        builder.HasIndex(x => x.FeeDefinitionId)
            .HasDatabaseName("IX_FeeCharges_FeeDefinitionId");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_FeeCharges_MemberId");

        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_FeeCharges_LoanId");

        builder.HasIndex(x => x.SavingsAccountId)
            .HasDatabaseName("IX_FeeCharges_SavingsAccountId");

        builder.HasIndex(x => x.ShareAccountId)
            .HasDatabaseName("IX_FeeCharges_ShareAccountId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_FeeCharges_Status");

        builder.HasIndex(x => x.ChargeDate)
            .HasDatabaseName("IX_FeeCharges_ChargeDate");

        builder.HasIndex(x => x.DueDate)
            .HasDatabaseName("IX_FeeCharges_DueDate");
    }
}
