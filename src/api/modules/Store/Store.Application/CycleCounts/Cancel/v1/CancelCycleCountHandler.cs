using Store.Domain.Exceptions.CycleCount;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Cancel.v1;

/// <summary>
/// Handler for cancelling a cycle count.
/// Cycle counts can only be cancelled if they are in 'Scheduled' or 'InProgress' status.
/// </summary>
public sealed class CancelCycleCountHandler(
    ILogger<CancelCycleCountHandler> logger,
    [FromKeyedServices("store:cycle-counts")] IRepository<CycleCount> repository)
    : IRequestHandler<CancelCycleCountCommand, CancelCycleCountResponse>
{
    public async Task<CancelCycleCountResponse> Handle(CancelCycleCountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var cycleCount = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = cycleCount ?? throw new CycleCountNotFoundException(request.Id);

        // Validate that the cycle count can be cancelled
        if (cycleCount.Status == "Completed")
        {
            throw new InvalidCycleCountStatusException(cycleCount.Status, 
                "Cannot cancel a completed cycle count");
        }

        if (cycleCount.Status == "Cancelled")
        {
            throw new InvalidCycleCountStatusException(cycleCount.Status, 
                "Cycle count is already cancelled");
        }

        cycleCount.Cancel(request.Reason);

        await repository.UpdateAsync(cycleCount, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Cycle count {CycleCountId} cancelled. Reason: {Reason}", 
            cycleCount.Id, request.Reason);

        return new CancelCycleCountResponse(cycleCount.Id, request.Reason);
    }
}

