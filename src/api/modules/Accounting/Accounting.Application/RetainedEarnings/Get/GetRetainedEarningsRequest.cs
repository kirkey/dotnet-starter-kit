using Accounting.Application.RetainedEarnings.Responses;

namespace Accounting.Application.RetainedEarnings.Get;

/// <summary>
/// Request to get detailed retained earnings information by ID.
/// </summary>
public record GetRetainedEarningsRequest(DefaultIdType Id) : IRequest<RetainedEarningsDetailsResponse>;
