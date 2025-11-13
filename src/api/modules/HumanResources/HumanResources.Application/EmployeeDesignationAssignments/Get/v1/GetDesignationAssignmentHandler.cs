using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.Specifications;
using FSH.Starter.WebApi.HumanResources.Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.HumanResources.Application.DesignationAssignments.Get.v1;

/// <summary>
/// Handler for getting designation assignment by ID.
/// </summary>
public sealed class GetDesignationAssignmentHandler(
    [FromKeyedServices("hr:designationassignments")] IReadRepository<DesignationAssignment> repository)
    : IRequestHandler<GetDesignationAssignmentRequest, DesignationAssignmentResponse>
{
    public async Task<DesignationAssignmentResponse> Handle(
        GetDesignationAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        var assignment = await repository
            .FirstOrDefaultAsync(
                new DesignationAssignmentByIdSpec(request.Id),
                cancellationToken)
            .ConfigureAwait(false);

        if (assignment is null)
            throw new DesignationAssignmentNotFoundException(request.Id);

        return new DesignationAssignmentResponse
        {
            Id = assignment.Id,
            EmployeeId = assignment.EmployeeId,
            EmployeeNumber = assignment.Employee.EmployeeNumber,
            EmployeeName = assignment.Employee.FullName,
            DesignationId = assignment.DesignationId,
            DesignationTitle = assignment.Designation.Title,
            EffectiveDate = assignment.EffectiveDate,
            EndDate = assignment.EndDate,
            IsPlantilla = assignment.IsPlantilla,
            IsActingAs = assignment.IsActingAs,
            AdjustedSalary = assignment.AdjustedSalary,
            Reason = assignment.Reason,
            TenureMonths = assignment.GetTenureMonths(),
            TenureDisplay = assignment.GetTenureDisplay(),
            IsCurrentlyActive = assignment.IsCurrentlyEffective()
        };
    }
}

