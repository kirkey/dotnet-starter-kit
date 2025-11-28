namespace FSH.Starter.Blazor.Client.Pages.Accounting.PostingBatches;

public class PostingBatchViewModel : UpdatePostingBatchCommand
{
    /// <summary>
    /// Batch number for display purposes (read-only after creation)
    /// </summary>
    public string? BatchNumber { get; set; }

    /// <summary>
    /// Source module that created this batch (read-only after creation)
    /// </summary>
    public string? SourceModule { get; set; }

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
