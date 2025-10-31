using Accounting.Application.BankReconciliations.Responses;
using Accounting.Application.BankReconciliations.Specs;

namespace Accounting.Application.BankReconciliations.Get.v1;

/// <summary>
/// Handler for getting a bank reconciliation by ID.
/// Uses database-level projection for optimal performance.
/// </summary>
public sealed class GetBankReconciliationHandler(
    IReadRepository<BankReconciliation> repository)
    : IRequestHandler<GetBankReconciliationRequest, BankReconciliationResponse>
{
    /// <summary>
    /// Handles the get bank reconciliation request.
    /// </summary>
    /// <param name="request">The request containing the bank reconciliation ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The bank reconciliation response.</returns>
    /// <exception cref="BankReconciliationNotFoundException">Thrown when reconciliation is not found.</exception>
    public async Task<BankReconciliationResponse> Handle(GetBankReconciliationRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new GetBankReconciliationSpec(request.Id);
        return await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new BankReconciliationNotFoundException(request.Id);
    }
}
