using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Resume.v1;

public sealed class ResumeTellerSessionHandler(
    ILogger<ResumeTellerSessionHandler> logger,
    [FromKeyedServices("microfinance:tellersessions")] IRepository<TellerSession> repository)
    : IRequestHandler<ResumeTellerSessionCommand, ResumeTellerSessionResponse>
{
    public async Task<ResumeTellerSessionResponse> Handle(ResumeTellerSessionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var session = await repository.FirstOrDefaultAsync(
            new TellerSessionByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (session is null)
            throw new NotFoundException($"Teller session with ID {request.Id} not found.");

        session.Resume();

        await repository.UpdateAsync(session, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Teller session resumed: {SessionId}", session.Id);

        return new ResumeTellerSessionResponse(session.Id, session.Status);
    }
}
