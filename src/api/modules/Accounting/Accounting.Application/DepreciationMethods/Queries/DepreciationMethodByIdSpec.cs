
using Accounting.Application.DepreciationMethods.Responses;

namespace Accounting.Application.DepreciationMethods.Queries;

public sealed class DepreciationMethodByIdSpec :
    Specification<DepreciationMethod, DepreciationMethodResponse>,
    ISingleResultSpecification<DepreciationMethod, DepreciationMethodResponse>
{
    public DepreciationMethodByIdSpec(DefaultIdType id) =>
        Query.Where(w => w.Id == id);
}
