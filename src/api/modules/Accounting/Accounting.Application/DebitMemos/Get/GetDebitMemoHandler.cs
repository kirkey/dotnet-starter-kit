using Accounting.Application.DebitMemos.Responses;
using Accounting.Domain.Exceptions;

namespace Accounting.Application.DebitMemos.Get;

/// <summary>
/// Handler for getting a debit memo by ID.
/// </summary>
public sealed class GetDebitMemoHandler(
    [FromKeyedServices("accounting:debitmemos")] IRepository<DebitMemo> repository)
    : IRequestHandler<GetDebitMemoQuery, DebitMemoResponse>
{
    public async Task<DebitMemoResponse> Handle(GetDebitMemoQuery request, CancellationToken cancellationToken)
    {
        var debitMemo = await repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (debitMemo == null)
        {
            throw new DebitMemoNotFoundException(request.Id);
        }

        return new DebitMemoResponse
        {
            Id = debitMemo.Id,
            MemoNumber = debitMemo.MemoNumber,
            MemoDate = debitMemo.MemoDate,
            Amount = debitMemo.Amount,
            AppliedAmount = debitMemo.AppliedAmount,
            UnappliedAmount = debitMemo.UnappliedAmount,
            ReferenceType = debitMemo.ReferenceType,
            ReferenceId = debitMemo.ReferenceId,
            OriginalDocumentId = debitMemo.OriginalDocumentId,
            Reason = debitMemo.Reason,
            Status = debitMemo.Status,
            IsApplied = debitMemo.IsApplied,
            AppliedDate = debitMemo.AppliedDate,
            ApprovalStatus = debitMemo.ApprovalStatus,
            ApprovedBy = debitMemo.ApprovedBy,
            ApprovedDate = debitMemo.ApprovedDate,
            Description = debitMemo.Description,
            Notes = debitMemo.Notes,
            CreatedOn = debitMemo.CreatedOn,
            CreatedBy = debitMemo.CreatedBy,
            LastModifiedOn = debitMemo.LastModifiedOn,
            LastModifiedBy = debitMemo.LastModifiedBy
        };
    }
}
