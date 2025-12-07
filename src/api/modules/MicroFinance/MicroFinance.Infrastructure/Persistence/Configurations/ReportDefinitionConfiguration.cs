namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the ReportDefinition entity.
/// </summary>
internal sealed class ReportDefinitionConfiguration : IEntityTypeConfiguration<ReportDefinition>
{
    public void Configure(EntityTypeBuilder<ReportDefinition> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(ReportDefinition.MaxLengths.Code);

        builder.Property(x => x.Category)
            .HasMaxLength(ReportDefinition.MaxLengths.Category);

        builder.Property(x => x.OutputFormat)
            .HasMaxLength(ReportDefinition.MaxLengths.OutputFormat);

        builder.Property(x => x.Query)
            .HasMaxLength(ReportDefinition.MaxLengths.Query);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.HasIndex(x => x.Status);
    }
}