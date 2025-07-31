using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Vendors.Create.v1;
public sealed class VendorCreateHandler(
    ILogger<VendorCreateHandler> logger,
    [FromKeyedServices("accounting:vendors")] IRepository<Vendor> repository)
    : IRequestHandler<VendorCreateCommand, VendorCreateResponse>
{
    public async Task<VendorCreateResponse> Handle(VendorCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = Vendor.Create(
            request.VendorCode,
            request.Name,
            request.Address,
            request.ExpenseAccountCode,
            request.ExpenseAccountName,
            request.Tin,
            request.Phone,
            request.Description,
            request.Notes);
        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("vendor created {VendorId}", entity.Id);
        return new VendorCreateResponse(entity.Id);
    }
}
