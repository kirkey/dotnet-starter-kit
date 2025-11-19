using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Get.v1;

/// <summary>
/// Handler for retrieving an employee dependent by ID.
/// </summary>
public sealed class GetEmployeeDependentHandler(
    [FromKeyedServices("hr:dependents")] IReadRepository<EmployeeDependent> repository)
    : IRequestHandler<GetEmployeeDependentRequest, EmployeeDependentResponse>
{
    /// <summary>
    /// Handles the request to get an employee dependent.
    /// </summary>
    public async Task<EmployeeDependentResponse> Handle(
        GetEmployeeDependentRequest request,
        CancellationToken cancellationToken)
    {
        var dependent = await repository
            .FirstOrDefaultAsync(new EmployeeDependentByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (dependent is null)
            throw new EmployeeDependentNotFoundException(request.Id);

        return new EmployeeDependentResponse
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
        };
    }
}

