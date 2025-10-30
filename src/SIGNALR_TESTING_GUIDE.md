# SignalR Hub Separation - Testing Guide

## Prerequisites

1. **API Server Running**: Ensure the API server is running and accessible
2. **Authentication**: Valid JWT token for authentication
3. **Multiple Browser Tabs/Windows**: To test multi-user scenarios
4. **Browser DevTools**: To monitor WebSocket connections and console logs

## Test Scenarios

### 1. Connection Hub - Single User Tests

#### Test 1.1: Initial Connection
**Steps:**
1. Log in to the Blazor application
2. Observe the connection status indicator in the top-right corner

**Expected Results:**
- ✅ Connection indicator appears beside sponsor button and theme toggle
- ✅ Icon changes from red/yellow to green
- ✅ Tooltip shows "Connected to Messaging Server (ID: {connectionId})"
- ✅ Browser console shows: "Connection hub started successfully"

#### Test 1.2: Connection Loss
**Steps:**
1. With application running, stop the API server
2. Observe the connection status indicator

**Expected Results:**
- ✅ Icon changes to yellow (reconnecting)
- ✅ Tooltip shows "Connecting to server... (Reconnecting...)"
- ✅ Icon eventually changes to red (disconnected)
- ✅ Tooltip shows "Disconnected from server"

#### Test 1.3: Reconnection
**Steps:**
1. With application disconnected, restart the API server
2. Wait for automatic reconnection

**Expected Results:**
- ✅ Icon changes from red to yellow (reconnecting)
- ✅ Icon changes to green (connected)
- ✅ New connection ID displayed in tooltip
- ✅ No data loss or UI corruption

### 2. Connection Hub - Multi-User Tests

#### Test 2.1: User Comes Online Notification
**Steps:**
1. Open application in Browser A (User A)
2. Log in as User A - observe User A is online
3. Open application in Browser B (User B)
4. Log in as User B

**Expected Results:**
- ✅ User A receives "UserOnline" event for User B
- ✅ User B receives "UserOnline" event for User A
- ✅ Both users can query GetOnlineUsers() and see each other

#### Test 2.2: User Goes Offline Notification
**Steps:**
1. With User A and User B online
2. Close Browser B (User B disconnects)
3. Observe User A's application

**Expected Results:**
- ✅ User A receives "UserOffline" event for User B
- ✅ GetOnlineUsers() no longer returns User B

#### Test 2.3: Multiple Connections Same User
**Steps:**
1. Open application in Browser A Tab 1 as User A
2. Open application in Browser A Tab 2 as User A (same user, different tab)
3. Close Tab 1

**Expected Results:**
- ✅ User A still shown as online (has active connection in Tab 2)
- ✅ Other users don't receive "UserOffline" event
- ✅ Close Tab 2, then User A goes offline

### 3. Messaging Hub - Single User Tests

#### Test 3.1: Messaging Hub Connection
**Steps:**
1. Navigate to a messaging/conversation page
2. Observe browser console

**Expected Results:**
- ✅ Messaging hub connection established
- ✅ Console shows: "Messaging hub connection started successfully"
- ✅ Separate WebSocket connection to /hubs/messaging

#### Test 3.2: Send Typing Indicator
**Steps:**
1. Open a conversation
2. Start typing in the message input

**Expected Results:**
- ✅ Typing indicator sent to other participants
- ✅ No errors in console
- ✅ Typing status cleared when stopped typing

### 4. Messaging Hub - Multi-User Tests

#### Test 4.1: Receive Message
**Steps:**
1. User A and User B in same conversation
2. User A sends a message
3. Observe User B's application

**Expected Results:**
- ✅ User B receives message via "ReceiveMessage" event
- ✅ Message displays in real-time
- ✅ No page refresh required

#### Test 4.2: Typing Indicator
**Steps:**
1. User A and User B in same conversation
2. User A starts typing
3. Observe User B's application

**Expected Results:**
- ✅ User B sees "User A is typing..." indicator
- ✅ Indicator disappears when User A stops typing

#### Test 4.3: Read Receipt
**Steps:**
1. User A sends message to User B
2. User B reads the message
3. Observe User A's application

