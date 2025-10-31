using Accounting.Application.Checks.Exceptions;
using Accounting.Application.Checks.Specs;

namespace Accounting.Application.Checks.Get.v1;

/// <summary>
/// Handler for retrieving a single check by ID.
/// Uses database-level projection for optimal performance.
/// </summary>
public sealed class CheckGetHandler(
    [FromKeyedServices("accounting:checks")] IReadRepository<Check> repository)
    : IRequestHandler<CheckGetQuery, CheckGetResponse>
{
    /// <summary>
    /// Handles the get check query.
    /// </summary>
    /// <param name="request">The query containing the check ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The check response.</returns>
    /// <exception cref="CheckNotFoundException">Thrown when check is not found.</exception>
    public async Task<CheckGetResponse> Handle(CheckGetQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetCheckSpec(request.CheckId);
        return await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new CheckNotFoundException(request.CheckId);
    }
}

