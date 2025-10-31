namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for InterCompanyTransaction entity.
/// </summary>
public class InterCompanyTransactionConfiguration : IEntityTypeConfiguration<InterCompanyTransaction>
{
    public void Configure(EntityTypeBuilder<InterCompanyTransaction> builder)
    {
        builder.ToTable("InterCompanyTransactions", SchemaNames.Accounting);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.TransactionNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.FromEntityName).IsRequired().HasMaxLength(256);
        builder.Property(x => x.ToEntityName).IsRequired().HasMaxLength(256);
        builder.Property(x => x.TransactionType).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(32);
        builder.Property(x => x.ReferenceNumber).HasMaxLength(100);
        builder.Property(x => x.ReconciledBy).HasMaxLength(256);
        builder.Property(x => x.TerminationReason).HasMaxLength(1000);
        builder.Property(x => x.Description).HasMaxLength(2048);
        builder.Property(x => x.Notes).HasMaxLength(2048);
        
        builder.Property(x => x.Amount).HasPrecision(18, 2);
        
        builder.HasIndex(x => x.TransactionNumber).IsUnique();
        builder.HasIndex(x => x.FromEntityId);
        builder.HasIndex(x => x.ToEntityId);
        builder.HasIndex(x => x.Status);
    }
}

