namespace FSH.Starter.WebApi.Messaging.Features.OnlineUsers;

/// <summary>
/// Request to get all currently online users.
/// </summary>
public sealed record GetOnlineUsersQuery : IRequest<GetOnlineUsersResponse>;
