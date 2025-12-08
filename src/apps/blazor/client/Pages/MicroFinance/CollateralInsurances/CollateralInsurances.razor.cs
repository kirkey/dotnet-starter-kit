namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollateralInsurances;

public partial class CollateralInsurances
{
    protected EntityServerTableContext<CollateralInsuranceSummaryResponse, DefaultIdType, CollateralInsuranceViewModel> Context { get; set; } = null!;
    private EntityTable<CollateralInsuranceSummaryResponse, DefaultIdType, CollateralInsuranceViewModel> _table = null!;

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

        Context = new EntityServerTableContext<CollateralInsuranceSummaryResponse, DefaultIdType, CollateralInsuranceViewModel>(
            fields:
            [
                new EntityField<CollateralInsuranceSummaryResponse>(dto => dto.PolicyNumber, "Policy #", "PolicyNumber"),
                new EntityField<CollateralInsuranceSummaryResponse>(dto => dto.InsurerName, "Insurer", "InsurerName"),
                new EntityField<CollateralInsuranceSummaryResponse>(dto => dto.InsuranceType, "Type", "InsuranceType"),
                new EntityField<CollateralInsuranceSummaryResponse>(dto => dto.CoverageAmount, "Coverage", "CoverageAmount", typeof(decimal)),
                new EntityField<CollateralInsuranceSummaryResponse>(dto => dto.ExpiryDate, "Expiry", "ExpiryDate", typeof(DateTimeOffset)),
                new EntityField<CollateralInsuranceSummaryResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCollateralInsurancesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCollateralInsurancesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CollateralInsuranceSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCollateralInsuranceAsync("1", viewModel.Adapt<CreateCollateralInsuranceCommand>()).ConfigureAwait(false);
            },
            entityName: "Collateral Insurance",
            entityNamePlural: "Collateral Insurances",
            entityResource: FshResources.CollateralInsurances,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var insurance = await Client.GetCollateralInsuranceAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Insurance", insurance } };
        await DialogService.ShowAsync<CollateralInsuranceDetailsDialog>("Collateral Insurance Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show collateral insurance help dialog.
    /// </summary>
    private async Task ShowCollateralInsuranceHelp()
    {
        await DialogService.ShowAsync<CollateralInsurancesHelpDialog>("Collateral Insurance Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
