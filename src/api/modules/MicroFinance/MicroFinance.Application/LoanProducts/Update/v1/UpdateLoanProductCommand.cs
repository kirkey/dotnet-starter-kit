using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Update.v1;

/// <summary>
/// Command to update an existing loan product.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Updates the configuration of an existing loan product. Only provided (non-null)
/// fields are updated; omitted fields remain unchanged. Existing loans created from
/// this product are not affected by the update.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate product exists</description></item>
///   <item><description>Apply only provided (non-null) updates</description></item>
///   <item><description>Validate updated values against rules</description></item>
///   <item><description>Update product record</description></item>
///   <item><description>Raise LoanProductUpdated domain event if changes made</description></item>
/// </list>
/// 
/// <para><strong>Impact on Existing Loans:</strong></para>
/// <para>
/// Changes to the product do NOT affect loans already created from this product.
/// Each loan locks in the product's terms at the time of application.
/// </para>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>Id: Required, must be an existing product</description></item>
///   <item><description>Name: If provided, 2-256 characters</description></item>
///   <item><description>InterestRate: If provided, must be between 0% and 100%</description></item>
///   <item><description>MinLoanAmount: If provided, must be > 0 and ≤ MaxLoanAmount</description></item>
///   <item><description>MaxLoanAmount: If provided, must be ≥ MinLoanAmount</description></item>
///   <item><description>MinTermMonths: If provided, must be > 0 and ≤ MaxTermMonths</description></item>
///   <item><description>MaxTermMonths: If provided, must be ≥ MinTermMonths</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Update</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "id": "7fa85f64-5717-4562-b3fc-2c963f66afa9",
///   "name": "Agricultural Loan - Enhanced",
///   "description": "Updated: Now includes equipment financing",
///   "maxLoanAmount": 150000.00,
///   "interestRate": 14.0,
///   "maxTermMonths": 18
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="UpdateLoanProductResponse"/> with updated product details.</para>
/// </remarks>
public sealed record UpdateLoanProductCommand(
    DefaultIdType Id,
    [property: DefaultValue("Personal Loan Updated")] string? Name,
    [property: DefaultValue("Updated description")] string? Description,
    [property: DefaultValue(2000)] decimal? MinLoanAmount,
    [property: DefaultValue(150000)] decimal? MaxLoanAmount,
    [property: DefaultValue(15.0)] decimal? InterestRate,
    [property: DefaultValue("Declining")] string? InterestMethod,
    [property: DefaultValue(1)] int? MinTermMonths,
    [property: DefaultValue(72)] int? MaxTermMonths,
    [property: DefaultValue("Monthly")] string? RepaymentFrequency,
    [property: DefaultValue(7)] int? GracePeriodDays,
    [property: DefaultValue(2.5)] decimal? LatePenaltyRate) : IRequest<UpdateLoanProductResponse>;
