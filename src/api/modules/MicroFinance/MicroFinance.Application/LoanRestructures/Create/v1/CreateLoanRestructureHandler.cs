using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Create.v1;

public sealed class CreateLoanRestructureHandler(
    [FromKeyedServices("microfinance:loanrestructures")] IRepository<LoanRestructure> repository,
    ILogger<CreateLoanRestructureHandler> logger)
    : IRequestHandler<CreateLoanRestructureCommand, CreateLoanRestructureResponse>
{
    public async Task<CreateLoanRestructureResponse> Handle(
        CreateLoanRestructureCommand request,
        CancellationToken cancellationToken)
    {
        var restructure = LoanRestructure.Create(
            request.LoanId,
            request.RestructureNumber,
            request.RestructureType,
            request.OriginalPrincipal,
            request.OriginalInterestRate,
            request.OriginalRemainingTerm,
            request.OriginalInstallmentAmount,
            request.NewPrincipal,
            request.NewInterestRate,
            request.NewTerm,
            request.NewInstallmentAmount,
            request.Reason,
            request.GracePeriodMonths,
            request.WaivedAmount,
            request.RestructureFee);

        await repository.AddAsync(restructure, cancellationToken);

        logger.LogInformation("Loan restructure created: {RestructureId} for loan {LoanId}",
            restructure.Id, request.LoanId);

        return new CreateLoanRestructureResponse(restructure.Id);
    }
}
