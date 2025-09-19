using Accounting.Application.DepreciationMethods.Responses;

namespace Accounting.Application.DepreciationMethods.Get;

public class GetDepreciationMethodRequest(DefaultIdType id) : IRequest<DepreciationMethodResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
