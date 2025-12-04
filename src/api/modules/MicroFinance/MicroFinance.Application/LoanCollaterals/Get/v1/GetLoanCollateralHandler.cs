using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Get.v1;

public sealed class GetLoanCollateralHandler(
    [FromKeyedServices("microfinance:loancollaterals")] IRepository<LoanCollateral> repository)
    : IRequestHandler<GetLoanCollateralRequest, LoanCollateralResponse>
{
    public async Task<LoanCollateralResponse> Handle(GetLoanCollateralRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var collateral = await repository.FirstOrDefaultAsync(
            new LoanCollateralByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (collateral is null)
        {
            throw new NotFoundException($"Loan collateral with ID {request.Id} not found.");
        }

        return new LoanCollateralResponse(
            collateral.Id,
            collateral.LoanId,
            collateral.CollateralType,
            collateral.Description,
            collateral.EstimatedValue,
            collateral.ForcedSaleValue,
            collateral.ValuationDate,
            collateral.Location,
            collateral.DocumentReference,
            collateral.Status,
            collateral.Notes
        );
    }
}
