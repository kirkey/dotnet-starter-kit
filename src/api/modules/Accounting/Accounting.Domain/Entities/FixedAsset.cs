using Accounting.Domain.Constants;
using Accounting.Domain.Events.FixedAsset;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a fixed asset tracked for depreciation, maintenance, regulatory reporting, and lifecycle management.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track capital assets for financial reporting and regulatory compliance (FERC, tax reporting).
/// - Calculate depreciation using various methods (straight-line, declining balance, etc.).
/// - Manage asset lifecycle from acquisition through disposal with proper accounting.
/// - Support regulatory asset tracking for utility rate-making and cost recovery.
/// - Enable maintenance scheduling and asset condition monitoring.
/// - Track asset locations and transfers between departments or facilities.
/// - Support insurance valuation and risk management reporting.
/// - Generate regulatory reports for plant in service and accumulated depreciation.
/// 
/// Default values:
/// - AssetName: required descriptive name (example: "Distribution Transformer - Station A")
/// - PurchaseDate: required acquisition date (example: 2025-03-15)
/// - PurchasePrice: required acquisition cost (example: 25000.00)
/// - UsefulLife: required useful life in years (example: 25 for utility equipment)
/// - SalvageValue: estimated residual value (example: 2500.00 or 0.00)
/// - CurrentBookValue: initially equals PurchasePrice, reduced by depreciation
/// - IsDisposed: false (asset starts as active)
/// - DisposalDate: null (set when asset is disposed)
/// - DisposalProceeds: null (set when asset is sold/disposed)
/// - DepreciationMethodId: required reference to depreciation calculation method
/// - AssetAccountId: required GL account for asset cost tracking
/// - AccumulatedDepreciationAccountId: required GL account for depreciation accumulation
/// 
/// Business rules:
/// - PurchasePrice must be positive
/// - UsefulLife must be greater than zero
/// - SalvageValue cannot exceed PurchasePrice
/// - CurrentBookValue cannot be negative (minimum zero after full depreciation)
/// - Cannot dispose asset with negative book value without proper authorization
/// - Depreciation stops when book value reaches salvage value
/// - Asset transfers require proper approval and GL account updates
/// - Disposal gains/losses must be properly recorded
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.FixedAsset.FixedAssetCreated"/>
/// <seealso cref="Accounting.Domain.Events.FixedAsset.FixedAssetUpdated"/>
/// <seealso cref="Accounting.Domain.Events.FixedAsset.FixedAssetDepreciated"/>
/// <seealso cref="Accounting.Domain.Events.FixedAsset.FixedAssetDisposed"/>
/// <seealso cref="Accounting.Domain.Events.FixedAsset.FixedAssetTransferred"/>
/// <seealso cref="Accounting.Domain.Events.FixedAsset.FixedAssetRevalued"/>
public class FixedAsset : AuditableEntityWithApproval, IAggregateRoot
{
    /// <summary>
    /// Human-readable name of the asset.
    /// </summary>
    public string AssetName { get; private set; }

    /// <summary>
    /// Date the asset was acquired.
    /// </summary>
    public DateTime PurchaseDate { get; private set; }

    /// <summary>
    /// Acquisition cost of the asset.
    /// </summary>
    public decimal PurchasePrice { get; private set; }

    /// <summary>
    /// Useful life in periods (typically years) used for depreciation.
    /// </summary>
    public int ServiceLife { get; private set; }

    /// <summary>
    /// Reference to the depreciation method used for this asset.
    /// </summary>
    public DefaultIdType DepreciationMethodId { get; private set; }

    /// <summary>
    /// Expected residual value at end of life.
    /// </summary>
    public decimal SalvageValue { get; private set; }

    /// <summary>
    /// Current net book value; starts at purchase price and reduces with depreciation.
    /// </summary>
    public decimal CurrentBookValue { get; private set; }

    /// <summary>
    /// Account to post accumulated depreciation.
    /// </summary>
    public DefaultIdType AccumulatedDepreciationAccountId { get; private set; }

    /// <summary>
    /// Account to post periodic depreciation expense.
    /// </summary>
    public DefaultIdType DepreciationExpenseAccountId { get; private set; }

    /// <summary>
    /// Optional serial number for the asset.
    /// </summary>
    public string? SerialNumber { get; private set; }

    /// <summary>
    /// Physical location description (e.g., facility or coordinates).
    /// </summary>
    public string? Location { get; private set; } // Physical location (e.g., GPS coordinates, substation name)

    /// <summary>
    /// Owning or responsible department.
    /// </summary>
    public string? Department { get; private set; }

