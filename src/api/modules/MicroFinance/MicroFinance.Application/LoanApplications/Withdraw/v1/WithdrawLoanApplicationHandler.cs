using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Withdraw.v1;

/// <summary>
/// Handler for withdrawing a loan application.
/// </summary>
public sealed class WithdrawLoanApplicationHandler(
    [FromKeyedServices("microfinance:loanapplications")] IRepository<LoanApplication> repository,
    ILogger<WithdrawLoanApplicationHandler> logger) : IRequestHandler<WithdrawLoanApplicationCommand, WithdrawLoanApplicationResponse>
{
    public async Task<WithdrawLoanApplicationResponse> Handle(WithdrawLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var application = await repository.FirstOrDefaultAsync(new LoanApplicationByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Loan application with ID {request.Id} not found.");

        application.Withdraw();

        await repository.UpdateAsync(application, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Loan application {ApplicationNumber} withdrawn by applicant", application.ApplicationNumber);

        return new WithdrawLoanApplicationResponse(application.Id, application.Status);
    }
}
