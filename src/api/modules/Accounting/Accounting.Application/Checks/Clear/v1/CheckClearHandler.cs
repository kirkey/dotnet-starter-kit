using Accounting.Application.Checks.Exceptions;

namespace Accounting.Application.Checks.Clear.v1;

/// <summary>
/// Handler for marking a check as cleared.
/// </summary>
public sealed class CheckClearHandler(
    ILogger<CheckClearHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Check> repository)
    : IRequestHandler<CheckClearCommand, CheckClearResponse>
{
    public async Task<CheckClearResponse> Handle(CheckClearCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var check = await repository.GetByIdAsync(request.CheckId, cancellationToken);
        if (check == null)
        {
            throw new CheckNotFoundException(request.CheckId);
        }

        check.MarkAsCleared(request.ClearedDate ?? DateTime.Today);

        await repository.UpdateAsync(check, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Check cleared: {CheckId} - {CheckNumber} on {ClearedDate}", 
            check.Id, check.CheckNumber, request.ClearedDate);

        return new CheckClearResponse(check.Id, check.CheckNumber, check.Status);
    }
}
