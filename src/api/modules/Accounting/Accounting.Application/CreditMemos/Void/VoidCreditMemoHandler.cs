using Accounting.Domain.Exceptions;

namespace Accounting.Application.CreditMemos.Void;

/// <summary>
/// Command to void a credit memo.
/// </summary>
public sealed record VoidCreditMemoCommand(
    DefaultIdType Id,
    string? VoidReason = null
) : IRequest<DefaultIdType>;

/// <summary>
/// Handler for voiding a credit memo.
/// </summary>
public sealed class VoidCreditMemoHandler(
    ILogger<VoidCreditMemoHandler> logger,
    [FromKeyedServices("accounting:creditmemos")] IRepository<CreditMemo> repository)
    : IRequestHandler<VoidCreditMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(VoidCreditMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var creditMemo = await repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (creditMemo == null)
        {
            throw new CreditMemoNotFoundException(request.Id);
        }

        creditMemo.Void(request.VoidReason);

        await repository.UpdateAsync(creditMemo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Credit memo {CreditMemoId} voided. Reason: {Reason}", 
            request.Id, request.VoidReason ?? "Not specified");

        return creditMemo.Id;
    }
}
