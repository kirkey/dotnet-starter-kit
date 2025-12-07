namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the UssdSession entity.
/// </summary>
internal sealed class UssdSessionConfiguration : IEntityTypeConfiguration<UssdSession>
{
    public void Configure(EntityTypeBuilder<UssdSession> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        // Indexes
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.WalletId);
        builder.HasIndex(x => x.Status);
    }
}