    /// <summary>
    /// Whether the asset has been disposed.
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// When the asset was disposed, if applicable.
    /// </summary>
    public DateTime? DisposalDate { get; private set; }

    /// <summary>
    /// Amount received or recorded upon disposal, if any.
    /// </summary>
    public decimal? DisposalAmount { get; private set; }
    
    // Power Company-specific fields
    /// <summary>
    /// Asset category such as Transformer, Transmission Line, Power Plant, Vehicle.
    /// </summary>
    public string AssetType { get; private set; } // "Transformer", "Transmission Line", "Power Plant", "Vehicle"

    /// <summary>
    /// Geographic coordinates in "lat,lng" format.
    /// </summary>
    public string? GpsCoordinates { get; private set; } // "lat,lng" format

    /// <summary>
    /// Substation name where the asset is located, if relevant.
    /// </summary>
    public string? SubstationName { get; private set; }

    /// <summary>
    /// Link to USOA account classification for the asset.
    /// </summary>
    public DefaultIdType? AssetUsoaId { get; private set; } // Links to USOA account

    /// <summary>
    /// Regulatory classification such as FERC category text.
    /// </summary>
    public string? RegulatoryClassification { get; private set; } // FERC classification

    /// <summary>
    /// Voltage rating for electrical equipment.
    /// </summary>
    public decimal? VoltageRating { get; private set; } // For electrical equipment

    /// <summary>
    /// Capacity (e.g., MW for generators, MVA for transformers).
    /// </summary>
    public decimal? Capacity { get; private set; } // MW for generators, MVA for transformers

    /// <summary>
    /// Manufacturer name.
    /// </summary>
    public string? Manufacturer { get; private set; }

    /// <summary>
    /// Manufacturer model number.
    /// </summary>
    public string? ModelNumber { get; private set; }

    /// <summary>
    /// Date of the last maintenance performed.
    /// </summary>
    public DateTime? LastMaintenanceDate { get; private set; }

    /// <summary>
    /// Scheduled date for the next maintenance activity.
    /// </summary>
    public DateTime? NextMaintenanceDate { get; private set; }

    /// <summary>
    /// Whether this asset requires USOA reporting in regulatory filings.
    /// </summary>
    public bool RequiresUsoaReporting { get; private set; }

    private readonly List<DepreciationEntry> _depreciationEntries = new();
    /// <summary>
    /// Collection of depreciation entries that reduce book value over time.
    /// </summary>
    public IReadOnlyCollection<DepreciationEntry> DepreciationEntries => _depreciationEntries.AsReadOnly();
    
