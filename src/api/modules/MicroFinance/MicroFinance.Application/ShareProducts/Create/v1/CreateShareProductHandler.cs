using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Create.v1;

public sealed class CreateShareProductHandler(
    [FromKeyedServices("microfinance:shareproducts")] IRepository<ShareProduct> repository,
    ILogger<CreateShareProductHandler> logger)
    : IRequestHandler<CreateShareProductCommand, CreateShareProductResponse>
{
    public async Task<CreateShareProductResponse> Handle(CreateShareProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate product code
        var existingProduct = await repository.FirstOrDefaultAsync(
            new ShareProductByCodeSpec(request.Code), cancellationToken).ConfigureAwait(false);

        if (existingProduct is not null)
        {
            throw new InvalidOperationException($"A share product with code '{request.Code}' already exists.");
        }

        var shareProduct = ShareProduct.Create(
            request.Code,
            request.Name,
            request.Description,
            request.NominalValue,
            request.CurrentPrice,
            request.MinSharesForMembership,
            request.MaxSharesPerMember,
            request.AllowTransfer,
            request.AllowRedemption,
            request.MinHoldingPeriodMonths,
            request.PaysDividends
        );

        await repository.AddAsync(shareProduct, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Share product {Code} created with ID {ShareProductId}", shareProduct.Code, shareProduct.Id);

        return new CreateShareProductResponse(shareProduct.Id, shareProduct.Code);
    }
}
