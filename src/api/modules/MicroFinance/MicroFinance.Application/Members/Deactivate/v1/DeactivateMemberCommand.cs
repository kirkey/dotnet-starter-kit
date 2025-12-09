using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Deactivate.v1;

/// <summary>
/// Command to deactivate an active member.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Deactivates a member, preventing them from accessing financial services.
/// Existing accounts remain but no new transactions are allowed.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate member exists</description></item>
///   <item><description>Check member is currently active</description></item>
///   <item><description>Set IsActive = false</description></item>
///   <item><description>Raise MemberDeactivated domain event</description></item>
/// </list>
/// 
/// <para><strong>Business Considerations:</strong></para>
/// <list type="bullet">
///   <item><description>Outstanding loans continue to accrue interest</description></item>
///   <item><description>Savings accounts become dormant</description></item>
///   <item><description>Member cannot apply for new loans or open new accounts</description></item>
///   <item><description>Member can be reactivated later via ActivateMemberCommand</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>Id: Required, must be an existing member</description></item>
///   <item><description>Member must currently be active</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Update</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="DeactivateMemberResponse"/> confirming deactivation.</para>
/// </remarks>
public sealed record DeactivateMemberCommand(DefaultIdType Id) : IRequest<DeactivateMemberResponse>;
