namespace Accounting.Domain;

/// <summary>
/// Represents an accrual entry in the accounting domain.
/// </summary>
/// <remarks>
/// An accrual records an obligation or revenue recognized before the associated
/// cash movement happens. This aggregate enforces domain invariants such as non-empty
/// accrual numbers, positive amounts and immutability after reversal.
///
/// Use <see cref="Create(string, DateTime, decimal, string)"/> to construct a new instance
/// and <see cref="Update(string?, DateTime?, decimal?, string?)"/> to change mutable fields
/// before a reversal. Once <see cref="Reverse(DateTime)"/> is called the accrual becomes
/// immutable for updates.
/// </remarks>
public class Accrual : AuditableEntity, IAggregateRoot
{
    private const int MaxAccrualNumberLength = 50;
    private const int MaxDescriptionLength = 200;

    /// <summary>
    /// A unique human-friendly identifier for the accrual (max length = 50).
    /// </summary>
    /// <remarks>
    /// Required. Stored with whitespace trimmed. Use the factory <see cref="Create"/> to ensure
    /// the value meets domain rules. Default (EF parameterless constructor) is an empty string.
    /// </remarks>
    public string AccrualNumber { get; private set; }

    /// <summary>
    /// The accounting date for the accrual.
    /// </summary>
    /// <remarks>
    /// This is the date the accrual is recognized. There is no implicit default — the constructor
    /// sets the provided value. Default (EF parameterless constructor) is <see cref="DateTime.MinValue"/>
    /// until set.
    /// </remarks>
    public DateTime AccrualDate { get; private set; }

    /// <summary>
    /// The monetary amount of the accrual. Must be positive.
    /// </summary>
    /// <remarks>
    /// Stored as <see cref="decimal"/> to preserve precision for financial calculations. Default
    /// (EF parameterless constructor) is 0m until set.
    /// </remarks>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Optional human-readable description of the accrual (max length = 200).
    /// </summary>
    /// <remarks>
    /// This property hides a base Description property (see <c>new</c>). The description is trimmed
    /// and truncated to the maximum length. Default (EF parameterless constructor) is empty string.
    /// </remarks>
    public new string? Description { get; private set; }

    /// <summary>
    /// Whether this accrual has been reversed.
    /// </summary>
    /// <remarks>
    /// Reversal marks the accrual as finalized: updates are disallowed once this is true.
    /// Default is <c>false</c>.
    /// </remarks>
    public bool IsReversed { get; private set; }

    /// <summary>
    /// The date the accrual was reversed, if applicable.
    /// </summary>
    /// <remarks>
    /// Null when not reversed. When reversed, this is set to the provided reversal date.
    /// </remarks>
    public DateTime? ReversalDate { get; private set; }

    /// <summary>
    /// Parameterless constructor for EF and serializers.
    /// </summary>
    private Accrual()
    {
        AccrualNumber = string.Empty;
        Description = string.Empty;
    }

    /// <summary>
    /// Private constructor that applies domain validation rules.
    /// </summary>
    /// <param name="accrualNumber">Human identifier for the accrual. Required, trimmed, max length 50.</param>
    /// <param name="accrualDate">Accounting date for the accrual.</param>
    /// <param name="amount">Monetary amount of the accrual. Must be &gt; 0.</param>
    /// <param name="description">Optional description (trimmed and truncated to 200 characters).</param>
    /// <exception cref="ArgumentException">Thrown when required values are invalid.</exception>
    private Accrual(string accrualNumber, DateTime accrualDate, decimal amount, string description)
    {
        var num = accrualNumber?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(num))
            throw new ArgumentException("Accrual number is required.");
        if (num.Length > MaxAccrualNumberLength)
            throw new ArgumentException($"Accrual number cannot exceed {MaxAccrualNumberLength} characters.");

        if (amount <= 0)
            throw new ArgumentException("Accrual amount must be positive.");

        AccrualNumber = num;
        AccrualDate = accrualDate;
        Amount = amount;

        var desc = description?.Trim();
        if (desc?.Length > MaxDescriptionLength)
            desc = desc.Substring(0, MaxDescriptionLength);

        Description = desc;
        IsReversed = false;
    }

    /// <summary>
    /// Factory method to create a new Accrual, enforcing domain invariants.
    /// </summary>
    /// <remarks>
    /// Prefer this method over direct construction to ensure validation is applied consistently.
    /// </remarks>
    public static Accrual Create(string accrualNumber, DateTime accrualDate, decimal amount, string description)
    {
        // Domain-level validation occurs in the private constructor
        return new Accrual(accrualNumber, accrualDate, amount, description);
    }

    /// <summary>
    /// Update mutable fields of the accrual.
    /// </summary>
    /// <remarks>
    /// This method enforces:
    /// - No updates allowed after the accrual was reversed.
    /// - AccrualNumber length limit and non-empty checks when provided.
    /// - Amount must remain positive if changed.
    /// - Description is trimmed and truncated to the maximum allowed length.
    ///
    /// The method returns the same instance to allow fluent usage.
    /// </remarks>
    /// <returns>The updated Accrual instance.</returns>
    /// <exception cref="InvalidOperationException">If the accrual has already been reversed.</exception>
    /// <exception cref="ArgumentException">If provided data violates domain rules.</exception>
    public Accrual Update(string? accrualNumber, DateTime? accrualDate, decimal? amount, string? description)
    {
        if (IsReversed)
            throw new InvalidOperationException("Cannot modify a reversed accrual.");

        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(accrualNumber) && AccrualNumber != accrualNumber)
        {
            var num = accrualNumber!.Trim();
            if (num.Length > MaxAccrualNumberLength)
                throw new ArgumentException($"Accrual number cannot exceed {MaxAccrualNumberLength} characters.");
            AccrualNumber = num;
            isUpdated = true;
        }

        if (accrualDate.HasValue && AccrualDate != accrualDate.Value)
        {
            AccrualDate = accrualDate.Value;
            isUpdated = true;
        }

        if (amount.HasValue && Amount != amount.Value)
        {
            if (amount.Value <= 0)
                throw new ArgumentException("Accrual amount must be positive.");
            Amount = amount.Value;
            isUpdated = true;
        }

        if (description != Description)
        {
            var desc = description?.Trim();
            if (desc?.Length > MaxDescriptionLength)
                desc = desc!.Substring(0, MaxDescriptionLength);
            Description = desc;
            isUpdated = true;
        }

        if (isUpdated)
        {
            // Optionally queue domain event here
        }

        return this;
    }

    /// <summary>
    /// Mark the accrual as reversed.
    /// </summary>
    /// <param name="reversalDate">Date when the accrual was reversed.</param>
    /// <exception cref="InvalidOperationException">Thrown if the accrual is already reversed.</exception>
    public void Reverse(DateTime reversalDate)
    {
        if (IsReversed)
            throw new InvalidOperationException("Accrual already reversed.");
        IsReversed = true;
        ReversalDate = reversalDate;
    }
}
