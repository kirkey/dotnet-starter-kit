using Accounting.Application.Accruals.Responses;
using Accounting.Application.Accruals.Queries;

namespace Accounting.Application.Accruals.Handlers;

public class SearchAccrualsHandler(IReadRepository<Accrual> repository)
    : IRequestHandler<SearchAccrualsQuery, List<AccrualResponse>>
{
    public async Task<List<AccrualResponse>> Handle(SearchAccrualsQuery request, CancellationToken cancellationToken)
    {
        var query = (await repository.ListAsync(cancellationToken)).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.AccrualNumber))
            query = query.Where(x => x.AccrualNumber.Contains(request.AccrualNumber));
        if (!string.IsNullOrWhiteSpace(request.Description))
            query = query.Where(x => x.Description.Contains(request.Description));
        if (request.Skip.HasValue)
            query = query.Skip(request.Skip.Value);
        if (request.Take.HasValue)
            query = query.Take(request.Take.Value);

        return query.Select(accrual => new AccrualResponse
        {
            Id = accrual.Id,
            AccrualNumber = accrual.AccrualNumber,
            AccrualDate = accrual.AccrualDate,
            Amount = accrual.Amount,
            Description = accrual.Description,
            IsReversed = accrual.IsReversed,
            ReversalDate = accrual.ReversalDate
        }).ToList();
    }
}
