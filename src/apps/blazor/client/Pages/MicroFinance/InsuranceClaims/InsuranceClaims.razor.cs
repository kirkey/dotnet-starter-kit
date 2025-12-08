namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InsuranceClaims;

public partial class InsuranceClaims
{
    protected EntityServerTableContext<InsuranceClaimResponse, DefaultIdType, InsuranceClaimViewModel> Context { get; set; } = null!;
    private EntityTable<InsuranceClaimResponse, DefaultIdType, InsuranceClaimViewModel> _table = null!;

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

        Context = new EntityServerTableContext<InsuranceClaimResponse, DefaultIdType, InsuranceClaimViewModel>(
            fields:
            [
                new EntityField<InsuranceClaimResponse>(dto => dto.ClaimNumber, "Claim #", "ClaimNumber"),
                new EntityField<InsuranceClaimResponse>(dto => dto.ClaimType, "Type", "ClaimType"),
                new EntityField<InsuranceClaimResponse>(dto => dto.ClaimAmount, "Claim Amount", "ClaimAmount", typeof(decimal)),
                new EntityField<InsuranceClaimResponse>(dto => dto.ApprovedAmount, "Approved", "ApprovedAmount", typeof(decimal)),
                new EntityField<InsuranceClaimResponse>(dto => dto.IncidentDate, "Incident Date", "IncidentDate", typeof(DateTimeOffset)),
                new EntityField<InsuranceClaimResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchInsuranceClaimsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchInsuranceClaimsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<InsuranceClaimResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.SubmitInsuranceClaimAsync("1", viewModel.Adapt<SubmitInsuranceClaimCommand>()).ConfigureAwait(false);
            },
            entityName: "Insurance Claim",
            entityNamePlural: "Insurance Claims",
            entityResource: FshResources.InsuranceClaims,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var claim = await Client.GetInsuranceClaimAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "Claim", claim } };
        await DialogService.ShowAsync<InsuranceClaimDetailsDialog>("Insurance Claim Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }
}