**Expected Results:**
- ✅ User A receives "MessageRead" event
- ✅ Message marked as read/seen
- ✅ Read indicator displayed

### 5. Independent Operation Tests

#### Test 5.1: Connection Hub Without Messaging Hub
**Steps:**
1. Log in to application
2. Stay on a page that doesn't use messaging (e.g., Dashboard)
3. Observe connection status indicator

**Expected Results:**
- ✅ Connection hub connected (green icon)
- ✅ No messaging hub connection established
- ✅ Online/offline status still tracked

#### Test 5.2: Messaging Hub With Connection Hub Down
**Steps:**
1. Modify code to prevent ConnectionHub from starting
2. Navigate to messaging page
3. Attempt to send messages

**Expected Results:**
- ✅ Connection status shows disconnected (red icon)
- ✅ Messaging hub can still connect independently
- ✅ Messages can still be sent/received
- ⚠️ User online/offline status not available

### 6. Resilience Tests

#### Test 6.1: Network Interruption
**Steps:**
1. Application fully connected
2. Disable network (airplane mode)
3. Wait 5 seconds
4. Re-enable network

**Expected Results:**
- ✅ Both hubs show reconnecting status
- ✅ Both hubs automatically reconnect
- ✅ Connection status updated correctly
- ✅ No manual refresh required

#### Test 6.2: API Server Restart
**Steps:**
1. Application fully connected
2. Restart API server
3. Wait for reconnection

**Expected Results:**
- ✅ Both hubs detect disconnection
- ✅ Automatic reconnection attempts
- ✅ Both hubs reconnect successfully
- ✅ State synchronized after reconnection

#### Test 6.3: Long-Running Connection
**Steps:**
1. Keep application open for extended period (hours)
2. Periodically check connection status

**Expected Results:**
- ✅ Connection remains stable
- ✅ Keep-alive pings maintain connection
- ✅ No memory leaks in browser

### 7. Performance Tests

#### Test 7.1: Multiple Connections Performance
**Steps:**
1. Open application in 10 different browser tabs
2. All tabs logged in as different users
3. Monitor server resources

**Expected Results:**
- ✅ All connections established successfully
- ✅ Reasonable memory/CPU usage on server
- ✅ All tabs receive online/offline notifications

#### Test 7.2: High Message Volume
**Steps:**
1. Multiple users in same conversation
2. Send many messages rapidly
3. Monitor both client and server

**Expected Results:**
- ✅ All messages delivered
- ✅ No message loss
- ✅ Proper message ordering
- ✅ No UI freezing

### 8. Error Handling Tests

#### Test 8.1: Invalid Authentication
**Steps:**
1. Attempt to connect with expired/invalid JWT token
2. Observe connection behavior

**Expected Results:**
- ✅ Connection fails gracefully
- ✅ Error message displayed to user
- ✅ No infinite retry loops
- ✅ User prompted to re-authenticate

#### Test 8.2: Hub Method Errors
**Steps:**
1. Call hub method with invalid parameters
2. Observe error handling

**Expected Results:**
- ✅ Error caught and logged
- ✅ User-friendly error message
- ✅ Application remains stable
- ✅ Connection not terminated

## Manual Testing Checklist

### Connection Hub
- [ ] Initial connection successful
- [ ] Connection status indicator displays correctly
- [ ] Reconnection works after network loss
- [ ] Multiple tabs for same user handled correctly
- [ ] User online notifications received
- [ ] User offline notifications received
- [ ] GetOnlineUsers() returns correct list
- [ ] IsUserOnline() returns correct status

### Messaging Hub
- [ ] Messages sent successfully
- [ ] Messages received in real-time
- [ ] Typing indicators work correctly
- [ ] Read receipts work correctly
- [ ] Multiple conversations supported
- [ ] Large messages handled correctly
- [ ] File attachments work (if implemented)

### UI/UX
- [ ] Connection indicator visible in header
- [ ] Icon colors correct (green/yellow/red)
- [ ] Tooltips display useful information
- [ ] No UI flickering during reconnection
- [ ] Responsive design maintained
- [ ] Mobile view displays correctly

