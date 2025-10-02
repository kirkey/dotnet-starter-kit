
using Accounting.Application.DepreciationMethods.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.DepreciationMethods.Queries;

public sealed class DepreciationMethodByCodeSpec :
    Specification<DepreciationMethod, DepreciationMethodResponse>,
    ISingleResultSpecification<DepreciationMethod, DepreciationMethodResponse>
{
    public DepreciationMethodByCodeSpec(string code) =>
        Query.Where(w => w.MethodCode == code);
}
