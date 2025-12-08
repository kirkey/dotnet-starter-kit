namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanProducts;

/// <summary>
/// LoanProducts page logic. Provides CRUD and search over LoanProduct entities using the generated API client.
/// Manages loan product configuration including interest rates, terms, and repayment schedules.
/// </summary>
public partial class LoanProducts
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<LoanProductResponse, DefaultIdType, LoanProductViewModel> Context { get; set; } = null!;

    private EntityTable<LoanProductResponse, DefaultIdType, LoanProductViewModel> _table = null!;

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

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchCode;
    private string? SearchCode
    {
        get => _searchCode;
        set
        {
            _searchCode = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchName;
    private string? SearchName
    {
        get => _searchName;
        set
        {
            _searchName = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private bool? _searchIsActive;
    private bool? SearchIsActive
    {
        get => _searchIsActive;
        set
        {
            _searchIsActive = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with loan product-specific configuration.
    /// </summary>
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

        Context = new EntityServerTableContext<LoanProductResponse, DefaultIdType, LoanProductViewModel>(
            fields:
            [
                new EntityField<LoanProductResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<LoanProductResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<LoanProductResponse>(dto => dto.InterestRate, "Interest Rate %", "InterestRate", typeof(decimal)),
                new EntityField<LoanProductResponse>(dto => dto.InterestMethod, "Method", "InterestMethod"),
                new EntityField<LoanProductResponse>(dto => dto.MinLoanAmount, "Min Amount", "MinLoanAmount", typeof(decimal)),
                new EntityField<LoanProductResponse>(dto => dto.MaxLoanAmount, "Max Amount", "MaxLoanAmount", typeof(decimal)),
                new EntityField<LoanProductResponse>(dto => dto.MinTermMonths, "Min Term", "MinTermMonths", typeof(int)),
                new EntityField<LoanProductResponse>(dto => dto.MaxTermMonths, "Max Term", "MaxTermMonths", typeof(int)),
                new EntityField<LoanProductResponse>(dto => dto.RepaymentFrequency, "Frequency", "RepaymentFrequency"),
                new EntityField<LoanProductResponse>(dto => dto.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchLoanProductsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLoanProductsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LoanProductResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateLoanProductAsync("1", viewModel.Adapt<CreateLoanProductCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateLoanProductAsync("1", id, viewModel.Adapt<UpdateLoanProductCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteLoanProductAsync("1", id).ConfigureAwait(false),
            entityName: "Loan Product",
            entityNamePlural: "Loan Products",
            entityResource: FshResources.LoanProducts);
    }

    /// <summary>
    /// Show loan products help dialog.
    /// </summary>
    private async Task ShowLoanProductsHelp()
    {
        await DialogService.ShowAsync<LoanProductsHelpDialog>("Loan Products Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
