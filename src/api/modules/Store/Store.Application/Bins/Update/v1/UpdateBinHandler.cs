using Store.Domain.Exceptions.Bin;

namespace FSH.Starter.WebApi.Store.Application.Bins.Update.v1;

public sealed class UpdateBinHandler(
    ILogger<UpdateBinHandler> logger,
    [FromKeyedServices("store:bins")] IRepository<Bin> repository)
    : IRequestHandler<UpdateBinCommand, UpdateBinResponse>
{
    public async Task<UpdateBinResponse> Handle(UpdateBinCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bin = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (bin is null)
            throw new BinNotFoundException(request.Id);

        bin.Update(
            request.Name,
            request.Description,
            request.BinType,
            request.Capacity,
            request.Priority,
            request.IsPickable,
            request.IsPutable);

        await repository.UpdateAsync(bin, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("bin updated {BinId}", bin.Id);
        return new UpdateBinResponse(bin.Id);
    }
}
