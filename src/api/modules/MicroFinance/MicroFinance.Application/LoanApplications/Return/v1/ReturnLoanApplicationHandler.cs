using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Return.v1;

/// <summary>
/// Handler for returning a loan application to the applicant.
/// </summary>
public sealed class ReturnLoanApplicationHandler(
    [FromKeyedServices("microfinance:loanapplications")] IRepository<LoanApplication> repository,
    ILogger<ReturnLoanApplicationHandler> logger)
    : IRequestHandler<ReturnLoanApplicationCommand, ReturnLoanApplicationResponse>
{
    public async Task<ReturnLoanApplicationResponse> Handle(ReturnLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await repository.GetByIdAsync(request.LoanApplicationId, cancellationToken)
            ?? throw new NotFoundException($"Loan application with ID {request.LoanApplicationId} not found.");

        application.Return(request.Reason, request.Notes);

        await repository.UpdateAsync(application, cancellationToken);
        logger.LogInformation("Returned loan application {LoanApplicationId} to applicant. Reason: {Reason}", 
            request.LoanApplicationId, request.Reason);

        return new ReturnLoanApplicationResponse(application.Id, application.Status, "Loan application returned to applicant successfully.");
    }
}
