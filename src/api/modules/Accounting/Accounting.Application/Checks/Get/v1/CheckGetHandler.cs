using Accounting.Application.Checks.Exceptions;
using Accounting.Domain.Entities;

namespace Accounting.Application.Checks.Get.v1;

/// <summary>
/// Handler for retrieving a single check by ID.
/// </summary>
public sealed class CheckGetHandler(
    [FromKeyedServices("accounting:checks")] IReadRepository<Check> repository)
    : IRequestHandler<CheckGetQuery, CheckGetResponse>
{
    public async Task<CheckGetResponse> Handle(CheckGetQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var check = await repository.GetByIdAsync(request.CheckId, cancellationToken);
        if (check == null)
        {
            throw new CheckNotFoundException(request.CheckId);
        }

        return new CheckGetResponse(
            check.Id,
            check.CheckNumber,
            check.BankAccountCode,
            check.BankAccountName,
            check.Status,
            check.Amount,
            check.PayeeName,
            check.VendorId,
            check.PayeeId,
            check.IssuedDate,
            check.ClearedDate,
            check.VoidedDate,
            check.VoidReason,
            check.PaymentId,
            check.ExpenseId,
            check.Memo,
            check.IsPrinted,
            check.PrintedDate,
            check.PrintedBy,
            check.IsStopPayment,
            check.StopPaymentDate,
            check.StopPaymentReason,
            check.Description,
            check.Notes,
            check.CreatedOn,
            check.CreatedBy,
            check.LastModifiedOn,
            check.LastModifiedBy
        );
    }
}

