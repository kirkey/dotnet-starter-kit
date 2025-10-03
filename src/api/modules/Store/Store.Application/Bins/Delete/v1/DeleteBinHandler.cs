using Store.Domain.Exceptions.Bin;

namespace FSH.Starter.WebApi.Store.Application.Bins.Delete.v1;

public sealed class DeleteBinHandler(
    ILogger<DeleteBinHandler> logger,
    [FromKeyedServices("store:bins")] IRepository<Bin> repository)
    : IRequestHandler<DeleteBinCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DeleteBinCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bin = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (bin is null)
            throw new BinNotFoundException(request.Id);

        await repository.DeleteAsync(bin, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("bin deleted {BinId}", bin.Id);
        return bin.Id;
    }
}
