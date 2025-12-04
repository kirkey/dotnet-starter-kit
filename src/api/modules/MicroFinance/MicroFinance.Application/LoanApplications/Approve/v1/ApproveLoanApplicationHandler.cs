using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Approve.v1;

/// <summary>
/// Handler for approving a loan application.
/// </summary>
public sealed class ApproveLoanApplicationHandler(
    [FromKeyedServices("microfinance:loanapplications")] IRepository<LoanApplication> repository,
    ILogger<ApproveLoanApplicationHandler> logger) : IRequestHandler<ApproveLoanApplicationCommand, ApproveLoanApplicationResponse>
{
    public async Task<ApproveLoanApplicationResponse> Handle(ApproveLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var application = await repository.FirstOrDefaultAsync(new LoanApplicationByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Loan application with ID {request.Id} not found.");

        application.Approve(
            request.ApproverId,
            request.ApprovedAmount,
            request.ApprovedTermMonths);

        await repository.UpdateAsync(application, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Loan application {ApplicationNumber} approved for {ApprovedAmount}", 
            application.ApplicationNumber, request.ApprovedAmount);

        return new ApproveLoanApplicationResponse(
            application.Id, 
            request.ApprovedAmount, 
            request.ApprovedTermMonths,
            request.ApprovedInterestRate,
            application.Status);
    }
}
