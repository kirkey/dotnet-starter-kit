namespace Accounting.Application.FixedAssets.UpdateMaintenance.v1;

public sealed class UpdateMaintenanceHandler(
    IRepository<FixedAsset> repository,
    ILogger<UpdateMaintenanceHandler> logger)
    : IRequestHandler<UpdateMaintenanceCommand, DefaultIdType>
{
    private readonly IRepository<FixedAsset> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly ILogger<UpdateMaintenanceHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<DefaultIdType> Handle(UpdateMaintenanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogInformation("Updating maintenance schedule for fixed asset {Id}", request.Id);

        var asset = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (asset == null) throw new NotFoundException($"Fixed asset with ID {request.Id} not found");

        asset.UpdateMaintenance(request.LastMaintenanceDate, request.NextMaintenanceDate);
        await _repository.UpdateAsync(asset, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Maintenance schedule updated for {AssetName}", asset.AssetName);
        return asset.Id;
    }
}
