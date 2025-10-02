using Accounting.Domain.Entities;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class BankReconciliationConfiguration : IEntityTypeConfiguration<BankReconciliation>
{
    public void Configure(EntityTypeBuilder<BankReconciliation> builder)
    {
        builder.ToTable("BankReconciliations", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BankAccountId)
            .IsRequired();

        builder.Property(x => x.ReconciliationDate)
            .IsRequired();

        builder.Property(x => x.StatementBalance)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.BookBalance)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.AdjustedBalance)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.OutstandingChecksTotal)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.DepositsInTransitTotal)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.BankErrors)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.BookErrors)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.IsReconciled)
            .IsRequired();

        builder.Property(x => x.ReconciledDate);

        builder.Property(x => x.ReconciledBy)
            .HasMaxLength(256);

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(256);

        builder.Property(x => x.ApprovedDate);

        builder.Property(x => x.StatementNumber)
            .HasMaxLength(100);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);
    }
}
