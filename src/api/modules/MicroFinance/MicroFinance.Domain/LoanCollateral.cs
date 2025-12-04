using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents collateral pledged against a loan.
/// Tracks assets securing loan repayment.
/// </summary>
public class LoanCollateral : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for collateral type field. (2^6 = 64)</summary>
    public const int CollateralTypeMaxLength = 64;

    /// <summary>Maximum length for description field. (2^9 = 512)</summary>
    public const int DescriptionMaxLength = 512;

    /// <summary>Maximum length for location field. (2^9 = 512)</summary>
    public const int LocationMaxLength = 512;

    /// <summary>Maximum length for document reference field. (2^7 = 128)</summary>
    public const int DocumentReferenceMaxLength = 128;

    /// <summary>Maximum length for status field. (2^5 = 32)</summary>
    public const int StatusMaxLength = 32;

    /// <summary>Maximum length for notes field. (2^11 = 2048)</summary>
    public const int NotesMaxLength = 2048;

    // Collateral Types
    public const string TypeRealEstate = "RealEstate";
    public const string TypeVehicle = "Vehicle";
    public const string TypeEquipment = "Equipment";
    public const string TypeInventory = "Inventory";
    public const string TypeSavingsDeposit = "SavingsDeposit";
    public const string TypeJewelry = "Jewelry";
    public const string TypeLivestock = "Livestock";
    public const string TypeOther = "Other";

    // Collateral Statuses
    public const string StatusPending = "Pending";
    public const string StatusVerified = "Verified";
    public const string StatusPledged = "Pledged";
    public const string StatusReleased = "Released";
    public const string StatusSeized = "Seized";

    /// <summary>Gets the loan ID this collateral is for.</summary>
    public DefaultIdType LoanId { get; private set; }

    /// <summary>Gets the loan navigation property.</summary>
    public virtual Loan Loan { get; private set; } = default!;

    /// <summary>Gets the type of collateral.</summary>
    public string CollateralType { get; private set; } = default!;

    /// <summary>Gets the description of the collateral.</summary>
    public string Description { get; private set; } = default!;

    /// <summary>Gets the estimated value of the collateral.</summary>
    public decimal EstimatedValue { get; private set; }

    /// <summary>Gets the forced sale value (lower estimate).</summary>
    public decimal? ForcedSaleValue { get; private set; }

    /// <summary>Gets the valuation date.</summary>
    public DateOnly ValuationDate { get; private set; }

    /// <summary>Gets the location of the collateral.</summary>
    public string? Location { get; private set; }

    /// <summary>Gets the document reference (title deed, registration, etc.).</summary>
    public string? DocumentReference { get; private set; }

    /// <summary>Gets the current status.</summary>
    public string Status { get; private set; } = default!;

    /// <summary>Gets internal notes.</summary>
    public new string? Notes { get; private set; }

    private LoanCollateral() { }

    private LoanCollateral(
        DefaultIdType id,
        DefaultIdType loanId,
        string collateralType,
        string description,
        decimal estimatedValue,
        decimal? forcedSaleValue,
        DateOnly valuationDate,
        string? location,
        string? documentReference,
        string? notes)
    {
        Id = id;
        LoanId = loanId;
        CollateralType = collateralType.Trim();
        Description = description.Trim();
        EstimatedValue = estimatedValue;
        ForcedSaleValue = forcedSaleValue;
        ValuationDate = valuationDate;
        Location = location?.Trim();
        DocumentReference = documentReference?.Trim();
        Status = StatusPending;
        Notes = notes?.Trim();

        QueueDomainEvent(new LoanCollateralCreated { LoanCollateral = this });
    }

    /// <summary>
    /// Creates a new LoanCollateral instance.
    /// </summary>
    public static LoanCollateral Create(
        DefaultIdType loanId,
        string collateralType,
        string description,
        decimal estimatedValue,
        decimal? forcedSaleValue = null,
        DateOnly? valuationDate = null,
        string? location = null,
        string? documentReference = null,
        string? notes = null)
    {
        return new LoanCollateral(
            DefaultIdType.NewGuid(),
            loanId,
            collateralType,
            description,
            estimatedValue,
            forcedSaleValue,
            valuationDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            location,
            documentReference,
            notes);
    }

    /// <summary>
    /// Updates the collateral valuation.
    /// </summary>
    public LoanCollateral UpdateValuation(
        decimal estimatedValue,
        decimal? forcedSaleValue = null,
        DateOnly? valuationDate = null)
    {
        if (estimatedValue <= 0)
            throw new ArgumentException("Estimated value must be positive.", nameof(estimatedValue));

        EstimatedValue = estimatedValue;
        ForcedSaleValue = forcedSaleValue;
        ValuationDate = valuationDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

        QueueDomainEvent(new LoanCollateralValuationUpdated { CollateralId = Id });
        return this;
    }

    /// <summary>
    /// Verifies the collateral.
    /// </summary>
    public LoanCollateral Verify()
    {
        if (Status != StatusPending)
            throw new InvalidOperationException($"Cannot verify collateral in {Status} status.");

        Status = StatusVerified;
        QueueDomainEvent(new LoanCollateralVerified { CollateralId = Id });
        return this;
    }

    /// <summary>
    /// Pledges the collateral against the loan.
    /// </summary>
    public LoanCollateral Pledge()
    {
        if (Status != StatusVerified)
            throw new InvalidOperationException($"Cannot pledge collateral in {Status} status.");

        Status = StatusPledged;
        QueueDomainEvent(new LoanCollateralPledged { CollateralId = Id });
        return this;
    }

    /// <summary>
    /// Releases the collateral.
    /// </summary>
    public LoanCollateral Release(string? reason = null)
    {
        if (Status != StatusPledged)
            throw new InvalidOperationException($"Cannot release collateral in {Status} status.");

        Status = StatusReleased;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Released: {reason}" : $"{Notes}\nReleased: {reason}";
        }

        QueueDomainEvent(new LoanCollateralReleased { CollateralId = Id });
        return this;
    }

    /// <summary>
    /// Seizes the collateral due to default.
    /// </summary>
    public LoanCollateral Seize(string? reason = null)
    {
        if (Status != StatusPledged)
            throw new InvalidOperationException($"Cannot seize collateral in {Status} status.");

        Status = StatusSeized;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? $"Seized: {reason}" : $"{Notes}\nSeized: {reason}";
        }

        QueueDomainEvent(new LoanCollateralSeized { CollateralId = Id, Reason = reason });
        return this;
    }
}
