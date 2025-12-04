using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Create.v1;

public sealed class CreateLoanWriteOffHandler(
    [FromKeyedServices("microfinance:loanwriteoffs")] IRepository<LoanWriteOff> repository,
    ILogger<CreateLoanWriteOffHandler> logger)
    : IRequestHandler<CreateLoanWriteOffCommand, CreateLoanWriteOffResponse>
{
    public async Task<CreateLoanWriteOffResponse> Handle(
        CreateLoanWriteOffCommand request,
        CancellationToken cancellationToken)
    {
        var writeOff = LoanWriteOff.Create(
            request.LoanId,
            request.WriteOffNumber,
            request.WriteOffType,
            request.Reason,
            request.PrincipalWriteOff,
            request.InterestWriteOff,
            request.PenaltiesWriteOff,
            request.FeesWriteOff,
            request.DaysPastDue,
            request.CollectionAttempts);

        await repository.AddAsync(writeOff, cancellationToken);

        logger.LogInformation("Loan write-off created: {WriteOffId} for loan {LoanId}",
            writeOff.Id, request.LoanId);

        return new CreateLoanWriteOffResponse(writeOff.Id);
    }
}
