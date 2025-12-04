using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Get.v1;

public sealed class GetLoanWriteOffHandler(
    [FromKeyedServices("microfinance:loanwriteoffs")] IReadRepository<LoanWriteOff> repository)
    : IRequestHandler<GetLoanWriteOffRequest, LoanWriteOffResponse>
{
    public async Task<LoanWriteOffResponse> Handle(
        GetLoanWriteOffRequest request,
        CancellationToken cancellationToken)
    {
        var writeOff = await repository.FirstOrDefaultAsync(
            new LoanWriteOffByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan write-off {request.Id} not found");

        return new LoanWriteOffResponse(
            writeOff.Id,
            writeOff.LoanId,
            writeOff.WriteOffNumber,
            writeOff.WriteOffType,
            writeOff.Reason,
            writeOff.RequestDate,
            writeOff.WriteOffDate,
            writeOff.PrincipalWriteOff,
            writeOff.InterestWriteOff,
            writeOff.PenaltiesWriteOff,
            writeOff.FeesWriteOff,
            writeOff.TotalWriteOff,
            writeOff.RecoveredAmount,
            writeOff.DaysPastDue,
            writeOff.CollectionAttempts,
            writeOff.Status,
            writeOff.ApprovedByUserId,
            writeOff.ApprovedBy,
            writeOff.ApprovedAt);
    }
}
