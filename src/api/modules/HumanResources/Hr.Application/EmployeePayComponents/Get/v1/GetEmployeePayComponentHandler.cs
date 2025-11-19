using Mapster;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Get.v1;

public sealed class GetEmployeePayComponentHandler(
    [FromKeyedServices("hr:employeepaycomponents")] IRepository<EmployeePayComponent> repository)
    : IRequestHandler<GetEmployeePayComponentRequest, EmployeePayComponentResponse>
{
    public async Task<EmployeePayComponentResponse> Handle(GetEmployeePayComponentRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var assignment = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = assignment ?? throw new EmployeePayComponentNotFoundException(request.Id);

        return assignment.Adapt<EmployeePayComponentResponse>();
    }
}

