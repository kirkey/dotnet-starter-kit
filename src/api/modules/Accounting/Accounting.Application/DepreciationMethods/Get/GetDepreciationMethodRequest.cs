using MediatR;
using Accounting.Application.DepreciationMethods.Dtos;

namespace Accounting.Application.DepreciationMethods.Get;

public class GetDepreciationMethodRequest(DefaultIdType id) : IRequest<DepreciationMethodDto>
{
    public DefaultIdType Id { get; set; } = id;
}
