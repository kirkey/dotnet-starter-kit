namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Update.v1;

/// <summary>
/// Response returned after successfully updating a member.
/// </summary>
/// <param name="Id">The unique identifier of the updated member.</param>
public sealed record UpdateMemberResponse(DefaultIdType Id);
