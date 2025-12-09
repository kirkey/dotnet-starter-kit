using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.PostInterest.v1;

/// <summary>
/// Command to post interest to a savings account.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Posts accrued interest earnings to a savings account. The account balance and
/// total interest earned are increased by the interest amount.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate account exists and is in Active status</description></item>
///   <item><description>Validate interest amount is positive</description></item>
///   <item><description>Increase account balance and total interest earned</description></item>
///   <item><description>Update last interest posting date</description></item>
///   <item><description>Create SavingsTransaction record (type: Interest)</description></item>
///   <item><description>Raise SavingsInterestPosted domain event</description></item>
/// </list>
/// 
/// <para><strong>Interest Calculation:</strong></para>
/// <para>
/// Interest is typically calculated based on the savings product's interest rate,
/// the average daily balance, and the interest accrual period (monthly, quarterly, annually).
/// The calculation is performed externally; this command only posts the calculated amount.
/// </para>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>AccountId: Required, must be an existing account</description></item>
///   <item><description>Account must be in Active status</description></item>
///   <item><description>InterestAmount: Required, must be positive</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.PostInterest</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "accountId": "9fa85f64-5717-4562-b3fc-2c963f66afb1",
///   "interestAmount": 45.75
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="PostInterestResponse"/> with updated balance and posting date.</para>
/// </remarks>
public sealed record PostInterestCommand(DefaultIdType AccountId, decimal InterestAmount) : IRequest<PostInterestResponse>;
