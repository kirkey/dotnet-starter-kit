namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Get.v1;

using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Specifications;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using FSH.Starter.WebApi.HumanResources.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for getting pay component details.
/// </summary>
public sealed class GetPayComponentHandler(
    [FromKeyedServices("hr:paycomponents")] IReadRepository<PayComponent> repository)
    : IRequestHandler<GetPayComponentRequest, PayComponentResponse>
{
    public async Task<PayComponentResponse> Handle(
        GetPayComponentRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new PayComponentByIdSpec(request.Id);
        var component = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (component is null)
            throw new PayComponentNotFoundException(request.Id);

        return new PayComponentResponse(
            component.Id,
            component.ComponentName,
            component.ComponentType,
            component.GlAccountCode,
            component.IsActive,
            component.IsCalculated,
            component.Description);
    }
}

