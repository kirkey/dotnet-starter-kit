namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the ShareAccount entity.
/// </summary>
internal sealed class ShareAccountConfiguration : IEntityTypeConfiguration<ShareAccount>
{
    public void Configure(EntityTypeBuilder<ShareAccount> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountNumber)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.Status)
            .HasMaxLength(32);


        // Relationships
        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ShareProduct>()
            .WithMany()
            .HasForeignKey(x => x.ShareProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.AccountNumber)
            .IsUnique()
            .HasDatabaseName("IX_ShareAccounts_AccountNumber");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_ShareAccounts_MemberId");

        builder.HasIndex(x => x.ShareProductId)
            .HasDatabaseName("IX_ShareAccounts_ShareProductId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_ShareAccounts_Status");
    }
}

