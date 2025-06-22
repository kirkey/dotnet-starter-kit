using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Payees.Create.v1;
public sealed class PayeeCreateHandler(
    ILogger<PayeeCreateHandler> logger,
    [FromKeyedServices("accounting:payees")] IRepository<Payee> repository)
    : IRequestHandler<PayeeCreateCommand, PayeeCreateResponse>
{
    public async Task<PayeeCreateResponse> Handle(PayeeCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = Payee.Create(
            request.PayeeCode, 
            request.Name, 
            request.Address, 
            request.ExpenseAccountCode,
            request.ExpenseAccountName, 
            request.Tin, 
            request.Description, 
            request.Notes);
        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("payee created {PayeeId}", entity.Id);
        return new PayeeCreateResponse(entity.Id);
    }
}
