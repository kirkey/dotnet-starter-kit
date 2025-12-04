using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Get.v1;

public sealed class GetUssdSessionHandler(
    [FromKeyedServices("microfinance:ussdsessions")] IReadRepository<UssdSession> repository)
    : IRequestHandler<GetUssdSessionRequest, UssdSessionResponse>
{
    public async Task<UssdSessionResponse> Handle(GetUssdSessionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var session = await repository.FirstOrDefaultAsync(new UssdSessionByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"USSD session with id {request.Id} not found");

        return new UssdSessionResponse(
            session.Id,
            session.SessionId,
            session.PhoneNumber,
            session.ServiceCode,
            session.MemberId,
            session.WalletId,
            session.Status,
            session.CurrentMenu,
            session.Language,
            session.CurrentOperation,
            session.SessionData,
            session.MenuLevel,
            session.StepCount,
            session.StartedAt,
            session.EndedAt,
            session.LastActivityAt,
            session.SessionTimeoutSeconds,
            session.LastInput,
            session.LastOutput,
            session.IsAuthenticated,
            session.ErrorMessage);
    }
}
