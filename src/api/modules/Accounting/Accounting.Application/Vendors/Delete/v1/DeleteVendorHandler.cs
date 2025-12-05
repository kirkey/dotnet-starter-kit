namespace Accounting.Application.Vendors.Delete.v1;

public sealed class DeleteVendorHandler(
    ILogger<DeleteVendorHandler> logger,
    [FromKeyedServices("accounting:vendors")] IRepository<Vendor> repository)
    : IRequestHandler<DeleteVendorCommand>
{
    public async Task Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new Exception($"Vendor with id {request.Id} not found");
        await repository.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("vendor with id: {VendorId} successfully deleted", entity.Id);
    }
}
