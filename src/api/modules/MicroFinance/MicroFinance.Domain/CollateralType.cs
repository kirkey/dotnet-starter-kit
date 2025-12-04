using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a type/category of collateral.
/// </summary>
public sealed class CollateralType : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int NameMaxLength = 128;
    public const int CodeMaxLength = 32;
    public const int CategoryMaxLength = 64;
    public const int DescriptionMaxLength = 512;
    public const int StatusMaxLength = 32;
    
    // Collateral Categories
    public const string CategoryRealEstate = "RealEstate";
    public const string CategoryVehicle = "Vehicle";
    public const string CategoryEquipment = "Equipment";
    public const string CategoryInventory = "Inventory";
    public const string CategoryCash = "Cash";
    public const string CategorySecurities = "Securities";
    public const string CategoryReceivables = "Receivables";
    public const string CategoryOther = "Other";
    
    // Status
    public const string StatusActive = "Active";
    public const string StatusInactive = "Inactive";

    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public string Category { get; private set; } = default!;
    public string? Description { get; private set; }
    public string Status { get; private set; } = StatusActive;
    public decimal DefaultLtvPercent { get; private set; }
    public decimal MaxLtvPercent { get; private set; }
    public int DefaultUsefulLifeYears { get; private set; }
    public decimal AnnualDepreciationRate { get; private set; }
    public bool RequiresInsurance { get; private set; }
    public bool RequiresAppraisal { get; private set; }
    public int RevaluationFrequencyMonths { get; private set; }
    public bool RequiresRegistration { get; private set; }
    public string? RequiredDocuments { get; private set; }
    public string? Notes { get; private set; }
    public int DisplayOrder { get; private set; }

    private CollateralType() { }

    public static CollateralType Create(
        string name,
        string code,
        string category,
        decimal defaultLtvPercent,
        decimal maxLtvPercent,
        int defaultUsefulLifeYears = 10,
        decimal annualDepreciationRate = 10,
        string? description = null)
    {
        var collateralType = new CollateralType
        {
            Name = name,
            Code = code,
            Category = category,
            DefaultLtvPercent = defaultLtvPercent,
            MaxLtvPercent = maxLtvPercent,
            DefaultUsefulLifeYears = defaultUsefulLifeYears,
            AnnualDepreciationRate = annualDepreciationRate,
            Description = description,
            Status = StatusActive
        };

        collateralType.QueueDomainEvent(new CollateralTypeCreated(collateralType));
        return collateralType;
    }

    public CollateralType Activate()
    {
        Status = StatusActive;
        QueueDomainEvent(new CollateralTypeActivated(Id, Code));
        return this;
    }

    public CollateralType Deactivate()
    {
        Status = StatusInactive;
        QueueDomainEvent(new CollateralTypeDeactivated(Id, Code));
        return this;
    }

    public CollateralType Update(
        string? name = null,
        string? description = null,
        decimal? defaultLtvPercent = null,
        decimal? maxLtvPercent = null,
        int? defaultUsefulLifeYears = null,
        decimal? annualDepreciationRate = null,
        bool? requiresInsurance = null,
        bool? requiresAppraisal = null,
        int? revaluationFrequencyMonths = null,
        bool? requiresRegistration = null,
        string? requiredDocuments = null,
        string? notes = null,
        int? displayOrder = null)
    {
        if (name is not null) Name = name;
        if (description is not null) Description = description;
        if (defaultLtvPercent.HasValue) DefaultLtvPercent = defaultLtvPercent.Value;
        if (maxLtvPercent.HasValue) MaxLtvPercent = maxLtvPercent.Value;
        if (defaultUsefulLifeYears.HasValue) DefaultUsefulLifeYears = defaultUsefulLifeYears.Value;
        if (annualDepreciationRate.HasValue) AnnualDepreciationRate = annualDepreciationRate.Value;
        if (requiresInsurance.HasValue) RequiresInsurance = requiresInsurance.Value;
        if (requiresAppraisal.HasValue) RequiresAppraisal = requiresAppraisal.Value;
        if (revaluationFrequencyMonths.HasValue) RevaluationFrequencyMonths = revaluationFrequencyMonths.Value;
        if (requiresRegistration.HasValue) RequiresRegistration = requiresRegistration.Value;
        if (requiredDocuments is not null) RequiredDocuments = requiredDocuments;
        if (notes is not null) Notes = notes;
        if (displayOrder.HasValue) DisplayOrder = displayOrder.Value;

        QueueDomainEvent(new CollateralTypeUpdated(this));
        return this;
    }
}
