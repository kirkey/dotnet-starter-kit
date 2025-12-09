namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the ApprovalLevel entity.
/// </summary>
internal sealed class ApprovalLevelConfiguration : IEntityTypeConfiguration<ApprovalLevel>
{
    public void Configure(EntityTypeBuilder<ApprovalLevel> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RequiredRole)
            .HasMaxLength(128);

        builder.Property(x => x.MaxApprovalAmount)
            .HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.Workflow)
            .WithMany(x => x.ApprovalLevels)
            .HasForeignKey(x => x.WorkflowId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => x.WorkflowId)
            .HasDatabaseName("IX_ApprovalLevels_WorkflowId");

        builder.HasIndex(x => new { x.WorkflowId, x.LevelNumber })
            .IsUnique()
            .HasDatabaseName("IX_ApprovalLevels_Workflow_Level");
    }
}
