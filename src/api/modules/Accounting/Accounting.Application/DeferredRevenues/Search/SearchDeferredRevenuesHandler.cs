using Accounting.Application.DeferredRevenues.Responses;
using Accounting.Application.DeferredRevenues.Specs;

namespace Accounting.Application.DeferredRevenues.Search;

public sealed class SearchDeferredRevenuesHandler(IRepository<DeferredRevenue> repository)
    : IRequestHandler<SearchDeferredRevenuesRequest, PagedList<DeferredRevenueResponse>>
{
    private readonly IRepository<DeferredRevenue> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public async Task<PagedList<DeferredRevenueResponse>> Handle(SearchDeferredRevenuesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchDeferredRevenuesSpec(request);
        
        var deferredRevenues = await _repository.ListAsync(spec, cancellationToken);
        var totalCount = await _repository.CountAsync(spec, cancellationToken);

        var responses = deferredRevenues.Select(d => new DeferredRevenueResponse
        {
            Id = d.Id,
            DeferredRevenueNumber = d.DeferredRevenueNumber,
            RecognitionDate = d.RecognitionDate,
            Amount = d.Amount,
            Description = d.Description,
            IsRecognized = d.IsRecognized,
            RecognizedDate = d.RecognizedDate
        }).ToList();

        return new PagedList<DeferredRevenueResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}
