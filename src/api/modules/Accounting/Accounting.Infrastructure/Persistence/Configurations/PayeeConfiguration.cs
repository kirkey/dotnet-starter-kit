using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class PayeeConfiguration : IEntityTypeConfiguration<Payee>
{
    public void Configure(EntityTypeBuilder<Payee> builder)
    {
        builder.ToTable("Payees", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.PayeeCode).IsUnique();
        
        builder.Property(x => x.Name)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.Address)
            .HasMaxLength(1024);

        builder.Property(x => x.ExpenseAccountCode)
            .HasMaxLength(16);
        
        builder.Property(x => x.ExpenseAccountName)
            .HasMaxLength(64);
    }
}
