using Accounting.Domain;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Payees.Delete.v1;
public sealed class PayeeDeleteHandler(
    ILogger<PayeeDeleteHandler> logger,
    [FromKeyedServices("accounting:payees")] IRepository<Payee> repository)
    : IRequestHandler<PayeeDeleteCommand>
{
    public async Task Handle(PayeeDeleteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new PayeeNotFoundException(request.Id);
        await repository.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("payee with id: {PayeeId} successfully deleted", entity.Id);
    }
}
