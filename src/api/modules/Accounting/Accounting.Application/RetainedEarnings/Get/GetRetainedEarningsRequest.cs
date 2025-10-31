using Accounting.Application.RetainedEarnings.Responses;

namespace Accounting.Application.RetainedEarnings.Get;

/// <summary>
/// Request to get retained earnings by ID.
/// </summary>
public record GetRetainedEarningsRequest(DefaultIdType Id) : IRequest<RetainedEarningsResponse>;
