using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Update.v1;

/// <summary>
/// Command to update a pending loan application.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Updates the terms of a loan application that is still in PENDING status.
/// Once approved, loan terms are locked and cannot be modified.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate loan exists and is in PENDING status</description></item>
///   <item><description>Apply only the provided (non-null) updates</description></item>
///   <item><description>Validate new values against product limits</description></item>
///   <item><description>Update loan record</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>Id: Required, must be an existing loan</description></item>
///   <item><description>Loan must be in PENDING status</description></item>
///   <item><description>InterestRate: If provided, must be between 0% and 100%</description></item>
///   <item><description>TermMonths: If provided, must be within product limits</description></item>
///   <item><description>RepaymentFrequency: If provided, must be valid frequency code</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Update</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "id": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "interestRate": 15.5,
///   "termMonths": 18,
///   "purpose": "Updated: Working capital for store renovation",
///   "repaymentFrequency": "MONTHLY"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="UpdateLoanResponse"/> with updated loan details.</para>
/// </remarks>
public sealed record UpdateLoanCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(10.5)] decimal? InterestRate,
    [property: DefaultValue(12)] int? TermMonths,
    [property: DefaultValue("Working capital for business expansion")] string? Purpose,
    [property: DefaultValue("Monthly")] string? RepaymentFrequency) : IRequest<UpdateLoanResponse>;
