using Accounting.Application.DepreciationMethods.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.DepreciationMethods.Queries;

public sealed class DepreciationMethodByNameSpec :
    Specification<DepreciationMethod, DepreciationMethodDto>,
    ISingleResultSpecification<DepreciationMethod, DepreciationMethodDto>
{
    public DepreciationMethodByNameSpec(string name) =>
        Query.Where(w => w.Name == name);
}
