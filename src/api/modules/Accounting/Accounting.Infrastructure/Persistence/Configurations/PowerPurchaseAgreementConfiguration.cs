namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for PowerPurchaseAgreement entity.
/// </summary>
public class PowerPurchaseAgreementConfiguration : IEntityTypeConfiguration<PowerPurchaseAgreement>
{
    public void Configure(EntityTypeBuilder<PowerPurchaseAgreement> builder)
    {
        builder.ToTable("PowerPurchaseAgreements", SchemaNames.Accounting);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ContractNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.CounterpartyName).IsRequired().HasMaxLength(256);
        builder.Property(x => x.ContractType).IsRequired().HasMaxLength(32);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(32);
        builder.Property(x => x.SettlementFrequency).IsRequired().HasMaxLength(32);
        builder.Property(x => x.EnergySource).HasMaxLength(100);
        builder.Property(x => x.TerminationReason).HasMaxLength(1000);
        builder.Property(x => x.Description).HasMaxLength(2048);
        builder.Property(x => x.Notes).HasMaxLength(2048);
        
        builder.Property(x => x.EnergyPricePerKWh).HasPrecision(18, 6);
        builder.Property(x => x.DemandChargePerKw).HasPrecision(18, 2);
        builder.Property(x => x.MinimumPurchaseKWh).HasPrecision(18, 2);
        builder.Property(x => x.MaximumPurchaseKWh).HasPrecision(18, 2);
        builder.Property(x => x.MonthlySettlementAmount).HasPrecision(18, 2);
        builder.Property(x => x.YearToDateCost).HasPrecision(18, 2);
        builder.Property(x => x.LifetimeCost).HasPrecision(18, 2);
        builder.Property(x => x.YearToDateEnergyKWh).HasPrecision(18, 2);
        builder.Property(x => x.LifetimeEnergyKWh).HasPrecision(18, 2);
        builder.Property(x => x.ContractCapacityMw).HasPrecision(18, 4);
        builder.Property(x => x.EscalationRate).HasPrecision(5, 4);
        
        builder.HasIndex(x => x.ContractNumber).IsUnique();
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.EndDate);
    }
}

