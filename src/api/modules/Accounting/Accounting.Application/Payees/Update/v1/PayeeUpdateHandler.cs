using Accounting.Domain;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Payees.Update.v1;
public sealed class PayeeUpdateHandler(
    ILogger<PayeeUpdateHandler> logger,
    [FromKeyedServices("accounting:payees")] IRepository<Payee> repository)
    : IRequestHandler<PayeeUpdateCommand, PayeeUpdateResponse>
{
    public async Task<PayeeUpdateResponse> Handle(PayeeUpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var payee = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = payee ?? throw new PayeeNotFoundException(request.Id);
        var updatedPayee = payee.Update(
            request.PayeeCode, 
            request.Name, 
            request.Address, 
            request.ExpenseAccountCode,
            request.ExpenseAccountName, 
            request.Tin, 
            request.Description, 
            request.Notes);
        await repository.UpdateAsync(updatedPayee, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("payee with id: {PayeeId} updated.", payee.Id);
        return new PayeeUpdateResponse(payee.Id);
    }
}
