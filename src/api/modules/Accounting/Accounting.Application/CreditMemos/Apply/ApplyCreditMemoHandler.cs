using Accounting.Domain.Entities;

namespace Accounting.Application.CreditMemos.Apply;

/// <summary>
/// Command to apply a credit memo to an invoice or bill.
/// </summary>
public sealed record ApplyCreditMemoCommand(
    DefaultIdType Id,
    decimal AmountToApply,
    DefaultIdType TargetDocumentId
) : IRequest<DefaultIdType>;

/// <summary>
/// Handler for applying a credit memo.
/// </summary>
public sealed class ApplyCreditMemoHandler(
    ILogger<ApplyCreditMemoHandler> logger,
    [FromKeyedServices("accounting:creditmemos")] IRepository<CreditMemo> repository)
    : IRequestHandler<ApplyCreditMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ApplyCreditMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var creditMemo = await repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (creditMemo == null)
        {
            throw new CreditMemoNotFoundException(request.Id);
        }

        creditMemo.Apply(request.AmountToApply, request.TargetDocumentId);

        await repository.UpdateAsync(creditMemo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Credit memo {CreditMemoId} applied {Amount} to document {DocumentId}", 
            request.Id, request.AmountToApply, request.TargetDocumentId);

        return creditMemo.Id;
    }
}
