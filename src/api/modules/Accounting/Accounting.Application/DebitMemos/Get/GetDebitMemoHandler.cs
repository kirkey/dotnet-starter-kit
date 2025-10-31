using Accounting.Application.DebitMemos.Responses;
using Accounting.Application.DebitMemos.Specs;

namespace Accounting.Application.DebitMemos.Get;

/// <summary>
/// Handler for getting a debit memo by ID.
/// Uses database-level projection for optimal performance.
/// </summary>
public sealed class GetDebitMemoHandler(
    [FromKeyedServices("accounting:debitmemos")] IReadRepository<DebitMemo> repository)
    : IRequestHandler<GetDebitMemoQuery, DebitMemoResponse>
{
    /// <summary>
    /// Handles the get debit memo query.
    /// </summary>
    /// <param name="request">The query containing the debit memo ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The debit memo response.</returns>
    /// <exception cref="DebitMemoNotFoundException">Thrown when debit memo is not found.</exception>
    public async Task<DebitMemoResponse> Handle(GetDebitMemoQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetDebitMemoSpec(request.Id);
        return await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new DebitMemoNotFoundException(request.Id);
    }
}
