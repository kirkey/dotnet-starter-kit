using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Approve.v1;

/// <summary>
/// Command to approve a pending loan application.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Approves a loan application that is currently in PENDING status.
/// The loan status changes to APPROVED, making it ready for disbursement.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate loan exists and is in PENDING status</description></item>
///   <item><description>Record approval date (current date)</description></item>
///   <item><description>Update loan status to APPROVED</description></item>
///   <item><description>Raise LoanApproved domain event</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>Id: Required, must be an existing loan</description></item>
///   <item><description>Loan must be in PENDING status</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Approve</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "id": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "notes": "Approved after credit assessment - good repayment history"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="ApproveLoanResponse"/> confirming approval with approval date.</para>
/// </remarks>
public sealed record ApproveLoanCommand(
    DefaultIdType Id,
    [property: DefaultValue("Loan approved after credit assessment")] string? Notes) : IRequest<ApproveLoanResponse>;
