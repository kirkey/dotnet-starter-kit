namespace Accounting.Application.DeferredRevenues.Delete;

/// <summary>
/// Command to delete a deferred revenue entry.
/// </summary>
public sealed record DeleteDeferredRevenueCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

