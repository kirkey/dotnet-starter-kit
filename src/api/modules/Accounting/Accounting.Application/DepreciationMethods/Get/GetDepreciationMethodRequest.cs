using MediatR;
using Accounting.Application.DepreciationMethods.Dtos;

namespace Accounting.Application.DepreciationMethods.Get;

public record GetDepreciationMethodRequest(DefaultIdType Id) : IRequest<DepreciationMethodDto>;
