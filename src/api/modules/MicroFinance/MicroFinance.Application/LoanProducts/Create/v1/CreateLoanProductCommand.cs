using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Create.v1;

/// <summary>
/// Command to create a new loan product in the microfinance system.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Creates a new loan product template that defines the terms and conditions
/// for a category of loans. Individual loans are created based on these product templates.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate all required fields</description></item>
///   <item><description>Verify product code is unique</description></item>
///   <item><description>Validate interest rate is within 0-100%</description></item>
///   <item><description>Validate term limits (min ≤ max)</description></item>
///   <item><description>Validate loan amount limits (min ≤ max)</description></item>
///   <item><description>Create product in active status</description></item>
///   <item><description>Raise LoanProductCreated domain event</description></item>
/// </list>
/// 
/// <para><strong>Interest Methods:</strong></para>
/// <list type="bullet">
///   <item><description><c>Flat</c>: Interest calculated on original principal throughout term</description></item>
///   <item><description><c>Declining</c>: Interest calculated on outstanding balance (reduces over time)</description></item>
///   <item><description><c>Compound</c>: Interest calculated on principal plus accumulated interest</description></item>
/// </list>
/// 
/// <para><strong>Repayment Frequencies:</strong></para>
/// <list type="bullet">
///   <item><description><c>Daily</c>: For micro-vendors with daily cash flow</description></item>
///   <item><description><c>Weekly</c>: Popular for group lending</description></item>
///   <item><description><c>Biweekly</c>: Aligned with pay cycles</description></item>
///   <item><description><c>Monthly</c>: Standard for salaried borrowers</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>Code: Required, unique, max 64 characters</description></item>
///   <item><description>Name: Required, 2-256 characters</description></item>
///   <item><description>InterestRate: Must be between 0% and 100%</description></item>
///   <item><description>MinLoanAmount: Must be greater than 0</description></item>
///   <item><description>MaxLoanAmount: Must be ≥ MinLoanAmount</description></item>
///   <item><description>MinTermMonths: Must be greater than 0</description></item>
///   <item><description>MaxTermMonths: Must be ≥ MinTermMonths</description></item>
///   <item><description>GracePeriodDays: Must be non-negative</description></item>
///   <item><description>LatePenaltyRate: Must be non-negative</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Create</c></para>
/// 
/// <para><strong>JSON Example (Agricultural Loan):</strong></para>
/// <code>
/// {
///   "code": "AGRI-001",
///   "name": "Agricultural Input Loan",
///   "description": "Seasonal loan for seeds, fertilizers, and farming equipment",
///   "minLoanAmount": 5000.00,
///   "maxLoanAmount": 100000.00,
///   "interestRate": 15.0,
///   "interestMethod": "Declining",
///   "minTermMonths": 3,
///   "maxTermMonths": 12,
///   "repaymentFrequency": "Monthly",
///   "gracePeriodDays": 90,
///   "latePenaltyRate": 2.0
/// }
/// </code>
/// 
/// <para><strong>JSON Example (Emergency Loan):</strong></para>
/// <code>
/// {
///   "code": "EMERG-001",
///   "name": "Emergency Loan",
///   "description": "Quick disbursement loan for urgent needs",
///   "minLoanAmount": 1000.00,
///   "maxLoanAmount": 25000.00,
///   "interestRate": 24.0,
///   "interestMethod": "Flat",
///   "minTermMonths": 1,
///   "maxTermMonths": 6,
///   "repaymentFrequency": "Weekly",
///   "gracePeriodDays": 0,
///   "latePenaltyRate": 3.0
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="CreateLoanProductResponse"/> containing the created product ID.</para>
/// </remarks>
public sealed record CreateLoanProductCommand(
    [property: DefaultValue("LP001")] string Code,
    [property: DefaultValue("Personal Loan")] string Name,
    [property: DefaultValue("Short-term personal loan for emergency needs")] string? Description,
    [property: DefaultValue(1000)] decimal MinLoanAmount,
    [property: DefaultValue(100000)] decimal MaxLoanAmount,
    [property: DefaultValue(12.5)] decimal InterestRate,
    [property: DefaultValue("Declining")] string InterestMethod,
    [property: DefaultValue(1)] int MinTermMonths,
    [property: DefaultValue(60)] int MaxTermMonths,
    [property: DefaultValue("Monthly")] string RepaymentFrequency,
    [property: DefaultValue(5)] int GracePeriodDays = 0,
    [property: DefaultValue(2.0)] decimal LatePenaltyRate = 0) : IRequest<CreateLoanProductResponse>;
