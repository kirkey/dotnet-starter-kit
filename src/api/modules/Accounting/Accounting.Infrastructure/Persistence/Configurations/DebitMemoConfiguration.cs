namespace Accounting.Infrastructure.Persistence.Configurations;

public class DebitMemoConfiguration : IEntityTypeConfiguration<DebitMemo>
{
    public void Configure(EntityTypeBuilder<DebitMemo> builder)
    {
        builder.ToTable("DebitMemos", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.MemoNumber).IsUnique();

        builder.Property(x => x.MemoNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.MemoDate)
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.AppliedAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.UnappliedAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.ReferenceType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ReferenceId)
            .IsRequired();

        builder.Property(x => x.OriginalDocumentId);

        builder.Property(x => x.Reason)
            .HasMaxLength(512);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.IsApplied)
            .IsRequired();

        builder.Property(x => x.AppliedDate);

        builder.Property(x => x.ApprovalStatus)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(256);

        builder.Property(x => x.ApprovedDate);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);
    }
}
