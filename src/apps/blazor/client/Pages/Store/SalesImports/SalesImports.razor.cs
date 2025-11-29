namespace FSH.Starter.Blazor.Client.Pages.Store.SalesImports;

/// <summary>
/// Sales Imports page logic. Manages POS sales data imports to maintain inventory accuracy.
/// </summary>
public partial class SalesImports
{
    [Inject] protected ICourier Courier { get; set; } = null!;

    protected EntityServerTableContext<SalesImportResponse, DefaultIdType, SalesImportViewModel> Context { get; set; } = null!;
    
    private ClientPreference _preference = new();
    private EntityTable<SalesImportResponse, DefaultIdType, SalesImportViewModel> _table = null!;

    private string? SearchImportNumber { get; set; }
    private string? SearchStatus { get; set; }
    private DateTime? SearchImportDateFrom { get; set; }
    private DateTime? SearchImportDateTo { get; set; }

    private IBrowserFile? _selectedFile;
    private string? _selectedFileName;
    private long _fileSize;
    private string? _fileContent;

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
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<SalesImportResponse, DefaultIdType, SalesImportViewModel>(
            entityName: "Sales Import",
            entityNamePlural: "Sales Imports",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<SalesImportResponse>(x => x.ImportNumber, "Import Number", "ImportNumber"),
                new EntityField<SalesImportResponse>(x => x.ImportDate, "Import Date", "ImportDate", typeof(DateOnly)),
                new EntityField<SalesImportResponse>(x => x.SalesPeriodFrom, "Period From", "SalesPeriodFrom", typeof(DateOnly)),
                new EntityField<SalesImportResponse>(x => x.SalesPeriodTo, "Period To", "SalesPeriodTo", typeof(DateOnly)),
                new EntityField<SalesImportResponse>(x => x.WarehouseName, "Warehouse", "WarehouseName"),
                new EntityField<SalesImportResponse>(x => x.FileName, "File Name", "FileName"),
                new EntityField<SalesImportResponse>(x => x.TotalRecords, "Total Records", "TotalRecords", typeof(int)),
                new EntityField<SalesImportResponse>(x => x.ProcessedRecords, "Processed", "ProcessedRecords", typeof(int)),
                new EntityField<SalesImportResponse>(x => x.ErrorRecords, "Errors", "ErrorRecords", typeof(int)),
                new EntityField<SalesImportResponse>(x => x.Status, "Status", "Status"),
                new EntityField<SalesImportResponse>(x => x.IsReversed, "Reversed", "IsReversed", typeof(bool))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchSalesImportsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    ImportNumber = SearchImportNumber,
                    Status = SearchStatus,
                    ImportDateFrom = SearchImportDateFrom,
                    ImportDateTo = SearchImportDateTo
                };
                var result = await Client.SearchSalesImportsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<SalesImportResponse>>();
            },
            createFunc: async viewModel =>
            {
                var command = viewModel.Adapt<CreateSalesImportCommand>();
                if (!string.IsNullOrWhiteSpace(_fileContent))
                {
                    // The file content will be handled by the server
                }
                await Client.CreateSalesImportEndpointAsync("1", command).ConfigureAwait(false);
            },
            deleteAction: null);
    }

    private async Task OnFileSelected(InputFileChangeEventArgs e)
    {
        _selectedFile = e.File;
        _selectedFileName = _selectedFile.Name;
        _fileSize = _selectedFile.Size;

        if (_selectedFile.Size > 10 * 1024 * 1024) // 10MB limit
        {
            Snackbar.Add("File size exceeds 10MB limit", Severity.Error);
            ClearFile();
            return;
        }

        try
        {
            using var memoryStream = new MemoryStream();
            await _selectedFile.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024).CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();
            _fileContent = Convert.ToBase64String(bytes);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error reading file: {ex.Message}", Severity.Error);
            ClearFile();
        }
    }

    private void ClearFile()
    {
        _selectedFile = null;
        _selectedFileName = null;
        _fileSize = 0;
        _fileContent = null;
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters<SalesImportDetailsDialog>
        {
            { x => x.Id, id }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true };
        var dialog = await DialogService.ShowAsync<SalesImportDetailsDialog>("Sales Import Details", parameters, options);
        await dialog.Result;
    }

    private async Task ReverseImport(DefaultIdType id)
    {
        var parameters = new DialogParameters<SalesImportReverseDialog>
        {
            { x => x.Id, id }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };
        var dialog = await DialogService.ShowAsync<SalesImportReverseDialog>("Reverse Sales Import", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Show sales imports help dialog.
    /// </summary>
    private async Task ShowSalesImportsHelp()
    {
        await DialogService.ShowAsync<SalesImportsHelpDialog>("Sales Imports Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

/// <summary>
/// View model for Sales Import create/edit operations.
/// </summary>
public class SalesImportViewModel
{
    public DefaultIdType Id { get; set; }
    public string ImportNumber { get; set; } = default!;
    public DefaultIdType? WarehouseId { get; set; }
    public DateTime? SalesPeriodFrom { get; set; }
    public DateTime? SalesPeriodTo { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
}

