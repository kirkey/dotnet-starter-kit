using FSH.Starter.WebApi.HumanResources.Application.Designations.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Get.v1;

/// <summary>
/// Handler for getting designation by ID.
/// </summary>
public sealed class GetDesignationHandler(
    [FromKeyedServices("hr:designations")] IReadRepository<Designation> repository)
    : IRequestHandler<GetDesignationRequest, DesignationResponse>
{
    public async Task<DesignationResponse> Handle(GetDesignationRequest request, CancellationToken cancellationToken)
    {
        var designation = await repository
            .FirstOrDefaultAsync(new DesignationByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (designation is null)
        {
            throw new DesignationNotFoundException(request.Id);
        }

        return new DesignationResponse
        {
            Id = designation.Id,
            Code = designation.Code,
            Title = designation.Title,
            Area = designation.Area,
            SalaryGrade = designation.SalaryGrade,
            Description = designation.Description,
            MinimumSalary = designation.MinimumSalary,
            MaximumSalary = designation.MaximumSalary,
            IsActive = designation.IsActive,
            IsManagerial = designation.IsManagerial
        };
    }
}

