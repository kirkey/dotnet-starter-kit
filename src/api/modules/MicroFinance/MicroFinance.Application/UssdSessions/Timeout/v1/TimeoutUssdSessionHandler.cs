using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Timeout.v1;

/// <summary>
/// Handler for timing out a USSD session.
/// </summary>
public sealed class TimeoutUssdSessionHandler(
    ILogger<TimeoutUssdSessionHandler> logger,
    [FromKeyedServices("microfinance:ussdsessions")] IRepository<UssdSession> repository)
    : IRequestHandler<TimeoutUssdSessionCommand, TimeoutUssdSessionResponse>
{
    /// <summary>
    /// Handles the timeout USSD session command.
    /// </summary>
    public async Task<TimeoutUssdSessionResponse> Handle(
        TimeoutUssdSessionCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var session = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new KeyNotFoundException($"USSD session with ID {request.Id} not found.");

        session.Timeout();

        await repository.UpdateAsync(session, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("USSD session {SessionId} timed out", session.SessionId);

        return new TimeoutUssdSessionResponse(
            session.Id,
            session.SessionId,
            session.Status,
            session.EndedAt!.Value);
    }
}
