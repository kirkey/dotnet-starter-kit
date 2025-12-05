namespace FSH.Starter.Blazor.Client.Pages.Accounting.Payments;

/// <summary>
/// Payments page for managing cash receipts from customers/members.
/// Supports Create, Update, Delete, Allocate, Refund, and Void operations.
/// </summary>
public partial class Payments
{
    protected EntityServerTableContext<PaymentSearchResponse, DefaultIdType, PaymentViewModel> Context { get; set; } = null!;
    private EntityTable<PaymentSearchResponse, DefaultIdType, PaymentViewModel> _table = null!;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    private string? PaymentNumber { get; set; }
    private string? PaymentMethod { get; set; }
    private DateTime? StartDate { get; set; }
    private DateTime? EndDate { get; set; }
    private bool HasUnappliedAmount { get; set; }

    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

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

        Context = new EntityServerTableContext<PaymentSearchResponse, DefaultIdType, PaymentViewModel>(
            entityName: "Payment",
            entityNamePlural: "Payments",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<PaymentSearchResponse>(x => x.PaymentNumber, "Payment #", "PaymentNumber"),
                new EntityField<PaymentSearchResponse>(x => x.PaymentDate, "Date", "PaymentDate", typeof(DateOnly)),
                new EntityField<PaymentSearchResponse>(x => x.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<PaymentSearchResponse>(x => x.UnappliedAmount, "Unapplied", "UnappliedAmount", typeof(decimal)),
                new EntityField<PaymentSearchResponse>(x => x.PaymentMethod, "Method", "PaymentMethod"),
                new EntityField<PaymentSearchResponse>(x => x.ReferenceNumber, "Reference", "ReferenceNumber"),
                new EntityField<PaymentSearchResponse>(x => x.AllocationCount, "Allocations", "AllocationCount", typeof(int)),
            ],
            enableAdvancedSearch: true,
            idFunc: x => x.Id,
            searchFunc: async filter =>
            {
                var request = new PaymentSearchRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    PaymentNumber = PaymentNumber,
                    PaymentMethod = PaymentMethod,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    HasUnappliedAmount = HasUnappliedAmount
                };
                var result = await Client.PaymentSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<PaymentSearchResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreatePaymentCommand
                {
                    PaymentNumber = vm.PaymentNumber,
                    MemberId = vm.MemberId,
                    PaymentDate = vm.PaymentDate!.Value,
                    Amount = vm.Amount,
                    PaymentMethod = vm.PaymentMethod,
                    ReferenceNumber = vm.ReferenceNumber,
                    DepositToAccountCode = vm.DepositToAccountCode,
                    Description = vm.Description,
                    Notes = vm.Notes
                };
                await Client.PaymentCreateEndpointAsync("1", command);
                Snackbar.Add("Payment created successfully", Severity.Success);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdatePaymentCommand
                {
                    Id = id,
                    ReferenceNumber = vm.ReferenceNumber,
                    DepositToAccountCode = vm.DepositToAccountCode,
                    Description = vm.Description,
                    Notes = vm.Notes
                };
                await Client.PaymentUpdateEndpointAsync("1", id, command);
                Snackbar.Add("Payment updated successfully", Severity.Success);
            },
            deleteFunc: async id =>
            {
                await Client.PaymentDeleteEndpointAsync("1", id);
                Snackbar.Add("Payment deleted successfully", Severity.Success);
            });

        await Task.CompletedTask;
    }

    /// <summary>
    /// Shows payment details in a side drawer.
    /// </summary>
    private async Task OnViewDetails(DefaultIdType paymentId)
    {
        try
        {
            var payment = await Client.PaymentGetEndpointAsync("1", paymentId);
            
            var parameters = new DialogParameters
            {
                { nameof(PaymentDetailsDialog.Payment), payment }
            };
            await DialogService.ShowAsync<PaymentDetailsDialog>("Payment Details", parameters, _dialogOptions);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading payment details: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Opens the allocation dialog for splitting payment across invoices.
    /// </summary>
    private async Task OnAllocate(DefaultIdType paymentId, decimal unappliedAmount)
    {
        var parameters = new DialogParameters
        {
            { nameof(PaymentAllocationDialog.PaymentId), paymentId },
            { nameof(PaymentAllocationDialog.UnappliedAmount), unappliedAmount }
        };
        var dialog = await DialogService.ShowAsync<PaymentAllocationDialog>("Allocate Payment", parameters, _dialogOptions);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Opens the refund dialog to process refunds.
    /// </summary>
    private async Task OnRefund(DefaultIdType paymentId)
    {
        var parameters = new DialogParameters
        {
            { nameof(RefundPaymentDialog.PaymentId), paymentId }
        };
        var dialog = await DialogService.ShowAsync<RefundPaymentDialog>("Refund Payment", parameters, _dialogOptions);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            Snackbar.Add("Payment refunded successfully", Severity.Success);
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Opens the void dialog to reverse entire payment transaction.
    /// </summary>
    private async Task OnVoid(DefaultIdType paymentId)
    {
        var parameters = new DialogParameters
        {
            { nameof(VoidPaymentDialog.PaymentId), paymentId }
        };
        var dialog = await DialogService.ShowAsync<VoidPaymentDialog>("Void Payment", parameters, _dialogOptions);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            Snackbar.Add("Payment voided successfully", Severity.Success);
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Shows help information about the Payments module.
    /// </summary>
    private async Task ShowPaymentInfo()
    {
        await DialogService.ShowAsync<PaymentHelpDialog>("Payment Help", new DialogParameters(), new DialogOptions 
        { 
            MaxWidth = MaxWidth.Medium, 
            FullWidth = true 
        });
    }
}
