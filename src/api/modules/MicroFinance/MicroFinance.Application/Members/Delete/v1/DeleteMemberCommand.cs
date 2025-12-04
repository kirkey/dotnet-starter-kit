using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Members.Delete.v1;

/// <summary>
/// Command to delete a member.
/// </summary>
/// <param name="Id">The unique identifier of the member to delete.</param>
public sealed record DeleteMemberCommand(DefaultIdType Id) : IRequest;
