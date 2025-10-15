using Accounting.Application.Checks.Exceptions;
using Accounting.Domain.Entities;

namespace Accounting.Application.Checks.StopPayment.v1;

/// <summary>
/// Handler for requesting stop payment on a check.
/// </summary>
public sealed class CheckStopPaymentHandler(
    ILogger<CheckStopPaymentHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Check> repository)
    : IRequestHandler<CheckStopPaymentCommand, CheckStopPaymentResponse>
{
    public async Task<CheckStopPaymentResponse> Handle(CheckStopPaymentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var check = await repository.GetByIdAsync(request.CheckId, cancellationToken);
        if (check == null)
        {
            throw new CheckNotFoundException(request.CheckId);
        }

        check.StopPayment(request.StopPaymentReason, request.StopPaymentDate);

        await repository.UpdateAsync(check, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Stop payment requested for check: {CheckId} - {CheckNumber}. Reason: {Reason}", 
            check.Id, check.CheckNumber, request.StopPaymentReason);

        return new CheckStopPaymentResponse(check.Id, check.CheckNumber, check.Status);
    }
}

