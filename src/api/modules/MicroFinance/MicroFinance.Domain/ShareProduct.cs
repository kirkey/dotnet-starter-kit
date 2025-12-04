using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a share product template in the microfinance system.
/// Defines the terms and conditions for share capital.
/// </summary>
public class ShareProduct : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for product code. (2^6 = 64)</summary>
    public const int CodeMaxLength = 64;

    /// <summary>Maximum length for product name. (2^8 = 256)</summary>
    public const int NameMaxLength = 256;

    /// <summary>Maximum length for description. (2^11 = 2048)</summary>
    public const int DescriptionMaxLength = 2048;

    /// <summary>Maximum length for currency code. (2^3 = 8)</summary>
    public const int CurrencyCodeMaxLength = 8;

    /// <summary>Minimum length for product name.</summary>
    public const int NameMinLength = 2;

    /// <summary>Gets the unique product code.</summary>
    public string Code { get; private set; } = default!;

    /// <summary>Gets the product name.</summary>
    public new string Name { get; private set; } = default!;

    /// <summary>Gets the product description.</summary>
    public new string? Description { get; private set; }

    /// <summary>Gets the currency code (e.g., USD, EUR).</summary>
    public string CurrencyCode { get; private set; } = default!;

    /// <summary>Gets the nominal value per share.</summary>
    public decimal NominalValue { get; private set; }

    /// <summary>Gets the current market price per share.</summary>
    public decimal CurrentPrice { get; private set; }

    /// <summary>Gets the minimum number of shares for membership.</summary>
    public int MinSharesForMembership { get; private set; }

    /// <summary>Gets the maximum shares per member.</summary>
    public int? MaxSharesPerMember { get; private set; }

    /// <summary>Gets whether shares are transferable.</summary>
    public bool AllowTransfer { get; private set; }

    /// <summary>Gets whether shares can be redeemed.</summary>
    public bool AllowRedemption { get; private set; }

    /// <summary>Gets the minimum holding period in months before redemption.</summary>
    public int? MinHoldingPeriodMonths { get; private set; }

    /// <summary>Gets whether dividends are paid on these shares.</summary>
    public bool PaysDividends { get; private set; }

    /// <summary>Gets whether the product is active.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets the collection of share accounts using this product.</summary>
    public virtual ICollection<ShareAccount> ShareAccounts { get; private set; } = new List<ShareAccount>();

    private ShareProduct() { }

    private ShareProduct(
        DefaultIdType id,
        string code,
        string name,
        string? description,
        string currencyCode,
        decimal nominalValue,
        decimal currentPrice,
        int minSharesForMembership,
        int? maxSharesPerMember,
        bool allowTransfer,
        bool allowRedemption,
        int? minHoldingPeriodMonths,
        bool paysDividends)
    {
        Id = id;
        Code = code.Trim();
        ValidateAndSetName(name);
        Description = description?.Trim();
        CurrencyCode = currencyCode.Trim().ToUpperInvariant();
        NominalValue = nominalValue;
        CurrentPrice = currentPrice;
        MinSharesForMembership = minSharesForMembership;
        MaxSharesPerMember = maxSharesPerMember;
        AllowTransfer = allowTransfer;
        AllowRedemption = allowRedemption;
        MinHoldingPeriodMonths = minHoldingPeriodMonths;
        PaysDividends = paysDividends;
        IsActive = true;

        QueueDomainEvent(new ShareProductCreated { ShareProduct = this });
    }

    /// <summary>
    /// Creates a new ShareProduct instance.
    /// </summary>
    public static ShareProduct Create(
        string code,
        string name,
        string? description,
        string currencyCode,
        decimal nominalValue,
        decimal currentPrice,
        int minSharesForMembership = 1,
        int? maxSharesPerMember = null,
        bool allowTransfer = false,
        bool allowRedemption = true,
        int? minHoldingPeriodMonths = null,
        bool paysDividends = true)
    {
        return new ShareProduct(
            DefaultIdType.NewGuid(),
            code,
            name,
            description,
            currencyCode,
            nominalValue,
            currentPrice,
            minSharesForMembership,
            maxSharesPerMember,
            allowTransfer,
            allowRedemption,
            minHoldingPeriodMonths,
            paysDividends);
    }

    /// <summary>
    /// Updates the share product information.
    /// </summary>
    public ShareProduct Update(
        string? name,
        string? description,
        decimal? currentPrice,
        int? maxSharesPerMember,
        bool? allowTransfer,
        bool? allowRedemption,
        int? minHoldingPeriodMonths,
        bool? paysDividends)
    {
        bool hasChanges = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            ValidateAndSetName(name);
            hasChanges = true;
        }

        if (description != Description) { Description = description?.Trim(); hasChanges = true; }
        if (currentPrice.HasValue && currentPrice != CurrentPrice) { CurrentPrice = currentPrice.Value; hasChanges = true; }
        if (maxSharesPerMember != MaxSharesPerMember) { MaxSharesPerMember = maxSharesPerMember; hasChanges = true; }
        if (allowTransfer.HasValue && allowTransfer != AllowTransfer) { AllowTransfer = allowTransfer.Value; hasChanges = true; }
        if (allowRedemption.HasValue && allowRedemption != AllowRedemption) { AllowRedemption = allowRedemption.Value; hasChanges = true; }
        if (minHoldingPeriodMonths != MinHoldingPeriodMonths) { MinHoldingPeriodMonths = minHoldingPeriodMonths; hasChanges = true; }
        if (paysDividends.HasValue && paysDividends != PaysDividends) { PaysDividends = paysDividends.Value; hasChanges = true; }

        if (hasChanges)
        {
            QueueDomainEvent(new ShareProductUpdated { ShareProduct = this });
        }

        return this;
    }

    /// <summary>
    /// Updates the current share price.
    /// </summary>
    public ShareProduct UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Price must be positive.", nameof(newPrice));

        CurrentPrice = newPrice;
        QueueDomainEvent(new ShareProductPriceUpdated { ProductId = Id, NewPrice = newPrice });
        return this;
    }

    /// <summary>Activates the share product.</summary>
    public ShareProduct Activate()
    {
        if (!IsActive) { IsActive = true; }
        return this;
    }

    /// <summary>Deactivates the share product.</summary>
    public ShareProduct Deactivate()
    {
        if (IsActive) { IsActive = false; }
        return this;
    }

    private void ValidateAndSetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));

        string trimmed = name.Trim();
        if (trimmed.Length < NameMinLength)
            throw new ArgumentException($"Product name must be at least {NameMinLength} characters.", nameof(name));
        if (trimmed.Length > NameMaxLength)
            throw new ArgumentException($"Product name cannot exceed {NameMaxLength} characters.", nameof(name));

        Name = trimmed;
    }
}
