using System.ComponentModel;
using System.Data;
using ClosedXML.Excel;

namespace FSH.Starter.Blazor.Client.Components.EntityTable;

public partial class EntityTable<TEntity, TId, TRequest>
    where TRequest : new()
{
    [Parameter]
    [EditorRequired]
    public EntityTableContext<TEntity, TId, TRequest> Context { get; set; } = null!;

    [Parameter] public bool Exporting { get; set; }
    [Parameter] public bool Importing { get; set; }
    [Parameter] public bool Loading { get; set; }
    [Parameter] public string? SearchString { get; set; }
    [Parameter] public int[] PageSizes { get; set; } = [10, 20, 50, 100, 500, 999];
    [Parameter] public EventCallback<string> SearchStringChanged { get; set; }
    [Parameter] public RenderFragment? AdvancedSearchContent { get; set; }
    [Parameter] public RenderFragment<TEntity>? ActionsContent { get; set; }
    [Parameter] public RenderFragment<TEntity>? ExtraActions { get; set; }
    [Parameter] public RenderFragment<TEntity>? ChildRowContent { get; set; }
    [Parameter] public RenderFragment<TRequest>? EditFormContent { get; set; }

    [CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = null!;
    [Inject] protected IAuthorizationService AuthService { get; set; } = null!;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;
    private bool _canImport;
    private bool _canExport;

    private bool _advancedSearchExpanded;

    private MudTable<TEntity> _table = null!;
    private IEnumerable<TEntity>? _entityList;
    private int _totalItems;

    private ClientPreference? _clientPreference;
    private InputFile? fileUploadInput;

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is not ClientPreference clientPreference)
            clientPreference = new ClientPreference();

        _clientPreference = clientPreference;
        
        var state = await AuthState;
        _canSearch = await CanDoActionAsync(Context.SearchAction, state);
        _canCreate = await CanDoActionAsync(Context.CreateAction, state);
        _canUpdate = await CanDoActionAsync(Context.UpdateAction, state);
        _canDelete = await CanDoActionAsync(Context.DeleteAction, state);
        _canImport = await CanDoActionAsync(Context.ImportAction, state);
        _canExport = await CanDoActionAsync(Context.ExportAction, state);

        await LocalLoadDataAsync();
    }

    public Task ReloadDataAsync() =>
        Context.IsClientContext
            ? LocalLoadDataAsync()
            : ServerLoadDataAsync();

    /// <summary>
            /// Checks if the current user can perform the specified action.
            /// </summary>
            /// <param name="action">The action to check permission for.</param>
            /// <param name="state">The current authentication state.</param>
            /// <returns>True if the action is allowed; otherwise, false.</returns>
            private async Task<bool> CanDoActionAsync(string? action, AuthenticationState state)
            {
                try
                {
                    return !string.IsNullOrWhiteSpace(action) &&
                           ((bool.TryParse(action, out bool isTrue) &&
                             isTrue) || // check if action equals "True", then it's allowed
                            (Context.EntityResource is { } resource &&
                             await AuthService.HasPermissionAsync(state.User, action, resource).ConfigureAwait(false)));
                }
                catch (Exception ex)
                {
                    // Log the exception or handle as needed
                    return false; // If any error occurs, deny the action
                }
            }

    private bool HasActions => _canUpdate || _canDelete || Context.HasExtraActionsFunc is not null && Context.HasExtraActionsFunc();
    private bool CanUpdateEntity(TEntity entity) => _canUpdate && Context.UpdateFunc is not null && (Context.CanUpdateEntityFunc is null || Context.CanUpdateEntityFunc(entity));
    private bool CanDeleteEntity(TEntity entity) => _canDelete && Context.DeleteFunc is not null && (Context.CanDeleteEntityFunc is null || Context.CanDeleteEntityFunc(entity));

    // Client side paging/filtering
    private bool LocalSearch(TEntity entity) =>
        Context.ClientContext?.SearchFunc is { } searchFunc
            ? searchFunc(SearchString, entity)
            : string.IsNullOrWhiteSpace(SearchString);

    private async Task LocalLoadDataAsync()
    {
        if (Loading || Context.ClientContext is null)
        {
            return;
        }

        Loading = true;

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => Context.ClientContext.LoadDataFunc())
            is { } result)
        {
            _entityList = result;
        }

        Loading = false;
    }

    // Server Side paging/filtering

    private async Task OnSearchStringChanged(string? text = null)
    {
        await SearchStringChanged.InvokeAsync(SearchString);

        await ServerLoadDataAsync();
    }

    private async Task ServerLoadDataAsync()
    {
        if (Context.IsServerContext)
        {
            await _table.ReloadServerData();
        }
    }

    private static bool GetBooleanValue(object valueFunc)
    {
        if (valueFunc is bool boolValue)
        {
            return boolValue;
        }
        return false;
    }

    private Func<TableState, CancellationToken, Task<TableData<TEntity>>>? ServerReloadFunc =>
        Context?.IsServerContext == true ? ServerReload : null;

    private async Task<TableData<TEntity>> ServerReload(TableState state, CancellationToken cancellationToken)
    {
        if (Loading || Context.ServerContext is null)
        {
            return new TableData<TEntity> { TotalItems = _totalItems, Items = _entityList };
        }

        Loading = true;

        var filter = GetPaginationFilter(state);

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => Context.ServerContext.SearchFunc(filter))
            is { } result)
        {
            _totalItems = result.TotalCount;
            _entityList = result.Items;
        }

        Loading = false;

        return new TableData<TEntity> { TotalItems = _totalItems, Items = _entityList };
    }


    private PaginationFilter GetPaginationFilter(TableState state)
    {
        string[]? orderings = null;
        if (!string.IsNullOrEmpty(state.SortLabel))
        {
            orderings = state.SortDirection == SortDirection.None
                ? [$"{state.SortLabel}"]
                : [$"{state.SortLabel} {state.SortDirection}"];
        }

        var filter = new PaginationFilter
        {
            PageSize = state.PageSize,
            PageNumber = state.Page + 1,
            Keyword = SearchString,
            OrderBy = orderings ?? []
        };

        if (Context.AllColumnsChecked)
        {
            return filter;
        }

        filter.AdvancedSearch = new Search
        {
            Fields = Context.SearchFields,
            
        };
        filter.Keyword = null;

        return filter;
    }

    private async Task InvokeModal(TEntity? entity = default, TEntity? entityToDuplicate = default, bool isViewMode = false)
    {
        bool isCreate = entity is null && !isViewMode;
    
        var parameters = new DialogParameters
        {
            { nameof(AddEditModal<TRequest>.ChildContent), EditFormContent },
            { nameof(AddEditModal<TRequest>.OnInitializedFunc), Context.EditFormInitializedFunc },
            { nameof(AddEditModal<TRequest>.IsCreate), isCreate },
            { nameof(AddEditModal<TRequest>.IsViewMode), isViewMode }
        };
    
        Func<TRequest, Task> saveFunc = isCreate
            ? Context.CreateFunc ?? throw new InvalidOperationException("CreateFunc can't be null!")
            : request => Context.UpdateFunc!(Context.IdFunc!(entity!), request);
    
        TRequest requestModel = isCreate || entityToDuplicate is not null
            ? await GetRequestModel(entityToDuplicate).ConfigureAwait(false)
            : Context.GetDetailsFunc is not null &&
              await ApiHelper.ExecuteCallGuardedAsync(() => Context.GetDetailsFunc(Context.IdFunc!(entity!)))
                  is { } detailsResult
                ? detailsResult
                : entity!.Adapt<TRequest>();
    
        string title = isViewMode ? $"View {Context.EntityName}" : isCreate ? $"Create {Context.EntityName}" : $"Edit {Context.EntityName}";
        string successMessage = isCreate ? $"{Context.EntityName} Created" : $"{Context.EntityName} Updated";
    
        parameters.Add(nameof(AddEditModal<TRequest>.SaveFunc), saveFunc);
        parameters.Add(nameof(AddEditModal<TRequest>.RequestModel), requestModel);
        parameters.Add(nameof(AddEditModal<TRequest>.Title), title);
        parameters.Add(nameof(AddEditModal<TRequest>.SuccessMessage), successMessage);
    
        var dialogOptions = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            BackdropClick = false
        };
        
        var dialog = DialogService.ShowModal<AddEditModal<TRequest>>(parameters);
    
        Context.SetAddEditModalRef(dialog);
    
        var result = await dialog.Result;
    
        if (!result!.Canceled)
        {
            await ReloadDataAsync();
        }
    }
    
    private async Task<TRequest> GetRequestModel(TEntity? entityToDuplicate)
    {
        if (entityToDuplicate is not null)
        {
            if (Context.GetDuplicateFunc is not null &&
                await ApiHelper.ExecuteCallGuardedAsync(() => 
                        Context.GetDuplicateFunc(entityToDuplicate))
                    .ConfigureAwait(false) is { } duplicateResult)
                return duplicateResult;

            return entityToDuplicate.Adapt<TRequest>();
        }

        if (Context.GetDefaultsFunc is not null &&
            await ApiHelper.ExecuteCallGuardedAsync(() =>
                    Context.GetDefaultsFunc())
                .ConfigureAwait(false) is { } defaultsResult)
            return defaultsResult;

        return new TRequest();
    }
    
    private async Task InvokeUpdateAsync(TEntity? entity = default)
    {
        const string transactionAction = "Update";
        var parameters = new DialogParameters
        {
            { nameof(TransactionConfirmation.TransactionIcon), Icons.Material.Filled.AutoFixHigh },
            { nameof(TransactionConfirmation.TransactionTitle), $"{transactionAction} Confirmation" },
            { nameof(TransactionConfirmation.ContentText), $"Do you want to Update this {Context.EntityName}?" },
            { nameof(TransactionConfirmation.ConfirmText), transactionAction },
            { nameof(TransactionConfirmation.ButtonColor), Color.Secondary }
        };

        var dialog = await DialogService.ShowAsync<TransactionConfirmation>(transactionAction, parameters)
            .ConfigureAwait(false);
        var result = await dialog.Result.ConfigureAwait(false);
        if (result is { Canceled: false })
        {
            _ = Context.IdFunc ?? throw new InvalidOperationException("IdFunc can't be null!");
            var id = Context.IdFunc(entity!);

            _ = Context.UpdateFunc ?? throw new InvalidOperationException("UpdateFunc can't be null!");
            Func<TRequest, Task> saveFunc = request => Context.UpdateFunc(id, request);

            var requestModel = entity!.Adapt<TRequest>();

            await ApiHelper.ExecuteCallGuardedAsync(
                () => Context.UpdateFunc(id, requestModel)).ConfigureAwait(false);

            await ReloadDataAsync().ConfigureAwait(false);
        }
    }

    private async Task Delete(TEntity entity)
    {
        _ = Context.IdFunc ?? throw new InvalidOperationException("IdFunc can't be null!");
        TId id = Context.IdFunc(entity);

        string deleteContent = "You're sure you want to delete {0} with id '{1}'?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), string.Format(deleteContent, Context.EntityName, id) }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            _ = Context.DeleteFunc ?? throw new InvalidOperationException("DeleteFunc can't be null!");

            await ApiHelper.ExecuteCallGuardedAsync(
                () => Context.DeleteFunc(id));

            await ReloadDataAsync();
        }
    }
    
    private async Task ImportAsync(IBrowserFile? file)
    {
        if (file == null)
            return;

        // Validate file extension
        var extension = Path.GetExtension(file.Name).ToLowerInvariant();
        if (extension != ".xlsx" && extension != ".xls")
        {
            Snackbar.Add("Please select an Excel file (.xlsx or .xls)", Severity.Warning);
            return;
        }

        try
        {
            Importing = true;

            await using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024); // 10 MB limit
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            // Validate file is not empty
            if (fileBytes.Length == 0)
            {
                Snackbar.Add("The selected file is empty. Please select a valid Excel file.", Severity.Warning);
                return;
            }

            // Determine MIME type based on extension
            var base64Data = Convert.ToBase64String(fileBytes);
            
            // Create FileUploadCommand with base64 data expected by DataImport service
            var fileUpload = new FileUploadCommand
            {
                Name = file.Name,
                Data = base64Data, // Send raw base64 string, not data URI format
                Extension = Path.GetExtension(file.Name),
                Size = file.Size
            };

            if (Context.ServerContext?.ImportFunc != null)
            {
                var importResult = await ApiHelper.ExecuteCallGuardedAsync(
                    () => Context.ServerContext.ImportFunc(fileUpload));

                if (importResult != null)
                {
                    if (importResult.IsSuccess)
                    {
                        Snackbar.Add($"Successfully imported {importResult.ImportedCount} {Context.EntityNamePlural}.", Severity.Success);
                        await ReloadDataAsync(); // Refresh the table data
                    }
                    else if (importResult is { ImportedCount: > 0, FailedCount: > 0 })
                    {
                        var message = $"Imported {importResult.ImportedCount} {Context.EntityNamePlural} successfully. {importResult.FailedCount} failed.";
                        Snackbar.Add(message, Severity.Warning);
                        
                        // Show first few errors if available
                        if (importResult.Errors.Any())
                        {
                            var errorSummary = string.Join("; ", importResult.Errors.Take(3));
                            Snackbar.Add($"Errors: {errorSummary}", Severity.Error);
                        }
                        
                        await ReloadDataAsync(); // Refresh the table data even with partial success
                    }
                    else if (importResult is { ImportedCount: 0, FailedCount: > 0 })
                    {
                        var message = $"Import failed. {importResult.FailedCount} records could not be imported.";
                        Snackbar.Add(message, Severity.Error);
                        
                        // Show first few errors
                        if (importResult.Errors.Any())
                        {
                            var errorSummary = string.Join("; ", importResult.Errors.Take(3));
                            Snackbar.Add($"Errors: {errorSummary}", Severity.Error);
                        }
                    }
                    else
                    {
                        Snackbar.Add("No records were imported from the file.", Severity.Warning);
                    }
                }
            }
            else
            {
                Snackbar.Add("Import function is not available for this entity.", Severity.Warning);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error importing file: {ex.Message}", Severity.Error);
        }
        finally
        {
            Importing = false;
        }
    }
    
    private async Task ExportAsync()
    {
        await Task.Yield();

        if (!Exporting)
        {
            if (Context.ServerContext?.ExportFunc != null)
            {
                const string action = "Export";
                const string content = "You're sure you want to export '{0}'?";
                var parameters = new DialogParameters
                {
                    { nameof(TransactionConfirmation.ContentText), string.Format(content, Context.EntityNamePlural) },
                    { nameof(TransactionConfirmation.ConfirmText), action }
                };
                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
                var dialog = await DialogService.ShowAsync<TransactionConfirmation>($"{action} {Context.EntityNamePlural}", parameters, options);
                var result = await dialog.Result;
                if (!result!.Canceled)
                {
                    Exporting = true;
                    var filter = GetBaseFilter();
                    if (await ApiHelper.ExecuteCallGuardedAsync(
                            () => Context.ServerContext.ExportFunc(filter)).ConfigureAwait(false)
                        is { } response)
                    {
                        using var streamRef = new DotNetStreamReference(response.Stream);
                        await Js.InvokeVoidAsync("downloadFileFromStream", $"{Context.EntityNamePlural}.xlsx",
                                streamRef)
                            .ConfigureAwait(false);
                    }

                    Exporting = false;
                }
            }
            else if (Context.ClientContext is not null)
            {
                Exporting = true;
                if (_entityList != null)
                {
                    var properties = TypeDescriptor.GetProperties(typeof(TEntity));
                    var table = new DataTable("table", "table");
                    foreach (PropertyDescriptor prop in properties)
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    foreach (var item in _entityList)
                    {
                        var row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        table.Rows.Add(row);
                    }

                    using var wb = new XLWorkbook();
                    var worksheet = wb.Worksheets.Add(table);
                    worksheet.Columns().AdjustToContents();

                    Stream stream = new MemoryStream();
                    wb.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    string fileName = $"{Context.EntityNamePlural}.xlsx";

                    // Return or use the fileResponse as needed
                    await Js.InvokeVoidAsync("downloadFileFromStream", fileName, new DotNetStreamReference(stream))
                        .ConfigureAwait(false);
                }

                Exporting = false;
            }
        }
    }
    
    private PaginationFilter GetBaseFilter()
    {
        var filter = new PaginationFilter
        {
            Keyword = SearchString
        };

        if (!Context.AllColumnsChecked)
        {
            filter.AdvancedSearch = new Search
            {
                Fields = Context.SearchFields,
                
            };
            filter.Keyword = null;
        }

        return filter;
    }

    /// <summary>
    /// Triggers the hidden file input to open the file selection dialog.
    /// </summary>
    private async Task TriggerFileUpload()
    {
        if (fileUploadInput?.Element is not null)
        {
            await Js.InvokeVoidAsync("triggerClick", fileUploadInput.Element);
        }
    }

    /// <summary>
    /// Handles the file selection event from the hidden InputFile component.
    /// </summary>
    /// <param name="e">The file selection event arguments.</param>
    private async Task OnFileSelected(InputFileChangeEventArgs e)
    {
        var file = e.GetMultipleFiles().FirstOrDefault();
        if (file is not null)
        {
            await ImportAsync(file);
        }
    }
}
