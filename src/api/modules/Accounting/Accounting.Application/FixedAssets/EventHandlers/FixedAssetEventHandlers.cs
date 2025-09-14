using Accounting.Domain.Events.FixedAsset;

namespace Accounting.Application.FixedAssets.EventHandlers;

public sealed class FixedAssetCreatedEventHandler(ILogger<FixedAssetCreatedEventHandler> logger)
    : INotificationHandler<FixedAssetCreated>
{
    public Task Handle(FixedAssetCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fixed Asset created: {AssetId} - {AssetName} - Purchase Price: {PurchasePrice}", 
            notification.Id, notification.AssetName, notification.PurchasePrice);
        return Task.CompletedTask;
    }
}

public sealed class FixedAssetUpdatedEventHandler(ILogger<FixedAssetUpdatedEventHandler> logger)
    : INotificationHandler<FixedAssetUpdated>
{
    public Task Handle(FixedAssetUpdated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fixed Asset updated: {AssetId}", notification.Id);
        return Task.CompletedTask;
    }
}

public sealed class FixedAssetDisposedEventHandler(ILogger<FixedAssetDisposedEventHandler> logger)
    : INotificationHandler<FixedAssetDisposed>
{
    public Task Handle(FixedAssetDisposed notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Fixed Asset disposed: {AssetId} - Disposal Amount: {DisposalAmount} - Gain/Loss: {GainLoss}",
            notification.Id, notification.DisposalAmount, 0); //notification.GainLoss);
        return Task.CompletedTask;
    }
}
