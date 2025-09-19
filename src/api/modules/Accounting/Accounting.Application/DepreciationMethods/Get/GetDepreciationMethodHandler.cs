using Accounting.Application.DepreciationMethods.Responses;

namespace Accounting.Application.DepreciationMethods.Get;

public sealed class GetDepreciationMethodHandler(
    [FromKeyedServices("accounting")] IReadRepository<DepreciationMethod> repository)
    : IRequestHandler<GetDepreciationMethodRequest, DepreciationMethodResponse>
{
    public async Task<DepreciationMethodResponse> Handle(GetDepreciationMethodRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var method = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (method == null) throw new DepreciationMethodNotFoundException(request.Id);

        return new DepreciationMethodResponse
        {
            Id = method.Id,
            Code = method.Code,
            Name = method.Name,
            Description = method.Description,
            UsefulLifeYears = method.UsefulLifeYears,
            DepreciationRate = method.DepreciationRate
        };
    }
}
