using Accounting.Domain.Entities;

namespace Accounting.Application.DepreciationMethods.Create;

public sealed class CreateDepreciationMethodHandler(
    [FromKeyedServices("accounting")] IRepository<DepreciationMethod> repository)
    : IRequestHandler<CreateDepreciationMethodRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateDepreciationMethodRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var depreciationMethod = DepreciationMethod.Create(
            request.MethodName,
            request.CalculationFormula,
            request.Description,
            request.Notes);

        await repository.AddAsync(depreciationMethod, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return depreciationMethod.Id;
    }
}
