using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Create.v1;

public sealed class CreateLoanProductHandler(
    [FromKeyedServices("microfinance:loanproducts")] IRepository<LoanProduct> repository,
    ILogger<CreateLoanProductHandler> logger)
    : IRequestHandler<CreateLoanProductCommand, CreateLoanProductResponse>
{
    public async Task<CreateLoanProductResponse> Handle(CreateLoanProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate product code
        var existingProduct = await repository.FirstOrDefaultAsync(
            new LoanProductByCodeSpec(request.Code), cancellationToken).ConfigureAwait(false);

        if (existingProduct is not null)
        {
            throw new InvalidOperationException($"A loan product with code '{request.Code}' already exists.");
        }

        var loanProduct = LoanProduct.Create(
            request.Code,
            request.Name,
            request.Description,
            request.CurrencyCode,
            request.MinLoanAmount,
            request.MaxLoanAmount,
            request.InterestRate,
            request.InterestMethod,
            request.MinTermMonths,
            request.MaxTermMonths,
            request.RepaymentFrequency,
            request.GracePeriodDays,
            request.LatePenaltyRate
        );

        await repository.AddAsync(loanProduct, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Loan product {Code} created with ID {LoanProductId}", loanProduct.Code, loanProduct.Id);

        return new CreateLoanProductResponse(loanProduct.Id, loanProduct.Code);
    }
}
