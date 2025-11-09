using Accounting.Application.DeferredRevenues.Responses;

namespace Accounting.Application.DeferredRevenues.Get;

/// <summary>
/// Request to get a deferred revenue by ID.
/// </summary>
public sealed record GetDeferredRevenueRequest(DefaultIdType Id) : IRequest<DeferredRevenueResponse>;

