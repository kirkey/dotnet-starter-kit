namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the ReportGeneration entity.
/// </summary>
internal sealed class ReportGenerationConfiguration : IEntityTypeConfiguration<ReportGeneration>
{
    public void Configure(EntityTypeBuilder<ReportGeneration> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.OutputFile)
            .HasMaxLength(ReportGeneration.MaxLengths.OutputFile);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.ErrorMessage)
            .HasMaxLength(ReportGeneration.MaxLengths.ErrorMessage);

        // Relationships
        builder.HasOne(x => x.ReportDefinition)
            .WithMany()
            .HasForeignKey(x => x.ReportDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.ReportDefinitionId);
        builder.HasIndex(x => x.RequestedByUserId);
        builder.HasIndex(x => x.BranchId);
        builder.HasIndex(x => x.Status);
    }
}