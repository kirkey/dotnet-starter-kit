namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CustomerCase entity.
/// </summary>
internal sealed class CustomerCaseConfiguration : IEntityTypeConfiguration<CustomerCase>
{
    public void Configure(EntityTypeBuilder<CustomerCase> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        // Indexes
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.AssignedToId);
        builder.HasIndex(x => x.RelatedLoanId);
        builder.HasIndex(x => x.RelatedSavingsAccountId);
        builder.HasIndex(x => x.EscalatedToId);
        builder.HasIndex(x => x.Status);
    }
}