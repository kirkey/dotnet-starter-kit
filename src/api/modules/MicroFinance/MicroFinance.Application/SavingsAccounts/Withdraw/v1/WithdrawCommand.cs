using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Withdraw.v1;

/// <summary>
/// Command to make a withdrawal from a savings account.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Records a withdrawal transaction from a member's savings account.
/// The account balance is decreased by the withdrawal amount.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate account exists and is in Active status</description></item>
///   <item><description>Validate withdrawal amount is positive</description></item>
///   <item><description>Verify sufficient balance (amount ≤ available balance)</description></item>
///   <item><description>Decrease account balance, increase total withdrawals</description></item>
///   <item><description>Create SavingsTransaction record</description></item>
///   <item><description>Raise SavingsWithdrawn domain event</description></item>
/// </list>
/// 
/// <para><strong>Payment Methods:</strong></para>
/// <list type="bullet">
///   <item><description><c>CASH</c>: Cash withdrawal at branch teller</description></item>
///   <item><description><c>BANK_TRANSFER</c>: Transfer to member's bank account</description></item>
///   <item><description><c>MOBILE_MONEY</c>: Transfer to mobile wallet</description></item>
///   <item><description><c>ATM</c>: ATM withdrawal</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>AccountId: Required, must be an existing account</description></item>
///   <item><description>Account must be in Active status (not Frozen, Closed, or Dormant)</description></item>
///   <item><description>Amount: Required, must be positive</description></item>
///   <item><description>Amount must not exceed available balance</description></item>
///   <item><description>Amount must respect product's minimum balance requirement (if any)</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Withdraw</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "accountId": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "amount": 500.00,
///   "paymentMethod": "CASH",
///   "notes": "Emergency medical expenses"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="WithdrawResponse"/> with updated balance and transaction ID.</para>
/// </remarks>
public sealed record WithdrawCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType AccountId,
    [property: DefaultValue(500)] decimal Amount,
    [property: DefaultValue("CASH")] string PaymentMethod,
    [property: DefaultValue("Emergency withdrawal")] string? Notes) : IRequest<WithdrawResponse>;
