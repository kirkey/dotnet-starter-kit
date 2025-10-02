
using Accounting.Application.DepreciationMethods.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.DepreciationMethods.Queries;

public sealed class DepreciationMethodByIdSpec :
    Specification<DepreciationMethod, DepreciationMethodResponse>,
    ISingleResultSpecification<DepreciationMethod, DepreciationMethodResponse>
{
    public DepreciationMethodByIdSpec(DefaultIdType id) =>
        Query.Where(w => w.Id == id);
}
