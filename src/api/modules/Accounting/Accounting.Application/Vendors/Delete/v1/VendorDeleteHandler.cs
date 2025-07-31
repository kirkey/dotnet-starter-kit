using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Vendors.Delete.v1;

public sealed class VendorDeleteHandler(
    ILogger<VendorDeleteHandler> logger,
    [FromKeyedServices("accounting:vendors")] IRepository<Vendor> repository)
    : IRequestHandler<VendorDeleteCommand>
{
    public async Task Handle(VendorDeleteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new Exception($"Vendor with id {request.Id} not found");
        await repository.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("vendor with id: {VendorId} successfully deleted", entity.Id);
    }
}
