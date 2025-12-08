using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Complete.v1;

/// <summary>
/// Handler for completing a USSD session.
/// </summary>
public sealed class CompleteUssdSessionHandler(
    ILogger<CompleteUssdSessionHandler> logger,
    [FromKeyedServices("microfinance:ussdsessions")] IRepository<UssdSession> repository)
    : IRequestHandler<CompleteUssdSessionCommand, CompleteUssdSessionResponse>
{
    /// <summary>
    /// Handles the complete USSD session command.
    /// </summary>
    public async Task<CompleteUssdSessionResponse> Handle(
        CompleteUssdSessionCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var session = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new KeyNotFoundException($"USSD session with ID {request.Id} not found.");

        session.Complete(request.FinalOutput);

        await repository.UpdateAsync(session, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("USSD session {SessionId} completed with {StepCount} steps", 
            session.SessionId, session.StepCount);

        return new CompleteUssdSessionResponse(
            session.Id,
            session.SessionId,
            session.Status,
            session.StepCount,
            session.EndedAt!.Value);
    }
}
