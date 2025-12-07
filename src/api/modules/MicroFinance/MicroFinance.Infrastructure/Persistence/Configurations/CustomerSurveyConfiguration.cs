namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CustomerSurvey entity.
/// </summary>
internal sealed class CustomerSurveyConfiguration : IEntityTypeConfiguration<CustomerSurvey>
{
    public void Configure(EntityTypeBuilder<CustomerSurvey> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.AverageScore)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.BranchId);
        builder.HasIndex(x => x.Status);
    }
}