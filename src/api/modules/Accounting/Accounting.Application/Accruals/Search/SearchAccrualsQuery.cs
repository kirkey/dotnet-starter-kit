using Accounting.Application.Accruals.Responses;

namespace Accounting.Application.Accruals.Search;

public sealed record SearchAccrualsQuery(
    string? NumberLike,
    DateTime? DateFrom,
    DateTime? DateTo,
    bool? IsReversed
) : IRequest<List<AccrualResponse>>;

