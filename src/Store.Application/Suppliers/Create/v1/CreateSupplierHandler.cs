namespace FSH.Starter.WebApi.Store.Application.Suppliers.Create.v1;

public sealed class CreateSupplierHandler(
    ILogger<CreateSupplierHandler> logger,
    [FromKeyedServices("store:suppliers")] IRepository<Supplier> repository)
    : IRequestHandler<CreateSupplierCommand, CreateSupplierResponse>
{
    public async Task<CreateSupplierResponse> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var supplier = Supplier.Create(
            request.Name,
            request.Description,
            request.Code,
            request.ContactPerson,
            request.Email,
            request.Phone,
            request.Address,
            request.City,
            request.State,
            request.Country,
            request.PostalCode,
            request.Website,
            request.CreditLimit,
            request.PaymentTermsDays,
            request.IsActive,
            request.Rating,
            request.Notes
        );

        await repository.AddAsync(supplier, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Supplier created {SupplierId}", supplier.Id);
        return new CreateSupplierResponse(supplier.Id);
    }
}

