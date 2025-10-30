namespace FSH.Starter.WebApi.Messaging.Features.OnlineUsers;

/// <summary>
/// Response containing the list of online users.
/// </summary>
/// <param name="UserIds">Collection of online user IDs.</param>
public sealed record GetOnlineUsersResponse(IEnumerable<string> UserIds);

