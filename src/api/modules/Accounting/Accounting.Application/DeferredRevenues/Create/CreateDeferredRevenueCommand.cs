namespace Accounting.Application.DeferredRevenues.Create;

/// <summary>
/// Command to create a new deferred revenue entry.
/// </summary>
public sealed record CreateDeferredRevenueCommand(
    string DeferredRevenueNumber,
    DateTime RecognitionDate,
    decimal Amount,
    string? Description = null) : IRequest<DefaultIdType>;

