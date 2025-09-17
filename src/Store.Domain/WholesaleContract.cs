namespace Store.Domain;

/// <summary>
/// Contract with a wholesale customer that defines pricing, minimums and payment terms.
/// </summary>
/// <remarks>
/// Use cases:
/// - Store negotiated terms for large customers.
/// - Apply wholesale pricing rules via linked <see cref="WholesalePricings"/>.
/// </remarks>
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
        if (volumeDiscountPercentage < 0m || volumeDiscountPercentage > 100m) throw new ArgumentException("VolumeDiscountPercentage must be between 0 and 100", nameof(volumeDiscountPercentage));
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
        if (Status == "Active" || Status == "Expired")
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
            if (volumeDiscountPercentage.Value < 0m || volumeDiscountPercentage.Value > 100m) throw new ArgumentException("VolumeDiscountPercentage must be between 0 and 100", nameof(volumeDiscountPercentage));
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
