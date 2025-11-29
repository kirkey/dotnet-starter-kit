﻿namespace FSH.Starter.Blazor.Client.Components;

public partial class PersonCard
{
    [Inject] protected ICourier Courier { get; set; } = null!;

    [Parameter]
    public string? Class { get; set; }
    [Parameter]
    public string? Style { get; set; }

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    private ClientPreference _preference = new();

    private string? UserId { get; set; }
    private string? Email { get; set; }
    private string? FullName { get; set; }
    private string? ImageUri { get; set; }

    protected override async Task OnInitializedAsync()
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
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadUserData();
        }
    }

    private async Task LoadUserData()
    {
        var user = (await AuthState).User;
        if (user.Identity?.IsAuthenticated == true && string.IsNullOrEmpty(UserId))
        {
            FullName = user.GetFullName();
            UserId = user.GetUserId();
            Email = user.GetEmail();
            if (user.GetImageUrl() != null)
            {
                ImageUri = user.GetImageUrl()!.ToString();
            }
            StateHasChanged();
        }
    }
}
