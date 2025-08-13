using Accounting.Application.DepreciationMethods.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.DepreciationMethods.Queries;

public sealed class DepreciationMethodByIdSpec :
    Specification<DepreciationMethod, DepreciationMethodDto>,
    ISingleResultSpecification<DepreciationMethod, DepreciationMethodDto>
{
    public DepreciationMethodByIdSpec(DefaultIdType id) =>
        Query.Where(w => w.Id == id);
}
