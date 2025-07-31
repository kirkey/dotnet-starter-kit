using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Vendors.Update.v1;
public sealed class VendorUpdateHandler(
    ILogger<VendorUpdateHandler> logger,
    [FromKeyedServices("accounting:vendors")] IRepository<Vendor> repository)
    : IRequestHandler<VendorUpdateCommand, VendorUpdateResponse>
{
    public async Task<VendorUpdateResponse> Handle(VendorUpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var vendor = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = vendor ?? throw new Exception($"Vendor with id {request.Id} not found");
        var updatedVendor = vendor.Update(
            request.VendorCode,
            request.Name,
            request.Address,
            request.ExpenseAccountCode,
            request.ExpenseAccountName,
            request.Tin,
            request.Phone,
            request.Description,
            request.Notes);
        await repository.UpdateAsync(updatedVendor, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("vendor with id: {VendorId} updated.", updatedVendor.Id);
        return new VendorUpdateResponse(updatedVendor.Id);
    }
}
