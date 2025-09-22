namespace FSH.Starter.Blazor.Client.Pages.Accounting.Accruals;

/// <summary>
/// ViewModel used by the Accruals page for add/edit operations.
/// Mirrors the shape of the API's UpdateAccrualCommand so Mapster/Adapt can map between them.
/// </summary>
public class AccrualViewModel
{
    /// <summary>
    /// Primary identifier of the accrual.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Business accrual number assigned to the accrual.
    /// </summary>
    public string? AccrualNumber { get; set; }

    /// <summary>
    /// Date when the accrual was recorded.
    /// </summary>
    public DateTime? AccrualDate { get; set; }

    /// <summary>
    /// Monetary amount of the accrual.
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// Optional free-text description for the accrual.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates whether the accrual has been reversed.
    /// </summary>
    public bool IsReversed { get; set; }

    /// <summary>
    /// Date when the accrual was reversed, if applicable.
    /// </summary>
    public DateTime? ReversalDate { get; set; }
}

