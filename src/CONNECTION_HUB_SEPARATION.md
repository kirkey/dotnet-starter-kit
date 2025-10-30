# Connection Hub Separation Implementation

## Overview
Successfully separated the connection status tracking from the messaging hub into two independent SignalR hubs:
- **ConnectionHub**: Handles user online/offline status tracking
- **MessagingHub**: Handles messaging, typing indicators, and read receipts

## Changes Made

### API Side

#### 1. New ConnectionHub (`/api/framework/Infrastructure/SignalR/ConnectionHub.cs`)
- Created dedicated hub for connection status tracking
- Handles `OnConnectedAsync` and `OnDisconnectedAsync` events
- Tracks user connections using `IConnectionTracker`
- Notifies clients of user online/offline status
- Provides methods:
  - `GetOnlineUsers()`: Returns list of online users
  - `IsUserOnline(userId)`: Checks if specific user is online

#### 2. Updated MessagingHub (`/api/modules/Messaging/Hubs/MessagingHub.cs`)
- Removed connection tracking logic from `OnConnectedAsync` and `OnDisconnectedAsync`
- Removed `GetOnlineUsers()` method
- Kept `IConnectionTracker` dependency for looking up user connections when sending messages
- Focused on messaging functionality:
  - Message sending
  - Typing indicators
  - Read receipts

#### 3. Updated MessagingModule (`/api/modules/Messaging/MessagingModule.cs`)
- Registered ConnectionHub at endpoint: `/hubs/connection`
- Kept MessagingHub at endpoint: `/hubs/messaging`

### Client Side (Blazor)

#### 1. New ConnectionHubService (`/apps/blazor/infrastructure/Connectivity/ConnectionHubService.cs`)
- Service for managing connection hub
- Publishes connection state changes via `INotificationPublisher`
- Handles connection events:
  - `OnUserOnline`: User comes online
  - `OnUserOffline`: User goes offline
- Provides connection state tracking:
  - `ConnectionState`: Current state (Connected/Connecting/Disconnected)
  - `ConnectionId`: SignalR connection ID
  - `IsConnected`: Boolean connection status
- Methods:
  - `StartAsync()`: Starts connection hub
  - `StopAsync()`: Stops connection hub
  - `GetOnlineUsersAsync()`: Gets list of online users
  - `IsUserOnlineAsync(userId)`: Checks if user is online

#### 2. New IConnectionHubService Interface (`/apps/blazor/infrastructure/Connectivity/IConnectionHubService.cs`)
- Interface for connection hub service
- Documents all events, properties, and methods

#### 3. Updated MessagingHubService (`/apps/blazor/infrastructure/Messaging/MessagingHubService.cs`)
- Removed connection state tracking
- Removed `INotificationPublisher` dependency
- Removed properties:
  - `ConnectionState`
  - `ConnectionId`
- Removed events:
  - `OnUserOnline`
  - `OnUserOffline`
- Removed method:
  - `GetOnlineUsersAsync()`
- Focused on messaging functionality only

#### 4. Updated IMessagingHubService Interface (`/apps/blazor/infrastructure/Messaging/IMessagingHubService.cs`)
- Removed connection-related events and properties
- Kept messaging-related functionality

#### 5. Updated MessagingConnection Component (`/apps/blazor/client/Components/Connectivity/MessagingConnection.razor.cs`)
- Changed to use `IConnectionHubService` instead of `IMessagingHubService`
- Initializes connection hub on component load
- Provides cascading connection state to child components

#### 6. MessagingConnectionStatus Component (`/apps/blazor/client/Components/Connectivity/MessagingConnectionStatus.razor`)
- Already created, displays connection status indicator
- Subscribes to `ConnectionStateChanged` notifications
- Shows visual status with icon and tooltip:
  - **Green WiFi Icon**: Connected
  - **Yellow WiFi Icon**: Connecting/Reconnecting
  - **Red WiFi Icon**: Disconnected

#### 7. Updated MainLayout (`/apps/blazor/client/Layout/MainLayout.razor`)
- Wrapped layout with `<MessagingConnection>` component
- Added `<MessagingConnectionStatus />` indicator in header
- Status indicator positioned beside sponsor button and theme toggle

#### 8. Updated Service Registration (`/apps/blazor/infrastructure/Extensions.cs`)
- Registered `IConnectionHubService` as scoped service
- Both hub services are now registered independently

#### 9. Updated GlobalUsings (`/apps/blazor/client/GlobalUsings.cs`)
- Added necessary namespaces:
  - `FSH.Starter.Blazor.Infrastructure.Messaging`
  - `FSH.Starter.Blazor.Infrastructure.Notifications`
  - `FSH.Starter.Blazor.Infrastructure.Connectivity`
  - `FSH.Starter.Blazor.Shared.Notifications`
  - `MediatR.Courier`

## Architecture Benefits

### Separation of Concerns
- Connection tracking is now independent of messaging
- Each hub has a single, well-defined responsibility
- Easier to maintain and extend

### Scalability
- Connection hub can be scaled independently
- Messaging hub can be scaled based on message volume
- Better resource utilization

### Flexibility
- Applications can connect to only the hubs they need
- Connection status tracking is available to all features
- Messaging features don't require connection hub

### Reusability
- ConnectionHub is in framework infrastructure (reusable across modules)
- MessagingHub is module-specific
- Connection tracking can be used by other modules

## Hub Endpoints

### API
- **Connection Hub**: `https://{api-url}/hubs/connection`
- **Messaging Hub**: `https://{api-url}/hubs/messaging`

### Client Usage
```csharp
// Connection Hub - tracks online/offline status
await connectionHubService.StartAsync();
var onlineUsers = await connectionHubService.GetOnlineUsersAsync();
var isOnline = await connectionHubService.IsUserOnlineAsync(userId);

// Messaging Hub - sends messages, typing indicators
await messagingHubService.StartAsync();
await messagingHubService.SendTypingIndicatorAsync(conversationId, true, participantIds);
```

## Testing Recommendations

1. **Connection Status**
   - Test connection indicator shows correct state
   - Test reconnection handling
   - Test online/offline notifications

2. **Messaging**
   - Test message sending still works
   - Test typing indicators
   - Test read receipts

3. **Independent Operation**
   - Test connection hub works without messaging hub
   - Test messaging hub works independently
   - Test both hubs running simultaneously

4. **Resilience**
   - Test what happens if one hub fails
   - Test automatic reconnection
   - Test state synchronization after reconnection

## Notes

- Both hubs use the same `IConnectionTracker` service on the backend
- Connection state changes are published via `INotificationPublisher`
- The `MessagingConnectionStatus` component displays the connection hub status
- The layout now shows real-time connection status in the top-right corner
- All hub connections require authentication (`[Authorize]` attribute)

