namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CollectionAction entity.
/// </summary>
internal sealed class CollectionActionConfiguration : IEntityTypeConfiguration<CollectionAction>
{
    public void Configure(EntityTypeBuilder<CollectionAction> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PromisedAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Latitude)
            .HasPrecision(18, 2);

        builder.Property(x => x.Longitude)
            .HasPrecision(18, 2);

        builder.Property(x => x.ActionType)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);
        
        builder.Property(x => x.Notes)
            .HasMaxLength(4096);

        builder.Property(x => x.Outcome)
            .HasMaxLength(64);

        // Relationships
        builder.HasOne(x => x.CollectionCase)
            .WithMany(x => x.CollectionActions)
            .HasForeignKey(x => x.CollectionCaseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Loan>()
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Staff>()
            .WithMany()
            .HasForeignKey(x => x.PerformedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.CollectionCaseId)
            .HasDatabaseName("IX_CollectionActions_CollectionCaseId");

        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_CollectionActions_LoanId");

        builder.HasIndex(x => x.PerformedById)
            .HasDatabaseName("IX_CollectionActions_PerformedById");

        builder.HasIndex(x => x.ActionDateTime)
            .HasDatabaseName("IX_CollectionActions_ActionDateTime");

        builder.HasIndex(x => x.ActionType)
            .HasDatabaseName("IX_CollectionActions_ActionType");
    }
}
