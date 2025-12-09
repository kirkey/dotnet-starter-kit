using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Create.v1;

/// <summary>
/// Command to create a new savings account for a member.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Opens a new savings account for a member based on a savings product template.
/// The account can be opened with an initial deposit or with zero balance (pending activation).
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate member exists and is active</description></item>
///   <item><description>Validate savings product exists and is active</description></item>
///   <item><description>Generate unique account number</description></item>
///   <item><description>Create account in Active status (if initial deposit > 0) or Pending (if 0)</description></item>
///   <item><description>Record initial deposit transaction if applicable</description></item>
///   <item><description>Raise SavingsAccountCreated domain event</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>MemberId: Required, must be a valid active member</description></item>
///   <item><description>SavingsProductId: Required, must be a valid active savings product</description></item>
///   <item><description>InitialDeposit: Optional, must be non-negative if provided</description></item>
///   <item><description>InitialDeposit must meet product's minimum opening balance requirement</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Create</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "memberId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
///   "savingsProductId": "7fa85f64-5717-4562-b3fc-2c963f66afa9",
///   "initialDeposit": 1000.00
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="CreateSavingsAccountResponse"/> containing the created account ID and account number.</para>
/// </remarks>
public sealed record CreateSavingsAccountCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType MemberId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType SavingsProductId,
    [property: DefaultValue(100)] decimal InitialDeposit) : IRequest<CreateSavingsAccountResponse>;
