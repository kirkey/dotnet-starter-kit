using Accounting.Application.Checks.Exceptions;
using Accounting.Domain.Entities;

namespace Accounting.Application.Checks.Void.v1;

/// <summary>
/// Handler for voiding a check.
/// </summary>
public sealed class CheckVoidHandler(
    ILogger<CheckVoidHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Check> repository)
    : IRequestHandler<CheckVoidCommand, CheckVoidResponse>
{
    public async Task<CheckVoidResponse> Handle(CheckVoidCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var check = await repository.GetByIdAsync(request.CheckId, cancellationToken);
        if (check == null)
        {
            throw new CheckNotFoundException(request.CheckId);
        }

        check.Void(request.VoidReason, request.VoidedDate);

        await repository.UpdateAsync(check, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Check voided: {CheckId} - {CheckNumber}. Reason: {VoidReason}", 
            check.Id, check.CheckNumber, request.VoidReason);

        return new CheckVoidResponse(check.Id, check.CheckNumber, check.Status);
    }
}

