namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the Bank entity.
/// Defines database schema, constraints, and indexes.
/// </summary>
public class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    /// <summary>
    /// Configures the Bank entity for the database.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.ToTable("Banks", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        // Unique indexes for business keys
        builder.HasIndex(x => x.BankCode).IsUnique();
        builder.HasIndex(x => x.RoutingNumber).IsUnique().HasFilter("[RoutingNumber] IS NOT NULL");

        // BankCode configuration
        builder.Property(x => x.BankCode)
            .HasMaxLength(16)
            .IsRequired();

        // Name configuration
        builder.Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        // RoutingNumber configuration
        builder.Property(x => x.RoutingNumber)
            .HasMaxLength(9);

        // SwiftCode configuration
        builder.Property(x => x.SwiftCode)
            .HasMaxLength(11);

        // Address configuration
        builder.Property(x => x.Address)
            .HasMaxLength(512);

        // ContactPerson configuration
        builder.Property(x => x.ContactPerson)
            .HasMaxLength(128);

        // PhoneNumber configuration
        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(32);

        // Email configuration
        builder.Property(x => x.Email)
            .HasMaxLength(128);

        // Website configuration
        builder.Property(x => x.Website)
            .HasMaxLength(256);

        // Description configuration
        builder.Property(x => x.Description)
            .HasMaxLength(1024);

        // Notes configuration
        builder.Property(x => x.Notes)
            .HasMaxLength(2048);

        // ImageUrl configuration
        builder.Property(x => x.ImageUrl)
            .HasMaxLength(512);

        // IsActive configuration
        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
    }
}

