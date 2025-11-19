namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Create.v1;

/// <summary>
/// Handler for creating benefit allocation.
/// </summary>
public sealed class CreateBenefitAllocationHandler(
    ILogger<CreateBenefitAllocationHandler> logger,
    [FromKeyedServices("hr:benefitallocations")] IRepository<BenefitAllocation> repository,
    [FromKeyedServices("hr:benefitenrollments")] IReadRepository<BenefitEnrollment> enrollmentRepository)
    : IRequestHandler<CreateBenefitAllocationCommand, CreateBenefitAllocationResponse>
{
    public async Task<CreateBenefitAllocationResponse> Handle(
        CreateBenefitAllocationCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify enrollment exists
        var enrollment = await enrollmentRepository.GetByIdAsync(request.EnrollmentId, cancellationToken);
        if (enrollment is null)
            throw new BenefitEnrollmentNotFoundException(request.EnrollmentId);

        // Use provided date or default to now
        var allocationDate = request.AllocationDate ?? DateTime.UtcNow;

        // Create allocation
        var allocation = BenefitAllocation.Create(
            request.EnrollmentId,
            allocationDate,
            request.AllocatedAmount,
            request.AllocationType);

        // Set optional fields
        if (!string.IsNullOrWhiteSpace(request.ReferenceNumber))
            allocation.SetReferenceNumber(request.ReferenceNumber);

        if (!string.IsNullOrWhiteSpace(request.Remarks))
            allocation.SetRemarks(request.Remarks);

        await repository.AddAsync(allocation, cancellationToken);

        logger.LogInformation(
            "Benefit allocation created: Enrollment {EnrollmentId}, Amount {Amount}, Type {Type}",
            allocation.EnrollmentId,
            allocation.AllocatedAmount,
            allocation.AllocationType);

        return new CreateBenefitAllocationResponse(
            allocation.Id,
            allocation.EnrollmentId,
            allocation.AllocationDate,
            allocation.AllocatedAmount,
            allocation.AllocationType,
            allocation.Status);
    }
}

