using System.Security.Claims;

namespace FSH.Starter.Blazor.Client.Pages;

public partial class Home
{
    private ClientPreference _preference = new();

    [Inject] 
    protected ICourier Courier { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the cascading authentication state parameter for the current user.
    /// </summary>
    [CascadingParameter]
    public Task<AuthenticationState> AuthState { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of claims for the currently authenticated user.
    /// </summary>
    public IEnumerable<Claim>? Claims { get; set; }
    
    /// <summary>
    /// Gets a value indicating whether the current user has Accounting Create permission.
    /// </summary>
    private bool HasAccountingPermission => Claims?.Any(c => 
        c.Type == FshClaims.Permission && 
        c.Value == FshPermission.NameFor(FshActions.Create, FshResources.Accounting)) ?? false;
    
    /// <summary>
    /// Gets a value indicating whether the current user has Store Create permission.
    /// </summary>
    private bool HasStorePermission => Claims?.Any(c => 
        c.Type == FshClaims.Permission && 
        c.Value == FshPermission.NameFor(FshActions.Create, FshResources.Store)) ?? false;
    
    protected override async Task OnInitializedAsync()
    {
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

        var authState = await AuthState;
        Claims = authState.User.Claims;
    }
}

