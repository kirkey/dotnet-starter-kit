using FSH.Starter.Blazor.Client.Components.Dialogs;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.InsuranceClaims.Dialogs;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InsuranceClaims;

/// <summary>
/// Insurance Claims page logic. Provides submission and workflow operations for insurance claims.
/// </summary>
public partial class InsuranceClaims
{
    protected EntityServerTableContext<InsuranceClaimResponse, DefaultIdType, InsuranceClaimViewModel> Context { get; set; } = null!;

    private EntityTable<InsuranceClaimResponse, DefaultIdType, InsuranceClaimViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    // Permission flags
    private bool _canReview;
    private bool _canApprove;
    private bool _canReject;
    private bool _canSettle;

    private ClientPreference _preference = new();

    // Date picker helper
    private DateTime? _incidentDate = DateTime.Today;

    // Status filter
    private string? _statusFilter;

    private void OnStatusFilterChanged(string? value)
    {
        _statusFilter = value;
        _ = _table.ReloadDataAsync();
    }

    // Advanced search filters
    private string? _searchClaimNumber;
    private string? SearchClaimNumber
    {
        get => _searchClaimNumber;
        set
        {
            _searchClaimNumber = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchClaimType;
    private string? SearchClaimType
    {
        get => _searchClaimType;
        set
        {
            _searchClaimType = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DateTime? _searchFromDate;

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

        // Check permissions
        var state = await AuthState;
        _canReview = await AuthService.HasPermissionAsync(state.User, FshPermissions.InsuranceClaims.Review);
        _canApprove = await AuthService.HasPermissionAsync(state.User, FshPermissions.InsuranceClaims.Approve);
        _canReject = await AuthService.HasPermissionAsync(state.User, FshPermissions.InsuranceClaims.Reject);
        _canSettle = await AuthService.HasPermissionAsync(state.User, FshPermissions.InsuranceClaims.Settle);

        Context = new EntityServerTableContext<InsuranceClaimResponse, DefaultIdType, InsuranceClaimViewModel>(
            fields:
            [
                new EntityField<InsuranceClaimResponse>(dto => dto.ClaimNumber, "Claim #", "ClaimNumber"),
                new EntityField<InsuranceClaimResponse>(dto => dto.ClaimType, "Type", "ClaimType"),
                new EntityField<InsuranceClaimResponse>(dto => dto.ClaimAmount, "Amount", "ClaimAmount", typeof(decimal)),
                new EntityField<InsuranceClaimResponse>(dto => dto.ApprovedAmount, "Approved", "ApprovedAmount", typeof(decimal)),
                new EntityField<InsuranceClaimResponse>(dto => dto.IncidentDate, "Incident Date", "IncidentDate", typeof(DateOnly)),
                new EntityField<InsuranceClaimResponse>(dto => dto.FiledDate, "Filed Date", "FiledDate", typeof(DateOnly)),
                new EntityField<InsuranceClaimResponse>(dto => dto.Status, "Status", "Status"),
            ],
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            getDefaultsFunc: () => Task.FromResult(new InsuranceClaimViewModel
            {
                IncidentDate = DateOnly.FromDateTime(DateTime.Today)
            }),
            searchFunc: async filter =>
            {
                var request = filter.Adapt<SearchInsuranceClaimsCommand>();

                if (!string.IsNullOrWhiteSpace(_statusFilter))
                    request = request with { Status = _statusFilter };
                if (!string.IsNullOrWhiteSpace(_searchClaimNumber))
                    request = request with { ClaimNumber = _searchClaimNumber };
                if (!string.IsNullOrWhiteSpace(_searchClaimType))
                    request = request with { ClaimType = _searchClaimType };

                var response = await Client.SearchInsuranceClaimsEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<InsuranceClaimResponse>>();
            },
            getDetailsFunc: async id =>
            {
                var response = await Client.GetInsuranceClaimEndpointAsync("1", id);
                return response.Adapt<InsuranceClaimViewModel>();
            },
            createFunc: async vm =>
            {
                var command = new SubmitInsuranceClaimCommand
                {
                    PolicyId = vm.PolicyId,
                    ClaimType = vm.ClaimType,
                    ClaimAmount = vm.ClaimAmount,
                    IncidentDate = vm.IncidentDate,
                    Description = vm.Description ?? string.Empty,
                    SupportingDocuments = vm.SupportingDocuments
                };
                var response = await Client.SubmitInsuranceClaimEndpointAsync("1", command);
                return response.Id;
            },
            deleteFunc: async id => await Client.DeleteInsuranceClaimEndpointAsync("1", id),
            exportAction: string.Empty,
            entityTypeName: "Insurance Claim",
            entityTypeNamePlural: "Insurance Claims",
            createPermission: FshPermissions.InsuranceClaims.Create,
            updatePermission: FshPermissions.InsuranceClaims.Update,
            deletePermission: FshPermissions.InsuranceClaims.Delete,
            searchPermission: FshPermissions.InsuranceClaims.View
        );
    }

    private void OnIncidentDateChanged(InsuranceClaimViewModel context, DateTime? date)
    {
        _incidentDate = date;
        if (date.HasValue)
        {
            context.IncidentDate = DateOnly.FromDateTime(date.Value);
        }
    }

    private async Task ReviewClaim(DefaultIdType id)
    {
        var command = new ReviewInsuranceClaimCommand { Id = id };
        await ApiHelper.ExecuteCallGuardedAsync(
            async () => await Client.ReviewInsuranceClaimEndpointAsync("1", id, command),
            Snackbar,
            successMessage: "Claim marked as under review.");
        await _table.ReloadDataAsync();
    }

    private async Task ShowApproveDialog(InsuranceClaimResponse entity)
    {
        var parameters = new DialogParameters<InsuranceClaimApproveDialog>
        {
            { x => x.ClaimId, entity.Id },
            { x => x.ClaimNumber, entity.ClaimNumber },
            { x => x.ClaimAmount, entity.ClaimAmount }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<InsuranceClaimApproveDialog>("Approve Claim", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowRejectDialog(InsuranceClaimResponse entity)
    {
        var parameters = new DialogParameters<InsuranceClaimRejectDialog>
        {
            { x => x.ClaimId, entity.Id },
            { x => x.ClaimNumber, entity.ClaimNumber }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<InsuranceClaimRejectDialog>("Reject Claim", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task SettleClaim(DefaultIdType id)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Settle Claim",
            "Are you sure you want to mark this claim as settled? This confirms payment has been made.",
            yesText: "Settle",
            cancelText: "Cancel");

        if (confirm == true)
        {
            var command = new SettleInsuranceClaimCommand { Id = id };
            await ApiHelper.ExecuteCallGuardedAsync(
                async () => await Client.SettleInsuranceClaimEndpointAsync("1", id, command),
                Snackbar,
                successMessage: "Claim settled successfully.");
            await _table.ReloadDataAsync();
        }
    }

    private async Task ViewClaimDetails(DefaultIdType id)
    {
        var entity = await Client.GetInsuranceClaimEndpointAsync("1", id);

        var parameters = new DialogParameters<InsuranceClaimDetailsDialog>
        {
            { x => x.Entity, entity }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<InsuranceClaimDetailsDialog>("Claim Details", parameters, options);
    }

    private async Task ShowInsuranceClaimsHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<InsuranceClaimsHelpDialog>("Insurance Claims Help", options);
    }
}
