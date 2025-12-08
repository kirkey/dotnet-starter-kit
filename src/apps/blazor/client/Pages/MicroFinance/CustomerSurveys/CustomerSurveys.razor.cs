namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CustomerSurveys;

/// <summary>
/// Customer Surveys page logic. Manages customer feedback surveys.
/// </summary>
public partial class CustomerSurveys
{
    protected EntityServerTableContext<CustomerSurveySummaryResponse, DefaultIdType, CustomerSurveyViewModel> Context { get; set; } = null!;
    private EntityTable<CustomerSurveySummaryResponse, DefaultIdType, CustomerSurveyViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();
    private bool _canActivate;
    private bool _canComplete;

    private string? _searchSurveyType;
    private string? SearchSurveyType
    {
        get => _searchSurveyType;
        set
        {
            _searchSurveyType = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchStatus;
    private string? SearchStatus
    {
        get => _searchStatus;
        set
        {
            _searchStatus = value;
            _ = _table.ReloadDataAsync();
        }
    }

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

        Context = new EntityServerTableContext<CustomerSurveySummaryResponse, DefaultIdType, CustomerSurveyViewModel>(
            fields:
            [
                new EntityField<CustomerSurveySummaryResponse>(dto => dto.Title, "Title", "Title"),
                new EntityField<CustomerSurveySummaryResponse>(dto => dto.SurveyType, "Type", "SurveyType"),
                new EntityField<CustomerSurveySummaryResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<CustomerSurveySummaryResponse>(dto => dto.StartDate, "Start", "StartDate", typeof(DateTimeOffset)),
                new EntityField<CustomerSurveySummaryResponse>(dto => dto.TotalResponses, "Responses", "TotalResponses", typeof(int)),
                new EntityField<CustomerSurveySummaryResponse>(dto => dto.AverageScore, "Avg Score", "AverageScore", typeof(decimal)),
                new EntityField<CustomerSurveySummaryResponse>(dto => dto.NpsScore, "NPS", "NpsScore", typeof(int)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCustomerSurveysCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCustomerSurveysAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CustomerSurveySummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCustomerSurveyAsync("1", viewModel.Adapt<CreateCustomerSurveyCommand>()).ConfigureAwait(false);
            },
            entityName: "Customer Survey",
            entityNamePlural: "Customer Surveys",
            entityResource: FshResources.CustomerSurveys,
            hasExtraActionsFunc: () => true);

        var state = await AuthState;
        _canActivate = await AuthService.HasPermissionAsync(state.User, FshActions.Activate, FshResources.CustomerSurveys);
        _canComplete = await AuthService.HasPermissionAsync(state.User, FshActions.Complete, FshResources.CustomerSurveys);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var survey = await Client.GetCustomerSurveyAsync("1", id).ConfigureAwait(false);
        
        var parameters = new DialogParameters
        {
            { "Survey", survey }
        };

        await DialogService.ShowAsync<CustomerSurveyDetailsDialog>("Customer Survey Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    private async Task ActivateSurvey(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Activate Survey",
            "Are you sure you want to activate this survey?",
            yesText: "Activate",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ActivateCustomerSurveyAsync("1", id),
                successMessage: "Survey activated successfully.");
            await _table.ReloadDataAsync();
        }
    }

    private async Task CompleteSurvey(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Complete Survey",
            "Are you sure you want to complete this survey? No more responses will be accepted.",
            yesText: "Complete",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.CompleteCustomerSurveyAsync("1", id),
                successMessage: "Survey completed successfully.");
            await _table.ReloadDataAsync();
        }
    }
}
