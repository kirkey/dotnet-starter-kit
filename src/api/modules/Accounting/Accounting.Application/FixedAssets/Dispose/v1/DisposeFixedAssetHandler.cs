namespace Accounting.Application.FixedAssets.Dispose.v1;

public sealed class DisposeFixedAssetHandler(
    IRepository<FixedAsset> repository,
    ILogger<DisposeFixedAssetHandler> logger)
    : IRequestHandler<DisposeFixedAssetCommand, DefaultIdType>
{
    private readonly IRepository<FixedAsset> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<DisposeFixedAssetHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(DisposeFixedAssetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Disposing fixed asset {Id} on {DisposalDate}", request.Id, request.DisposalDate);

        var asset = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (asset == null) throw new NotFoundException($"Fixed asset with ID {request.Id} not found");

        asset.MarkAsDisposed(request.DisposalDate, request.DisposalAmount, request.DisposalReason);
        await _repository.UpdateAsync(asset, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Fixed asset {AssetName} disposed successfully", asset.AssetName);
        return asset.Id;
    }
}

