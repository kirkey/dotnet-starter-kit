namespace Accounting.Application.PostingBatches.Update.v1;

/// <summary>
/// Command to update a posting batch.
/// Only draft/pending batches can be updated.
/// </summary>
public sealed record UpdatePostingBatchCommand(
    DefaultIdType Id,
    DateTime BatchDate,
    string? Description = null,
    DefaultIdType? PeriodId = null
) : IRequest<DefaultIdType>;

