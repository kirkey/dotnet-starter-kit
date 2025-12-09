using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.WriteOff.v1;

/// <summary>
/// Command to write off a non-performing loan.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Writes off a loan that has become uncollectible. The loan status changes from
/// DISBURSED to WRITTEN_OFF (terminal state). This is typically done for loans
/// that have exceeded a certain number of days past due (e.g., 180+ days).
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate loan exists and is in DISBURSED status</description></item>
///   <item><description>Record write-off reason for audit trail</description></item>
///   <item><description>Record actual end date (write-off date)</description></item>
///   <item><description>Update loan status to WRITTEN_OFF</description></item>
///   <item><description>Raise LoanDefaulted domain event</description></item>
/// </list>
/// 
/// <para><strong>Common Write-Off Reasons:</strong></para>
/// <list type="bullet">
///   <item><description>Non-performing asset, exceeded 180 days overdue</description></item>
///   <item><description>Borrower deceased with no recoverable collateral</description></item>
///   <item><description>Borrower declared bankrupt</description></item>
///   <item><description>Collateral liquidated, remaining balance unrecoverable</description></item>
///   <item><description>Legal collection exhausted</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>Id: Required, must be an existing loan</description></item>
///   <item><description>Loan must be in DISBURSED status</description></item>
///   <item><description>WriteOffReason: Required, cannot be empty</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.WriteOff</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "id": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "writeOffReason": "Non-performing asset - exceeded 180 days past due, all collection efforts exhausted"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="WriteOffLoanResponse"/> confirming write-off with date.</para>
/// </remarks>
public sealed record WriteOffLoanCommand(
    DefaultIdType Id,
    [property: DefaultValue("Non-performing asset, exceeded 180 days overdue")] string WriteOffReason) : IRequest<WriteOffLoanResponse>;
