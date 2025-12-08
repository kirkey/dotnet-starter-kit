namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollateralValuations;

/// <summary>
/// Collateral Valuations page logic. Manages collateral appraisals and valuations.
/// </summary>
public partial class CollateralValuations
{
    protected EntityServerTableContext<CollateralValuationSummaryResponse, DefaultIdType, CollateralValuationViewModel> Context { get; set; } = null!;
    private EntityTable<CollateralValuationSummaryResponse, DefaultIdType, CollateralValuationViewModel> _table = null!;

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

        Context = new EntityServerTableContext<CollateralValuationSummaryResponse, DefaultIdType, CollateralValuationViewModel>(
            fields:
            [
                new EntityField<CollateralValuationSummaryResponse>(dto => dto.ValuationReference, "Reference", "ValuationReference"),
                new EntityField<CollateralValuationSummaryResponse>(dto => dto.CollateralId, "Collateral", "CollateralId"),
                new EntityField<CollateralValuationSummaryResponse>(dto => dto.ValuationMethod, "Method", "ValuationMethod"),
                new EntityField<CollateralValuationSummaryResponse>(dto => dto.ValuationDate, "Date", "ValuationDate", typeof(DateTimeOffset)),
                new EntityField<CollateralValuationSummaryResponse>(dto => dto.ExpiryDate, "Expiry", "ExpiryDate", typeof(DateTimeOffset)),
                new EntityField<CollateralValuationSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCollateralValuationsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCollateralValuationsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CollateralValuationSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCollateralValuationAsync("1", viewModel.Adapt<CreateCollateralValuationCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateCollateralValuationAsync("1", id, viewModel.Adapt<UpdateCollateralValuationCommand>()).ConfigureAwait(false);
            },
            entityName: "Collateral Valuation",
            entityNamePlural: "Collateral Valuations",
            entityResource: FshResources.CollateralValuations,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var valuation = await Client.GetCollateralValuationAsync("1", id).ConfigureAwait(false);

        var parameters = new DialogParameters
        {
            { "Valuation", valuation }
        };

        await DialogService.ShowAsync<CollateralValuationDetailsDialog>("Collateral Valuation Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show collateral valuation help dialog.
    /// </summary>
    private async Task ShowCollateralValuationHelp()
    {
        await DialogService.ShowAsync<CollateralValuationsHelpDialog>("Collateral Valuation Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
