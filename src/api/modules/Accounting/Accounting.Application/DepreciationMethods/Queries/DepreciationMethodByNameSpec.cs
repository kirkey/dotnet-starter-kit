using Accounting.Application.DepreciationMethods.Dtos;

namespace Accounting.Application.DepreciationMethods.Queries;

public sealed class DepreciationMethodByNameSpec :
    Specification<DepreciationMethod, DepreciationMethodDto>,
    ISingleResultSpecification<DepreciationMethod, DepreciationMethodDto>
{
    public DepreciationMethodByNameSpec(string name) =>
        Query.Where(w => w.Name == name);
}
