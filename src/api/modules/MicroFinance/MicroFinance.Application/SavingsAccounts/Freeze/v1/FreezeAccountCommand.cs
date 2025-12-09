using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Freeze.v1;

/// <summary>
/// Command to freeze a savings account.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Places a freeze (hold) on a savings account, preventing deposits and withdrawals.
/// The account status changes to Frozen. Interest continues to accrue if applicable.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate account exists and is in Active status</description></item>
///   <item><description>Update account status to Frozen</description></item>
///   <item><description>Record freeze reason in notes</description></item>
/// </list>
/// 
/// <para><strong>Common Freeze Reasons:</strong></para>
/// <list type="bullet">
///   <item><description>Suspicious activity investigation</description></item>
///   <item><description>Court order / legal hold</description></item>
///   <item><description>Member request</description></item>
///   <item><description>Fraud investigation</description></item>
///   <item><description>Regulatory compliance check</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>AccountId: Required, must be an existing account</description></item>
///   <item><description>Account must be in Active status</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Freeze</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "accountId": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "reason": "Pending fraud investigation - case #FR-2024-0042"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="FreezeAccountResponse"/> confirming the freeze.</para>
/// </remarks>
public sealed record FreezeAccountCommand(DefaultIdType AccountId, string? Reason = null) : IRequest<FreezeAccountResponse>;
