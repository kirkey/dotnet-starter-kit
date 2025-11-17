namespace FSH.Starter.Blazor.Client.Pages.Accounting.Payments;

public partial class Payments
{
    protected EntityServerTableContext<PaymentSearchResponse, DefaultIdType, PaymentViewModel> Context { get; set; } = null!;
    private EntityTable<PaymentSearchResponse, DefaultIdType, PaymentViewModel> _table = null!;

    private string? PaymentNumber { get; set; }
    private string? PaymentMethod { get; set; }
    private DateTime? StartDate { get; set; }
    private DateTime? EndDate { get; set; }
    private bool HasUnappliedAmount { get; set; }

    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

    protected override Task OnInitializedAsync()
    {
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
                var command = new PaymentCreateCommand
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
                var command = new PaymentUpdateCommand
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

        return Task.CompletedTask;
    }

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
}
