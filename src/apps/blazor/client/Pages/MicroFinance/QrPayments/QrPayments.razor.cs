namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.QrPayments;

public partial class QrPayments
{
    protected EntityServerTableContext<QrPaymentResponse, DefaultIdType, QrPaymentViewModel> Context { get; set; } = null!;
    private EntityTable<QrPaymentResponse, DefaultIdType, QrPaymentViewModel> _table = null!;

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

        Context = new EntityServerTableContext<QrPaymentResponse, DefaultIdType, QrPaymentViewModel>(
            fields:
            [
                new EntityField<QrPaymentResponse>(dto => dto.QrCode, "QR Code", "QrCode"),
                new EntityField<QrPaymentResponse>(dto => dto.QrType, "Type", "QrType"),
                new EntityField<QrPaymentResponse>(dto => dto.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<QrPaymentResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<QrPaymentResponse>(dto => dto.CurrentUses, "Uses", "CurrentUses", typeof(int)),
                new EntityField<QrPaymentResponse>(dto => dto.ExpiresAt, "Expires", "ExpiresAt", typeof(DateTime)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchQrPaymentsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchQrPaymentsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<QrPaymentResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            entityName: "QR Payment",
            entityNamePlural: "QR Payments",
            entityResource: FshResources.QrPayments,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var qrPayment = await Client.GetQrPaymentAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "QrPayment", qrPayment } };
        await DialogService.ShowAsync<QrPaymentDetailsDialog>("QR Payment Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show QR payment help dialog.
    /// </summary>
    private async Task ShowQrPaymentHelp()
    {
        await DialogService.ShowAsync<QrPaymentsHelpDialog>("QR Payment Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
