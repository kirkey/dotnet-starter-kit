
using Accounting.Application.DepreciationMethods.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.DepreciationMethods.Queries;

public sealed class DepreciationMethodByNameSpec :
    Specification<DepreciationMethod, DepreciationMethodResponse>,
    ISingleResultSpecification<DepreciationMethod, DepreciationMethodResponse>
{
    public DepreciationMethodByNameSpec(string name) =>
        Query.Where(w => w.Name == name);
}
