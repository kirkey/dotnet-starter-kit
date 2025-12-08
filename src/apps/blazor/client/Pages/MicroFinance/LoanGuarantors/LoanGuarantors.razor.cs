namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanGuarantors;

/// <summary>
/// LoanGuarantors page logic. Provides search and view operations for LoanGuarantor entities.
/// Manages member guarantees for loans.
/// </summary>
public partial class LoanGuarantors
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<LoanGuarantorResponse, DefaultIdType, LoanGuarantorViewModel> Context { get; set; } = null!;

    private EntityTable<LoanGuarantorResponse, DefaultIdType, LoanGuarantorViewModel> _table = null!;

    /// <summary>
    /// Authorization state for permission checks.
    /// </summary>
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    /// <summary>
    /// Authorization service for permission checks.
    /// </summary>
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    // Permission flags
    private bool _canManage;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchRelationship;
    private string? SearchRelationship
    {
        get => _searchRelationship;
        set
        {
            _searchRelationship = value;
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

    /// <summary>
    /// Initializes the table context with loan guarantor-specific configuration.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Load initial preference from localStorage
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

        Context = new EntityServerTableContext<LoanGuarantorResponse, DefaultIdType, LoanGuarantorViewModel>(
            fields:
            [
                new EntityField<LoanGuarantorResponse>(dto => dto.GuarantorMemberId, "Guarantor ID", "GuarantorMemberId"),
                new EntityField<LoanGuarantorResponse>(dto => dto.GuaranteedAmount, "Amount", "GuaranteedAmount", typeof(decimal)),
                new EntityField<LoanGuarantorResponse>(dto => dto.Relationship, "Relationship", "Relationship"),
                new EntityField<LoanGuarantorResponse>(dto => dto.GuaranteeDate, "Guarantee Date", "GuaranteeDate", typeof(DateTimeOffset)),
                new EntityField<LoanGuarantorResponse>(dto => dto.ExpiryDate, "Expiry Date", "ExpiryDate", typeof(DateTimeOffset?)),
                new EntityField<LoanGuarantorResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchLoanGuarantorsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLoanGuarantorsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LoanGuarantorResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateLoanGuarantorAsync("1", viewModel.Adapt<CreateLoanGuarantorCommand>()).ConfigureAwait(false);
            },
            entityName: "Loan Guarantor",
            entityNamePlural: "Loan Guarantors",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canManage);

        // Check permissions for extra actions
        var state = await AuthState;
        _canManage = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show help dialog.
    /// </summary>
    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<LoanGuarantorsHelpDialog>("Loan Guarantors Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View guarantor details in a dialog.
    /// </summary>
    private async Task ViewGuarantorDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(LoanGuarantorDetailsDialog.GuarantorId), id }
        };

        await DialogService.ShowAsync<LoanGuarantorDetailsDialog>("Guarantor Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Release a guarantor from their obligation.
    /// </summary>
    private async Task ReleaseGuarantor(DefaultIdType id)
    {
        var result = await DialogService.ShowMessageBox(
            "Release Guarantee",
            "Are you sure you want to release this guarantor from their obligation?",
            "Release", cancelText: "Cancel");

        if (result == true)
        {
            var command = new ReleaseGuarantorCommand { Id = id, Reason = "Released by user" };
            await Client.ReleaseLoanGuarantorAsync("1", id, command).ConfigureAwait(false);
            await _table.ReloadDataAsync();
        }
    }
}
