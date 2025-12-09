namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the ApprovalRequest entity.
/// </summary>
internal sealed class ApprovalRequestConfiguration : IEntityTypeConfiguration<ApprovalRequest>
{
    public void Configure(EntityTypeBuilder<ApprovalRequest> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.RequestNumber)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.EntityType)
            .IsRequired()
            .HasMaxLength(64);

        // Relationships
        builder.HasOne(x => x.Workflow)
            .WithMany()
            .HasForeignKey(x => x.WorkflowId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Decisions)
            .WithOne(x => x.Request)
            .HasForeignKey(x => x.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => x.RequestNumber)
            .IsUnique()
            .HasDatabaseName("IX_ApprovalRequests_RequestNumber");

        builder.HasIndex(x => x.WorkflowId)
            .HasDatabaseName("IX_ApprovalRequests_WorkflowId");

        builder.HasIndex(x => new { x.EntityType, x.EntityId })
            .HasDatabaseName("IX_ApprovalRequests_Entity");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_ApprovalRequests_Status");
    }
}
