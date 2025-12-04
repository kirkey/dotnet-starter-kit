using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Update.v1;

public sealed class UpdateShareProductHandler(
    [FromKeyedServices("microfinance:shareproducts")] IRepository<ShareProduct> repository,
    ILogger<UpdateShareProductHandler> logger)
    : IRequestHandler<UpdateShareProductCommand, UpdateShareProductResponse>
{
    public async Task<UpdateShareProductResponse> Handle(UpdateShareProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var shareProduct = await repository.FirstOrDefaultAsync(
            new ShareProductByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (shareProduct is null)
        {
            throw new NotFoundException($"Share product with ID {request.Id} not found.");
        }

        shareProduct.Update(
            request.Name,
            request.Description,
            request.CurrentPrice,
            request.MaxSharesPerMember,
            request.AllowTransfer,
            request.AllowRedemption,
            request.MinHoldingPeriodMonths,
            request.PaysDividends
        );

        await repository.UpdateAsync(shareProduct, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Share product {ShareProductId} updated", shareProduct.Id);

        return new UpdateShareProductResponse(shareProduct.Id);
    }
}
