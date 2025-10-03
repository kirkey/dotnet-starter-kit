using FSH.Starter.WebApi.Store.Application.Bins.Specs;
using Store.Domain.Exceptions.Bin;

namespace FSH.Starter.WebApi.Store.Application.Bins.Get.v1;

public sealed class GetBinHandler(
    [FromKeyedServices("store:bins")] IReadRepository<Bin> repository)
    : IRequestHandler<GetBinRequest, BinResponse>
{
    public async Task<BinResponse> Handle(GetBinRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bin = await repository.FirstOrDefaultAsync(
            new GetBinByIdSpec(request.Id), 
            cancellationToken).ConfigureAwait(false);

        if (bin is null)
            throw new BinNotFoundException(request.Id);

        return new BinResponse(
            bin.Id,
            bin.Name,
            bin.Description,
            bin.Code,
            bin.WarehouseLocationId,
            bin.BinType,
            bin.Capacity,
            bin.CurrentUtilization,
            bin.IsActive,
            bin.IsPickable,
            bin.IsPutable,
            bin.Priority,
            bin.CreatedOn,
            bin.CreatedBy);
    }
}
