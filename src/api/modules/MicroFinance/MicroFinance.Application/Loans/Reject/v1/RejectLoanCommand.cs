using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Reject.v1;

/// <summary>
/// Command to reject a pending loan application.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Rejects a loan application that is currently in PENDING status.
/// The loan status changes to REJECTED (terminal state) with a documented reason.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate loan exists and is in PENDING status</description></item>
///   <item><description>Record rejection reason</description></item>
///   <item><description>Update loan status to REJECTED</description></item>
///   <item><description>Raise LoanRejected domain event</description></item>
/// </list>
/// 
/// <para><strong>Common Rejection Reasons:</strong></para>
/// <list type="bullet">
///   <item><description>Insufficient credit score</description></item>
///   <item><description>Incomplete documentation</description></item>
///   <item><description>Existing loan in arrears</description></item>
///   <item><description>Income below minimum threshold</description></item>
///   <item><description>Collateral insufficient</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>Id: Required, must be an existing loan</description></item>
///   <item><description>Loan must be in PENDING status</description></item>
///   <item><description>RejectionReason: Required, cannot be empty</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Reject</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "id": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "rejectionReason": "Insufficient credit score - below 600 threshold"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="RejectLoanResponse"/> confirming rejection.</para>
/// </remarks>
public sealed record RejectLoanCommand(
    DefaultIdType Id,
    [property: DefaultValue("Insufficient credit score")] string RejectionReason) : IRequest<RejectLoanResponse>;
