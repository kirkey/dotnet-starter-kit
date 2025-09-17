using Mapster;

namespace FSH.Starter.Blazor.Client.Components.EntityTable;

public partial class EntityTable<TEntity, TId, TRequest>
    where TRequest : new()
{
    [Parameter]
    [EditorRequired]
    public EntityTableContext<TEntity, TId, TRequest> Context { get; set; } = default!;

    [Parameter] public bool Loading { get; set; }
    [Parameter] public string? SearchString { get; set; }
    [Parameter] public EventCallback<string> SearchStringChanged { get; set; }
    [Parameter] public RenderFragment? AdvancedSearchContent { get; set; }
    [Parameter] public RenderFragment<TEntity>? ActionsContent { get; set; }
    [Parameter] public RenderFragment<TEntity>? ExtraActions { get; set; }
    [Parameter] public RenderFragment<TEntity>? ChildRowContent { get; set; }
    [Parameter] public RenderFragment<TRequest>? EditFormContent { get; set; }

    [CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = default!;
    [Inject] protected IAuthorizationService AuthService { get; set; } = default!;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;
    private bool _canExport;

    private bool _advancedSearchExpanded;

    private MudTable<TEntity> _table = default!;
    private IEnumerable<TEntity>? _entityList;
    private int _totalItems;

    private ClientPreference? _clientPreference;
    
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
        _canExport = await CanDoActionAsync(Context.ExportAction, state);

        await LocalLoadDataAsync();
    }

    public Task ReloadDataAsync() =>
        Context.IsClientContext
            ? LocalLoadDataAsync()
            : ServerLoadDataAsync();

    private async Task<bool> CanDoActionAsync(string? action, AuthenticationState state) =>
        !string.IsNullOrWhiteSpace(action) &&
            (bool.TryParse(action, out bool isTrue) && isTrue || // check if action equals "True", then it's allowed
            Context.EntityResource is { } resource && await AuthService.HasPermissionAsync(state.User, action, resource));

    private bool HasActions => _canUpdate || _canDelete || Context.HasExtraActionsFunc is not null && Context.HasExtraActionsFunc();
    private bool CanUpdateEntity(TEntity entity) => _canUpdate && (Context.CanUpdateEntityFunc is null || Context.CanUpdateEntityFunc(entity));
    private bool CanDeleteEntity(TEntity entity) => _canDelete && (Context.CanDeleteEntityFunc is null || Context.CanDeleteEntityFunc(entity));

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
                () => Context.ClientContext.LoadDataFunc(), Toast, Navigation)
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
        Context.IsServerContext ? ServerReload : null;

    private async Task<TableData<TEntity>> ServerReload(TableState state, CancellationToken cancellationToken)
    {
        if (Loading || Context.ServerContext is null)
        {
            return new TableData<TEntity> { TotalItems = _totalItems, Items = _entityList };
        }

        Loading = true;

        var filter = GetPaginationFilter(state);

        if (await ApiHelper.ExecuteCallGuardedAsync(
                () => Context.ServerContext.SearchFunc(filter), Toast, Navigation)
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
            Keyword = filter.Keyword
        };
        filter.Keyword = null;

        return filter;
    }

    private async Task InvokeModal(TEntity? entity = default, TEntity? entityToDuplicate = default)
    {
        bool isCreate = entity is null;
    
        var parameters = new DialogParameters
        {
            { nameof(AddEditModal<TRequest>.ChildContent), EditFormContent },
            { nameof(AddEditModal<TRequest>.OnInitializedFunc), Context.EditFormInitializedFunc },
            { nameof(AddEditModal<TRequest>.IsCreate), isCreate }
        };
    
        Func<TRequest, Task> saveFunc = isCreate
            ? Context.CreateFunc ?? throw new InvalidOperationException("CreateFunc can't be null!")
            : request => Context.UpdateFunc!(Context.IdFunc!(entity!), request);
    
        TRequest requestModel = isCreate || entityToDuplicate is not null
            ? await GetRequestModel(entityToDuplicate).ConfigureAwait(false)
            : Context.GetDetailsFunc is not null &&
              await ApiHelper.ExecuteCallGuardedAsync(() => Context.GetDetailsFunc(Context.IdFunc!(entity!)), Toast, Navigation)
                  is { } detailsResult
                ? detailsResult
                : entity!.Adapt<TRequest>();
    
        string title = isCreate ? $"Create {Context.EntityName}" : $"Edit {Context.EntityName}";
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
                        Context.GetDuplicateFunc(entityToDuplicate), Toast, Navigation)
                    .ConfigureAwait(false) is { } duplicateResult)
                return duplicateResult;

            return entityToDuplicate.Adapt<TRequest>();
        }

        if (Context.GetDefaultsFunc is not null &&
            await ApiHelper.ExecuteCallGuardedAsync(() =>
                    Context.GetDefaultsFunc(), Toast, Navigation)
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
                () => Context.UpdateFunc(id, requestModel),
                Toast).ConfigureAwait(false);

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
        var dialog = DialogService.Show<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            _ = Context.DeleteFunc ?? throw new InvalidOperationException("DeleteFunc can't be null!");

            await ApiHelper.ExecuteCallGuardedAsync(
                () => Context.DeleteFunc(id),
                Toast);

            await ReloadDataAsync();
        }
    }
}
