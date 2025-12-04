using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Open.v1;

public sealed class OpenTellerSessionHandler(
    ILogger<OpenTellerSessionHandler> logger,
    [FromKeyedServices("microfinance:tellersessions")] IRepository<TellerSession> repository)
    : IRequestHandler<OpenTellerSessionCommand, OpenTellerSessionResponse>
{
    public async Task<OpenTellerSessionResponse> Handle(OpenTellerSessionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var session = TellerSession.Open(
            branchId: request.BranchId,
            cashVaultId: request.CashVaultId,
            sessionNumber: request.SessionNumber,
            tellerUserId: request.TellerUserId,
            tellerName: request.TellerName,
            openingBalance: request.OpeningBalance);

        await repository.AddAsync(session, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Teller session opened: {SessionNumber} by {TellerName}",
            session.SessionNumber, session.TellerName);

        return new OpenTellerSessionResponse(
            session.Id,
            session.SessionNumber,
            session.TellerName,
            session.OpeningBalance,
            session.Status,
            session.StartTime);
    }
}
