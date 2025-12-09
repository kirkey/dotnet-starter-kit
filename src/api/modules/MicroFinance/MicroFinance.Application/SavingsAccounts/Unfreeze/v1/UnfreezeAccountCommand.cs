using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Unfreeze.v1;

/// <summary>
/// Command to unfreeze a savings account.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Removes the freeze (hold) from a frozen savings account, restoring normal operations.
/// The account status changes from Frozen back to Active.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate account exists and is in Frozen status</description></item>
///   <item><description>Update account status to Active</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>AccountId: Required, must be an existing account</description></item>
///   <item><description>Account must be in Frozen status</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Freeze</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "accountId": "9fa85f64-5717-4562-b3fc-2c963f66afb1"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="UnfreezeAccountResponse"/> confirming the account is now active.</para>
/// </remarks>
public sealed record UnfreezeAccountCommand(DefaultIdType AccountId) : IRequest<UnfreezeAccountResponse>;
