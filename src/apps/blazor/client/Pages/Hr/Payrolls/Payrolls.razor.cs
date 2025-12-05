using FSH.Starter.Blazor.Infrastructure.Api;
namespace FSH.Starter.Blazor.Client.Pages.Hr.Payrolls;

/// <summary>
/// Payrolls page for managing payroll periods and processing.
/// Provides CRUD operations and workflow actions for payroll lifecycle.
/// </summary>
public partial class Payrolls
{
    

    protected EntityServerTableContext<PayrollResponse, DefaultIdType, PayrollViewModel> Context { get; set; } = null!;

    private EntityTable<PayrollResponse, DefaultIdType, PayrollViewModel>? _table;
    private ClientPreference _preference = new();

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

    /// <summary>
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Load preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to preference changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<PayrollResponse, DefaultIdType, PayrollViewModel>(
            entityName: "Payroll",
            entityNamePlural: "Payrolls",
            entityResource: FshResources.Payroll,
            fields:
            [
                new EntityField<PayrollResponse>(response => response.StartDate.ToShortDateString(), "Start Date", "StartDate"),
                new EntityField<PayrollResponse>(response => response.EndDate.ToShortDateString(), "End Date", "EndDate"),
                new EntityField<PayrollResponse>(response => response.PayFrequency ?? "-", "Frequency", "PayFrequency"),
                new EntityField<PayrollResponse>(response => response.Status ?? "Draft", "Status", "Status"),
                new EntityField<PayrollResponse>(response => response.EmployeeCount, "Employees", "EmployeeCount"),
                new EntityField<PayrollResponse>(response => response.TotalGrossPay.ToString("C"), "Gross Pay", "TotalGrossPay"),
                new EntityField<PayrollResponse>(response => response.TotalNetPay.ToString("C"), "Net Pay", "TotalNetPay"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchPayrollsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchPayrollsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PayrollResponse>>();
            },
            createFunc: async payroll =>
            {
                await Client.CreatePayrollEndpointAsync("1", payroll.Adapt<CreatePayrollCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, payroll) =>
            {
                await Client.UpdatePayrollEndpointAsync("1", id, payroll.Adapt<UpdatePayrollCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id =>
            {
                await Client.DeletePayrollEndpointAsync("1", id).ConfigureAwait(false);
            },
            getDefaultsFunc: () => Task.FromResult(new PayrollViewModel()),
            hasExtraActionsFunc: () => true);

        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Shows the payrolls help dialog.
    /// </summary>
    private async Task ShowPayrollsHelp()
    {
        await DialogService.ShowAsync<PayrollsHelpDialog>("Payrolls Help", new DialogParameters(), _helpDialogOptions);
    }

    /// <summary>
    /// Processes the payroll to calculate pay for all employees in the period.
    /// </summary>
    private async Task ProcessPayrollAsync(PayrollResponse payroll)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Confirm Process",
            "Are you sure you want to process this payroll? This will calculate pay for all employees in the period.",
            yesText: "Process", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Client.ProcessPayrollEndpointAsync("1", payroll.Id);
                Snackbar.Add("Payroll is now processing", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }
    }

    /// <summary>
    /// Completes payroll processing and finalizes calculations.
    /// </summary>
    private async Task CompleteProcessingAsync(PayrollResponse payroll)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Confirm Complete",
            "Are you sure you want to complete processing? This will finalize payroll calculations.",
            yesText: "Complete", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Client.CompletePayrollProcessingEndpointAsync("1", payroll.Id);
                Snackbar.Add("Payroll processing completed", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }
    }

    /// <summary>
    /// Posts the payroll to the General Ledger.
    /// </summary>
    private async Task PostPayrollAsync(PayrollResponse payroll)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Confirm Post",
            "Are you sure you want to post this payroll to the General Ledger? This action cannot be undone.",
            yesText: "Post", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                var command = new PostPayrollCommand { Id = payroll.Id };
                await Client.PostPayrollEndpointAsync("1", payroll.Id, command);
                Snackbar.Add("Payroll posted to GL", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }
    }

    /// <summary>
    /// Marks the payroll as paid, indicating all employees have been paid.
    /// </summary>
    private async Task MarkAsPaidAsync(PayrollResponse payroll)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Confirm Payment",
            "Are you sure you want to mark this payroll as paid? This indicates all employees have been paid.",
            yesText: "Mark as Paid", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Client.MarkPayrollAsPaidEndpointAsync("1", payroll.Id);
                Snackbar.Add("Payroll marked as paid", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }
    }
}
