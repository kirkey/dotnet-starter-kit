using FSH.Starter.WebApi.Store.Application.Bins.Specs;
using Store.Domain.Exceptions.Bin;

namespace FSH.Starter.WebApi.Store.Application.Bins.Create.v1;

public sealed class CreateBinHandler(
    ILogger<CreateBinHandler> logger,
    [FromKeyedServices("store:bins")] IRepository<Bin> repository,
    [FromKeyedServices("store:bins")] IReadRepository<Bin> readRepository)
    : IRequestHandler<CreateBinCommand, CreateBinResponse>
{
    public async Task<CreateBinResponse> Handle(CreateBinCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate code within the warehouse location
        var existingByCode = await readRepository.FirstOrDefaultAsync(
            new BinByCodeSpec(request.Code!, request.WarehouseLocationId!.Value), 
            cancellationToken).ConfigureAwait(false);
        
        if (existingByCode is not null)
            throw new DuplicateBinCodeException(request.Code!, request.WarehouseLocationId!.Value);

        var bin = Bin.Create(
            request.Name!,
            request.Description,
            request.Code!,
            request.WarehouseLocationId!.Value,
            request.BinType!,
            request.Capacity,
            request.Priority);

        await repository.AddAsync(bin, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("bin created {BinId}", bin.Id);
        return new CreateBinResponse(bin.Id);
    }
}
