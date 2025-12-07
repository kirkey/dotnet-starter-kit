using FSH.Starter.Blazor.Client.Components.Dialogs;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanApplications.Dialogs;
using FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Get.v1;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanApplications;

/// <summary>
/// Loan Applications page logic. Provides CRUD and workflow operations for loan applications.
/// </summary>
public partial class LoanApplications
{
    protected EntityServerTableContext<LoanApplicationResponse, DefaultIdType, LoanApplicationViewModel> Context { get; set; } = null!;

    private EntityTable<LoanApplicationResponse, DefaultIdType, LoanApplicationViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    // Permission flags
    private bool _canSubmit;
    private bool _canAssign;
    private bool _canReview;
    private bool _canApprove;
    private bool _canReject;
    private bool _canWithdraw;

    private ClientPreference _preference = new();

    // Status filter
    private string? _statusFilter;

    private void OnStatusFilterChanged(string? value)
    {
        _statusFilter = value;
        _ = _table.ReloadDataAsync();
    }

    // Advanced search filters
    private string? _searchApplicationNumber;
    private string? SearchApplicationNumber
    {
        get => _searchApplicationNumber;
        set
        {
            _searchApplicationNumber = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DateTime? _searchFromDate;
    private DateTime? _searchToDate;

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
        _canSubmit = await AuthService.HasPermissionAsync(state.User, FshPermissions.LoanApplications.Submit);
        _canAssign = await AuthService.HasPermissionAsync(state.User, FshPermissions.LoanApplications.Assign);
        _canReview = await AuthService.HasPermissionAsync(state.User, FshPermissions.LoanApplications.Review);
        _canApprove = await AuthService.HasPermissionAsync(state.User, FshPermissions.LoanApplications.Approve);
        _canReject = await AuthService.HasPermissionAsync(state.User, FshPermissions.LoanApplications.Reject);
        _canWithdraw = await AuthService.HasPermissionAsync(state.User, FshPermissions.LoanApplications.Withdraw);

        Context = new EntityServerTableContext<LoanApplicationResponse, DefaultIdType, LoanApplicationViewModel>(
            fields:
            [
                new EntityField<LoanApplicationResponse>(dto => dto.ApplicationNumber, "Application #", "ApplicationNumber"),
                new EntityField<LoanApplicationResponse>(dto => dto.ApplicationDate, "Date", "ApplicationDate", typeof(DateOnly)),
                new EntityField<LoanApplicationResponse>(dto => dto.RequestedAmount, "Requested Amount", "RequestedAmount", typeof(decimal)),
                new EntityField<LoanApplicationResponse>(dto => dto.ApprovedAmount, "Approved Amount", "ApprovedAmount", typeof(decimal)),
                new EntityField<LoanApplicationResponse>(dto => dto.RequestedTermMonths, "Term (Months)", "RequestedTermMonths"),
                new EntityField<LoanApplicationResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<LoanApplicationResponse>(dto => dto.Purpose, "Purpose", "Purpose"),
            ],
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            getDefaultsFunc: () => Task.FromResult(new LoanApplicationViewModel
            {
                RequestedAmount = 100000m,
                RequestedTermMonths = 12
            }),
            searchFunc: async filter =>
            {
                var request = filter.Adapt<SearchLoanApplicationsCommand>();

                if (!string.IsNullOrWhiteSpace(_statusFilter))
                    request = request with { Status = _statusFilter };
                if (!string.IsNullOrWhiteSpace(_searchApplicationNumber))
                    request = request with { ApplicationNumber = _searchApplicationNumber };

                var response = await MicroFinanceClient.SearchLoanApplicationsEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<LoanApplicationResponse>>();
            },
            getDetailsFunc: async id =>
            {
                var response = await MicroFinanceClient.GetLoanApplicationEndpointAsync("1", id);
                return response.Adapt<LoanApplicationViewModel>();
            },
            createFunc: async vm =>
            {
                var command = new CreateLoanApplicationCommand
                {
                    MemberId = vm.MemberId,
                    ProductId = vm.LoanProductId,
                    RequestedAmount = vm.RequestedAmount,
                    RequestedTermMonths = vm.RequestedTermMonths,
                    Purpose = vm.Purpose ?? string.Empty,
                    GroupId = vm.MemberGroupId
                };
                var response = await MicroFinanceClient.CreateLoanApplicationEndpointAsync("1", command);
                return response.Id;
            },
            deleteFunc: async id => await MicroFinanceClient.DeleteLoanApplicationEndpointAsync("1", id),
            exportAction: string.Empty,
            entityTypeName: "Loan Application",
            entityTypeNamePlural: "Loan Applications",
            createPermission: FshPermissions.LoanApplications.Create,
            updatePermission: FshPermissions.LoanApplications.Update,
            deletePermission: FshPermissions.LoanApplications.Delete,
            searchPermission: FshPermissions.LoanApplications.View
        );
    }

    private async Task SubmitApplication(DefaultIdType id)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Submit Application",
            "Are you sure you want to submit this application for review?",
            yesText: "Submit",
            cancelText: "Cancel");

