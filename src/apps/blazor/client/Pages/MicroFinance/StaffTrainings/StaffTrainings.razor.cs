namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.StaffTrainings;

/// <summary>
/// Staff Trainings page logic. Manages training records for staff members.
/// </summary>
public partial class StaffTrainings
{
    protected EntityServerTableContext<StaffTrainingSummaryResponse, DefaultIdType, StaffTrainingViewModel> Context { get; set; } = null!;
    private EntityTable<StaffTrainingSummaryResponse, DefaultIdType, StaffTrainingViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();

    private string? _searchTrainingType;
    private string? SearchTrainingType
    {
        get => _searchTrainingType;
        set
        {
            _searchTrainingType = value;
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

        Context = new EntityServerTableContext<StaffTrainingSummaryResponse, DefaultIdType, StaffTrainingViewModel>(
            fields:
            [
                new EntityField<StaffTrainingSummaryResponse>(dto => dto.TrainingCode, "Code", "TrainingCode"),
                new EntityField<StaffTrainingSummaryResponse>(dto => dto.TrainingName, "Training Name", "TrainingName"),
                new EntityField<StaffTrainingSummaryResponse>(dto => dto.TrainingType, "Type", "TrainingType"),
                new EntityField<StaffTrainingSummaryResponse>(dto => dto.DeliveryMethod, "Method", "DeliveryMethod"),
                new EntityField<StaffTrainingSummaryResponse>(dto => dto.StartDate, "Start Date", "StartDate", typeof(DateTimeOffset)),
                new EntityField<StaffTrainingSummaryResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<StaffTrainingSummaryResponse>(dto => dto.IsMandatory, "Mandatory", "IsMandatory", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchStaffTrainingsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchStaffTrainingsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<StaffTrainingSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.ScheduleStaffTrainingAsync("1", viewModel.Adapt<ScheduleStaffTrainingCommand>()).ConfigureAwait(false);
            },
            entityName: "Staff Training",
            entityNamePlural: "Staff Trainings",
            entityResource: FshResources.StaffTrainings,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var training = await Client.GetStaffTrainingAsync("1", id).ConfigureAwait(false);
        
        var parameters = new DialogParameters
        {
            { "Training", training }
        };

        await DialogService.ShowAsync<StaffTrainingDetailsDialog>("Staff Training Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Show staff trainings help dialog.
    /// </summary>
    private async Task ShowStaffTrainingsHelp()
    {
        await DialogService.ShowAsync<StaffTrainingsHelpDialog>("Staff Trainings Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
