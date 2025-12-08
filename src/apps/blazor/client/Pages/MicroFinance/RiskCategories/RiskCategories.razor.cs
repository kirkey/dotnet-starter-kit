namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.RiskCategories;

public partial class RiskCategories
{
    protected EntityServerTableContext<RiskCategorySummaryResponse, DefaultIdType, RiskCategoryViewModel> Context { get; set; } = null!;
    private EntityTable<RiskCategorySummaryResponse, DefaultIdType, RiskCategoryViewModel> _table = null!;

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

        Context = new EntityServerTableContext<RiskCategorySummaryResponse, DefaultIdType, RiskCategoryViewModel>(
            fields:
            [
                new EntityField<RiskCategorySummaryResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<RiskCategorySummaryResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<RiskCategorySummaryResponse>(dto => dto.RiskType, "Risk Type", "RiskType"),
                new EntityField<RiskCategorySummaryResponse>(dto => dto.DefaultSeverity, "Severity", "DefaultSeverity"),
                new EntityField<RiskCategorySummaryResponse>(dto => dto.WeightFactor, "Weight", "WeightFactor", typeof(decimal)),
                new EntityField<RiskCategorySummaryResponse>(dto => dto.RequiresEscalation, "Escalation", "RequiresEscalation", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchRiskCategoriesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchRiskCategoriesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<RiskCategorySummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateRiskCategoryAsync("1", viewModel.Adapt<CreateRiskCategoryCommand>()).ConfigureAwait(false);
            },
            entityName: "Risk Category",
            entityNamePlural: "Risk Categories",
            entityResource: FshResources.RiskCategories,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var category = await Client.GetRiskCategoryAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Category", category } };
        await DialogService.ShowAsync<RiskCategoryDetailsDialog>("Risk Category Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show risk category help dialog.
    /// </summary>
    private async Task ShowRiskCategoryHelp()
    {
        await DialogService.ShowAsync<RiskCategoryHelpDialog>("Risk Category Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
