namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InsurancePolicies;

/// <summary>
/// Insurance Policies page logic. Manages customer insurance policies.
/// </summary>
public partial class InsurancePolicies
{
    protected EntityServerTableContext<InsurancePolicyResponse, DefaultIdType, InsurancePolicyViewModel> Context { get; set; } = null!;
    private EntityTable<InsurancePolicyResponse, DefaultIdType, InsurancePolicyViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();
    private bool _canActivate;
    private bool _canCancel;

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

        Context = new EntityServerTableContext<InsurancePolicyResponse, DefaultIdType, InsurancePolicyViewModel>(
            fields:
            [
                new EntityField<InsurancePolicyResponse>(dto => dto.PolicyNumber, "Policy #", "PolicyNumber"),
                new EntityField<InsurancePolicyResponse>(dto => dto.MemberId, "Member", "MemberId"),
                new EntityField<InsurancePolicyResponse>(dto => dto.CoverageAmount, "Coverage", "CoverageAmount", typeof(decimal)),
                new EntityField<InsurancePolicyResponse>(dto => dto.PremiumAmount, "Premium", "PremiumAmount", typeof(decimal)),
                new EntityField<InsurancePolicyResponse>(dto => dto.StartDate, "Start Date", "StartDate", typeof(DateTimeOffset)),
                new EntityField<InsurancePolicyResponse>(dto => dto.EndDate, "End Date", "EndDate", typeof(DateTimeOffset)),
                new EntityField<InsurancePolicyResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchInsurancePoliciesCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchInsurancePoliciesAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<InsurancePolicyResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateInsurancePolicyAsync("1", viewModel.Adapt<CreateInsurancePolicyCommand>()).ConfigureAwait(false);
            },
            entityName: "Insurance Policy",
            entityNamePlural: "Insurance Policies",
            entityResource: FshResources.InsurancePolicies,
            hasExtraActionsFunc: () => true);

        var state = await AuthState;
        _canActivate = await AuthService.HasPermissionAsync(state.User, FshActions.Activate, FshResources.InsurancePolicies);
        _canCancel = await AuthService.HasPermissionAsync(state.User, FshActions.Cancel, FshResources.InsurancePolicies);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var policy = await Client.GetInsurancePolicyAsync("1", id).ConfigureAwait(false);

        var parameters = new DialogParameters
        {
            { "Policy", policy }
        };

        await DialogService.ShowAsync<InsurancePolicyDetailsDialog>("Insurance Policy Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    private async Task ActivatePolicy(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Activate Policy",
            "Are you sure you want to activate this insurance policy?",
            yesText: "Activate",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.ActivateInsurancePolicyAsync("1", id),
                successMessage: "Insurance policy activated successfully.");
            await _table.ReloadDataAsync();
        }
    }

    private async Task CancelPolicy(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Cancel Policy",
            "Are you sure you want to cancel this insurance policy? This action cannot be undone.",
            yesText: "Cancel Policy",
            cancelText: "Go Back");

        if (confirmed == true)
        {
            var command = new CancelInsurancePolicyCommand
            {
                Id = id,
                Reason = "User requested cancellation"
            };
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.CancelInsurancePolicyAsync("1", id, command),
                successMessage: "Insurance policy cancelled successfully.");
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Show insurance policy help dialog.
    /// </summary>
    private async Task ShowInsurancePolicyHelp()
    {
        await DialogService.ShowAsync<InsurancePoliciesHelpDialog>("Insurance Policy Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
