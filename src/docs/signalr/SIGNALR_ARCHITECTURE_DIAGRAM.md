# SignalR Hub Architecture - Quick Reference

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                         BLAZOR CLIENT                            │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌──────────────────────┐        ┌──────────────────────┐      │
│  │ ConnectionHubService │        │ MessagingHubService  │      │
│  │                      │        │                      │      │
│  │ - Track Online/      │        │ - Send Messages      │      │
│  │   Offline Status     │        │ - Typing Indicators  │      │
│  │ - Get Online Users   │        │ - Read Receipts      │      │
│  │ - Connection State   │        │                      │      │
│  └──────────┬───────────┘        └──────────┬───────────┘      │
│             │                               │                   │
│             │ Publishes                     │                   │
│             │ ConnectionStateChanged        │                   │
│             ↓                               │                   │
│  ┌──────────────────────────────────────────┴──────────┐       │
│  │        INotificationPublisher (MediatR.Courier)     │       │
│  └──────────────────────────────────────────────────────┘       │
│             ↓                                                    │
│  ┌──────────────────────────────────────────────────────┐       │
│  │      MessagingConnectionStatus Component             │       │
│  │      (Displays connection indicator in UI)           │       │
│  └──────────────────────────────────────────────────────┘       │
│                                                                   │
└────────────┬─────────────────────────────┬────────────────────┘
             │                             │
             │ SignalR WebSocket           │ SignalR WebSocket
             │ /hubs/connection            │ /hubs/messaging
             ↓                             ↓
┌─────────────────────────────────────────────────────────────────┐
│                          API SERVER                              │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌──────────────────────┐        ┌──────────────────────┐      │
│  │   ConnectionHub      │        │   MessagingHub       │      │
│  │                      │        │                      │      │
│  │ - OnConnectedAsync   │        │ - SendMessage        │      │
│  │ - OnDisconnectedAsync│        │ - SendTyping         │      │
│  │ - GetOnlineUsers     │        │ - SendReadNotif      │      │
│  │ - IsUserOnline       │        │                      │      │
│  └──────────┬───────────┘        └──────────┬───────────┘      │
│             │                               │                   │
│             └───────────┬───────────────────┘                   │
│                         ↓                                        │
│              ┌──────────────────────┐                           │
│              │  IConnectionTracker  │                           │
│              │  (Shared Service)    │                           │
│              │                      │                           │
│              │  Tracks user→        │                           │
│              │  connection mappings │                           │
│              └──────────────────────┘                           │
│                                                                   │
└─────────────────────────────────────────────────────────────────┘
```

## Connection Flow

### User Connects to Application

```
1. User logs in to Blazor app
2. MainLayout loads MessagingConnection component
3. MessagingConnection.OnInitializedAsync() called
4. ConnectionHubService.StartAsync() called
5. WebSocket connection established to /hubs/connection
6. ConnectionHub.OnConnectedAsync() executes
7. IConnectionTracker.AddConnectionAsync(userId, connectionId)
8. Server broadcasts "UserOnline" event to other users
9. ConnectionStateChanged notification published (Connected)
10. MessagingConnectionStatus updates UI (green icon)
```

### User Disconnects from Application

```
1. User closes browser or loses connection
2. ConnectionHub.OnDisconnectedAsync() executes
3. IConnectionTracker.RemoveConnectionAsync(userId, connectionId)
4. If user has no more connections:
   - Server broadcasts "UserOffline" event to other users
5. ConnectionStateChanged notification published (Disconnected)
6. MessagingConnectionStatus updates UI (red icon)
```

### User Sends a Message

```
1. User types message in conversation
2. Client calls MessagingHubService.SendTypingIndicatorAsync()
3. Message sent via /hubs/messaging WebSocket
4. MessagingHub.SendTypingIndicator() executes
5. IConnectionTracker used to find recipient connections
6. Server sends "UserTyping" event to recipients
7. Recipients see typing indicator
```

## Hub Endpoints

| Hub              | Endpoint             | Purpose                           |
|------------------|----------------------|-----------------------------------|
| ConnectionHub    | /hubs/connection     | Online/offline status tracking    |
| MessagingHub     | /hubs/messaging      | Real-time messaging features      |

## Client Services

| Service              | Interface                  | Purpose                      |
|----------------------|----------------------------|------------------------------|
| ConnectionHubService | IConnectionHubService      | Connection status tracking   |
| MessagingHubService  | IMessagingHubService       | Messaging functionality      |

## Events

### ConnectionHub Events (Server → Client)

| Event        | Parameters | Description                    |
|--------------|------------|--------------------------------|
| UserOnline   | userId     | User came online               |
| UserOffline  | userId     | User went offline              |

### MessagingHub Events (Server → Client)

| Event          | Parameters                      | Description                    |
|----------------|---------------------------------|--------------------------------|
| ReceiveMessage | conversationId, message         | New message received           |
| UserTyping     | conversationId, userId, typing  | User typing status changed     |
| MessageRead    | conversationId, messageId, userId | Message was read             |

## Client Methods

### ConnectionHub Methods (Client → Server)

| Method         | Parameters | Returns              | Description                |
|----------------|------------|----------------------|----------------------------|
| GetOnlineUsers | -          | IEnumerable<string>  | Get all online user IDs    |
| IsUserOnline   | userId     | bool                 | Check if user is online    |

### MessagingHub Methods (Client → Server)

| Method                       | Parameters                              | Description                    |
|------------------------------|-----------------------------------------|--------------------------------|
| SendMessageToConversation    | conversationId, message, participantIds | Send message to conversation   |
| SendTypingIndicator          | conversationId, isTyping, participantIds| Send typing status             |
| SendMessageReadNotification  | conversationId, messageId, participantIds| Mark message as read          |

## Component Hierarchy

```
App.razor
└── MainLayout.razor
    ├── MessagingConnection.razor (Cascading connection state)
    │   ├── MudAppBar
    │   │   ├── [Other header items]
    │   │   └── MessagingConnectionStatus.razor (Status indicator)
    │   ├── MudDrawer
    │   └── MudMainContent
    │       └── @ChildContent (Page content)
    └── [Rest of layout]
```

## Dependency Injection Registration

```csharp
// In Extensions.cs
services.AddScoped<IConnectionHubService, ConnectionHubService>();
services.AddScoped<IMessagingHubService, MessagingHubService>();
```

## Key Design Decisions

1. **Separation of Concerns**: Connection tracking separate from messaging
2. **Shared Connection Tracker**: Both hubs use same IConnectionTracker service
3. **Framework vs Module**: ConnectionHub in framework (reusable), MessagingHub in module
4. **Independent Connections**: Each hub maintains its own SignalR connection
5. **State Publishing**: ConnectionHub publishes state changes via INotificationPublisher
6. **Cascading State**: MessagingConnection provides state to child components
7. **Visual Feedback**: MessagingConnectionStatus shows real-time connection status

