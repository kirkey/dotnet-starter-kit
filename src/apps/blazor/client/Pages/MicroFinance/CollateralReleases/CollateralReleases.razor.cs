namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollateralReleases;

public partial class CollateralReleases
{
    protected EntityServerTableContext<CollateralReleaseSummaryResponse, DefaultIdType, CollateralReleaseViewModel> Context { get; set; } = null!;
    private EntityTable<CollateralReleaseSummaryResponse, DefaultIdType, CollateralReleaseViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<CollateralReleaseSummaryResponse, DefaultIdType, CollateralReleaseViewModel>(
            fields:
            [
                new EntityField<CollateralReleaseSummaryResponse>(dto => dto.ReleaseReference, "Reference", "ReleaseReference"),
                new EntityField<CollateralReleaseSummaryResponse>(dto => dto.CollateralId, "Collateral", "CollateralId"),
                new EntityField<CollateralReleaseSummaryResponse>(dto => dto.ReleaseMethod, "Method", "ReleaseMethod"),
                new EntityField<CollateralReleaseSummaryResponse>(dto => dto.RecipientName, "Recipient", "RecipientName"),
                new EntityField<CollateralReleaseSummaryResponse>(dto => dto.RequestDate, "Request Date", "RequestDate", typeof(DateTimeOffset)),
                new EntityField<CollateralReleaseSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCollateralReleasesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCollateralReleasesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CollateralReleaseSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCollateralReleaseAsync("1", viewModel.Adapt<CreateCollateralReleaseCommand>()).ConfigureAwait(false);
            },
            entityName: "Collateral Release",
            entityNamePlural: "Collateral Releases",
            entityResource: FshResources.CollateralReleases,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var release = await Client.GetCollateralReleaseAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Release", release } };
        await DialogService.ShowAsync<CollateralReleaseDetailsDialog>("Collateral Release Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show collateral release help dialog.
    /// </summary>
    private async Task ShowCollateralReleaseHelp()
    {
        await DialogService.ShowAsync<CollateralReleaseHelpDialog>("Collateral Release Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
