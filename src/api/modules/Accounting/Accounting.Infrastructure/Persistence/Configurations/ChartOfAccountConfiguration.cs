using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class ChartOfAccountConfiguration : IEntityTypeConfiguration<ChartOfAccount>
{
    public void Configure(EntityTypeBuilder<ChartOfAccount> builder)
    {
        builder.ToTable("ChartOfAccounts", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.AccountCode).IsUnique();

        builder.Property(x => x.AccountCode)
            .HasMaxLength(16)
            .IsRequired();

        // AccountName / Name mapping
        builder.Property(x => x.AccountName)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.AccountType)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.ParentCode)
            .HasMaxLength(16);

        builder.Property(x => x.UsoaCategory)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(x => x.Balance)
            .HasPrecision(18, 2)
            .IsRequired();

        // Additional mappings
        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.NormalBalance)
            .HasMaxLength(8);

        builder.Property(x => x.RegulatoryClassification)
            .HasMaxLength(256);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);
    }
}
