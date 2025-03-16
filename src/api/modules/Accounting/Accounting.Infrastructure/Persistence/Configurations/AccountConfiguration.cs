using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Code).IsUnique();
        
        builder.Property(x => x.AccountCategory)
            .HasMaxLength(16)
            .IsRequired();
        
        builder.Property(x => x.ParentCode)
            .HasMaxLength(16)
            .IsRequired();
        
        builder.Property(x => x.Code)
            .HasMaxLength(16)
            .IsRequired();
        
        builder.Property(x => x.Balance)
            .HasPrecision(18, 2)
            .IsRequired();
    }
}