### Integration
- [ ] Both hubs run simultaneously
- [ ] Independent operation verified
- [ ] Shared IConnectionTracker works correctly
- [ ] No conflicts between hubs
- [ ] Authentication works for both hubs

## Automated Testing Examples

### Unit Test: ConnectionHubService
```csharp
[Fact]
public async Task StartAsync_ShouldConnectAndPublishState()
{
    // Arrange
    var mockNotificationPublisher = new Mock<INotificationPublisher>();
    var service = new ConnectionHubService(
        new Uri("https://api.test"),
        mockAccessTokenProvider,
        mockNotificationPublisher.Object,
        mockLogger);

    // Act
    await service.StartAsync();

    // Assert
    Assert.True(service.IsConnected);
    mockNotificationPublisher.Verify(
        x => x.PublishAsync(It.IsAny<ConnectionStateChanged>()), 
        Times.Once);
}
```

### Integration Test: ConnectionHub
```csharp
[Fact]
public async Task OnConnectedAsync_ShouldTrackConnection()
{
    // Arrange
    var hubContext = CreateHubContext();
    var hub = new ConnectionHub(mockConnectionTracker, mockLogger);
    hub.Context = hubContext;

    // Act
    await hub.OnConnectedAsync();

    // Assert
    mockConnectionTracker.Verify(
        x => x.AddConnectionAsync(userId, connectionId), 
        Times.Once);
}
```

## Browser Developer Tools Inspection

### Check WebSocket Connections
1. Open DevTools (F12)
2. Go to Network tab
3. Filter by "WS" (WebSocket)
4. Should see two connections:
   - `wss://api.url/hubs/connection`
   - `wss://api.url/hubs/messaging` (when in messaging area)

### Monitor SignalR Messages
1. In DevTools Console, check for logs:
   - Connection established messages
   - Reconnection attempts
   - Error messages
2. Watch for SignalR protocol messages in Network tab

### Check Memory Leaks
1. Open DevTools Memory tab
2. Take heap snapshot before opening app
3. Use app for several minutes
4. Take another heap snapshot
5. Compare - should not have significant growth

## Common Issues and Solutions

### Issue: Connection indicator always red
**Solution:**
- Check API server is running
- Verify /hubs/connection endpoint is accessible
- Check JWT token is valid
- Verify CORS settings allow WebSocket connections

### Issue: Connection hub works but messaging doesn't
**Solution:**
- Ensure MessagingHub is registered in MessagingModule
- Verify /hubs/messaging endpoint mapped
- Check if user navigated to messaging area
- Verify MessagingHubService.StartAsync() is called

### Issue: Multiple "UserOnline" events for same user
**Solution:**
- Check if user has multiple tabs open
- Verify IConnectionTracker correctly aggregates connections
- Ensure OnDisconnectedAsync removes connection properly

### Issue: Connection constantly reconnecting
**Solution:**
- Check server logs for errors
- Verify authentication token is not expired
- Check network stability
- Verify server keep-alive settings

## Performance Metrics to Monitor

### Server-Side
- **Connection Count**: Track active SignalR connections
- **Memory Usage**: Monitor for memory leaks
- **CPU Usage**: Should remain reasonable even with many connections
- **Message Throughput**: Messages per second handled

### Client-Side
- **Reconnection Frequency**: Should be rare under normal conditions
- **Message Latency**: Time from send to receive
- **Memory Usage**: Browser memory should be stable
- **UI Responsiveness**: Should remain smooth

## Success Criteria

✅ **Connection Hub**
- Connects automatically on app load
- Reconnects automatically after network loss
- Correctly tracks user online/offline status
- UI indicator reflects actual state

✅ **Messaging Hub**
- Messages delivered in real-time
- Typing indicators work smoothly
- Read receipts accurate
- No message loss

✅ **Integration**
- Both hubs operate independently
- No conflicts between hubs
- Shared services work correctly
- Overall application stable

✅ **User Experience**
- Connection status always visible
- Smooth reconnection without user action
- No noticeable delays
- Intuitive visual feedback

