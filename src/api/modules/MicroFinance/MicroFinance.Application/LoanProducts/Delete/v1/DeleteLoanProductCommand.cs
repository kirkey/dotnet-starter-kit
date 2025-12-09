using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanProducts.Delete.v1;

/// <summary>
/// Command to delete (deactivate) a loan product.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Deactivates a loan product, preventing new loans from being created using this product.
/// Existing loans created from this product continue to operate normally.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate product exists</description></item>
///   <item><description>Set IsActive = false</description></item>
///   <item><description>Product is hidden from new loan applications</description></item>
/// </list>
/// 
/// <para><strong>Business Considerations:</strong></para>
/// <list type="bullet">
///   <item><description>This is a soft delete - the product record remains for historical reference</description></item>
///   <item><description>Existing loans using this product are not affected</description></item>
///   <item><description>Product can be reactivated later if needed</description></item>
///   <item><description>Use for phasing out products or seasonal offerings</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>Id: Required, must be an existing product</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Delete</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "id": "7fa85f64-5717-4562-b3fc-2c963f66afa9"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> No content (204) on successful deactivation.</para>
/// </remarks>
public sealed record DeleteLoanProductCommand(DefaultIdType Id) : IRequest<Unit>;
