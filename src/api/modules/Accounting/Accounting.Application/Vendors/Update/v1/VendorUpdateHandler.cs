using Accounting.Application.Vendors.Exceptions;
using Accounting.Application.Vendors.Queries;

namespace Accounting.Application.Vendors.Update.v1;
public sealed class VendorUpdateHandler(
    ILogger<VendorUpdateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Vendor> repository)
    : IRequestHandler<VendorUpdateCommand, VendorUpdateResponse>
{
    public async Task<VendorUpdateResponse> Handle(VendorUpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var vendor = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (vendor == null) 
            throw new VendorNotFoundException(request.Id);

        // Check for duplicate vendor code (excluding current vendor)
        if (!string.IsNullOrEmpty(request.VendorCode) && request.VendorCode != vendor.VendorCode)
        {
            var existingByCode = await repository.FirstOrDefaultAsync(
                new VendorByCodeSpec(request.VendorCode), cancellationToken);
            if (existingByCode != null)
            {
                throw new VendorCodeAlreadyExistsException(request.VendorCode);
            }
        }

        // Check for duplicate vendor name (excluding current vendor)
        if (!string.IsNullOrEmpty(request.Name) && request.Name != vendor.Name)
        {
            var existingByName = await repository.FirstOrDefaultAsync(
                new VendorByNameSpec(request.Name), cancellationToken);
            if (existingByName != null)
            {
                throw new VendorNameAlreadyExistsException(request.Name);
            }
        }

        var updatedVendor = vendor.Update(
            request.VendorCode,
            request.Name,
            request.Address,
            request.BillingAddress,
            request.ContactPerson,
            request.Email,
            request.Terms,
            request.ExpenseAccountCode,
            request.ExpenseAccountName,
            request.Tin,
            request.Phone,
            request.Description,
            request.Notes);

        await repository.UpdateAsync(updatedVendor, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Vendor with id: {VendorId} updated.", updatedVendor.Id);
        return new VendorUpdateResponse(updatedVendor.Id);
    }
}
