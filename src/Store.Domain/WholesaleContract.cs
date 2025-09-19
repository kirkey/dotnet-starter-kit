namespace Store.Domain;

/// <summary>
/// Represents a wholesale contract with a customer defining pricing, terms, and volume commitments for bulk purchasing.
/// </summary>
/// <remarks>
/// Use cases:
/// - Establish formal agreements with wholesale customers for volume purchasing and special pricing.
/// - Define minimum order quantities and volume commitments for preferential pricing tiers.
/// - Manage contract lifecycle from negotiation through renewal with automated expiration tracking.
/// - Support tiered pricing structures based on purchase volumes and contract terms.
/// - Enable contract-specific payment terms different from standard customer terms.
/// - Track contract performance against committed volumes and pricing compliance.
/// - Support contract amendments and renegotiation with proper approval workflows.
/// - Generate contract reports for sales management and customer relationship analysis.
/// 
/// Default values:
/// - ContractNumber: required unique identifier (example: "WC-2025-09-001")
/// - CustomerId: required customer reference for contract assignment
/// - StartDate: required contract effective date (example: 2025-10-01)
/// - EndDate: required contract expiration date (example: 2026-09-30)
/// - Status: "Draft" (new contracts start as draft until signed)
/// - MinimumOrderQuantity: 0 (no minimum unless specified)
/// - MinimumOrderValue: 0.00 (no minimum unless specified)
/// - AnnualVolumeCommitment: null (optional volume guarantee)
/// - DiscountPercentage: 0.00 (standard pricing unless discount applied)
/// - PaymentTerms: "Net 30" (default payment terms)
/// - IsActive: true (contracts are active when within date range)
/// - RenewalNoticeDate: null (set for automatic renewal notifications)
/// 
/// Business rules:
/// - ContractNumber must be unique within the system
/// - EndDate must be after StartDate
/// - Cannot modify signed contracts without amendment process
/// - Customer must be active to create new contracts
/// - Minimum order requirements must be non-negative
/// - Discount percentages must be between 0 and 100
/// - Contract pricing takes precedence over standard customer pricing
/// - Automatic expiration when EndDate is reached
/// - Renewal requires new contract creation or amendment
/// - Volume commitments tracked against actual purchases
/// </remarks>
/// <seealso cref="Store.Domain.Events.WholesaleContractCreated"/>
/// <seealso cref="Store.Domain.Events.WholesaleContractUpdated"/>
/// <seealso cref="Store.Domain.Events.WholesaleContractSigned"/>
/// <seealso cref="Store.Domain.Events.WholesaleContractExpired"/>
/// <seealso cref="Store.Domain.Events.WholesaleContractRenewed"/>
/// <seealso cref="Store.Domain.Events.WholesaleContractAmended"/>
/// <seealso cref="Store.Domain.Exceptions.WholesaleContract.WholesaleContractNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.WholesaleContract.WholesaleContractCannotBeModifiedException"/>
/// <seealso cref="Store.Domain.Exceptions.WholesaleContract.InvalidWholesaleContractStatusException"/>
public sealed class WholesaleContract : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique contract number. Example: "WC-2025-001".
    /// </summary>
    public string ContractNumber { get; private set; } = default!;

    /// <summary>
    /// Customer id this contract applies to.
    /// </summary>
    public DefaultIdType CustomerId { get; private set; }

    /// <summary>
    /// Contract start date.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// Contract end date.
    /// </summary>
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// Status of the contract. Possible values: Active, Expired, Terminated, Pending.
    /// </summary>
    public string Status { get; private set; } = default!; // Active, Expired, Terminated, Pending

    /// <summary>
    /// Minimum order value to qualify under this contract.
    /// </summary>
    public decimal MinimumOrderValue { get; private set; }

    /// <summary>
    /// Percentage discount applied on bulk orders.
    /// </summary>
    public decimal VolumeDiscountPercentage { get; private set; }

    /// <summary>
    /// Payment terms in days.
    /// </summary>
    public int PaymentTermsDays { get; private set; }

    /// <summary>
    /// Credit limit for the customer under this contract.
    /// </summary>
    public decimal CreditLimit { get; private set; }

    /// <summary>
    /// Optional delivery terms.
    /// </summary>
    public string? DeliveryTerms { get; private set; }

    /// <summary>
    /// Optional contract terms and conditions.
    /// </summary>
    public string? ContractTerms { get; private set; }

    /// <summary>
    /// Indicates if the contract should automatically renew at the end date.
    /// </summary>
    public bool AutoRenewal { get; private set; }
    
    
    public Customer Customer { get; private set; } = default!;
    public ICollection<WholesalePricing> WholesalePricings { get; private set; } = new List<WholesalePricing>();

    private WholesaleContract() { }

    private WholesaleContract(
        DefaultIdType id,
        string contractNumber,
        DefaultIdType customerId,
        DateTime startDate,
        DateTime endDate,
        string status,
        decimal minimumOrderValue,
        decimal volumeDiscountPercentage,
        int paymentTermsDays,
        decimal creditLimit,
        string? deliveryTerms,
        string? contractTerms,
        bool autoRenewal,
        string? notes)
    {
        // validations
        if (string.IsNullOrWhiteSpace(contractNumber)) throw new ArgumentException("ContractNumber is required", nameof(contractNumber));
        if (contractNumber.Length > 100) throw new ArgumentException("ContractNumber must not exceed 100 characters", nameof(contractNumber));
        if (customerId == default) throw new ArgumentException("CustomerId is required", nameof(customerId));
        if (startDate == default) throw new ArgumentException("StartDate is required", nameof(startDate));
        if (endDate == default) throw new ArgumentException("EndDate is required", nameof(endDate));
        if (endDate < startDate) throw new ArgumentException("EndDate must be equal or later than StartDate", nameof(endDate));
        if (minimumOrderValue < 0m) throw new ArgumentException("MinimumOrderValue must be zero or greater", nameof(minimumOrderValue));
        if (volumeDiscountPercentage is < 0m or > 100m) throw new ArgumentException("VolumeDiscountPercentage must be between 0 and 100", nameof(volumeDiscountPercentage));
        if (paymentTermsDays < 0) throw new ArgumentException("PaymentTermsDays must be zero or greater", nameof(paymentTermsDays));
        if (creditLimit < 0m) throw new ArgumentException("CreditLimit must be zero or greater", nameof(creditLimit));

        Id = id;
        ContractNumber = contractNumber;
        CustomerId = customerId;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        MinimumOrderValue = minimumOrderValue;
        VolumeDiscountPercentage = volumeDiscountPercentage;
        PaymentTermsDays = paymentTermsDays;
        CreditLimit = creditLimit;
        DeliveryTerms = deliveryTerms;
        ContractTerms = contractTerms;
        AutoRenewal = autoRenewal;
        Notes = notes;

        QueueDomainEvent(new WholesaleContractCreated { WholesaleContract = this });
    }

    public static WholesaleContract Create(
        string contractNumber,
        DefaultIdType customerId,
        DateTime startDate,
        DateTime endDate,
        decimal minimumOrderValue,
        decimal volumeDiscountPercentage,
        int paymentTermsDays = 30,
        decimal creditLimit = 0,
        string? deliveryTerms = null,
        string? contractTerms = null,
        bool autoRenewal = false,
        string? notes = null)
    {
        return new WholesaleContract(
            DefaultIdType.NewGuid(),
            contractNumber,
            customerId,
            startDate,
            endDate,
            "Pending",
            minimumOrderValue,
            volumeDiscountPercentage,
            paymentTermsDays,
            creditLimit,
            deliveryTerms,
            contractTerms,
            autoRenewal,
            notes);
    }

    public WholesaleContract Activate()
    {
        if (Status != "Active")
        {
            Status = "Active";
            QueueDomainEvent(new WholesaleContractActivated { WholesaleContract = this });
        }
        return this;
    }

    public WholesaleContract Terminate(string reason)
    {
        if (Status == "Active")
        {
            Status = "Terminated";
            Notes = string.IsNullOrEmpty(Notes) ? reason : $"{Notes}; Terminated: {reason}";
            QueueDomainEvent(new WholesaleContractTerminated { WholesaleContract = this, Reason = reason });
        }
        return this;
    }

    public WholesaleContract Renew(DateTime newEndDate)
    {
        if (Status is "Active" or "Expired")
        {
            EndDate = newEndDate;
            Status = "Active";
            QueueDomainEvent(new WholesaleContractRenewed { WholesaleContract = this });
        }
        return this;
    }

    public WholesaleContract Update(
        DateTime? startDate = null,
        DateTime? endDate = null,
        decimal? minimumOrderValue = null,
        decimal? volumeDiscountPercentage = null,
        int? paymentTermsDays = null,
        decimal? creditLimit = null,
        string? deliveryTerms = null,
        string? contractTerms = null,
        bool? autoRenewal = null,
        string? notes = null,
        string? status = null)
    {
        bool isUpdated = false;

        if (startDate.HasValue && startDate.Value != StartDate)
        {
            StartDate = startDate.Value;
            isUpdated = true;
        }

        if (endDate.HasValue && endDate.Value != EndDate)
        {
            if (endDate.Value < StartDate)
                throw new ArgumentException("EndDate must be equal or later than StartDate", nameof(endDate));
            EndDate = endDate.Value;
            isUpdated = true;
        }

        if (minimumOrderValue.HasValue && minimumOrderValue.Value != MinimumOrderValue)
        {
            if (minimumOrderValue.Value < 0m) throw new ArgumentException("MinimumOrderValue must be zero or greater", nameof(minimumOrderValue));
            MinimumOrderValue = minimumOrderValue.Value;
            isUpdated = true;
        }

        if (volumeDiscountPercentage.HasValue && volumeDiscountPercentage.Value != VolumeDiscountPercentage)
        {
            if (volumeDiscountPercentage.Value is < 0m or > 100m) throw new ArgumentException("VolumeDiscountPercentage must be between 0 and 100", nameof(volumeDiscountPercentage));
            VolumeDiscountPercentage = volumeDiscountPercentage.Value;
            isUpdated = true;
        }

        if (paymentTermsDays.HasValue && paymentTermsDays.Value != PaymentTermsDays)
        {
            if (paymentTermsDays.Value < 0) throw new ArgumentException("PaymentTermsDays must be zero or greater", nameof(paymentTermsDays));
            PaymentTermsDays = paymentTermsDays.Value;
            isUpdated = true;
        }

        if (creditLimit.HasValue && creditLimit.Value != CreditLimit)
        {
            if (creditLimit.Value < 0m) throw new ArgumentException("CreditLimit must be zero or greater", nameof(creditLimit));
            CreditLimit = creditLimit.Value;
            isUpdated = true;
        }

        if (deliveryTerms != null && deliveryTerms != DeliveryTerms)
        {
            if (deliveryTerms.Length > 500) throw new ArgumentException("DeliveryTerms must not exceed 500 characters", nameof(deliveryTerms));
            DeliveryTerms = deliveryTerms;
            isUpdated = true;
        }

        if (contractTerms != null && contractTerms != ContractTerms)
        {
            if (contractTerms.Length > 5000) throw new ArgumentException("ContractTerms must not exceed 5000 characters", nameof(contractTerms));
            ContractTerms = contractTerms;
            isUpdated = true;
        }

        if (autoRenewal.HasValue && autoRenewal.Value != AutoRenewal)
        {
            AutoRenewal = autoRenewal.Value;
            isUpdated = true;
        }

        if (notes != null && notes != Notes)
        {
            if (notes.Length > 2048) throw new ArgumentException("Notes must not exceed 2048 characters", nameof(notes));
            Notes = notes;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(status) && status != Status)
        {
            if (status.Length > 50) throw new ArgumentException("Status must not exceed 50 characters", nameof(status));
            Status = status;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new WholesaleContractUpdated { WholesaleContract = this });
        }

        return this;
    }

    public bool IsActive() => Status == "Active" && DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
    public bool IsExpired() => DateTime.UtcNow > EndDate;
    public bool IsExpiringSoon(int daysThreshold = 30) => (EndDate - DateTime.UtcNow).Days <= daysThreshold;
}
