using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Create.v1;

public sealed class CreateSavingsProductHandler(
    [FromKeyedServices("microfinance:savingsproducts")] IRepository<SavingsProduct> repository,
    ILogger<CreateSavingsProductHandler> logger)
    : IRequestHandler<CreateSavingsProductCommand, CreateSavingsProductResponse>
{
    public async Task<CreateSavingsProductResponse> Handle(CreateSavingsProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate product code
        var existingProduct = await repository.FirstOrDefaultAsync(
            new SavingsProductByCodeSpec(request.Code), cancellationToken).ConfigureAwait(false);

        if (existingProduct is not null)
        {
            throw new InvalidOperationException($"A savings product with code '{request.Code}' already exists.");
        }

        var savingsProduct = SavingsProduct.Create(
            request.Code,
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
            request.OverdraftLimit
        );

        await repository.AddAsync(savingsProduct, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Savings product {Code} created with ID {SavingsProductId}", savingsProduct.Code, savingsProduct.Id);

        return new CreateSavingsProductResponse(savingsProduct.Id, savingsProduct.Code);
    }
}
