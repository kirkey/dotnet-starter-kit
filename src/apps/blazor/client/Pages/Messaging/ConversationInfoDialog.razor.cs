namespace FSH.Starter.Blazor.Client.Pages.Messaging;

public partial class ConversationInfoDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public GetConversationResponse? Conversation { get; set; }

    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }
    }

    private string GetTitle() =>
        Conversation?.ConversationType == "group"
            ? Conversation.Title ?? "Group Chat"
            : "Direct Message";

    private string GetInitials()
    {
        var title = GetTitle();
        var words = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return words.Length > 1 ? $"{words[0][0]}{words[1][0]}" : title[..Math.Min(2, title.Length)];
    }

    private string GetMemberInitials(DefaultIdType userId)
    {
        var name = $"User {userId}";
        return name[..Math.Min(2, name.Length)];
    }

    private void Close() => MudDialog.Close();
}

