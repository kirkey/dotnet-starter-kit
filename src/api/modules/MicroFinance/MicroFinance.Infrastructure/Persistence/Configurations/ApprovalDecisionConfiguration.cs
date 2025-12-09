namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the ApprovalDecision entity.
/// </summary>
internal sealed class ApprovalDecisionConfiguration : IEntityTypeConfiguration<ApprovalDecision>
{
    public void Configure(EntityTypeBuilder<ApprovalDecision> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Decision)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Comments)
            .HasMaxLength(2048);

        builder.Property(x => x.DecisionAt)
            .IsRequired();

        // Relationships
        builder.HasOne(x => x.Request)
            .WithMany(x => x.Decisions)
            .HasForeignKey(x => x.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => x.RequestId)
            .HasDatabaseName("IX_ApprovalDecisions_RequestId");

        builder.HasIndex(x => new { x.RequestId, x.Level })
            .HasDatabaseName("IX_ApprovalDecisions_Request_Level");

        builder.HasIndex(x => x.ApproverId)
            .HasDatabaseName("IX_ApprovalDecisions_ApproverId");

        builder.HasIndex(x => x.DecisionAt)
            .HasDatabaseName("IX_ApprovalDecisions_DecisionAt");
    }
}
