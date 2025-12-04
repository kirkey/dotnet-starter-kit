using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Verify.v1;

public sealed class VerifyTellerSessionHandler(
    ILogger<VerifyTellerSessionHandler> logger,
    [FromKeyedServices("microfinance:tellersessions")] IRepository<TellerSession> repository)
    : IRequestHandler<VerifyTellerSessionCommand, VerifyTellerSessionResponse>
{
    public async Task<VerifyTellerSessionResponse> Handle(VerifyTellerSessionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var session = await repository.FirstOrDefaultAsync(
            new TellerSessionByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (session is null)
            throw new NotFoundException($"Teller session with ID {request.Id} not found.");

        session.SupervisorVerify(request.SupervisorUserId, request.SupervisorName, request.Notes);

        await repository.UpdateAsync(session, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Teller session verified by supervisor: {SessionId}, Supervisor: {Supervisor}",
            session.Id, request.SupervisorName);

        return new VerifyTellerSessionResponse(
            session.Id,
            request.SupervisorName,
            session.SupervisorVerificationTime!.Value);
    }
}
