using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareProducts.Get.v1;

public sealed class GetShareProductHandler(
    [FromKeyedServices("microfinance:shareproducts")] IRepository<ShareProduct> repository)
    : IRequestHandler<GetShareProductRequest, ShareProductResponse>
{
    public async Task<ShareProductResponse> Handle(GetShareProductRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var shareProduct = await repository.FirstOrDefaultAsync(
            new ShareProductByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (shareProduct is null)
        {
            throw new NotFoundException($"Share product with ID {request.Id} not found.");
        }

        return new ShareProductResponse(
            shareProduct.Id,
            shareProduct.Code,
            shareProduct.Name,
            shareProduct.Description,
            shareProduct.NominalValue,
            shareProduct.CurrentPrice,
            shareProduct.MinSharesForMembership,
            shareProduct.MaxSharesPerMember,
            shareProduct.AllowTransfer,
            shareProduct.AllowRedemption,
            shareProduct.MinHoldingPeriodMonths,
            shareProduct.PaysDividends,
            shareProduct.IsActive
        );
    }
}
