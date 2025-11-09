using FSH.Starter.WebApi.Store.Application.Bins.Get.v1;
using FSH.Starter.WebApi.Store.Application.Bins.Specs;

namespace FSH.Starter.WebApi.Store.Application.Bins.Search.v1;

public sealed class SearchBinsHandler(
    [FromKeyedServices("store:bins")] IReadRepository<Bin> repository)
    : IRequestHandler<SearchBinsCommand, PagedList<BinResponse>>
{
    public async Task<PagedList<BinResponse>> Handle(SearchBinsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchBinsSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<BinResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
