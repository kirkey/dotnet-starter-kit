namespace FSH.Starter.Blazor.Client.Pages.Accounting.Customers;

/// <summary>
/// Customers page logic. Provides CRUD and search over Customer entities using the generated API client.
/// Manages customer accounts including billing information, credit management, and payment terms.
/// </summary>
public partial class Customers
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<CustomerSearchResponse, DefaultIdType, CustomerViewModel> Context { get; set; } = null!;

    private EntityTable<CustomerSearchResponse, DefaultIdType, CustomerViewModel> _table = null!;

    // Advanced search filters
    private string? _searchCustomerNumber;
    private string? SearchCustomerNumber
    {
        get => _searchCustomerNumber;
        set
        {
            _searchCustomerNumber = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchCustomerName;
    private string? SearchCustomerName
    {
        get => _searchCustomerName;
        set
        {
            _searchCustomerName = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchCustomerType;
    private string? SearchCustomerType
    {
        get => _searchCustomerType;
        set
        {
            _searchCustomerType = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with customer-specific configuration including fields, CRUD operations, and search functionality.
    /// </summary>
    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<CustomerSearchResponse, DefaultIdType, CustomerViewModel>(
            fields:
            [
                new EntityField<CustomerSearchResponse>(dto => dto.CustomerNumber, "Customer #", "CustomerNumber"),
                new EntityField<CustomerSearchResponse>(dto => dto.CustomerName, "Name", "CustomerName"),
                new EntityField<CustomerSearchResponse>(dto => dto.CustomerType, "Type", "CustomerType"),
                new EntityField<CustomerSearchResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<CustomerSearchResponse>(dto => dto.Email, "Email", "Email"),
                new EntityField<CustomerSearchResponse>(dto => dto.Phone, "Phone", "Phone"),
                new EntityField<CustomerSearchResponse>(dto => dto.CreditLimit, "Credit Limit", "CreditLimit", typeof(decimal)),
                new EntityField<CustomerSearchResponse>(dto => dto.CurrentBalance, "Balance", "CurrentBalance", typeof(decimal)),
                new EntityField<CustomerSearchResponse>(dto => dto.AvailableCredit, "Available Credit", "AvailableCredit", typeof(decimal)),
                new EntityField<CustomerSearchResponse>(dto => dto.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<CustomerSearchResponse>(dto => dto.IsOnCreditHold, "Credit Hold", "IsOnCreditHold", typeof(bool)),
            ],
            searchFunc: async filter =>
            {
                var request = new CustomerSearchRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.CustomerSearchEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<CustomerSearchResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CustomerCreateEndpointAsync("1", viewModel.Adapt<CustomerCreateCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.CustomerUpdateEndpointAsync("1", id, viewModel.Adapt<CustomerUpdateCommand>()).ConfigureAwait(false);
            },
            deleteFunc: null,
            entityName: "Customer",
            entityNamePlural: "Customers",
            entityResource: FshResources.Accounting);

    /// <summary>
    /// Show customers help dialog.
    /// </summary>
    private async Task ShowCustomersHelp()
    {
        await DialogService.ShowAsync<CustomersHelpDialog>("Customers Management Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

