using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Assign.v1;

/// <summary>
/// Handler for assigning a loan application to an officer.
/// </summary>
public sealed class AssignLoanApplicationHandler(
    [FromKeyedServices("microfinance:loanapplications")] IRepository<LoanApplication> repository,
    ILogger<AssignLoanApplicationHandler> logger) : IRequestHandler<AssignLoanApplicationCommand, AssignLoanApplicationResponse>
{
    public async Task<AssignLoanApplicationResponse> Handle(AssignLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var application = await repository.FirstOrDefaultAsync(new LoanApplicationByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Loan application with ID {request.Id} not found.");

        application.AssignToOfficer(request.OfficerId);

        await repository.UpdateAsync(application, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Loan application {ApplicationNumber} assigned to officer {OfficerId}", application.ApplicationNumber, request.OfficerId);

        return new AssignLoanApplicationResponse(application.Id, request.OfficerId, application.Status);
    }
}
