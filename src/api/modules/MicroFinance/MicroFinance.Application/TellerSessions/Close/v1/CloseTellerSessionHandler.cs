using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Close.v1;

public sealed class CloseTellerSessionHandler(
    ILogger<CloseTellerSessionHandler> logger,
    [FromKeyedServices("microfinance:tellersessions")] IRepository<TellerSession> repository)
    : IRequestHandler<CloseTellerSessionCommand, CloseTellerSessionResponse>
{
    public async Task<CloseTellerSessionResponse> Handle(CloseTellerSessionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var session = await repository.FirstOrDefaultAsync(
            new TellerSessionByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (session is null)
            throw new NotFoundException($"Teller session with ID {request.Id} not found.");

        session.Close(request.ActualClosingBalance, request.ClosingDenominations);

        await repository.UpdateAsync(session, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Teller session closed: {SessionId}, Status: {Status}, Variance: {Variance}",
            session.Id, session.Status, session.Variance);

        return new CloseTellerSessionResponse(
            session.Id,
            session.Status,
            session.ExpectedClosingBalance,
            session.ActualClosingBalance!.Value,
            session.Variance!.Value,
            session.EndTime);
    }
}
