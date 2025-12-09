using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.MicroFinance.Domain.Events;

namespace FSH.Starter.WebApi.MicroFinance.Domain;

/// <summary>
/// Represents a share product template for member equity in the microfinance institution.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define share capital requirements for cooperative membership.
/// - Configure share purchase limits and pricing.
/// - Manage share transfer and redemption policies.
/// - Set dividend payment eligibility rules.
/// - Track share ownership for voting rights and profit distribution.
/// 
/// Default values and constraints:
/// - Code: Unique product identifier (max 64 chars).
/// - Name: Display name for the product (max 256 chars, min length required).
/// - Description: Detailed product description (max 2048 chars).
/// - NominalValue: Price per share unit (positive decimal).
/// - MinimumShares: Minimum shares required for membership.
/// - MaximumShares: Maximum shares a member can hold.
/// 
/// Business rules:
/// - Share capital demonstrates member commitment to the institution.
/// - Provides capital base for lending operations.
/// - Determines dividend distributions and voting power.
/// - May be prerequisite for accessing certain loan products.
/// - Different products for common, preferred, or institutional shares.
/// </remarks>
/// <seealso cref="ShareAccount"/>
/// <seealso cref="ShareTransaction"/>
/// <seealso cref="Member"/>
public class ShareProduct : AuditableEntity, IAggregateRoot
{
    // Domain Constants
    /// <summary>Maximum length for product code. (2^6 = 64)</summary>
    public const int CodeMaxLength = 64;

    /// <summary>Maximum length for product name. (2^8 = 256)</summary>
    public const int NameMaxLength = 256;

    /// <summary>Maximum length for description. (2^11 = 2048)</summary>
    public const int DescriptionMaxLength = 2048;

    /// <summary>Minimum length for product name.</summary>
    public const int NameMinLength = 2;

    /// <summary>
    /// Gets the unique product code.
    /// </summary>
    /// <remarks>
    /// Short identifier (e.g., "SHR-COM" for common shares, "SHR-PREF" for preferred shares).
    /// </remarks>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Gets the nominal (par/face) value per share.
    /// </summary>
    /// <remarks>
    /// The original value assigned when the share class was created. Used for accounting
    /// and regulatory purposes. Dividends are often calculated as a percentage of nominal value.
    /// </remarks>
    public decimal NominalValue { get; private set; }

    /// <summary>
    /// Gets the current market/trading price per share.
    /// </summary>
    /// <remarks>
    /// The price at which shares are currently bought or sold. May differ from nominal value
    /// based on the institution's performance and retained earnings. Updated periodically.
    /// </remarks>
    public decimal CurrentPrice { get; private set; }

    /// <summary>
    /// Gets the minimum number of shares required for membership eligibility.
    /// </summary>
    /// <remarks>
    /// Members must own at least this many shares to maintain active membership status
    /// and access member benefits (loans, voting rights, etc.).
    /// </remarks>
    public int MinSharesForMembership { get; private set; }

    /// <summary>
    /// Gets the maximum shares any single member can hold.
    /// </summary>
    /// <remarks>
    /// Prevents concentration of ownership. Null means no limit.
    /// Important for maintaining democratic control in cooperatives.
    /// </remarks>
    public int? MaxSharesPerMember { get; private set; }

    /// <summary>
    /// Gets whether shares can be transferred between members.
    /// </summary>
    /// <remarks>
    /// If true, members can sell/transfer shares to other eligible members.
    /// Transfers may require board approval or have other restrictions.
    /// </remarks>
    public bool AllowTransfer { get; private set; }

    /// <summary>
    /// Gets whether shares can be redeemed (sold back to the institution).
    /// </summary>
    /// <remarks>
    /// Redemption allows members to exit their investment. May be restricted
    /// to protect the institution's capital base.
    /// </remarks>
    public bool AllowRedemption { get; private set; }

    /// <summary>
    /// Gets the minimum holding period in months before redemption is allowed.
    /// </summary>
    /// <remarks>
    /// Lock-up period ensuring capital stability. Members cannot redeem shares
    /// until this period has passed since purchase.
    /// </remarks>
    public int? MinHoldingPeriodMonths { get; private set; }

    /// <summary>
    /// Gets whether dividends are paid on these shares.
    /// </summary>
    /// <remarks>
    /// Dividend-paying shares receive a portion of the institution's profits.
    /// Dividend rates are typically set annually by the board/AGM.
    /// </remarks>
    public bool PaysDividends { get; private set; }

    /// <summary>
    /// Gets whether the product is active and available.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets the collection of share accounts using this product.
    /// </summary>
    public virtual ICollection<ShareAccount> ShareAccounts { get; private set; } = new List<ShareAccount>();

    private ShareProduct() { }

    private ShareProduct(
        DefaultIdType id,
        string code,
        string name,
        string? description,
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
