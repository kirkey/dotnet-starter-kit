namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Create.v1;

/// <summary>
/// Response returned after successfully creating a member.
/// </summary>
/// <param name="Id">The unique identifier of the newly created member.</param>
public sealed record CreateMemberResponse(DefaultIdType Id);
