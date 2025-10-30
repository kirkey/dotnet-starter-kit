# Messaging Endpoints Quick Reference

## Active Endpoints (12 Total)

### Conversations (7 endpoints)

| Endpoint | Route | Purpose | UI Usage |
|----------|-------|---------|----------|
| Create | POST /api/v1/conversations | Create new conversation | New Conversation button |
| Get | GET /api/v1/conversations/{id} | Get conversation details | On conversation select |
| GetList | GET /api/v1/conversations | List all conversations | Sidebar conversation list |
| AddMember | POST /api/v1/conversations/{id}/members | Add member to conversation | Add Member dialog |
| RemoveMember | DELETE /api/v1/conversations/{id}/members/{userId} | Remove member | Remove Member action |
| AssignAdmin | POST /api/v1/conversations/{id}/members/{userId}/admin | Assign admin role | Toggle admin role |
| MarkAsRead | POST /api/v1/conversations/{id}/read | Mark conversation as read | On conversation view |

### Messages (4 endpoints)

| Endpoint | Route | Purpose | UI Usage |
|----------|-------|---------|----------|
| Create | POST /api/v1/messages | Send new message | Send button |
| GetList | GET /api/v1/messages?conversationId={id} | List messages | Message display area |
| Update | PUT /api/v1/messages/{id} | Edit message | Edit Message dialog |
| Delete | DELETE /api/v1/messages/{id} | Delete message | Delete Message action |

### Online Users (1 endpoint)

| Endpoint | Route | Purpose | UI Usage |
|----------|-------|---------|----------|
| GetOnlineUsers | GET /api/v1/messaging/online-users | Get online user list | User status badges |

## Removed Endpoints (8 Total)

### ❌ No Longer Available

- `GET /api/v1/messages/{id}` - Use GetList instead
- `PUT /api/v1/conversations/{id}` - Not implemented in UI
- `DELETE /api/v1/conversations/{id}` - Not implemented in UI
- Messages/Edit/ - Duplicate of Update
- Messages/Send/ - Duplicate of Create
- Messages/MarkAsRead/ - Moved to Conversations
- Conversations/CreateDirect/ - Use Create with type="direct"
- Conversations/CreateGroup/ - Use Create with type="group"

## SignalR Hub

| Hub | Route | Purpose |
|-----|-------|---------|
| MessagingHub | /hubs/messaging | Real-time messaging & online status |

### Hub Methods
- `SendMessageToConversation()` - Broadcast message to participants
- `SendTypingIndicator()` - Send typing status
- `SendMessageReadNotification()` - Notify message read
- `GetOnlineUsers()` - Get online users list

## Quick Stats

- **Total Active Endpoints:** 12
- **Conversations:** 7
- **Messages:** 4  
- **Online Users:** 1
- **SignalR Hubs:** 1
- **Endpoints Removed:** 8 (40% reduction)

---

*Last Updated: October 30, 2025*
*After Cleanup: 12 lean, actively-used endpoints*

