namespace FSH.Starter.WebApi.Catalog.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the Product entity.
/// Defines table schema, column constraints, indexes, and relationships.
/// </summary>
internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configures the Product entity mapping.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        
        // Configure string properties with lengths from domain constants
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Product.NameMaxLength);
        
        builder.Property(x => x.Description)
            .HasMaxLength(Product.DescriptionMaxLength);
        
        // Configure decimal with precision
        builder.Property(x => x.Price)
            .IsRequired()
            .HasPrecision(18, 2);
        
        // Configure Brand relationship
        builder.HasOne(x => x.Brand)
            .WithMany()
            .HasForeignKey(x => x.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Add indexes for frequently queried fields
        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_Products_Name");
        
        builder.HasIndex(x => x.BrandId)
            .HasDatabaseName("IX_Products_BrandId");
        
        builder.HasIndex(x => x.Price)
            .HasDatabaseName("IX_Products_Price");
    }
}