    private FixedAsset()
    {
        AssetName = string.Empty;
        PurchasePrice = 0;
        SalvageValue = 0;
        CurrentBookValue = 0;
        AssetType = string.Empty;
    }

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
        PurchaseDate = purchaseDate;
        PurchasePrice = purchasePrice;
        DepreciationMethodId = depreciationMethodId;
        ServiceLife = serviceLife;
        SalvageValue = salvageValue;
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
        CurrentBookValue = purchasePrice;
        QueueDomainEvent(new FixedAssetCreated(Id, AssetName, PurchaseDate, PurchasePrice, AssetType, Description, Notes));
    }

    /// <summary>
    /// Factory to create a fixed asset with validation for purchase price, service life, salvage value, and asset type.
    /// </summary>
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
            throw new ArgumentException($"Invalid asset type: {assetType}. Allowed: {FixedAssetTypes.AsDisplayList()}");

        return new FixedAsset(assetName, purchaseDate, purchasePrice,
            depreciationMethodId, serviceLife, salvageValue, accumulatedDepreciationAccountId,
            depreciationExpenseAccountId, assetType, serialNumber, location, department,
            gpsCoordinates, substationName, assetUsoaId, regulatoryClassification, voltageRating,
            capacity, manufacturer, modelNumber, requiresUsoaReporting, description, notes);
    }

    /// <summary>
    /// Update metadata fields (not financial) when the asset is not disposed.
    /// </summary>
    public FixedAsset Update(string? assetName = null, string? location = null, string? department = null,
        string? serialNumber = null, string? gpsCoordinates = null, string? substationName = null,
        string? regulatoryClassification = null, decimal? voltageRating = null, decimal? capacity = null,
        string? manufacturer = null, string? modelNumber = null, bool requiresUsoaReporting = false,
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

        if (RequiresUsoaReporting != requiresUsoaReporting)
        {
            RequiresUsoaReporting = requiresUsoaReporting;
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

    /// <summary>
    /// Update maintenance dates metadata.
    /// </summary>
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

    /// <summary>
    /// Add a depreciation entry and reduce the current book value by the amount.
    /// </summary>
    public FixedAsset AddDepreciation(decimal depreciationAmount, DateTime depreciationDate, string? method = null)
    {
        if (IsDisposed)
            throw new FixedAssetCannotBeModifiedException(Id);

        if (depreciationAmount <= 0)
            throw new InvalidDepreciationAmountException();

        var entry = DepreciationEntry.Create(Id, depreciationAmount, depreciationDate, method);
        _depreciationEntries.Add(entry);

        // Fix: CurrentBookValue is Money, so subtract using Money
        CurrentBookValue = Math.Max(0, CurrentBookValue - depreciationAmount);

        QueueDomainEvent(new FixedAssetDepreciationAdded(Id, AssetName, depreciationAmount, CurrentBookValue));
        return this;
    }

    /// <summary>
    /// Mark the asset as disposed and set disposal metadata.
    /// </summary>
    public FixedAsset MarkAsDisposed(DateTime disposalDate, decimal? disposalAmount = null, string? disposalReason = null)
    {
        if (IsDisposed)
            throw new FixedAssetAlreadyDisposedException(Id);

        IsDisposed = true;
        DisposalDate = disposalDate;
        DisposalAmount = disposalAmount;

        QueueDomainEvent(new FixedAssetDisposed(Id, AssetName, DisposalDate!.Value, DisposalAmount, disposalReason));
        return this;
    }

    /// <summary>
    /// Approve the fixed asset acquisition.
    /// </summary>
    /// <param name="approverId">User ID of the person approving the asset.</param>
    /// <param name="approverName">Optional name/email of the approver for display purposes.</param>
    /// <exception cref="FixedAssetAlreadyApprovedException">Thrown if the asset is already approved.</exception>
    /// <exception cref="FixedAssetAlreadyDisposedException">Thrown if the asset is already disposed.</exception>
    public void Approve(DefaultIdType approverId, string? approverName = null)
    {
        if (IsDisposed)
            throw new FixedAssetAlreadyDisposedException(Id);

        if (Status == "Approved")
            throw new FixedAssetAlreadyApprovedException(Id);

        Status = "Approved";
        ApprovedBy = approverId;
        ApproverName = approverName?.Trim();
        ApprovedOn = DateTime.UtcNow;

        QueueDomainEvent(new FixedAssetApproved(Id, AssetName, approverId.ToString(), ApprovedOn.Value));
    }

    /// <summary>
    /// Reject the fixed asset acquisition.
    /// </summary>
    /// <param name="rejectedBy">Username or identifier of the person rejecting the asset.</param>
    /// <param name="reason">Optional reason for rejection.</param>
    /// <exception cref="FixedAssetAlreadyDisposedException">Thrown if the asset is already disposed.</exception>
    public void Reject(string rejectedBy, string? reason = null)
    {
        if (IsDisposed)
            throw new FixedAssetAlreadyDisposedException(Id);

        Status = "Rejected";
        ApprovedBy = Guid.TryParse(rejectedBy, out var guidValue) ? guidValue : null;
        ApproverName = rejectedBy.Trim();
        ApprovedOn = DateTime.UtcNow;
        Remarks = reason?.Trim();

        QueueDomainEvent(new FixedAssetRejected(Id, AssetName, rejectedBy, ApprovedOn.Value, reason));
    }

    private static bool IsValidAssetType(string assetType)
    {
        return FixedAssetTypes.Contains(assetType);
    }
}

/// <summary>
/// Represents a single depreciation event for an asset.
/// </summary>
public class DepreciationEntry : BaseEntity
{
    /// <summary>
    /// The parent fixed asset identifier.
    /// </summary>
    public DefaultIdType FixedAssetId { get; private set; }

    /// <summary>
    /// Depreciation amount applied on <see cref="Date"/>.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Date the depreciation was recorded.
    /// </summary>
    public DateTime Date { get; private set; }

    /// <summary>
    /// Optional method hint used for this specific entry.
    /// </summary>
    public string? Method { get; private set; }

    private DepreciationEntry(DefaultIdType fixedAssetId, decimal amount, DateTime date, string? method = null)
    {
        FixedAssetId = fixedAssetId;
        Amount = amount;
        Date = date;
        Method = method?.Trim();
    }

    /// <summary>
    /// Create a depreciation entry with positive amount.
    /// </summary>
    public static DepreciationEntry Create(DefaultIdType fixedAssetId, decimal amount, DateTime date, string? method = null)
    {
        if (amount <= 0)
            throw new InvalidDepreciationAmountException();

        return new DepreciationEntry(fixedAssetId, amount, date, method);
    }
}
