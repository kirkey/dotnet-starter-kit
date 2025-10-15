using Accounting.Application.TaxCodes.Responses;
using Accounting.Application.TaxCodes.Specs;
using Accounting.Domain.Entities;

namespace Accounting.Application.TaxCodes.Get.v1;

/// <summary>
/// Handler for getting a tax code by ID.
/// Uses database-level projection for optimal performance.
/// </summary>
public sealed class GetTaxCodeHandler(
    IReadRepository<TaxCode> repository)
    : IRequestHandler<GetTaxCodeRequest, TaxCodeResponse>
{
    /// <summary>
    /// Handles the get tax code request.
    /// </summary>
    /// <param name="request">The request containing the tax code ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The tax code response.</returns>
    /// <exception cref="TaxCodeNotFoundException">Thrown when tax code is not found.</exception>
    public async Task<TaxCodeResponse> Handle(GetTaxCodeRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetTaxCodeSpec(request.Id);
        return await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new TaxCodeNotFoundException(request.Id);
    }
}
