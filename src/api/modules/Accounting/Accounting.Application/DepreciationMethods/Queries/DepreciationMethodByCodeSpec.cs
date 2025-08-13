using Accounting.Application.DepreciationMethods.Dtos;
using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.DepreciationMethods.Queries;

public sealed class DepreciationMethodByCodeSpec :
    Specification<DepreciationMethod, DepreciationMethodDto>,
    ISingleResultSpecification<DepreciationMethod, DepreciationMethodDto>
{
    public DepreciationMethodByCodeSpec(string code) =>
        Query.Where(w => w.MethodCode == code);
}
