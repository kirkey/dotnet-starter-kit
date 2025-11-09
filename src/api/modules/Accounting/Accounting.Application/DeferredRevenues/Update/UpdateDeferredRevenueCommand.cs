namespace Accounting.Application.DeferredRevenues.Update;

/// <summary>
/// Command to update an existing deferred revenue entry.
/// </summary>
public sealed record UpdateDeferredRevenueCommand(
    DefaultIdType Id,
    DateTime? RecognitionDate = null,
    decimal? Amount = null,
    string? Description = null) : IRequest<DefaultIdType>;

