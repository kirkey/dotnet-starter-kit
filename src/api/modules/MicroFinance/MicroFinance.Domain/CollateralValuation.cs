using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a valuation/appraisal of collateral.
/// </summary>
public sealed class CollateralValuation : AuditableEntity, IAggregateRoot
{
    // Constants
    public const int ReferenceMaxLength = 64;
    public const int StatusMaxLength = 32;
    public const int AppraiserMaxLength = 128;
    public const int MethodMaxLength = 64;
    public const int NotesMaxLength = 1024;
    public const int DocumentMaxLength = 512;
    
    // Valuation Status
    public const string StatusDraft = "Draft";
    public const string StatusPending = "Pending";
    public const string StatusApproved = "Approved";
    public const string StatusRejected = "Rejected";
    public const string StatusExpired = "Expired";
    
    // Valuation Methods
    public const string MethodMarket = "MarketComparison";
    public const string MethodCost = "CostApproach";
    public const string MethodIncome = "IncomeApproach";
    public const string MethodForcedSale = "ForcedSale";
    public const string MethodBook = "BookValue";

    public Guid CollateralId { get; private set; }
    public string ValuationReference { get; private set; } = default!;
    public string Status { get; private set; } = StatusDraft;
    public DateOnly ValuationDate { get; private set; }
    public DateOnly? ExpiryDate { get; private set; }
    public string ValuationMethod { get; private set; } = default!;
    public string? AppraiserName { get; private set; }
    public string? AppraiserCompany { get; private set; }
    public string? AppraiserLicense { get; private set; }
    public decimal MarketValue { get; private set; }
    public decimal ForcedSaleValue { get; private set; }
    public decimal InsurableValue { get; private set; }
    public decimal? PreviousValue { get; private set; }
    public decimal? ValueChange { get; private set; }
    public decimal? ValueChangePercent { get; private set; }
    public string? Condition { get; private set; }
    public string? Notes { get; private set; }
    public string? DocumentPath { get; private set; }
    public Guid? ApprovedById { get; private set; }
    public DateOnly? ApprovedDate { get; private set; }
    public string? RejectionReason { get; private set; }

    private CollateralValuation() { }

    public static CollateralValuation Create(
        Guid collateralId,
        string valuationReference,
        DateOnly valuationDate,
        string valuationMethod,
        decimal marketValue,
        decimal forcedSaleValue,
        decimal insurableValue,
        string? appraiserName = null,
        string? appraiserCompany = null,
        decimal? previousValue = null)
    {
        var valuation = new CollateralValuation
        {
            CollateralId = collateralId,
            ValuationReference = valuationReference,
            ValuationDate = valuationDate,
            ValuationMethod = valuationMethod,
            MarketValue = marketValue,
            ForcedSaleValue = forcedSaleValue,
            InsurableValue = insurableValue,
            AppraiserName = appraiserName,
            AppraiserCompany = appraiserCompany,
            PreviousValue = previousValue,
            Status = StatusDraft,
            ExpiryDate = valuationDate.AddYears(1)
        };

        if (previousValue.HasValue && previousValue.Value > 0)
        {
            valuation.ValueChange = marketValue - previousValue.Value;
            valuation.ValueChangePercent = (valuation.ValueChange / previousValue.Value) * 100;
        }

        valuation.QueueDomainEvent(new CollateralValuationCreated(valuation));
        return valuation;
    }

    public CollateralValuation Submit()
    {
        Status = StatusPending;
        QueueDomainEvent(new CollateralValuationSubmitted(Id, CollateralId));
        return this;
    }

    public CollateralValuation Approve(Guid approvedById)
    {
        Status = StatusApproved;
        ApprovedById = approvedById;
        ApprovedDate = DateOnly.FromDateTime(DateTime.UtcNow);
        QueueDomainEvent(new CollateralValuationApproved(Id, CollateralId, MarketValue));
        return this;
    }

    public CollateralValuation Reject(string reason)
    {
        Status = StatusRejected;
        RejectionReason = reason;
        QueueDomainEvent(new CollateralValuationRejected(Id, CollateralId, reason));
        return this;
    }

    public CollateralValuation Expire()
    {
        Status = StatusExpired;
        QueueDomainEvent(new CollateralValuationExpired(Id, CollateralId));
        return this;
    }

    public CollateralValuation Update(
        string? condition = null,
        string? notes = null,
        string? documentPath = null)
    {
        if (condition is not null) Condition = condition;
        if (notes is not null) Notes = notes;
        if (documentPath is not null) DocumentPath = documentPath;

        QueueDomainEvent(new CollateralValuationUpdated(this));
        return this;
    }
}
