namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.PaymentGateways;

public partial class PaymentGateways
{
    protected EntityServerTableContext<PaymentGatewayResponse, DefaultIdType, PaymentGatewayViewModel> Context { get; set; } = null!;
    private EntityTable<PaymentGatewayResponse, DefaultIdType, PaymentGatewayViewModel> _table = null!;

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

        Context = new EntityServerTableContext<PaymentGatewayResponse, DefaultIdType, PaymentGatewayViewModel>(
            fields:
            [
                new EntityField<PaymentGatewayResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<PaymentGatewayResponse>(dto => dto.Provider, "Provider", "Provider"),
                new EntityField<PaymentGatewayResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<PaymentGatewayResponse>(dto => dto.TransactionFeePercent, "Fee %", "TransactionFeePercent", typeof(decimal)),
                new EntityField<PaymentGatewayResponse>(dto => dto.TransactionFeeFixed, "Fixed Fee", "TransactionFeeFixed", typeof(decimal)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchPaymentGatewaysCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchPaymentGatewaysAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PaymentGatewayResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async vm =>
            {
                var command = new CreatePaymentGatewayCommand
                {
                    Name = vm.Name,
                    Provider = vm.Provider,
                    TransactionFeePercent = vm.TransactionFeePercent,
                    TransactionFeeFixed = vm.TransactionFeeFixed,
                    MinTransactionAmount = vm.MinTransactionAmount,
                    MaxTransactionAmount = vm.MaxTransactionAmount
                };
                await Client.CreatePaymentGatewayAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdatePaymentGatewayCommand
                {
                    Id = id,
                    Name = vm.Name,
                    TransactionFeePercent = vm.TransactionFeePercent,
                    TransactionFeeFixed = vm.TransactionFeeFixed,
                    MinTransactionAmount = vm.MinTransactionAmount,
                    MaxTransactionAmount = vm.MaxTransactionAmount
                };
                await Client.UpdatePaymentGatewayAsync("1", id, command).ConfigureAwait(false);
            },
            entityName: "Payment Gateway",
            entityNamePlural: "Payment Gateways",
            entityResource: FshResources.PaymentGateways,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var gateway = await Client.GetPaymentGatewayAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "PaymentGateway", gateway } };
        await DialogService.ShowAsync<PaymentGatewayDetailsDialog>("Payment Gateway Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Show payment gateway help dialog.
    /// </summary>
    private async Task ShowPaymentGatewayHelp()
    {
        await DialogService.ShowAsync<PaymentGatewaysHelpDialog>("Payment Gateway Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
