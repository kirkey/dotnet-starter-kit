using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Close.v1;

/// <summary>
/// Command to close a savings account.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Permanently closes a savings account. The account status changes to Closed (terminal state).
/// The account balance must be zero before closing.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate account exists and is not already closed</description></item>
///   <item><description>Verify account balance is zero</description></item>
///   <item><description>Update account status to Closed</description></item>
///   <item><description>Record closed date and reason</description></item>
///   <item><description>Raise SavingsAccountClosed domain event</description></item>
/// </list>
/// 
/// <para><strong>Pre-requisites:</strong></para>
/// <list type="bullet">
///   <item><description>Withdraw all remaining balance before closing</description></item>
///   <item><description>Post any pending interest before closing</description></item>
///   <item><description>Settle any outstanding fees</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>AccountId: Required, must be an existing account</description></item>
///   <item><description>Account must not already be closed</description></item>
///   <item><description>Account balance must be zero</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Close</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "accountId": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "reason": "Member requested closure - relocating to another city"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="CloseAccountResponse"/> confirming closure with date.</para>
/// </remarks>
public sealed record CloseAccountCommand(DefaultIdType AccountId, string? Reason = null) : IRequest<CloseAccountResponse>;
