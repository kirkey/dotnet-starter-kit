using Accounting.Domain.Exceptions;

namespace Accounting.Application.CreditMemos.Update;

/// <summary>
/// Command to update an existing credit memo.
/// </summary>
public sealed record UpdateCreditMemoCommand(
    DefaultIdType Id,
    string? Description = null,
    string? Notes = null,
    string? Reason = null
) : IRequest<DefaultIdType>;

/// <summary>
/// Handler for updating a credit memo.
/// </summary>
public sealed class UpdateCreditMemoHandler(
    ILogger<UpdateCreditMemoHandler> logger,
    [FromKeyedServices("accounting:creditmemos")] IRepository<CreditMemo> repository)
    : IRequestHandler<UpdateCreditMemoCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateCreditMemoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var creditMemo = await repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (creditMemo == null)
        {
            throw new CreditMemoNotFoundException(request.Id);
        }

        creditMemo.Update(request.Description, request.Notes, request.Reason);

        await repository.UpdateAsync(creditMemo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Credit memo {CreditMemoId} updated", request.Id);

        return creditMemo.Id;
    }
}
