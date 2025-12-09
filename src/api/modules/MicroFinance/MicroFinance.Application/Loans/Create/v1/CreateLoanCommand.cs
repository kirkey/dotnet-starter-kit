using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Create.v1;

/// <summary>
/// Command to create a new loan application in the microfinance system.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Creates a new loan application for a member based on a loan product template.
/// The loan is created in PENDING status awaiting credit committee approval.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate member exists and is active</description></item>
///   <item><description>Validate loan product exists and is active</description></item>
///   <item><description>Validate requested amount is within product limits</description></item>
///   <item><description>Generate unique loan number</description></item>
///   <item><description>Create loan in PENDING status</description></item>
///   <item><description>Raise LoanCreated domain event</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>MemberId: Required, must be a valid active member</description></item>
///   <item><description>LoanProductId: Required, must be a valid active loan product</description></item>
///   <item><description>RequestedAmount: Must be within product's MinLoanAmount and MaxLoanAmount</description></item>
///   <item><description>TermMonths: Must be within product's MinTermMonths and MaxTermMonths</description></item>
///   <item><description>RepaymentFrequency: Must be one of "DAILY", "WEEKLY", "BIWEEKLY", "MONTHLY"</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Create</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "memberId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "loanProductId": "7fa85f64-5717-4562-b3fc-2c963f66afa9",
///   "requestedAmount": 50000.00,
///   "termMonths": 12,
///   "purpose": "Business expansion - purchase of inventory",
///   "repaymentFrequency": "MONTHLY"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="CreateLoanResponse"/> containing the created loan ID and loan number.</para>
/// </remarks>
public sealed record CreateLoanCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType MemberId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType LoanProductId,
    [property: DefaultValue(50000)] decimal RequestedAmount,
    [property: DefaultValue(12)] int TermMonths,
    [property: DefaultValue("Business expansion loan")] string? Purpose,
    [property: DefaultValue("MONTHLY")] string RepaymentFrequency) : IRequest<CreateLoanResponse>;
