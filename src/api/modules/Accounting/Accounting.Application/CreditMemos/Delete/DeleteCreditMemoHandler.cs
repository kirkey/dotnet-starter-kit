using Accounting.Domain.Constants;
using Accounting.Domain.Entities;

namespace Accounting.Application.CreditMemos.Delete;

/// <summary>
/// Handler for deleting a credit memo.
/// Only draft credit memos can be deleted.
/// </summary>
public sealed class DeleteCreditMemoHandler(IRepository<CreditMemo> repository)
    : IRequestHandler<DeleteCreditMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DeleteCreditMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var creditMemo = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new CreditMemoNotFoundException(request.Id);

        // Only allow deletion of draft memos
        if (creditMemo.Status != CreditMemoStatus.Draft)
        {
            throw new CreditMemoCannotBeModifiedException(request.Id);
        }

        await repository.DeleteAsync(creditMemo, cancellationToken);
        
        return request.Id;
    }
}
