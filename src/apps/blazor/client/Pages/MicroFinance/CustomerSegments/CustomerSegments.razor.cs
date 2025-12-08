namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CustomerSegments;

/// <summary>
/// Customer Segments page logic. Manages customer segmentation for targeted services.
/// </summary>
public partial class CustomerSegments
{
    protected EntityServerTableContext<CustomerSegmentSummaryResponse, DefaultIdType, CustomerSegmentViewModel> Context { get; set; } = null!;
    private EntityTable<CustomerSegmentSummaryResponse, DefaultIdType, CustomerSegmentViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();

    private string? _searchSegmentType;
    private string? SearchSegmentType
    {
        get => _searchSegmentType;
        set
        {
            _searchSegmentType = value;
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

        Context = new EntityServerTableContext<CustomerSegmentSummaryResponse, DefaultIdType, CustomerSegmentViewModel>(
            fields:
            [
                new EntityField<CustomerSegmentSummaryResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<CustomerSegmentSummaryResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<CustomerSegmentSummaryResponse>(dto => dto.SegmentType, "Type", "SegmentType"),
                new EntityField<CustomerSegmentSummaryResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<CustomerSegmentSummaryResponse>(dto => dto.Priority, "Priority", "Priority", typeof(int)),
                new EntityField<CustomerSegmentSummaryResponse>(dto => dto.MemberCount, "Members", "MemberCount", typeof(int)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchCustomerSegmentsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchCustomerSegmentsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CustomerSegmentSummaryResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCustomerSegmentAsync("1", viewModel.Adapt<CreateCustomerSegmentCommand>()).ConfigureAwait(false);
            },
            entityName: "Customer Segment",
            entityNamePlural: "Customer Segments",
            entityResource: FshResources.CustomerSegments,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var segment = await Client.GetCustomerSegmentAsync("1", id).ConfigureAwait(false);
        
        var parameters = new DialogParameters
        {
            { "Segment", segment }
        };

        await DialogService.ShowAsync<CustomerSegmentDetailsDialog>("Customer Segment Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
