using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Create.v1;

/// <summary>
/// Handler for creating a new loan application.
/// </summary>
public sealed class CreateLoanApplicationHandler(
    [FromKeyedServices("microfinance:loanapplications")] IRepository<LoanApplication> repository,
    ILogger<CreateLoanApplicationHandler> logger) : IRequestHandler<CreateLoanApplicationCommand, CreateLoanApplicationResponse>
{
    public async Task<CreateLoanApplicationResponse> Handle(CreateLoanApplicationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var applicationNumber = $"LA-{DateTime.UtcNow:yyyyMMdd}-{DefaultIdType.NewGuid().ToString()[..8].ToUpperInvariant()}";

        var application = LoanApplication.Create(
            applicationNumber,
            request.MemberId,
            request.ProductId,
            request.RequestedAmount,
            request.RequestedTermMonths,
            request.Purpose,
            request.GroupId);

        await repository.AddAsync(application, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Loan application {ApplicationNumber} created for member {MemberId}", applicationNumber, request.MemberId);

        return new CreateLoanApplicationResponse(application.Id, applicationNumber);
    }
}
