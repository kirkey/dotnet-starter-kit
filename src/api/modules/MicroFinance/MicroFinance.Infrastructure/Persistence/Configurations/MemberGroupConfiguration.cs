namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the MemberGroup entity.
/// </summary>
internal sealed class MemberGroupConfiguration : IEntityTypeConfiguration<MemberGroup>
{
    public void Configure(EntityTypeBuilder<MemberGroup> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(256);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Status)
            .HasMaxLength(32);

        // Indexes
        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_MemberGroups_Status");
    }
}

