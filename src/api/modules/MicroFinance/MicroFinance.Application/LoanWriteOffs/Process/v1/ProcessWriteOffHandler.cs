using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Process.v1;

public sealed class ProcessWriteOffHandler(
    [FromKeyedServices("microfinance:loanwriteoffs")] IRepository<LoanWriteOff> repository,
    ILogger<ProcessWriteOffHandler> logger)
    : IRequestHandler<ProcessWriteOffCommand, ProcessWriteOffResponse>
{
    public async Task<ProcessWriteOffResponse> Handle(
        ProcessWriteOffCommand request,
        CancellationToken cancellationToken)
    {
        var writeOff = await repository.FirstOrDefaultAsync(
            new LoanWriteOffByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan write-off {request.Id} not found");

        writeOff.Process();
        await repository.UpdateAsync(writeOff, cancellationToken);

        logger.LogInformation("Loan write-off processed: {WriteOffId}, Total: {TotalWriteOff}",
            writeOff.Id, writeOff.TotalWriteOff);

        return new ProcessWriteOffResponse(writeOff.Id, writeOff.Status, writeOff.TotalWriteOff);
    }
}
