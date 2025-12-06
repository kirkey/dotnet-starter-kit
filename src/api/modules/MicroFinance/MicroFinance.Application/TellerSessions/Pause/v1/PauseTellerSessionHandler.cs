using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Pause.v1;

public sealed class PauseTellerSessionHandler(
    ILogger<PauseTellerSessionHandler> logger,
    [FromKeyedServices("microfinance:tellersessions")] IRepository<TellerSession> repository)
    : IRequestHandler<PauseTellerSessionCommand, PauseTellerSessionResponse>
{
    public async Task<PauseTellerSessionResponse> Handle(PauseTellerSessionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var session = await repository.FirstOrDefaultAsync(
            new TellerSessionByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (session is null)
            throw new NotFoundException($"Teller session with ID {request.Id} not found.");

        session.Pause();

        await repository.UpdateAsync(session, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Teller session paused: {SessionId}", session.Id);

        return new PauseTellerSessionResponse(session.Id, session.Status);
    }
}
