using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Get.v1;

public sealed class GetSavingsProductHandler(
    [FromKeyedServices("microfinance:savingsproducts")] IRepository<SavingsProduct> repository)
    : IRequestHandler<GetSavingsProductRequest, SavingsProductResponse>
{
    public async Task<SavingsProductResponse> Handle(GetSavingsProductRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var savingsProduct = await repository.FirstOrDefaultAsync(
            new SavingsProductByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (savingsProduct is null)
        {
            throw new NotFoundException($"Savings product with ID {request.Id} not found.");
        }

        return new SavingsProductResponse(
            savingsProduct.Id,
            savingsProduct.Code,
            savingsProduct.Name,
            savingsProduct.Description,
            savingsProduct.CurrencyCode,
            savingsProduct.InterestRate,
            savingsProduct.InterestCalculation,
            savingsProduct.InterestPostingFrequency,
            savingsProduct.MinOpeningBalance,
            savingsProduct.MinBalanceForInterest,
            savingsProduct.MinWithdrawalAmount,
            savingsProduct.MaxWithdrawalPerDay,
            savingsProduct.AllowOverdraft,
            savingsProduct.OverdraftLimit,
            savingsProduct.IsActive
        );
    }
}
