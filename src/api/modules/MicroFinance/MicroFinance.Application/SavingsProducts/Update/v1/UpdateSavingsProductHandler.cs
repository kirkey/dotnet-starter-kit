using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Update.v1;

/// <summary>
/// Handler for updating a savings product.
/// </summary>
public sealed class UpdateSavingsProductHandler(
    ILogger<UpdateSavingsProductHandler> logger,
    [FromKeyedServices("microfinance:savingsproducts")] IRepository<SavingsProduct> repository,
    ICacheService cacheService)
    : IRequestHandler<UpdateSavingsProductCommand, UpdateSavingsProductResponse>
{
    public async Task<UpdateSavingsProductResponse> Handle(UpdateSavingsProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var savingsProduct = await repository.FirstOrDefaultAsync(
            new SavingsProductByIdSpec(request.Id),
            cancellationToken).ConfigureAwait(false);

        if (savingsProduct is null)
        {
            throw new NotFoundException($"Savings product with ID {request.Id} not found.");
        }

        savingsProduct.Update(
            request.Name,
            request.Description,
            request.InterestRate,
            request.InterestCalculation,
            request.InterestPostingFrequency,
            request.MinOpeningBalance,
            request.MinBalanceForInterest,
            request.MinWithdrawalAmount,
            request.MaxWithdrawalPerDay,
            request.AllowOverdraft,
            request.OverdraftLimit);

        await repository.UpdateAsync(savingsProduct, cancellationToken).ConfigureAwait(false);

        // Invalidate cache
        await cacheService.RemoveAsync($"savingsproduct:{request.Id}", cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Updated savings product {SavingsProductId}", request.Id);

        return new UpdateSavingsProductResponse(savingsProduct.Id);
    }
}
