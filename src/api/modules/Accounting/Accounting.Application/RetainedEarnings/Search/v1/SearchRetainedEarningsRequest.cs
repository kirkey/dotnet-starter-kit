using Accounting.Application.RetainedEarnings.Responses;

namespace Accounting.Application.RetainedEarnings.Search.v1;

/// <summary>
/// Request to search for retained earnings with optional filters.
/// </summary>
public record SearchRetainedEarningsRequest(
    int? FiscalYear = null,
    string? Status = null,
    bool? IsClosed = null) : IRequest<List<RetainedEarningsResponse>>;


