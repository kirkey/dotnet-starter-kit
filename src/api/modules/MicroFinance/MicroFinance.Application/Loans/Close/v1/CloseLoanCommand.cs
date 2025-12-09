using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Close.v1;

/// <summary>
/// Command to close a fully paid loan.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Closes a loan that has been fully repaid. The loan status changes from
/// DISBURSED to CLOSED (terminal state). This marks the successful completion
/// of the loan lifecycle.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate loan exists and is in DISBURSED status</description></item>
///   <item><description>Verify outstanding principal and interest are zero</description></item>
///   <item><description>Record actual end date</description></item>
///   <item><description>Update loan status to CLOSED</description></item>
///   <item><description>Raise LoanPaidOff domain event</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>Id: Required, must be an existing loan</description></item>
///   <item><description>Loan must be in DISBURSED status</description></item>
///   <item><description>OutstandingPrincipal must be zero</description></item>
///   <item><description>OutstandingInterest must be zero</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Close</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "id": "9fa85f64-5717-4562-b3fc-2c963f66afb1"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="CloseLoanResponse"/> confirming closure with actual end date.</para>
/// </remarks>
public sealed record CloseLoanCommand(DefaultIdType Id) : IRequest<CloseLoanResponse>;
