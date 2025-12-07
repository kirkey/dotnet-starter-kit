namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the ApprovalWorkflow entity.
/// </summary>
internal sealed class ApprovalWorkflowConfiguration : IEntityTypeConfiguration<ApprovalWorkflow>
{
    public void Configure(EntityTypeBuilder<ApprovalWorkflow> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(ApprovalWorkflowConstants.CodeMaxLength);


        builder.Property(x => x.EntityType)
            .HasMaxLength(ApprovalWorkflowConstants.EntityTypeMaxLength);

        builder.Property(x => x.MinAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.MaxAmount)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.EntityType);
        builder.HasIndex(x => x.IsActive);
    }
}
