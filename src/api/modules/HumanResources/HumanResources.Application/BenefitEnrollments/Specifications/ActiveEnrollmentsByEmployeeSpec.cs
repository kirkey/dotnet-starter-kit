namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Specifications;

using Ardalis.Specification;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Specification for getting active enrollments by employee.
/// </summary>
public sealed class ActiveEnrollmentsByEmployeeSpec : Specification<BenefitEnrollment>
{
    public ActiveEnrollmentsByEmployeeSpec(DefaultIdType employeeId)
    {
        Query.Where(x => x.EmployeeId == employeeId && x.IsActive)
            .OrderByDescending(x => x.EffectiveDate)
            .Include(x => x.Benefit);
    }
}

