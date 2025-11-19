using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

internal sealed class PerformanceReviewConfiguration : IEntityTypeConfiguration<PerformanceReview>
{
    public void Configure(EntityTypeBuilder<PerformanceReview> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(r => r.Id);

        builder.Property(r => r.ReviewType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Strengths)
            .HasMaxLength(2000);

        builder.Property(r => r.AreasForImprovement)
            .HasMaxLength(2000);

        builder.Property(r => r.Goals)
            .HasMaxLength(2000);

        builder.Property(r => r.ReviewerComments)
            .HasMaxLength(2000);

        builder.Property(r => r.EmployeeComments)
            .HasMaxLength(2000);

        builder.Property(r => r.OverallRating)
            .HasPrecision(3, 2);

        builder.HasOne(r => r.Employee)
            .WithMany()
            .HasForeignKey(r => r.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Reviewer)
            .WithMany()
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(r => r.EmployeeId)
            .HasDatabaseName("IX_PerformanceReview_EmployeeId");

        builder.HasIndex(r => r.ReviewerId)
            .HasDatabaseName("IX_PerformanceReview_ReviewerId");

        builder.HasIndex(r => r.Status)
            .HasDatabaseName("IX_PerformanceReview_Status");

        builder.HasIndex(r => new { r.EmployeeId, r.ReviewPeriodStart, r.ReviewPeriodEnd })
            .HasDatabaseName("IX_PerformanceReview_Employee_Period");

        builder.HasIndex(r => new { r.ReviewerId, r.ReviewPeriodStart, r.ReviewPeriodEnd })
            .HasDatabaseName("IX_PerformanceReview_Reviewer_Period");
    }
}

