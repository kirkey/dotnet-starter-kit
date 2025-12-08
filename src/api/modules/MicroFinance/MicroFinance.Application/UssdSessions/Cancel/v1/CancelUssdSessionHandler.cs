using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Cancel.v1;

/// <summary>
/// Handler for cancelling a USSD session.
/// </summary>
public sealed class CancelUssdSessionHandler(
    ILogger<CancelUssdSessionHandler> logger,
    [FromKeyedServices("microfinance:ussdsessions")] IRepository<UssdSession> repository)
    : IRequestHandler<CancelUssdSessionCommand, CancelUssdSessionResponse>
{
    /// <summary>
    /// Handles the cancel USSD session command.
    /// </summary>
    public async Task<CancelUssdSessionResponse> Handle(
        CancelUssdSessionCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var session = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new KeyNotFoundException($"USSD session with ID {request.Id} not found.");

        session.Cancel();

        await repository.UpdateAsync(session, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("USSD session {SessionId} cancelled", session.SessionId);

        return new CancelUssdSessionResponse(
            session.Id,
            session.SessionId,
            session.Status,
            session.EndedAt!.Value);
    }
}
