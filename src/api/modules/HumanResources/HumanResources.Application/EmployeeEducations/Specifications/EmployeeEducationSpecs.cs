namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Specifications;

/// <summary>
/// Specification for getting an employee education record by ID.
/// </summary>
public class EmployeeEducationByIdSpec : Specification<FSH.Starter.WebApi.HumanResources.Domain.Entities.EmployeeEducation>, ISingleResultSpecification<FSH.Starter.WebApi.HumanResources.Domain.Entities.EmployeeEducation>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeEducationByIdSpec"/> class.
    /// </summary>
    public EmployeeEducationByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.Employee);
    }
}

/// <summary>
/// Specification for searching employee education records with filters.
/// </summary>
public class SearchEmployeeEducationsSpec : Specification<FSH.Starter.WebApi.HumanResources.Domain.Entities.EmployeeEducation>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchEmployeeEducationsSpec"/> class.
    /// </summary>
    public SearchEmployeeEducationsSpec(Search.v1.SearchEmployeeEducationsRequest request)
    {
        Query
            .Include(x => x.Employee)
            .OrderByDescending(x => x.GraduationDate)
            .ThenBy(x => x.EducationLevel);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (!string.IsNullOrWhiteSpace(request.EducationLevel))
            Query.Where(x => x.EducationLevel == request.EducationLevel);

        if (!string.IsNullOrWhiteSpace(request.FieldOfStudy))
            Query.Where(x => x.FieldOfStudy.Contains(request.FieldOfStudy));

        if (!string.IsNullOrWhiteSpace(request.Institution))
            Query.Where(x => x.Institution.Contains(request.Institution));

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive);

        if (request.IsVerified.HasValue)
            Query.Where(x => x.IsVerified == request.IsVerified);

        if (request.GraduationDateFrom.HasValue)
            Query.Where(x => x.GraduationDate >= request.GraduationDateFrom);

        if (request.GraduationDateTo.HasValue)
            Query.Where(x => x.GraduationDate <= request.GraduationDateTo);
    }
}

