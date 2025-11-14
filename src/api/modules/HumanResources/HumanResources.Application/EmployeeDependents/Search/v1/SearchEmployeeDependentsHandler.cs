using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Search.v1;

/// <summary>
/// Handler for searching employee dependents.
/// </summary>
public sealed class SearchEmployeeDependentsHandler(
    [FromKeyedServices("hr:dependents")] IReadRepository<EmployeeDependent> repository)
    : IRequestHandler<SearchEmployeeDependentsRequest, PagedList<EmployeeDependentResponse>>
{
    /// <summary>
    /// Handles the request to search employee dependents.
    /// </summary>
    public async Task<PagedList<EmployeeDependentResponse>> Handle(
        SearchEmployeeDependentsRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchEmployeeDependentsSpec(request);
        var dependents = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = dependents.Select(dependent => new EmployeeDependentResponse
        {
            Id = dependent.Id,
            EmployeeId = dependent.EmployeeId,
            FirstName = dependent.FirstName,
            LastName = dependent.LastName,
            FullName = dependent.FullName,
            DependentType = dependent.DependentType,
            DateOfBirth = dependent.DateOfBirth,
            Age = dependent.Age,
            Relationship = dependent.Relationship,
            Email = dependent.Email,
            PhoneNumber = dependent.PhoneNumber,
            IsBeneficiary = dependent.IsBeneficiary,
            IsClaimableDependent = dependent.IsClaimableDependent,
            EligibilityEndDate = dependent.EligibilityEndDate,
            IsActive = dependent.IsActive
        }).ToList();

        return new PagedList<EmployeeDependentResponse>(responses, request.PageNumber, request.PageSize, totalCount);
    }
}

