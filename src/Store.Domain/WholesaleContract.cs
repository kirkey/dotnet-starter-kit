namespace Store.Domain;

public sealed class WholesaleContract : AuditableEntity, IAggregateRoot
{
    public string ContractNumber { get; private set; } = default!;
    public DefaultIdType CustomerId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string Status { get; private set; } = default!; // Active, Expired, Terminated, Pending
    public decimal MinimumOrderValue { get; private set; }
    public decimal VolumeDiscountPercentage { get; private set; }
    public int PaymentTermsDays { get; private set; }
    public decimal CreditLimit { get; private set; }
    public string? DeliveryTerms { get; private set; }
    public string? ContractTerms { get; private set; }
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

    public bool IsActive() => Status == "Active" && DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
    public bool IsExpired() => DateTime.UtcNow > EndDate;
    public bool IsExpiringSoon(int daysThreshold = 30) => (EndDate - DateTime.UtcNow).Days <= daysThreshold;
}
