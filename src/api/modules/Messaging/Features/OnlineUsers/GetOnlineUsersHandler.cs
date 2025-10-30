using FSH.Framework.Infrastructure.SignalR;

namespace FSH.Starter.WebApi.Messaging.Features.OnlineUsers;

/// <summary>
/// Handler for retrieving the list of currently online users.
/// </summary>
public sealed class GetOnlineUsersHandler(IConnectionTracker connectionTracker)
    : IRequestHandler<GetOnlineUsersQuery, GetOnlineUsersResponse>
{
    public async Task<GetOnlineUsersResponse> Handle(GetOnlineUsersQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var onlineUsers = await connectionTracker.GetOnlineUsersAsync();

        return new GetOnlineUsersResponse(onlineUsers);
    }
}

