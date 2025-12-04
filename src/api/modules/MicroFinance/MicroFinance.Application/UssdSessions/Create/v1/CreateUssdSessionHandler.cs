using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Create.v1;

public sealed class CreateUssdSessionHandler(
    ILogger<CreateUssdSessionHandler> logger,
    [FromKeyedServices("microfinance:ussdsessions")] IRepository<UssdSession> repository)
    : IRequestHandler<CreateUssdSessionCommand, CreateUssdSessionResponse>
{
    public async Task<CreateUssdSessionResponse> Handle(CreateUssdSessionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var session = UssdSession.Create(
            request.SessionId,
            request.PhoneNumber,
            request.ServiceCode,
            request.MemberId,
            request.WalletId,
            request.Language);

        await repository.AddAsync(session, cancellationToken);
        logger.LogInformation("USSD session {SessionId} created with ID {Id}", session.SessionId, session.Id);

        return new CreateUssdSessionResponse(session.Id, session.SessionId);
    }
}
