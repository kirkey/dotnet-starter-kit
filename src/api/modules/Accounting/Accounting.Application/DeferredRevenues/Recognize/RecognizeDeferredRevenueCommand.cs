namespace Accounting.Application.DeferredRevenues.Recognize;

/// <summary>
/// Command to recognize deferred revenue.
/// </summary>
public sealed record RecognizeDeferredRevenueCommand(
    DefaultIdType DeferredRevenueId,
    DateTime RecognitionDate
) : IRequest<DefaultIdType>;
