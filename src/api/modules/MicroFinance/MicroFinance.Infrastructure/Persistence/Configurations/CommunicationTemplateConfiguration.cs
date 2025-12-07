namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CommunicationTemplate entity.
/// </summary>
internal sealed class CommunicationTemplateConfiguration : IEntityTypeConfiguration<CommunicationTemplate>
{
    public void Configure(EntityTypeBuilder<CommunicationTemplate> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(CommunicationTemplate.MaxLengths.Code);

        builder.Property(x => x.Channel)
            .HasMaxLength(CommunicationTemplate.MaxLengths.Channel);

        builder.Property(x => x.Category)
            .HasMaxLength(CommunicationTemplate.MaxLengths.Category);

        builder.Property(x => x.Subject)
            .HasMaxLength(CommunicationTemplate.MaxLengths.Subject);

        builder.Property(x => x.Body)
            .HasMaxLength(CommunicationTemplate.MaxLengths.Body);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.HasIndex(x => x.Status);
    }
}