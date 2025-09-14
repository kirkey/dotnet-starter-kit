using Accounting.Application.Vendors.Exceptions;
using Accounting.Application.Vendors.Queries;

namespace Accounting.Application.Vendors.Create.v1;
public sealed class VendorCreateHandler(
    ILogger<VendorCreateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Vendor> repository)
    : IRequestHandler<VendorCreateCommand, VendorCreateResponse>
{
    public async Task<VendorCreateResponse> Handle(VendorCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate vendor code
        var existingByCode = await repository.FirstOrDefaultAsync(
            new VendorByCodeSpec(request.VendorCode), cancellationToken);
        if (existingByCode != null)
        {
            throw new VendorCodeAlreadyExistsException(request.VendorCode);
        }

        // Check for duplicate vendor name
        var existingByName = await repository.FirstOrDefaultAsync(
            new VendorByNameSpec(request.Name), cancellationToken);
        if (existingByName != null)
        {
            throw new VendorNameAlreadyExistsException(request.Name);
        }

        var entity = Vendor.Create(
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

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Vendor created {VendorId}", entity.Id);
        return new VendorCreateResponse(entity.Id);
    }
}
