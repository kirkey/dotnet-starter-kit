namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.FixedDeposits;

/// <summary>
/// FixedDeposits page logic. Provides CRUD and search over FixedDeposit entities.
/// Manages time deposits and term savings products.
/// </summary>
public partial class FixedDeposits
{
    protected EntityServerTableContext<FixedDepositResponse, DefaultIdType, FixedDepositViewModel> Context { get; set; } = null!;

    private EntityTable<FixedDepositResponse, DefaultIdType, FixedDepositViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();

    // Autocomplete selection
    private MemberResponse? _selectedMember;

    // Advanced search filters
    private string? _searchCertificateNumber;
    private string? SearchCertificateNumber
    {
        get => _searchCertificateNumber;
        set
        {
            _searchCertificateNumber = value;
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

        Context = new EntityServerTableContext<FixedDepositResponse, DefaultIdType, FixedDepositViewModel>(
            fields:
            [
                new EntityField<FixedDepositResponse>(dto => dto.CertificateNumber, "Certificate #", "CertificateNumber"),
                new EntityField<FixedDepositResponse>(dto => dto.MemberName, "Member", "MemberName"),
                new EntityField<FixedDepositResponse>(dto => dto.PrincipalAmount, "Principal", "PrincipalAmount", typeof(decimal)),
                new EntityField<FixedDepositResponse>(dto => dto.InterestRate, "Rate (%)", "InterestRate", typeof(decimal)),
                new EntityField<FixedDepositResponse>(dto => dto.TermMonths, "Term", "TermMonths", typeof(int)),
                new EntityField<FixedDepositResponse>(dto => dto.MaturityDate, "Maturity", "MaturityDate", typeof(DateOnly)),
                new EntityField<FixedDepositResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchFixedDepositsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchFixedDepositsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<FixedDepositResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                if (_selectedMember != null) viewModel.MemberId = _selectedMember.Id;
                await Client.CreateFixedDepositAsync("1", viewModel.Adapt<CreateFixedDepositCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                if (_selectedMember != null) viewModel.MemberId = _selectedMember.Id;
                await Client.UpdateFixedDepositAsync("1", id, viewModel.Adapt<UpdateFixedDepositCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteFixedDepositAsync("1", id).ConfigureAwait(false),
            entityName: "Fixed Deposit",
            entityNamePlural: "Fixed Deposits",
            entityResource: FshResources.FixedDeposits,
            hasExtraActionsFunc: () => true);

        await AuthState;
    }

    private async Task<IEnumerable<MemberResponse>> SearchMembers(string searchText, CancellationToken ct)
    {
        var request = new SearchMembersCommand { PageNumber = 1, PageSize = 10, Keyword = searchText };
        var result = await Client.SearchMembersAsync("1", request).ConfigureAwait(false);
        return result.Data ?? Enumerable.Empty<MemberResponse>();
    }

    private async Task ShowFixedDepositsHelp()
    {
        await DialogService.ShowAsync<FixedDepositsHelpDialog>("Fixed Deposits Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    private async Task ViewDepositDetails(DefaultIdType id)
    {
        var deposit = await Client.GetFixedDepositAsync("1", id).ConfigureAwait(false);
        if (deposit != null)
        {
            var parameters = new DialogParameters { ["Deposit"] = deposit };
            await DialogService.ShowAsync<FixedDepositDetailsDialog>("Fixed Deposit Details", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
        }
    }

    private async Task CalculateInterest(DefaultIdType id)
    {
        var deposit = await Client.GetFixedDepositAsync("1", id).ConfigureAwait(false);
        if (deposit != null)
        {
            var parameters = new DialogParameters { ["Deposit"] = deposit };
            await DialogService.ShowAsync<FixedDepositInterestDialog>("Interest Calculation", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
        }
    }

    private async Task RenewDeposit(DefaultIdType id)
    {
        var deposit = await Client.GetFixedDepositAsync("1", id).ConfigureAwait(false);
        if (deposit != null)
        {
            var parameters = new DialogParameters { ["Deposit"] = deposit };
            var dialog = await DialogService.ShowAsync<FixedDepositRenewalDialog>("Renew Fixed Deposit", parameters, new DialogOptions
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

    private async Task WithdrawDeposit(DefaultIdType id)
    {
        var deposit = await Client.GetFixedDepositAsync("1", id).ConfigureAwait(false);
        if (deposit != null)
        {
            var parameters = new DialogParameters { ["Deposit"] = deposit };
            var dialog = await DialogService.ShowAsync<FixedDepositWithdrawalDialog>("Withdraw Fixed Deposit", parameters, new DialogOptions
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
