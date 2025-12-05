namespace FSH.Starter.Blazor.Client.Pages.Accounting.PostingBatches;

public class PostingBatchViewModel
{
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Batch number for display purposes (read-only after creation)
    /// </summary>
    public string? BatchNumber { get; set; }

    /// <summary>
    /// Description of the batch
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Source module that created this batch
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Batch date
    /// </summary>
    public DateTime? BatchDate { get; set; }

    /// <summary>
    /// Batch status
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Alias for BatchDate to match UI expectations
    /// </summary>
    public DateTime? PostingDate
    {
        get => BatchDate;
        set => BatchDate = value ?? default;
    }

    /// <summary>
    /// Alias for Description to match UI expectations
    /// </summary>
    public string? Notes
    {
        get => Description;
        set => Description = value;
    }
}
