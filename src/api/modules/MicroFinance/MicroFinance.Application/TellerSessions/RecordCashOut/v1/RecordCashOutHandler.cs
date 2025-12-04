using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.RecordCashOut.v1;

public sealed class RecordCashOutHandler(
    ILogger<RecordCashOutHandler> logger,
    [FromKeyedServices("microfinance:tellersessions")] IRepository<TellerSession> repository)
    : IRequestHandler<RecordCashOutCommand, RecordCashOutResponse>
{
    public async Task<RecordCashOutResponse> Handle(RecordCashOutCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var session = await repository.FirstOrDefaultAsync(
            new TellerSessionByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (session is null)
            throw new NotFoundException($"Teller session with ID {request.Id} not found.");

        session.RecordCashOut(request.Amount);

        await repository.UpdateAsync(session, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Cash out recorded for session {SessionId}: {Amount}",
            session.Id, request.Amount);

        return new RecordCashOutResponse(
            session.Id,
            request.Amount,
            session.TotalCashOut,
            session.ExpectedClosingBalance,
            session.TransactionCount);
    }
}
