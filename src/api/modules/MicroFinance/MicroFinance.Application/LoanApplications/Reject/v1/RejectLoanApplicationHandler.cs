using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Reject.v1;

/// <summary>
/// Handler for rejecting a loan application.
/// </summary>
public sealed class RejectLoanApplicationHandler(
    [FromKeyedServices("microfinance:loanapplications")] IRepository<LoanApplication> repository,
    ILogger<RejectLoanApplicationHandler> logger) : IRequestHandler<RejectLoanApplicationCommand, RejectLoanApplicationResponse>
{
    public async Task<RejectLoanApplicationResponse> Handle(RejectLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var application = await repository.FirstOrDefaultAsync(new LoanApplicationByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Loan application with ID {request.Id} not found.");

        application.Reject(request.RejectedById, request.Reason);

        await repository.UpdateAsync(application, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Loan application {ApplicationNumber} rejected: {Reason}", 
            application.ApplicationNumber, request.Reason);

        return new RejectLoanApplicationResponse(application.Id, application.Status);
    }
}
