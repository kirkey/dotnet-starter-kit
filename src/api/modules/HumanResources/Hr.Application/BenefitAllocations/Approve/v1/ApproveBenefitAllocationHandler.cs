namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Approve.v1;

/// <summary>
/// Handler for approving benefit allocation.
/// </summary>
public sealed class ApproveBenefitAllocationHandler(
    ILogger<ApproveBenefitAllocationHandler> logger,
    [FromKeyedServices("hr:benefitallocations")] IRepository<BenefitAllocation> repository)
    : IRequestHandler<ApproveBenefitAllocationCommand, ApproveBenefitAllocationResponse>
{
    public async Task<ApproveBenefitAllocationResponse> Handle(
        ApproveBenefitAllocationCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var allocation = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (allocation is null)
            throw new BenefitAllocationNotFoundException(request.Id);

        allocation.Approve(request.ApprovedBy);

        await repository.UpdateAsync(allocation, cancellationToken);

        logger.LogInformation("Benefit allocation {Id} approved by {ApprovedBy}", allocation.Id, request.ApprovedBy);

        return new ApproveBenefitAllocationResponse(
            allocation.Id,
            allocation.Status,
            allocation.ApprovalDate!.Value,
            allocation.ApprovedBy!.Value);
    }
}

