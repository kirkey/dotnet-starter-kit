using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Transfer.v1;

/// <summary>
/// Command to transfer funds between savings accounts.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Transfers funds from one savings account to another within the MFI.
/// Both accounts must be active, and the source account must have sufficient balance.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate both accounts exist and are in Active status</description></item>
///   <item><description>Verify source account has sufficient balance</description></item>
///   <item><description>Debit source account (withdrawal)</description></item>
///   <item><description>Credit destination account (deposit)</description></item>
///   <item><description>Create linked SavingsTransaction records for both accounts</description></item>
/// </list>
/// 
/// <para><strong>Transfer Types:</strong></para>
/// <list type="bullet">
///   <item><description><strong>Internal</strong>: Between member's own accounts</description></item>
///   <item><description><strong>Member-to-Member</strong>: Between different members' accounts</description></item>
///   <item><description><strong>Loan Repayment</strong>: From savings to loan account</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>FromAccountId: Required, must be an existing active account</description></item>
///   <item><description>ToAccountId: Required, must be an existing active account</description></item>
///   <item><description>FromAccountId and ToAccountId must be different</description></item>
///   <item><description>Amount: Required, must be positive</description></item>
///   <item><description>Amount must not exceed source account's available balance</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Transfer</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "fromAccountId": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "toAccountId": "8fa85f64-5717-4562-b3fc-2c963f66afc2",
///   "amount": 1000.00,
///   "notes": "Transfer to children's education savings"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="TransferFundsResponse"/> with transaction IDs for both accounts.</para>
/// </remarks>
public sealed record TransferFundsCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType FromAccountId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType ToAccountId,
    [property: DefaultValue(1000)] decimal Amount,
    [property: DefaultValue("Internal transfer")] string? Notes) : IRequest<TransferFundsResponse>;
