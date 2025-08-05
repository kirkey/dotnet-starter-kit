using Accounting.Domain.Events.FixedAsset;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class FixedAsset : AuditableEntity, IAggregateRoot
{
    public string AssetName { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public decimal PurchasePrice { get; private set; }
    public int ServiceLife { get; private set; }
    public DefaultIdType DepreciationMethodId { get; private set; }
    public decimal SalvageValue { get; private set; }
    public decimal CurrentBookValue { get; private set; }
    public DefaultIdType AccumulatedDepreciationAccountId { get; private set; }
    public DefaultIdType DepreciationExpenseAccountId { get; private set; }
    public string? SerialNumber { get; private set; }
    public string? Location { get; private set; } // Physical location (e.g., GPS coordinates, substation name)
    public string? Department { get; private set; }
    public bool IsDisposed { get; private set; }
    public DateTime? DisposalDate { get; private set; }
    public decimal? DisposalAmount { get; private set; }
    
    // Power Company specific fields
    public string AssetType { get; private set; } // "Transformer", "Transmission Line", "Power Plant", "Vehicle"
    public string? GpsCoordinates { get; private set; } // "lat,lng" format
    public string? SubstationName { get; private set; }
    public DefaultIdType? AssetUsoaId { get; private set; } // Links to USOA account
    public string? RegulatoryClassification { get; private set; } // FERC classification
    public decimal? VoltageRating { get; private set; } // For electrical equipment
    public decimal? Capacity { get; private set; } // MW for generators, MVA for transformers
    public string? Manufacturer { get; private set; }
    public string? ModelNumber { get; private set; }
    public DateTime? LastMaintenanceDate { get; private set; }
    public DateTime? NextMaintenanceDate { get; private set; }
    public bool RequiresUsoaReporting { get; private set; }

    private readonly List<DepreciationEntry> _depreciationEntries = new();
    public IReadOnlyCollection<DepreciationEntry> DepreciationEntries => _depreciationEntries.AsReadOnly();

    private FixedAsset(string assetName, DateTime purchaseDate, decimal purchasePrice,
        DefaultIdType depreciationMethodId, int serviceLife, decimal salvageValue,
        DefaultIdType accumulatedDepreciationAccountId, DefaultIdType depreciationExpenseAccountId,
        string assetType, string? serialNumber = null, string? location = null, string? department = null,
        string? gpsCoordinates = null, string? substationName = null, DefaultIdType? assetUsoaId = null,
        string? regulatoryClassification = null, decimal? voltageRating = null, decimal? capacity = null,
        string? manufacturer = null, string? modelNumber = null, bool requiresUsoaReporting = true,
        string? description = null, string? notes = null)
    {
        AssetName = assetName.Trim();
        Name = assetName.Trim(); // Keep for compatibility
        PurchaseDate = purchaseDate;
        PurchasePrice = purchasePrice;
        DepreciationMethodId = depreciationMethodId;
        ServiceLife = serviceLife;
        SalvageValue = salvageValue;
        CurrentBookValue = purchasePrice;
        AccumulatedDepreciationAccountId = accumulatedDepreciationAccountId;
        DepreciationExpenseAccountId = depreciationExpenseAccountId;
        AssetType = assetType.Trim();
        SerialNumber = serialNumber?.Trim();
        Location = location?.Trim();
        Department = department?.Trim();
        IsDisposed = false;
        GpsCoordinates = gpsCoordinates?.Trim();
        SubstationName = substationName?.Trim();
        AssetUsoaId = assetUsoaId;
        RegulatoryClassification = regulatoryClassification?.Trim();
        VoltageRating = voltageRating;
        Capacity = capacity;
        Manufacturer = manufacturer?.Trim();
        ModelNumber = modelNumber?.Trim();
        RequiresUsoaReporting = requiresUsoaReporting;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new FixedAssetCreated(Id, AssetName, PurchaseDate, PurchasePrice, AssetType, Description, Notes));
    }

    public static FixedAsset Create(string assetName, DateTime purchaseDate, decimal purchasePrice,
        DefaultIdType depreciationMethodId, int serviceLife, decimal salvageValue,
        DefaultIdType accumulatedDepreciationAccountId, DefaultIdType depreciationExpenseAccountId,
        string assetType, string? serialNumber = null, string? location = null, string? department = null,
        string? gpsCoordinates = null, string? substationName = null, DefaultIdType? assetUsoaId = null,
        string? regulatoryClassification = null, decimal? voltageRating = null, decimal? capacity = null,
        string? manufacturer = null, string? modelNumber = null, bool requiresUsoaReporting = true,
        string? description = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(assetName))
            throw new ArgumentException("Asset name cannot be empty");

        if (purchasePrice <= 0)
            throw new InvalidAssetPurchasePriceException();

        if (serviceLife <= 0)
            throw new InvalidAssetServiceLifeException();

        if (salvageValue < 0 || salvageValue > purchasePrice)
            throw new InvalidAssetSalvageValueException();

        if (!IsValidAssetType(assetType))
            throw new ArgumentException($"Invalid asset type: {assetType}");

        return new FixedAsset(assetName, purchaseDate, purchasePrice,
            depreciationMethodId, serviceLife, salvageValue, accumulatedDepreciationAccountId,
            depreciationExpenseAccountId, assetType, serialNumber, location, department,
            gpsCoordinates, substationName, assetUsoaId, regulatoryClassification, voltageRating,
            capacity, manufacturer, modelNumber, requiresUsoaReporting, description, notes);
    }

    public FixedAsset Update(string? assetName = null, string? location = null, string? department = null,
        string? serialNumber = null, string? gpsCoordinates = null, string? substationName = null,
        string? regulatoryClassification = null, decimal? voltageRating = null, decimal? capacity = null,
        string? manufacturer = null, string? modelNumber = null, bool? requiresUsoaReporting = null,
        string? description = null, string? notes = null)
    {
        if (IsDisposed)
            throw new FixedAssetCannotBeModifiedException(Id);

        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(assetName) && AssetName != assetName.Trim())
        {
            AssetName = assetName.Trim();
            Name = AssetName; // Keep for compatibility
            isUpdated = true;
        }

        if (location != Location)
        {
            Location = location?.Trim();
            isUpdated = true;
        }

        if (department != Department)
        {
            Department = department?.Trim();
            isUpdated = true;
        }

        if (serialNumber != SerialNumber)
        {
            SerialNumber = serialNumber?.Trim();
            isUpdated = true;
        }

        if (gpsCoordinates != GpsCoordinates)
        {
            GpsCoordinates = gpsCoordinates?.Trim();
            isUpdated = true;
        }

        if (substationName != SubstationName)
        {
            SubstationName = substationName?.Trim();
            isUpdated = true;
        }

        if (regulatoryClassification != RegulatoryClassification)
        {
            RegulatoryClassification = regulatoryClassification?.Trim();
            isUpdated = true;
        }

        if (voltageRating != VoltageRating)
        {
            VoltageRating = voltageRating;
            isUpdated = true;
        }

        if (capacity != Capacity)
        {
            Capacity = capacity;
            isUpdated = true;
        }

        if (manufacturer != Manufacturer)
        {
            Manufacturer = manufacturer?.Trim();
            isUpdated = true;
        }

        if (modelNumber != ModelNumber)
        {
            ModelNumber = modelNumber?.Trim();
            isUpdated = true;
        }

        if (requiresUsoaReporting.HasValue && RequiresUsoaReporting != requiresUsoaReporting.Value)
        {
            RequiresUsoaReporting = requiresUsoaReporting.Value;
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new FixedAssetUpdated(Id, AssetName, AssetType, Description, Notes));
        }

        return this;
    }

    public FixedAsset UpdateMaintenance(DateTime? lastMaintenanceDate, DateTime? nextMaintenanceDate)
    {
        bool isUpdated = false;

        if (lastMaintenanceDate != LastMaintenanceDate)
        {
            LastMaintenanceDate = lastMaintenanceDate;
            isUpdated = true;
        }

        if (nextMaintenanceDate != NextMaintenanceDate)
        {
            NextMaintenanceDate = nextMaintenanceDate;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new FixedAssetMaintenanceUpdated(Id, AssetName, LastMaintenanceDate, NextMaintenanceDate));
        }

        return this;
    }

    public FixedAsset AddDepreciation(decimal depreciationAmount, DateTime depreciationDate, string? method = null)
    {
        if (IsDisposed)
            throw new FixedAssetCannotBeModifiedException(Id);

        if (depreciationAmount <= 0)
            throw new InvalidDepreciationAmountException();

        var entry = DepreciationEntry.Create(Id, depreciationAmount, depreciationDate, method);
        _depreciationEntries.Add(entry);

        CurrentBookValue = Math.Max(0, CurrentBookValue - depreciationAmount);

        QueueDomainEvent(new FixedAssetDepreciationAdded(Id, AssetName, depreciationAmount, CurrentBookValue));
        return this;
    }

    public FixedAsset Dispose(DateTime disposalDate, decimal? disposalAmount = null, string? disposalReason = null)
    {
        if (IsDisposed)
            throw new FixedAssetAlreadyDisposedException(Id);

        IsDisposed = true;
        DisposalDate = disposalDate;
        DisposalAmount = disposalAmount;

        QueueDomainEvent(new FixedAssetDisposed(Id, AssetName, disposalDate, disposalAmount, disposalReason));
        return this;
    }

    private static bool IsValidAssetType(string assetType)
    {
        var validTypes = new[] { "Transformer", "Transmission Line", "Distribution Line", "Power Plant", 
            "Generator", "Substation", "Meter", "Vehicle", "Building", "Equipment", "Software", "Land" };
        return validTypes.Contains(assetType.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}

public class DepreciationEntry : BaseEntity
{
    public DefaultIdType FixedAssetId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string? Method { get; private set; }

    private DepreciationEntry(DefaultIdType fixedAssetId, decimal amount, DateTime date, string? method = null)
    {
        FixedAssetId = fixedAssetId;
        Amount = amount;
        Date = date;
        Method = method?.Trim();
    }

    public static DepreciationEntry Create(DefaultIdType fixedAssetId, decimal amount, DateTime date, string? method = null)
    {
        if (amount <= 0)
            throw new InvalidDepreciationAmountException();

        return new DepreciationEntry(fixedAssetId, amount, date, method);
    }
}
