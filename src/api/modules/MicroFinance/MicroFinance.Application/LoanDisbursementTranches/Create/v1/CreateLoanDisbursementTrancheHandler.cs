using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Create.v1;

public sealed class CreateLoanDisbursementTrancheHandler(
    [FromKeyedServices("microfinance:loandisbursementtranches")] IRepository<LoanDisbursementTranche> repository,
    ILogger<CreateLoanDisbursementTrancheHandler> logger)
    : IRequestHandler<CreateLoanDisbursementTrancheCommand, CreateLoanDisbursementTrancheResponse>
{
    public async Task<CreateLoanDisbursementTrancheResponse> Handle(
        CreateLoanDisbursementTrancheCommand request,
        CancellationToken cancellationToken)
    {
        var tranche = LoanDisbursementTranche.Create(
            request.LoanId,
            request.TrancheSequence,
            request.TrancheNumber,
            request.ScheduledDate,
            request.Amount,
            request.DisbursementMethod,
            request.Milestone,
            request.BankAccountNumber,
            request.BankName,
            request.Deductions);

        await repository.AddAsync(tranche, cancellationToken);

        logger.LogInformation("Loan disbursement tranche created: {TrancheId}", tranche.Id);

        return new CreateLoanDisbursementTrancheResponse(tranche.Id);
    }
}
