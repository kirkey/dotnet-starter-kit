using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using FSH.Starter.WebApi.Warehouse.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.Sales.Update.v1;

public sealed class UpdateSaleHandler(
    ILogger<UpdateSaleHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<Sale> repository)
    : IRequestHandler<UpdateSaleCommand, UpdateSaleResponse>
{
    public async Task<UpdateSaleResponse> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new SaleNotFoundException(request.Id);
        entity.Update(request.SaleNumber, request.SaleDate, request.Status, request.SubTotal, request.TaxAmount, request.DiscountAmount, request.TotalAmount, request.AmountPaid, request.ChangeAmount, request.CustomerName, request.CustomerPhone, request.CashierName, request.Notes);
        await repository.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("sale updated {SaleId}", entity.Id);
        return new UpdateSaleResponse(entity.Id);
    }
}

