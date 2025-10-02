using Accounting.Domain.Entities;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class WriteOffConfiguration : IEntityTypeConfiguration<WriteOff>
{
    public void Configure(EntityTypeBuilder<WriteOff> builder)
    {
        builder.ToTable("WriteOffs", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.ReferenceNumber).IsUnique();

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.WriteOffDate)
            .IsRequired();

        builder.Property(x => x.WriteOffType)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.RecoveredAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.IsRecovered)
            .IsRequired();

        builder.Property(x => x.CustomerId);

        builder.Property(x => x.CustomerName)
            .HasMaxLength(256);

        builder.Property(x => x.InvoiceId);

        builder.Property(x => x.InvoiceNumber)
            .HasMaxLength(50);

        builder.Property(x => x.ReceivableAccountId)
            .IsRequired();

        builder.Property(x => x.ExpenseAccountId)
            .IsRequired();

        builder.Property(x => x.JournalEntryId);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ApprovalStatus)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(256);

        builder.Property(x => x.ApprovedDate);

        builder.Property(x => x.Reason)
            .HasMaxLength(512);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);
    }
}
