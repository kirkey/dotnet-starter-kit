using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Delete.v1;

/// <summary>
/// Command to delete a member.
/// </summary>
/// <remarks>
/// <para><strong>What does this do?</strong></para>
/// <para>
/// Permanently deletes a member from the system. This is a hard delete
/// and should only be used for data correction purposes.
/// </para>
/// 
/// <para><strong>Workflow:</strong></para>
/// <list type="number">
///   <item><description>Validate member exists</description></item>
///   <item><description>Check for active loans (deletion blocked if any)</description></item>
///   <item><description>Check for non-zero savings balances (deletion blocked if any)</description></item>
///   <item><description>Delete member record</description></item>
/// </list>
/// 
/// <para><strong>Business Considerations:</strong></para>
/// <list type="bullet">
///   <item><description>Consider using DeactivateMemberCommand instead for soft delete</description></item>
///   <item><description>Members with active loans cannot be deleted</description></item>
///   <item><description>Members with savings accounts must have zero balance</description></item>
///   <item><description>Audit trail is maintained for regulatory compliance</description></item>
/// </list>
/// 
/// <para><strong>Validation Rules:</strong></para>
/// <list type="bullet">
///   <item><description>Id: Required, must be an existing member</description></item>
///   <item><description>Member must have no active loans</description></item>
///   <item><description>All savings account balances must be zero</description></item>
/// </list>
/// 
/// <para><strong>Required Permission:</strong> <c>MicroFinance.Delete</c></para>
/// 
/// <para><strong>JSON Example:</strong></para>
/// <code>
/// {
///   "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
/// }
/// </code>
/// 
/// <para><strong>Response:</strong> No content (204) on successful deletion.</para>
/// </remarks>
/// <param name="Id">The unique identifier of the member to delete.</param>
public sealed record DeleteMemberCommand(DefaultIdType Id) : IRequest;
