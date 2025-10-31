namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for InterconnectionAgreement entity.
/// </summary>
public class InterconnectionAgreementConfiguration : IEntityTypeConfiguration<InterconnectionAgreement>
{
    public void Configure(EntityTypeBuilder<InterconnectionAgreement> builder)
    {
        builder.ToTable("InterconnectionAgreements", SchemaNames.Accounting);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.AgreementNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.GenerationType).IsRequired().HasMaxLength(50);
        builder.Property(x => x.AgreementStatus).IsRequired().HasMaxLength(32);
        builder.Property(x => x.InverterManufacturer).HasMaxLength(100);
        builder.Property(x => x.InverterModel).HasMaxLength(100);
        builder.Property(x => x.PanelManufacturer).HasMaxLength(100);
        builder.Property(x => x.PanelModel).HasMaxLength(100);
        builder.Property(x => x.TerminationReason).HasMaxLength(500);
        builder.Property(x => x.Description).HasMaxLength(2048);
        builder.Property(x => x.Notes).HasMaxLength(2048);
        
        builder.Property(x => x.InstalledCapacityKW).HasPrecision(18, 4);
        builder.Property(x => x.NetMeteringRate).HasPrecision(18, 6);
        builder.Property(x => x.ExcessGenerationRate).HasPrecision(18, 6);
        builder.Property(x => x.MonthlyServiceCharge).HasPrecision(18, 2);
        builder.Property(x => x.CurrentCreditBalance).HasPrecision(18, 2);
        builder.Property(x => x.AnnualGenerationLimit).HasPrecision(18, 2);
        builder.Property(x => x.YearToDateGeneration).HasPrecision(18, 2);
        builder.Property(x => x.LifetimeGeneration).HasPrecision(18, 2);
        builder.Property(x => x.InterconnectionFee).HasPrecision(18, 2);
        builder.Property(x => x.DepositAmount).HasPrecision(18, 2);
        
        builder.HasIndex(x => x.AgreementNumber).IsUnique();
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.AgreementStatus);
    }
}

