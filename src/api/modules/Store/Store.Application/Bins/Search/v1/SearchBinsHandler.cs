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

        var bins = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var binResponses = bins.Select(b => new BinResponse(
            b.Id,
            b.Name,
            b.Description,
            b.Code,
            b.WarehouseLocationId,
            b.BinType,
            b.Capacity,
            b.CurrentUtilization,
            b.IsActive,
            b.IsPickable,
            b.IsPutable,
            b.Priority,
            b.CreatedOn,
            b.CreatedBy)).ToList();

        return new PagedList<BinResponse>(binResponses, request.PageNumber, request.PageSize, totalCount);
    }
}
