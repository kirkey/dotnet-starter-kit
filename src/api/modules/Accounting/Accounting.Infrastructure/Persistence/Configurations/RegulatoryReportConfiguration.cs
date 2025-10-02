using Accounting.Domain.Entities;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class RegulatoryReportConfiguration : IEntityTypeConfiguration<RegulatoryReport>
{
    public void Configure(EntityTypeBuilder<RegulatoryReport> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.ReportName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.ReportType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.ReportingPeriod)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(r => r.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("Draft");

        builder.Property(r => r.RegulatoryBody)
            .HasMaxLength(100);

        builder.Property(r => r.FilingNumber)
            .HasMaxLength(50);

        builder.Property(r => r.FilePath)
            .HasMaxLength(500);

        builder.Property(r => r.PreparedBy)
            .HasMaxLength(100);

        builder.Property(r => r.ReviewedBy)
            .HasMaxLength(100);

        builder.Property(r => r.ApprovedBy)
            .HasMaxLength(100);

        builder.Property(r => r.AuditFirm)
            .HasMaxLength(200);

        // Decimal properties with precision
        builder.Property(r => r.TotalAssets)
            .HasPrecision(18, 2);

        builder.Property(r => r.TotalLiabilities)
            .HasPrecision(18, 2);

        builder.Property(r => r.TotalEquity)
            .HasPrecision(18, 2);

        builder.Property(r => r.TotalRevenue)
            .HasPrecision(18, 2);

        builder.Property(r => r.TotalExpenses)
            .HasPrecision(18, 2);

        builder.Property(r => r.NetIncome)
            .HasPrecision(18, 2);

        builder.Property(r => r.RateBase)
            .HasPrecision(18, 2);

        builder.Property(r => r.AllowedReturn)
            .HasPrecision(18, 2);

        // Indexes for common queries
        builder.HasIndex(r => r.ReportType);
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => r.DueDate);
        builder.HasIndex(r => new { r.PeriodStartDate, r.PeriodEndDate });
        builder.HasIndex(r => r.RegulatoryBody);

        // Unique constraint on report name per tenant
        builder.HasIndex(r => r.ReportName)
            .IsUnique();
    }
}
