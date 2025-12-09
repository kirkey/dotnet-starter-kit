using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Deposit.v1;

/// <summary>
/// Command to make a deposit to a savings account.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Records a deposit transaction to a member's savings account.
/// The account balance and total deposits are increased by the deposit amount.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate account exists and is in Active status</description></item>
///   <item><description>Validate deposit amount is positive</description></item>
///   <item><description>Increase account balance and total deposits</description></item>
///   <item><description>Create SavingsTransaction record</description></item>
///   <item><description>Raise SavingsDeposited domain event</description></item>
/// </list>
/// 
/// <para><strong>Payment Methods:</strong></para>
/// <list type="bullet">
///   <item><description><c>CASH</c>: Cash deposit at branch teller</description></item>
///   <item><description><c>BANK_TRANSFER</c>: Bank wire transfer</description></item>
///   <item><description><c>MOBILE_MONEY</c>: Mobile wallet transfer</description></item>
///   <item><description><c>CHECK</c>: Cheque deposit</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>AccountId: Required, must be an existing account</description></item>
///   <item><description>Account must be in Active status</description></item>
///   <item><description>Amount: Required, must be positive</description></item>
///   <item><description>PaymentMethod: Required, must be a valid method code</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Deposit</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "accountId": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "amount": 1000.00,
///   "paymentMethod": "CASH",
///   "transactionReference": "DEP-2024-001234",
///   "notes": "Monthly savings deposit"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="DepositResponse"/> with updated balance and transaction ID.</para>
/// </remarks>
public sealed record DepositCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType AccountId,
    [property: DefaultValue(1000)] decimal Amount,
    [property: DefaultValue("CASH")] string PaymentMethod,
    [property: DefaultValue("TXN-2024-001")] string? TransactionReference,
    [property: DefaultValue("Regular savings deposit")] string? Notes) : IRequest<DepositResponse>;
