using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.RecordRecovery.v1;

public sealed class RecordRecoveryHandler(
    [FromKeyedServices("microfinance:loanwriteoffs")] IRepository<LoanWriteOff> repository,
    ILogger<RecordRecoveryHandler> logger)
    : IRequestHandler<RecordRecoveryCommand, RecordRecoveryResponse>
{
    public async Task<RecordRecoveryResponse> Handle(
        RecordRecoveryCommand request,
        CancellationToken cancellationToken)
    {
        var writeOff = await repository.FirstOrDefaultAsync(
            new LoanWriteOffByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan write-off {request.Id} not found");

        writeOff.RecordRecovery(request.Amount);
        await repository.UpdateAsync(writeOff, cancellationToken);

        logger.LogInformation("Recovery recorded for write-off: {WriteOffId}, Amount: {Amount}",
            writeOff.Id, request.Amount);

        return new RecordRecoveryResponse(
            writeOff.Id,
            writeOff.RecoveredAmount,
            writeOff.TotalWriteOff,
            writeOff.TotalWriteOff - writeOff.RecoveredAmount);
    }
}
