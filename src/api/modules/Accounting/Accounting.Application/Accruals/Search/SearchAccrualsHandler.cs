using Accounting.Application.Accruals.Responses;

namespace Accounting.Application.Accruals.Search;

public sealed class SearchAccrualsHandler(IReadRepository<Accrual> repository)
    : IRequestHandler<SearchAccrualsQuery, List<AccrualResponse>>
{
    public async Task<List<AccrualResponse>> Handle(SearchAccrualsQuery request, CancellationToken ct)
    {
        var items = await repository.ListAsync(new Specs.SearchAccrualsSpec(request.NumberLike, request.DateFrom, request.DateTo, request.IsReversed), ct);
        return items.Select(a => new AccrualResponse(a.Id, a.AccrualNumber, a.AccrualDate, a.Amount, a.Description, a.IsReversed, a.ReversalDate)).ToList();
    }
}

