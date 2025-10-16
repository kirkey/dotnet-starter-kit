using Accounting.Domain.Constants;
using Accounting.Domain.Entities;

namespace Accounting.Application.DebitMemos.Delete;

/// <summary>
/// Handler for deleting a debit memo.
/// Only draft debit memos can be deleted.
/// </summary>
public sealed class DeleteDebitMemoHandler(IRepository<DebitMemo> repository)
    : IRequestHandler<DeleteDebitMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DeleteDebitMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var debitMemo = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new DebitMemoNotFoundException(request.Id);

        // Only allow deletion of draft memos
        if (debitMemo.Status != DebitMemoStatus.Draft)
        {
            throw new DebitMemoCannotBeModifiedException(request.Id);
        }

        await repository.DeleteAsync(debitMemo, cancellationToken);
        
        return request.Id;
    }
}
