namespace FSH.Starter.WebApi.Catalog.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the Brand entity.
/// Defines table schema, column constraints, indexes, and relationships.
/// </summary>
internal sealed class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    /// <summary>
    /// Configures the Brand entity mapping.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        
        // Configure string properties with lengths from domain constants
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Brand.NameMaxLength);
        
        builder.Property(x => x.Description)
            .HasMaxLength(Brand.DescriptionMaxLength);
        
        builder.Property(x => x.Notes)
            .HasMaxLength(Brand.NotesMaxLength);
        
        // Add indexes for frequently queried fields
        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_Brands_Name");
    }
}
