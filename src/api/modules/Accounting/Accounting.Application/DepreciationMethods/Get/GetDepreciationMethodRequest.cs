using MediatR;
using Accounting.Application.DepreciationMethods.Dtos;

namespace Accounting.Application.DepreciationMethods.Get;

public class GetDepreciationMethodRequest : IRequest<DepreciationMethodDto>
{
    public DefaultIdType Id { get; set; }

    public GetDepreciationMethodRequest(DefaultIdType id)
    {
        Id = id;
    }
}
