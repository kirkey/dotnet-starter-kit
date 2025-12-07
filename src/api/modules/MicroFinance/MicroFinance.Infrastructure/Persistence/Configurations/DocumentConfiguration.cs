namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the Document entity.
/// </summary>
internal sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(128);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        // Indexes
        builder.HasIndex(x => x.EntityId);
        builder.HasIndex(x => x.VerifiedById);
        builder.HasIndex(x => x.Status);
    }
}