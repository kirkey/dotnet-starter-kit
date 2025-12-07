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

        builder.Property(x => x.SessionId)
            .IsRequired()
            .HasMaxLength(UssdSession.SessionIdMaxLength);

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(UssdSession.PhoneNumberMaxLength);

        builder.Property(x => x.ServiceCode)
            .IsRequired()
            .HasMaxLength(UssdSession.ServiceCodeMaxLength);

        builder.Property(x => x.CurrentMenu)
            .HasMaxLength(UssdSession.CurrentMenuMaxLength);

        builder.Property(x => x.Language)
            .HasMaxLength(UssdSession.LanguageMaxLength);

        builder.Property(x => x.CurrentOperation)
            .HasMaxLength(UssdSession.OperationMaxLength);

        builder.Property(x => x.SessionData)
            .HasMaxLength(UssdSession.DataMaxLength);

        builder.Property(x => x.LastInput)
            .HasMaxLength(512);

        builder.Property(x => x.LastOutput)
            .HasMaxLength(2048);

        builder.Property(x => x.ErrorMessage)
            .HasMaxLength(1024);

        // Relationships
        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<MobileWallet>()
            .WithMany()
            .HasForeignKey(x => x.WalletId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.SessionId)
            .IsUnique()
            .HasDatabaseName("IX_UssdSessions_SessionId");

        builder.HasIndex(x => x.PhoneNumber)
            .HasDatabaseName("IX_UssdSessions_PhoneNumber");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_UssdSessions_MemberId");

        builder.HasIndex(x => x.WalletId)
            .HasDatabaseName("IX_UssdSessions_WalletId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_UssdSessions_Status");
    }
}
