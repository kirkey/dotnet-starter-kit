using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Sales.Create.v1;

public sealed class CreateSaleHandler(
    ILogger<CreateSaleHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Sale> repository)
    : IRequestHandler<CreateSaleCommand, CreateSaleResponse>
{
    public async Task<CreateSaleResponse> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var entity = Sale.Create(request.SaleNumber, request.SaleDate, request.SubTotal, request.TaxAmount, request.DiscountAmount, request.TotalAmount, request.AmountPaid, request.ChangeAmount, request.CustomerName, request.CustomerPhone, request.CashierName, request.Notes, request.StoreId, request.CustomerId);
        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("sale created {SaleId}", entity.Id);
        return new CreateSaleResponse(entity.Id);
    }
}

