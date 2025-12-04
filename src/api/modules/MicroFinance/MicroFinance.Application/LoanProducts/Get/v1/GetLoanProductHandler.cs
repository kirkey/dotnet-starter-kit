using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Get.v1;

public sealed class GetLoanProductHandler(
    [FromKeyedServices("microfinance:loanproducts")] IReadRepository<LoanProduct> repository)
    : IRequestHandler<GetLoanProductRequest, LoanProductResponse>
{
    public async Task<LoanProductResponse> Handle(GetLoanProductRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var loanProduct = await repository.FirstOrDefaultAsync(
            new LoanProductByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (loanProduct is null)
        {
            throw new NotFoundException($"Loan product with ID {request.Id} not found.");
        }

        return new LoanProductResponse(
            loanProduct.Id,
            loanProduct.Code,
            loanProduct.Name,
            loanProduct.Description,
            loanProduct.CurrencyCode,
            loanProduct.MinLoanAmount,
            loanProduct.MaxLoanAmount,
            loanProduct.InterestRate,
            loanProduct.InterestMethod,
            loanProduct.MinTermMonths,
            loanProduct.MaxTermMonths,
            loanProduct.RepaymentFrequency,
            loanProduct.GracePeriodDays,
            loanProduct.LatePenaltyRate,
            loanProduct.IsActive
        );
    }
}
