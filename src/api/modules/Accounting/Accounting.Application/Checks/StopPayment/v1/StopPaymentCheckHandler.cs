using Accounting.Application.Checks.Exceptions;

namespace Accounting.Application.Checks.StopPayment.v1;

/// <summary>
/// Handler for requesting stop payment on a check.
/// </summary>
public sealed class StopPaymentCheckHandler(
    ILogger<StopPaymentCheckHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Check> repository)
    : IRequestHandler<StopPaymentCheckCommand, CheckStopPaymentResponse>
{
    public async Task<CheckStopPaymentResponse> Handle(StopPaymentCheckCommand request, CancellationToken cancellationToken)
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

