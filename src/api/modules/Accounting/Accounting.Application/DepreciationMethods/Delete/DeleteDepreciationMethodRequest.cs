using MediatR;

namespace Accounting.Application.DepreciationMethods.Delete;

public record DeleteDepreciationMethodRequest(DefaultIdType Id) : IRequest;
