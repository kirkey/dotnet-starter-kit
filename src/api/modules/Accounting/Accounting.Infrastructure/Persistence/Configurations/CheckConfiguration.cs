namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for Check entity.
/// </summary>
public class CheckConfiguration : IEntityTypeConfiguration<Check>
{
    public void Configure(EntityTypeBuilder<Check> builder)
    {
        builder.ToTable("Checks");
        
        builder.HasKey(x => x.Id);
        
        // Required fields
        builder.Property(x => x.CheckNumber)
            .IsRequired()
            .HasMaxLength(64);
        
        builder.Property(x => x.BankAccountCode)
            .IsRequired()
            .HasMaxLength(64);
        
        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(32);
        
        // Optional fields with max lengths
        builder.Property(x => x.BankAccountName)
            .HasMaxLength(256);
        
        builder.Property(x => x.PayeeName)
            .HasMaxLength(256);
        
        builder.Property(x => x.Memo)
            .HasMaxLength(512);
        
        builder.Property(x => x.VoidReason)
            .HasMaxLength(512);
        
        builder.Property(x => x.StopPaymentReason)
            .HasMaxLength(512);
        
        builder.Property(x => x.PrintedBy)
            .HasMaxLength(256);
        
        builder.Property(x => x.Description)
            .HasMaxLength(1024);
        
        builder.Property(x => x.Notes)
            .HasMaxLength(1024);
        
        // Decimal precision
        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);
        
        // Indexes for performance
        builder.HasIndex(x => new { x.CheckNumber, x.BankAccountCode })
            .IsUnique()
            .HasDatabaseName("IX_Checks_CheckNumber_BankAccount");
        
        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_Checks_Status");
        
        builder.HasIndex(x => x.BankAccountCode)
            .HasDatabaseName("IX_Checks_BankAccountCode");
        
        builder.HasIndex(x => x.IssuedDate)
            .HasDatabaseName("IX_Checks_IssuedDate");
        
        builder.HasIndex(x => x.VendorId)
            .HasDatabaseName("IX_Checks_VendorId");
        
        builder.HasIndex(x => x.PayeeId)
            .HasDatabaseName("IX_Checks_PayeeId");
    }
}

