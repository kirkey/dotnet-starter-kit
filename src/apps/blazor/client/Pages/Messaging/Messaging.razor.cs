namespace FSH.Starter.Blazor.Client.Pages.Messaging;

/// <summary>
/// Messaging page component for real-time chat functionality.
/// Implements conversation management, messaging, and member operations.
/// </summary>
public partial class Messaging : IDisposable
{
    [Inject] private IClient UsersClient { get; set; } = null!;
    
    
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    private ClientPreference _preference = new();

    // State
    private List<ConversationDto>? _conversations;
    private ConversationDto? _selectedConversation;
    private GetConversationResponse? _conversationDetails;
    private List<MessageDto>? _messages;
    private List<UserDto>? _allUsers;
    private HashSet<string> _onlineUserIds = [];
    private DefaultIdType? _currentUserId;
    private string _newMessage = string.Empty;
    private string _searchConversation = string.Empty;
    private MessageDto? _replyToMessage;
    private ElementReference _messagesEndRef;
    private Timer? _refreshTimer;
    private bool _isLoading;

    // Pagination
    private int _currentPage = 1;
    private const int PageSize = 50;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            // Load preference
            if (await ClientPreferences.GetPreference() is ClientPreference preference)
            {
                _preference = preference;
            }

            // Subscribe to preference changes
            Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
            {
                _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
                _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
                StateHasChanged();
                return Task.CompletedTask;
            });

            // Get current user
            var authState = await AuthState;
            _currentUserId = DefaultIdType.Parse(authState.User.GetUserId());

            // Load initial data
            await LoadConversationsAsync();
            await LoadOnlineUsersAsync();

            // Setup auto-refresh every 5 seconds
            _refreshTimer = new Timer(async _ =>
            {
                await InvokeAsync(async () =>
                {
                    await LoadConversationsAsync(silent: true);
                    if (_selectedConversation != null)
                    {
                        await LoadMessagesAsync(_selectedConversation.Id, silent: true);
                    }
                    StateHasChanged();
                });
            }, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error initializing messaging: {ex.Message}", Severity.Error);
        }
    }

    #region Conversations

    private async Task LoadConversationsAsync(bool silent = false)
    {
        try
        {
            if (!silent) _isLoading = true;

            var filter = new PaginationFilter
            {
                PageNumber = _currentPage,
                PageSize = PageSize
            };

            var result = await Client.GetConversationListEndpointAsync("1", filter);
            _conversations = result.Items?.ToList() ?? [];
        }
        catch (Exception ex)
        {
            if (!silent)
            {
                Snackbar.Add($"Error loading conversations: {ex.Message}", Severity.Error);
            }
        }
        finally
        {
            if (!silent) _isLoading = false;
        }
    }

    private async Task SelectConversation(ConversationDto conversation)
    {
        try
        {
            _selectedConversation = conversation;
            _messages = null;
            _conversationDetails = null;

            // Load conversation details
            _conversationDetails = await Client.GetConversationEndpointAsync("1", conversation.Id);

            // Load messages
            await LoadMessagesAsync(conversation.Id);

            // Mark as read
            await MarkConversationAsRead(conversation.Id);

            // Scroll to bottom
            await ScrollToBottomAsync();
        }
        catch (ApiException ex) when (ex.StatusCode == 403)
        {
            Snackbar.Add("You don't have permission to access this conversation. You may have been removed from it.", Severity.Warning);
            _selectedConversation = null;
            // Reload conversation list to remove inaccessible conversations
            await LoadConversationsAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading conversation: {ex.Message}", Severity.Error);
            _selectedConversation = null;
        }
    }

    private async Task LoadMessagesAsync(DefaultIdType conversationId, bool silent = false)
    {
        try
        {
            var filter = new PaginationFilter
            {
                PageNumber = 1,
                PageSize = 100
            };

            var result = await Client.GetMessageListEndpointAsync("1", conversationId, filter);
            _messages = result.Items?.OrderBy(m => m.SentAt).ToList() ?? [];

            if (!silent)
            {
                await ScrollToBottomAsync();
            }
        }
        catch (Exception ex)
        {
            if (!silent)
            {
                Snackbar.Add($"Error loading messages: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task MarkConversationAsRead(DefaultIdType conversationId)
    {
        try
        {
            await Client.MarkAsReadEndpointAsync("1", conversationId);

            // Update unread count in conversations list
            var conv = _conversations?.FirstOrDefault(c => c.Id == conversationId);
            if (conv != null)
            {
                conv.UnreadCount = 0;
            }
        }
        catch
        {
            // Silent fail - not critical
        }
    }

    private async Task OpenNewConversationDialog()
    {
        var parameters = new DialogParameters
        {
            { nameof(NewConversationDialog.AllUsers), _allUsers },
            { nameof(NewConversationDialog.OnlineUserIds), _onlineUserIds }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<NewConversationDialog>("New Conversation", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data is CreateConversationCommand command)
        {
            await CreateConversationAsync(command);
        }
    }

    private async Task CreateConversationAsync(CreateConversationCommand command)
    {
        try
        {
            // Ensure current user is always included in the member list
            if (_currentUserId.HasValue && !command.MemberIds.Contains(_currentUserId.Value))
            {
                command.MemberIds.Add(_currentUserId.Value);
            }
            
            var response = await Client.CreateConversationEndpointAsync("1", command);
            Snackbar.Add("Conversation created successfully!", Severity.Success);

            await LoadConversationsAsync();

            // Select the new conversation
            var newConversation = _conversations?.FirstOrDefault(c => c.Id == response.Id);
            if (newConversation != null)
            {
                await SelectConversation(newConversation);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error creating conversation: {ex.Message}", Severity.Error);
        }
    }

    #endregion

    #region Messages

    private async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(_newMessage) || _selectedConversation == null) return;

        try
        {
            var command = new CreateMessageCommand
            {
                ConversationId = _selectedConversation.Id,
                Content = _newMessage,
                MessageType = "text",
                ReplyToMessageId = _replyToMessage?.Id
            };

            await Client.CreateMessageEndpointAsync("1", command);

            _newMessage = string.Empty;
            _replyToMessage = null;

            // Reload messages
            await LoadMessagesAsync(_selectedConversation.Id);
            await ScrollToBottomAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error sending message: {ex.Message}", Severity.Error);
        }
    }

    private async Task EditMessage(MessageDto message)
    {
        var parameters = new DialogParameters
        {
            { "Content", message.Content }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<EditMessageDialog>("Edit Message", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data is string newContent)
        {
            try
            {
                // Show loading indicator
                var loadingSnackbar = Snackbar.Add("Updating message...", Severity.Info);
                
                var command = new UpdateMessageCommand
                {
                    Id = message.Id,
                    Content = newContent
                };

                await Client.UpdateMessageEndpointAsync("1", message.Id, command);
                
                // Close loading indicator
                Snackbar.Remove(loadingSnackbar);
                Snackbar.Add("Message updated successfully!", Severity.Success, config =>
                {
                    config.Icon = Icons.Material.Filled.CheckCircle;
                });

                if (_selectedConversation != null)
                {
                    await LoadMessagesAsync(_selectedConversation.Id, silent: true);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to update message: {ex.Message}", Severity.Error, config =>
                {
                    config.Icon = Icons.Material.Filled.Error;
                });
            }
        }
    }

    private async Task DeleteMessage(MessageDto message)
    {
        var messagePreview = message.Content.Length > 50 
            ? message.Content.Substring(0, 50) + "..." 
            : message.Content;
        
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small
        };

        var parameters = new DialogParameters
        {
            { "ContentText", $"Are you sure you want to delete this message?\n\n\"{messagePreview}\"\n\nThis action cannot be undone." },
            { "ButtonText", "Delete" },
            { "Color", Color.Error }
        };

        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Delete Message", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            try
            {
                // Show loading indicator
                var loadingSnackbar = Snackbar.Add("Deleting message...", Severity.Info);
                
                await Client.DeleteMessageEndpointAsync("1", message.Id);
                
                // Close loading indicator
                Snackbar.Remove(loadingSnackbar);
                Snackbar.Add("Message deleted successfully!", Severity.Success, config =>
                {
                    config.Icon = Icons.Material.Filled.CheckCircle;
                });

                if (_selectedConversation != null)
                {
                    await LoadMessagesAsync(_selectedConversation.Id, silent: true);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to delete message: {ex.Message}", Severity.Error, config =>
                {
                    config.Icon = Icons.Material.Filled.Error;
                });
            }
        }
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e is { Key: "Enter", ShiftKey: false })
        {
            await SendMessage();
        }
    }

    private async Task OpenFileUpload()
    {
        // TODO: Implement file upload dialog
        Snackbar.Add("File upload coming soon!", Severity.Info);
    }

    #endregion

    #region Members

    private async Task LoadOnlineUsersAsync()
    {
        try
        {
            // Load all users from the system
            var allUsers = await UsersClient.GetUsersListEndpointAsync();
            _allUsers = allUsers?
                .Where(u => u.Id != _currentUserId)
                .Select(u => new UserDto { Id = u.Id, Name = u.UserName ?? u.Email ?? "Unknown" })
                .ToList() ?? [];
            
            // Get online user IDs from the API
            try
            {
                var onlineResponse = await Client.GetOnlineUsersEndpointAsync("1");
                _onlineUserIds = onlineResponse?.UserIds?.ToHashSet() ?? [];
            }
            catch
            {
                // If online users endpoint fails, continue with empty set
                _onlineUserIds = [];
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading users: {ex.Message}", Severity.Error);
            _allUsers = [];
        }
    }

    private async Task OpenAddMemberDialog()
    {
        var parameters = new DialogParameters
        {
            { nameof(AddMemberDialog.ConversationId), _selectedConversation!.Id },
            { nameof(AddMemberDialog.ExistingMembers), _conversationDetails?.Members?.Select(m => m.UserId).ToList() },
            { nameof(AddMemberDialog.AllUsers), _allUsers },
            { nameof(AddMemberDialog.OnlineUserIds), _onlineUserIds }
        };

        var dialog = await DialogService.ShowAsync<AddMemberDialog>("Add Member", parameters);
        var result = await dialog.Result;

        if (!result.Canceled && result.Data is DefaultIdType userId)
        {
            await AddMemberAsync(userId);
        }
    }

    private async Task AddMemberAsync(DefaultIdType userId)
    {
        if (_selectedConversation == null) return;

        try
        {
            var command = new AddMemberCommand
            {
                ConversationId = _selectedConversation.Id,
                UserId = userId,
                Role = "member"
            };

            await Client.AddMemberEndpointAsync("1", _selectedConversation.Id, command);
            Snackbar.Add("Member added successfully!", Severity.Success);

            // Reload conversation details
            _conversationDetails = await Client.GetConversationEndpointAsync("1", _selectedConversation.Id);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error adding member: {ex.Message}", Severity.Error);
        }
    }

    private async Task RemoveMember(ConversationMemberDto member)
    {
        if (_selectedConversation == null) return;

        bool? confirm = await DialogService.ShowMessageBox(
            "Remove Member",
            $"Are you sure you want to remove this member from the conversation?",
            yesText: "Remove",
            cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Client.RemoveMemberEndpointAsync("1", _selectedConversation.Id, member.UserId);
                Snackbar.Add("Member removed successfully!", Severity.Success);

                // Reload conversation details
                _conversationDetails = await Client.GetConversationEndpointAsync("1", _selectedConversation.Id);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error removing member: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task ToggleMemberRole(ConversationMemberDto member)
    {
        if (_selectedConversation == null) return;

        try
        {
            var newRole = member.Role == "admin" ? "member" : "admin";
            var command = new AssignAdminCommand
            {
                ConversationId = _selectedConversation.Id,
                UserId = member.UserId,
                Role = newRole
            };

            await Client.AssignAdminEndpointAsync("1", _selectedConversation.Id, member.UserId, command);
            Snackbar.Add($"Role updated to {newRole}!", Severity.Success);

            // Reload conversation details
            _conversationDetails = await Client.GetConversationEndpointAsync("1", _selectedConversation.Id);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error updating role: {ex.Message}", Severity.Error);
        }
    }

    private async Task StartDirectConversation(UserDto user)
    {
        try
        {
            var command = new CreateConversationCommand
            {
                ConversationType = "direct",
                MemberIds = new List<DefaultIdType> { _currentUserId!.Value, user.Id }
            };

            var response = await Client.CreateConversationEndpointAsync("1", command);
            Snackbar.Add("Direct conversation started!", Severity.Success);

            await LoadConversationsAsync();

            var newConversation = _conversations?.FirstOrDefault(c => c.Id == response.Id);
            if (newConversation != null)
            {
                await SelectConversation(newConversation);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error starting conversation: {ex.Message}", Severity.Error);
        }
    }

    #endregion

    #region Conversation Info & Menu

    private async Task OpenConversationInfo()
    {
        if (_conversationDetails == null) return;

        var parameters = new DialogParameters
        {
            { nameof(ConversationInfoDialog.Conversation), _conversationDetails }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        await DialogService.ShowAsync<ConversationInfoDialog>("Conversation Info", parameters, options);
    }

    private void OpenConversationMenu()
    {
        // TODO: Implement conversation menu (mute, archive, delete)
        Snackbar.Add("Conversation menu coming soon!", Severity.Info);
    }

    #endregion

    #region Helpers

    private IEnumerable<ConversationDto> FilteredConversations =>
        _conversations?.Where(c =>
            string.IsNullOrWhiteSpace(_searchConversation) ||
            GetConversationTitle(c).Contains(_searchConversation, StringComparison.OrdinalIgnoreCase)) ?? [];

    private bool IsSelectedConversation(DefaultIdType id) =>
        _selectedConversation?.Id == id;

    private string GetConversationTitle(ConversationDto conversation) =>
        conversation.ConversationType == "group"
            ? conversation.Title ?? "Group Chat"
            : "Direct Message"; // TODO: Get other user's name

    private string GetConversationInitials(ConversationDto conversation)
    {
        var title = GetConversationTitle(conversation);
        var words = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return words.Length > 1
            ? $"{words[0][0]}{words[1][0]}"
            : title.Length > 1
                ? $"{title[0]}{title[1]}"
                : title[0].ToString();
    }

    private string GetMembersInfo(ConversationDto conversation) =>
        _conversationDetails?.Members?.Count(m => m.IsActive) + " members" ?? "Loading...";

    private bool IsCurrentUserAdmin() =>
        _conversationDetails?.Members?.Any(m => m.UserId == _currentUserId && m.Role == "admin") ?? false;

    private bool IsUserOnline(DefaultIdType userId)
    {
        var member = _conversationDetails?.Members?.FirstOrDefault(m => m.UserId == userId);
        return member?.LastReadAt.HasValue == true &&
               (DateTime.UtcNow - member.LastReadAt.Value).TotalMinutes < 5;
    }

    private string GetUserName(DefaultIdType userId) =>
        $"User {userId}"; // TODO: Load user names from identity service

    private string GetUserInitials(DefaultIdType userId)
    {
        var name = GetUserName(userId);
        var words = name.Split(' ');
        return words.Length > 1 ? $"{words[0][0]}{words[1][0]}" : name[..Math.Min(2, name.Length)];
    }

    private string GetSenderName(DefaultIdType senderId) =>
        GetUserName(senderId);

    private string FormatDateTime(DateTime dateTime)
    {
        var now = DateTime.Now;
        var diff = now - dateTime;

        if (diff.TotalMinutes < 1) return "Just now";
        if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes}m ago";
        if (diff.TotalHours < 24) return $"{(int)diff.TotalHours}h ago";
        if (diff.TotalDays < 7) return $"{(int)diff.TotalDays}d ago";

        return dateTime.ToString("MMM dd, yyyy");
    }

    private string FormatDateTime(DateTimeOffset dateTime) =>
        FormatDateTime(dateTime.DateTime);

    private async Task ScrollToBottomAsync()
    {
        try
        {
            await Task.Delay(100); // Give time for rendering
            await Js.InvokeVoidAsync("scrollToElement", "chat-messages");
        }
        catch
        {
            // Silently fail if JS interop fails
        }
    }

    #endregion

    public void Dispose()
    {
        _refreshTimer?.Dispose();
    }
}

// DTOs (will be generated by API client)
public class UserDto
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = null!;
}
