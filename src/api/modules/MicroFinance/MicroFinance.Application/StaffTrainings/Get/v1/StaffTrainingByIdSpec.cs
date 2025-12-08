using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.StaffTrainings.Get.v1;

/// <summary>
/// Specification to get a staff training by ID.
/// </summary>
public sealed class StaffTrainingByIdSpec : Specification<StaffTraining>, ISingleResultSpecification<StaffTraining>
{
    public StaffTrainingByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
