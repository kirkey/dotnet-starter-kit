using Accounting.Application.DeferredRevenues.Dtos;
using Accounting.Application.DeferredRevenues.Queries;

namespace Accounting.Application.DeferredRevenues.Handlers;

public class SearchDeferredRevenuesHandler(IReadRepository<DeferredRevenue> repository)
    : IRequestHandler<SearchDeferredRevenuesQuery, List<DeferredRevenueDto>>
{
    public async Task<List<DeferredRevenueDto>> Handle(SearchDeferredRevenuesQuery request, CancellationToken cancellationToken)
    {
        var query = (await repository.ListAsync(cancellationToken)).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.DeferredRevenueNumber))
            query = query.Where(x => x.DeferredRevenueNumber.Contains(request.DeferredRevenueNumber));
        if (!string.IsNullOrWhiteSpace(request.Description))
            query = query.Where(x => x.Description.Contains(request.Description));
        if (request.Skip.HasValue)
            query = query.Skip(request.Skip.Value);
        if (request.Take.HasValue)
            query = query.Take(request.Take.Value);

        return query.Select(deferred => new DeferredRevenueDto
        {
            Id = deferred.Id,
            DeferredRevenueNumber = deferred.DeferredRevenueNumber,
            RecognitionDate = deferred.RecognitionDate,
            Amount = deferred.Amount,
            Description = deferred.Description,
            IsRecognized = deferred.IsRecognized,
            RecognizedDate = deferred.RecognizedDate
        }).ToList();
    }
}