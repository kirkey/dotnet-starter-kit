using Accounting.Application.CreditMemos.Responses;
using Accounting.Domain.Exceptions;

namespace Accounting.Application.CreditMemos.Get;

/// <summary>
/// Handler for getting a credit memo by ID.
/// </summary>
public sealed class GetCreditMemoHandler(
    ILogger<GetCreditMemoHandler> logger,
    [FromKeyedServices("accounting:creditmemos")] IRepository<CreditMemo> repository)
    : IRequestHandler<GetCreditMemoQuery, CreditMemoResponse>
{
    public async Task<CreditMemoResponse> Handle(GetCreditMemoQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var creditMemo = await repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (creditMemo == null)
        {
            throw new CreditMemoNotFoundException(request.Id);
        }

        logger.LogInformation("Retrieved credit memo {CreditMemoId}", request.Id);

        return new CreditMemoResponse
        {
            Id = creditMemo.Id,
            MemoNumber = creditMemo.MemoNumber,
            MemoDate = creditMemo.MemoDate,
            Amount = creditMemo.Amount,
            AppliedAmount = creditMemo.AppliedAmount,
            RefundedAmount = creditMemo.RefundedAmount,
            UnappliedAmount = creditMemo.UnappliedAmount,
            ReferenceType = creditMemo.ReferenceType,
            ReferenceId = creditMemo.ReferenceId,
            OriginalDocumentId = creditMemo.OriginalDocumentId,
            Reason = creditMemo.Reason,
            Status = creditMemo.Status.ToString(),
            IsApplied = creditMemo.IsApplied,
            AppliedDate = creditMemo.AppliedDate,
            ApprovalStatus = creditMemo.ApprovalStatus.ToString(),
            ApprovedBy = creditMemo.ApprovedBy,
            ApprovedDate = creditMemo.ApprovedDate,
            Description = creditMemo.Description,
            Notes = creditMemo.Notes,
            CreatedOn = creditMemo.CreatedOn,
            CreatedBy = creditMemo.CreatedBy,
            LastModifiedOn = creditMemo.LastModifiedOn,
            LastModifiedBy = creditMemo.LastModifiedBy
        };
    }
}
