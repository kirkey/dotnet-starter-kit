using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.TransferCash.v1;

/// <summary>
/// Handler for transferring cash to/from a teller session.
/// </summary>
public sealed class TransferCashHandler(
    [FromKeyedServices("microfinance:tellersessions")] IRepository<TellerSession> repository,
    ILogger<TransferCashHandler> logger)
    : IRequestHandler<TransferCashCommand, TransferCashResponse>
{
    public async Task<TransferCashResponse> Handle(TransferCashCommand request, CancellationToken cancellationToken)
    {
        var session = await repository.GetByIdAsync(request.TellerSessionId, cancellationToken)
            ?? throw new NotFoundException($"Teller session with ID {request.TellerSessionId} not found.");

        session.TransferCash(request.Amount, request.IsTransferIn, request.Reference);

        await repository.UpdateAsync(session, cancellationToken);
        
        var direction = request.IsTransferIn ? "in" : "out";
        logger.LogInformation("Transferred {Amount} {Direction} for teller session {TellerSessionId}", 
            request.Amount, direction, request.TellerSessionId);

        return new TransferCashResponse(
            session.Id, 
            request.Amount,
            request.IsTransferIn,
            session.ExpectedClosingBalance,
            $"Cash transfer {direction} completed successfully.");
    }
}
