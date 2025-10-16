namespace Accounting.Infrastructure.Persistence.Configurations;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable("Vendors");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.VendorCode).IsRequired().HasMaxLength(64);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Address).HasMaxLength(512);
        builder.Property(x => x.ExpenseAccountCode).HasMaxLength(64);
        builder.Property(x => x.ExpenseAccountName).HasMaxLength(256);
        builder.Property(x => x.Tin).HasMaxLength(32);
        builder.Property(x => x.Description).HasMaxLength(1024);
        builder.Property(x => x.Notes).HasMaxLength(1024);
        builder.Property(x => x.PhoneNumber).HasMaxLength(32);
    }
}
