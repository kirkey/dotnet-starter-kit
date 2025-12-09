using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Disburse.v1;

/// <summary>
/// Command to disburse an approved loan to the member.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Disburses funds for an approved loan to the member. The loan status changes from
/// APPROVED to DISBURSED, and the repayment schedule becomes active.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate loan exists and is in APPROVED status</description></item>
///   <item><description>Record disbursement date and method</description></item>
///   <item><description>Calculate expected end date based on term</description></item>
///   <item><description>Generate repayment schedule</description></item>
///   <item><description>Update loan status to DISBURSED</description></item>
///   <item><description>Raise LoanDisbursed domain event</description></item>
/// </list>
/// 
/// <para><strong>Disbursement Methods:</strong></para>
/// <list type="bullet">
///   <item><description><c>BANK_TRANSFER</c>: Transfer to member's bank account</description></item>
///   <item><description><c>MOBILE_MONEY</c>: Transfer to mobile wallet</description></item>
///   <item><description><c>CASH</c>: Cash disbursement at branch</description></item>
///   <item><description><c>CHECK</c>: Cheque issued to member</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>Id: Required, must be an existing loan</description></item>
///   <item><description>Loan must be in APPROVED status</description></item>
///   <item><description>DisbursementMethod: Required, valid method code</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Disburse</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "id": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "disbursementMethod": "BANK_TRANSFER",
///   "transactionReference": "TXN-2024-001234",
///   "notes": "Disbursed to member's registered bank account"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="DisburseLoanResponse"/> with disbursement date and schedule details.</para>
/// </remarks>
public sealed record DisburseLoanCommand(
    DefaultIdType Id,
    [property: DefaultValue("BANK_TRANSFER")] string DisbursementMethod,
    [property: DefaultValue("TXN-2024-001")] string? TransactionReference,
    [property: DefaultValue("Disbursed to member's bank account")] string? Notes) : IRequest<DisburseLoanResponse>;
