using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Loans.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Create.v1;

public sealed class CreateLoanCollateralHandler(
    [FromKeyedServices("microfinance:loans")] IReadRepository<Loan> loanRepository,
    [FromKeyedServices("microfinance:loancollaterals")] IRepository<LoanCollateral> collateralRepository,
    ILogger<CreateLoanCollateralHandler> logger)
    : IRequestHandler<CreateLoanCollateralCommand, CreateLoanCollateralResponse>
{
    public async Task<CreateLoanCollateralResponse> Handle(CreateLoanCollateralCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify loan exists
        var loan = await loanRepository.FirstOrDefaultAsync(
            new LoanByIdSpec(request.LoanId), cancellationToken).ConfigureAwait(false);

        if (loan is null)
        {
            throw new NotFoundException($"Loan with ID {request.LoanId} not found.");
        }

        // Create the collateral
        var collateral = LoanCollateral.Create(
            request.LoanId,
            request.CollateralType,
            request.Description,
            request.EstimatedValue,
            request.ForcedSaleValue,
            request.ValuationDate,
            request.Location,
            request.DocumentReference,
            request.Notes);

        await collateralRepository.AddAsync(collateral, cancellationToken).ConfigureAwait(false);
        await collateralRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Created loan collateral {CollateralId} for loan {LoanId}. Type: {Type}, Value: {Value}",
            collateral.Id, request.LoanId, request.CollateralType, request.EstimatedValue);

        return new CreateLoanCollateralResponse(collateral.Id);
    }
}
