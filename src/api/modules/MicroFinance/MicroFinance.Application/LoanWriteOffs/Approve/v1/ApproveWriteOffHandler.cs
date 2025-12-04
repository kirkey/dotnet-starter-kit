using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Approve.v1;

public sealed class ApproveWriteOffHandler(
    [FromKeyedServices("microfinance:loanwriteoffs")] IRepository<LoanWriteOff> repository,
    ILogger<ApproveWriteOffHandler> logger)
    : IRequestHandler<ApproveWriteOffCommand, ApproveWriteOffResponse>
{
    public async Task<ApproveWriteOffResponse> Handle(
        ApproveWriteOffCommand request,
        CancellationToken cancellationToken)
    {
        var writeOff = await repository.FirstOrDefaultAsync(
            new LoanWriteOffByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Loan write-off {request.Id} not found");

        writeOff.Approve(request.UserId, request.ApproverName, request.WriteOffDate);
        await repository.UpdateAsync(writeOff, cancellationToken);

        logger.LogInformation("Loan write-off approved: {WriteOffId}", writeOff.Id);

        return new ApproveWriteOffResponse(writeOff.Id, writeOff.Status, request.WriteOffDate);
    }
}
