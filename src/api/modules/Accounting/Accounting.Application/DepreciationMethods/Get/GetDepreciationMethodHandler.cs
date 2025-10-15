using Accounting.Application.DepreciationMethods.Responses;
using Accounting.Application.DepreciationMethods.Specs;
using Accounting.Domain.Entities;

namespace Accounting.Application.DepreciationMethods.Get;

/// <summary>
/// Handler for getting a depreciation method by ID.
/// Uses database-level projection for optimal performance.
/// </summary>
public sealed class GetDepreciationMethodHandler(
    [FromKeyedServices("accounting")] IReadRepository<DepreciationMethod> repository)
    : IRequestHandler<GetDepreciationMethodRequest, DepreciationMethodResponse>
{
    /// <summary>
    /// Handles the get depreciation method request.
    /// </summary>
    /// <param name="request">The request containing the depreciation method ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The depreciation method response.</returns>
    /// <exception cref="DepreciationMethodNotFoundException">Thrown when depreciation method is not found.</exception>
    public async Task<DepreciationMethodResponse> Handle(GetDepreciationMethodRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetDepreciationMethodSpec(request.Id);
        return await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new DepreciationMethodNotFoundException(request.Id);
    }
}
