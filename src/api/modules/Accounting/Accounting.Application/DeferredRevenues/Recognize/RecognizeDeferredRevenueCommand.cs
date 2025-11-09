namespace Accounting.Application.DeferredRevenues.Recognize;

/// <summary>
/// Command to recognize deferred revenue.
/// </summary>
public sealed record RecognizeDeferredRevenueCommand(
    DefaultIdType Id,
    DateTime RecognizedDate) : IRequest<DefaultIdType>;

