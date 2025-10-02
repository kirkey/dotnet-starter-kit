using Accounting.Domain.Entities;

namespace Accounting.Application.CreditMemos.Refund;

/// <summary>
/// Command to refund a credit memo.
/// </summary>
public sealed record RefundCreditMemoCommand(
    DefaultIdType Id,
    decimal RefundAmount,
    string? RefundMethod = null,
    string? RefundReference = null
) : IRequest<DefaultIdType>;

/// <summary>
/// Handler for refunding a credit memo.
/// </summary>
public sealed class RefundCreditMemoHandler(
    ILogger<RefundCreditMemoHandler> logger,
    [FromKeyedServices("accounting:creditmemos")] IRepository<CreditMemo> repository)
    : IRequestHandler<RefundCreditMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(RefundCreditMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var creditMemo = await repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (creditMemo == null)
        {
            throw new CreditMemoNotFoundException(request.Id);
        }

        creditMemo.Refund(request.RefundAmount, request.RefundMethod, request.RefundReference);

        await repository.UpdateAsync(creditMemo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Credit memo {CreditMemoId} refunded {Amount}. Method: {Method}, Reference: {Reference}", 
            request.Id, request.RefundAmount, request.RefundMethod ?? "Not specified", request.RefundReference ?? "Not specified");

        return creditMemo.Id;
    }
}
