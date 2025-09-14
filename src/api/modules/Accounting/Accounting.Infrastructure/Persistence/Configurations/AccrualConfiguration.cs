using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class AccrualConfiguration : IEntityTypeConfiguration<Accounting.Domain.Accrual>
{
    public void Configure(EntityTypeBuilder<Accounting.Domain.Accrual> builder)
    {
        builder.ToTable("Accruals", SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.AccrualNumber).IsUnique();

        builder.Property(x => x.AccrualNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(200);

        builder.Property(x => x.AccrualDate)
            .IsRequired();

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.IsReversed)
            .IsRequired();

        builder.Property(x => x.ReversalDate);
    }
}

