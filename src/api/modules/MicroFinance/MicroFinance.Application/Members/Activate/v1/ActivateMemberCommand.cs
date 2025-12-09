using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Activate.v1;

/// <summary>
/// Command to activate an inactive member.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Re-activates a member who was previously deactivated. The member regains
/// access to all financial services (loans, savings, etc.).
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate member exists</description></item>
///   <item><description>Check member is currently inactive</description></item>
///   <item><description>Set IsActive = true</description></item>
///   <item><description>Raise MemberActivated domain event</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>Id: Required, must be an existing member</description></item>
///   <item><description>Member must currently be inactive</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Activate</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> <see cref="ActivateMemberResponse"/> confirming activation.</para>
/// </remarks>
public sealed record ActivateMemberCommand(DefaultIdType Id) : IRequest<ActivateMemberResponse>;
