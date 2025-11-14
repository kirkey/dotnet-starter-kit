namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Get.v1;

using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Specifications;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using FSH.Starter.WebApi.HumanResources.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for getting benefit allocation details.
/// </summary>
public sealed class GetBenefitAllocationHandler(
    [FromKeyedServices("hr:benefitallocations")] IReadRepository<BenefitAllocation> repository)
    : IRequestHandler<GetBenefitAllocationRequest, BenefitAllocationResponse>
{
    public async Task<BenefitAllocationResponse> Handle(
        GetBenefitAllocationRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new BenefitAllocationByIdSpec(request.Id);
        var allocation = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (allocation is null)
            throw new BenefitAllocationNotFoundException(request.Id);

        return new BenefitAllocationResponse(
            allocation.Id,
            allocation.EnrollmentId,
            allocation.Enrollment.Employee.Name,
            allocation.Enrollment.Benefit.BenefitName,
            allocation.AllocationDate,
            allocation.AllocatedAmount,
            allocation.AllocationType,
            allocation.Status,
            allocation.ReferenceNumber,
            allocation.ApprovalDate,
            allocation.PaymentDate,
            allocation.Remarks);
    }
}

