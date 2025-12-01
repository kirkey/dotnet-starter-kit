using System.Security.Claims;

namespace FSH.Starter.Blazor.Client.Pages;

public partial class Home
{
    [Inject]
    private AuthenticationStateProvider AuthState { get; set; } = default!;

    private ClientPreference _preference = new();
    private IEnumerable<Claim>? Claims { get; set; }

    /// <summary>
    /// Gets a value indicating whether the current user has Accounting permission.
    /// </summary>
    private bool HasAccountingPermission => Claims?.Any(c => 
        c.Type == FshClaims.Permission && 
        c.Value == FshPermission.NameFor(FshActions.View, FshResources.Accounting)) ?? false;

    /// <summary>
    /// Gets a value indicating whether the current user has Store Create permission.
    /// </summary>
    private bool HasStorePermission => Claims?.Any(c => 
        c.Type == FshClaims.Permission && 
        c.Value == FshPermission.NameFor(FshActions.Create, FshResources.Store)) ?? false;

    /// <summary>
    /// Initializes the component asynchronously by loading the user's claims and theme preferences.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthState.GetAuthenticationStateAsync();
        Claims = authState.User.Claims;

        // Load initial preference from localStorage
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to elevation changes using weak subscription
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            // Extract elevation from notification
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);

            // Trigger UI re-render
            StateHasChanged();
            return Task.CompletedTask;
        });

        // Subscribe to border radius changes using weak subscription
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            // Extract border radius from notification
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            
            // Trigger UI re-render
            StateHasChanged();
            return Task.CompletedTask;
        });

        await base.OnInitializedAsync();
    }
}

