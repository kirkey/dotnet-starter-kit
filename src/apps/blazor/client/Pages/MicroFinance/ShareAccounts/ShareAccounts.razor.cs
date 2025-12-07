namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ShareAccounts;

/// <summary>
/// ShareAccounts page logic. Provides CRUD and search over ShareAccount entities.
/// Manages member share ownership and transactions.
/// </summary>
public partial class ShareAccounts
{
    protected EntityServerTableContext<ShareAccountResponse, DefaultIdType, ShareAccountViewModel> Context { get; set; } = null!;

    private EntityTable<ShareAccountResponse, DefaultIdType, ShareAccountViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();

    // Autocomplete selections
    private MemberResponse? _selectedMember;
    private ShareProductResponse? _selectedShareProduct;

    // Advanced search filters
    private string? _searchAccountNumber;
    private string? SearchAccountNumber
    {
        get => _searchAccountNumber;
        set
        {
            _searchAccountNumber = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchMemberName;
    private string? SearchMemberName
    {
        get => _searchMemberName;
        set
        {
            _searchMemberName = value;
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

        Context = new EntityServerTableContext<ShareAccountResponse, DefaultIdType, ShareAccountViewModel>(
            fields:
            [
                new EntityField<ShareAccountResponse>(dto => dto.AccountNumber, "Account #", "AccountNumber"),
                new EntityField<ShareAccountResponse>(dto => dto.MemberName, "Member", "MemberName"),
                new EntityField<ShareAccountResponse>(dto => dto.ShareProductName, "Product", "ShareProductName"),
                new EntityField<ShareAccountResponse>(dto => dto.NumberOfShares, "Shares", "NumberOfShares", typeof(int)),
                new EntityField<ShareAccountResponse>(dto => dto.TotalShareValue, "Total Value", "TotalShareValue", typeof(decimal)),
                new EntityField<ShareAccountResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchShareAccountsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchShareAccountsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<ShareAccountResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                if (_selectedMember != null) viewModel.MemberId = _selectedMember.Id;
                if (_selectedShareProduct != null) viewModel.ShareProductId = _selectedShareProduct.Id;
                await Client.CreateShareAccountAsync("1", viewModel.Adapt<CreateShareAccountCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                if (_selectedMember != null) viewModel.MemberId = _selectedMember.Id;
                if (_selectedShareProduct != null) viewModel.ShareProductId = _selectedShareProduct.Id;
                await Client.UpdateShareAccountAsync("1", id, viewModel.Adapt<UpdateShareAccountCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteShareAccountAsync("1", id).ConfigureAwait(false),
            entityName: "Share Account",
            entityNamePlural: "Share Accounts",
            entityResource: FshResources.ShareAccounts,
            hasExtraActionsFunc: () => true);

        await AuthState;
    }

    private async Task<IEnumerable<MemberResponse>> SearchMembers(string searchText, CancellationToken ct)
    {
        var request = new SearchMembersCommand { PageNumber = 1, PageSize = 10, Keyword = searchText };
        var result = await Client.SearchMembersAsync("1", request).ConfigureAwait(false);
        return result.Data ?? Enumerable.Empty<MemberResponse>();
    }

    private async Task<IEnumerable<ShareProductResponse>> SearchShareProducts(string searchText, CancellationToken ct)
    {
        var request = new SearchShareProductsCommand { PageNumber = 1, PageSize = 10, Keyword = searchText };
        var result = await Client.SearchShareProductsAsync("1", request).ConfigureAwait(false);
        return result.Data ?? Enumerable.Empty<ShareProductResponse>();
    }

    private async Task ShowShareAccountsHelp()
    {
        await DialogService.ShowAsync<ShareAccountsHelpDialog>("Share Accounts Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    private async Task ViewAccountDetails(DefaultIdType id)
    {
        var account = await Client.GetShareAccountAsync("1", id).ConfigureAwait(false);
        if (account != null)
        {
            var parameters = new DialogParameters { ["Account"] = account };
            await DialogService.ShowAsync<ShareAccountDetailsDialog>("Share Account Details", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
        }
    }

    private async Task PurchaseShares(DefaultIdType id)
    {
        var account = await Client.GetShareAccountAsync("1", id).ConfigureAwait(false);
        if (account != null)
        {
            var parameters = new DialogParameters 
            { 
                ["Account"] = account,
                ["TransactionType"] = "Purchase"
            };
            var dialog = await DialogService.ShowAsync<ShareTransactionDialog>("Purchase Shares", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await _table.ReloadDataAsync();
            }
        }
    }

    private async Task RedeemShares(DefaultIdType id)
    {
        var account = await Client.GetShareAccountAsync("1", id).ConfigureAwait(false);
        if (account != null)
        {
            var parameters = new DialogParameters 
            { 
                ["Account"] = account,
                ["TransactionType"] = "Redeem"
            };
            var dialog = await DialogService.ShowAsync<ShareTransactionDialog>("Redeem Shares", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await _table.ReloadDataAsync();
            }
        }
    }

    private async Task TransferShares(DefaultIdType id)
    {
        var account = await Client.GetShareAccountAsync("1", id).ConfigureAwait(false);
        if (account != null)
        {
            var parameters = new DialogParameters 
            { 
                ["Account"] = account,
                ["TransactionType"] = "Transfer"
            };
            var dialog = await DialogService.ShowAsync<ShareTransactionDialog>("Transfer Shares", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await _table.ReloadDataAsync();
            }
        }
    }
}
