using Accounting.Domain;
using Accounting.Application.DepreciationMethods.Dtos;
using Accounting.Application.DepreciationMethods.Exceptions;
using FSH.Framework.Core.Persistence;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.DepreciationMethods.Get;

public sealed class GetDepreciationMethodHandler(
    [FromKeyedServices("accounting")] IReadRepository<DepreciationMethod> repository)
    : IRequestHandler<GetDepreciationMethodRequest, DepreciationMethodDto>
{
    public async Task<DepreciationMethodDto> Handle(GetDepreciationMethodRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var method = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (method == null) throw new DepreciationMethodNotFoundException(request.Id);

        return method.Adapt<DepreciationMethodDto>();
    }
}
