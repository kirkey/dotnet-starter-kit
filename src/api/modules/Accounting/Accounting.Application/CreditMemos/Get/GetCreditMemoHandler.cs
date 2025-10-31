using Accounting.Application.CreditMemos.Responses;
using Accounting.Application.CreditMemos.Specs;

namespace Accounting.Application.CreditMemos.Get;

/// <summary>
/// Handler for getting a credit memo by ID.
/// Uses database-level projection for optimal performance.
/// </summary>
public sealed class GetCreditMemoHandler(
    ILogger<GetCreditMemoHandler> logger,
    [FromKeyedServices("accounting:creditmemos")] IReadRepository<CreditMemo> repository)
    : IRequestHandler<GetCreditMemoQuery, CreditMemoResponse>
{
    /// <summary>
    /// Handles the get credit memo query.
    /// </summary>
    /// <param name="request">The query containing the credit memo ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The credit memo response.</returns>
    /// <exception cref="CreditMemoNotFoundException">Thrown when credit memo is not found.</exception>
    public async Task<CreditMemoResponse> Handle(GetCreditMemoQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetCreditMemoSpec(request.Id);
        var response = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new CreditMemoNotFoundException(request.Id);

        logger.LogInformation("Retrieved credit memo {CreditMemoId}", request.Id);

        return response;
    }
}