        if (confirm == true)
        {
            var command = new SubmitLoanApplicationCommand { Id = id };
            await ApiHelper.ExecuteCallGuardedAsync(
                async () => await MicroFinanceClient.SubmitLoanApplicationEndpointAsync("1", id, command),
                Snackbar,
                successMessage: "Application submitted successfully.");
            await _table.ReloadDataAsync();
        }
    }

    private async Task WithdrawApplication(DefaultIdType id)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Withdraw Application",
            "Are you sure you want to withdraw this application?",
            yesText: "Withdraw",
            cancelText: "Cancel");

        if (confirm == true)
        {
            var command = new WithdrawLoanApplicationCommand { Id = id };
            await ApiHelper.ExecuteCallGuardedAsync(
                async () => await MicroFinanceClient.WithdrawLoanApplicationEndpointAsync("1", id, command),
                Snackbar,
                successMessage: "Application withdrawn successfully.");
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowAssignDialog(LoanApplicationResponse entity)
    {
        var parameters = new DialogParameters<LoanApplicationAssignDialog>
        {
            { x => x.ApplicationId, entity.Id },
            { x => x.ApplicationNumber, entity.ApplicationNumber }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<LoanApplicationAssignDialog>("Assign Loan Officer", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowReviewDialog(LoanApplicationResponse entity)
    {
        var parameters = new DialogParameters<LoanApplicationReviewDialog>
        {
            { x => x.ApplicationId, entity.Id },
            { x => x.ApplicationNumber, entity.ApplicationNumber },
            { x => x.RequestedAmount, entity.RequestedAmount }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        var dialog = await DialogService.ShowAsync<LoanApplicationReviewDialog>("Review Application", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowApproveDialog(LoanApplicationResponse entity)
    {
        var parameters = new DialogParameters<LoanApplicationApproveDialog>
        {
            { x => x.ApplicationId, entity.Id },
            { x => x.ApplicationNumber, entity.ApplicationNumber },
            { x => x.RequestedAmount, entity.RequestedAmount },
            { x => x.RequestedTermMonths, entity.RequestedTermMonths }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<LoanApplicationApproveDialog>("Approve Application", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowRejectDialog(LoanApplicationResponse entity)
    {
        var parameters = new DialogParameters<LoanApplicationRejectDialog>
        {
            { x => x.ApplicationId, entity.Id },
            { x => x.ApplicationNumber, entity.ApplicationNumber }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<LoanApplicationRejectDialog>("Reject Application", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ViewApplicationDetails(DefaultIdType id)
    {
        var entity = await MicroFinanceClient.GetLoanApplicationEndpointAsync("1", id);

        var parameters = new DialogParameters<LoanApplicationDetailsDialog>
        {
            { x => x.Entity, entity }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<LoanApplicationDetailsDialog>("Application Details", parameters, options);
    }

    private async Task ShowLoanApplicationsHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<LoanApplicationsHelpDialog>("Loan Applications Help", options);
    }
}
