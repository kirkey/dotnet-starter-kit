namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Reject.v1;

/// <summary>
/// Handler for rejecting benefit allocation.
/// </summary>
public sealed class RejectBenefitAllocationHandler(
    ILogger<RejectBenefitAllocationHandler> logger,
    [FromKeyedServices("hr:benefitallocations")] IRepository<BenefitAllocation> repository)
    : IRequestHandler<RejectBenefitAllocationCommand, RejectBenefitAllocationResponse>
{
    public async Task<RejectBenefitAllocationResponse> Handle(
        RejectBenefitAllocationCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var allocation = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (allocation is null)
            throw new BenefitAllocationNotFoundException(request.Id);

        allocation.Reject(request.RejectedBy, request.Reason);

        await repository.UpdateAsync(allocation, cancellationToken);

        logger.LogInformation("Benefit allocation {Id} rejected by {RejectedBy}", allocation.Id, request.RejectedBy);

        return new RejectBenefitAllocationResponse(
            allocation.Id,
            allocation.Status,
            allocation.Remarks);
    }
}

