using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Review.v1;

/// <summary>
/// Handler for completing review of a loan application.
/// </summary>
public sealed class ReviewLoanApplicationHandler(
    [FromKeyedServices("microfinance:loanapplications")] IRepository<LoanApplication> repository,
    ILogger<ReviewLoanApplicationHandler> logger) : IRequestHandler<ReviewLoanApplicationCommand, ReviewLoanApplicationResponse>
{
    public async Task<ReviewLoanApplicationResponse> Handle(ReviewLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var application = await repository.FirstOrDefaultAsync(new LoanApplicationByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Loan application with ID {request.Id} not found.");

        application.SubmitForApproval();

        await repository.UpdateAsync(application, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Loan application {ApplicationNumber} review completed", application.ApplicationNumber);

        return new ReviewLoanApplicationResponse(application.Id, application.Status);
    }
}
