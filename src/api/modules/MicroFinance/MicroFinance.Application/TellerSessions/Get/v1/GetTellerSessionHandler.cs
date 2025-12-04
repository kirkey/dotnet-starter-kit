using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.TellerSessions.Get.v1;

public sealed class GetTellerSessionHandler(
    [FromKeyedServices("microfinance:tellersessions")] IReadRepository<TellerSession> repository)
    : IRequestHandler<GetTellerSessionRequest, TellerSessionResponse>
{
    public async Task<TellerSessionResponse> Handle(GetTellerSessionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var session = await repository.FirstOrDefaultAsync(
            new TellerSessionByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (session is null)
            throw new NotFoundException($"Teller session with ID {request.Id} not found.");

        return new TellerSessionResponse(
            session.Id,
            session.BranchId,
            session.CashVaultId,
            session.SessionNumber,
            session.TellerUserId,
            session.TellerName,
            session.SessionDate,
            session.StartTime,
            session.EndTime,
            session.OpeningBalance,
            session.TotalCashIn,
            session.TotalCashOut,
            session.ExpectedClosingBalance,
            session.ActualClosingBalance,
            session.Variance,
            session.TransactionCount,
            session.Status,
            session.SupervisorUserId,
            session.SupervisorName,
            session.SupervisorVerificationTime);
    }
}
