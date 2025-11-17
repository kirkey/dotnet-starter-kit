using Accounting.Application.AccountReconciliations.Responses;

namespace Accounting.Application.AccountReconciliations.Get.v1;

/// <summary>
/// Request to get an account reconciliation by ID.
/// </summary>
public sealed record GetAccountReconciliationRequest(DefaultIdType Id) : IRequest<AccountReconciliationResponse>;

