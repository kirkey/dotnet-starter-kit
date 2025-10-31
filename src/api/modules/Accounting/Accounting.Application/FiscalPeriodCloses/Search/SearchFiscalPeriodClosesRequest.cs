using Accounting.Application.FiscalPeriodCloses.Responses;

namespace Accounting.Application.FiscalPeriodCloses.Search;

/// <summary>
/// Request to search for fiscal period closes with optional filters.
/// </summary>
public record SearchFiscalPeriodClosesRequest(
    string? CloseNumber = null,
    string? Status = null,
    string? CloseType = null) : IRequest<List<FiscalPeriodCloseResponse>>;
