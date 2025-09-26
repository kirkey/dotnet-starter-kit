namespace FSH.Starter.Blazor.Client.Components.EntityTable;

/// <summary>
/// Initialization Context for the EntityTable Component.
/// Use this one if you want to use Server Paging, Sorting and Filtering.
/// </summary>
/// <typeparam name="TEntity">The type of the entity displayed in the table.</typeparam>
/// <typeparam name="TId">The type of the entity identifier.</typeparam>
/// <typeparam name="TRequest">The request model type used by the Add/Edit form.</typeparam>
public class EntityServerTableContext<TEntity, TId, TRequest>(
    List<EntityField<TEntity>> fields,
    Func<PaginationFilter, Task<PaginationResponse<TEntity>>> searchFunc,
    Func<FileUploadCommand, Task<int>>? importFunc = null,
    Func<PaginationFilter, Task<FileResponse>>? exportFunc = null,
    bool enableAdvancedSearch = false,
    Func<TEntity, TId>? idFunc = null,
    Func<Task<TRequest>>? getDefaultsFunc = null,
    Func<TRequest, Task>? createFunc = null,
    Func<TEntity, Task<TRequest>>? duplicateFunc = null,
    Func<TId, Task<TRequest>>? getDetailsFunc = null,
    Func<TId, TRequest, Task>? updateFunc = null,
    Func<TId, Task>? deleteFunc = null,
    string? entityName = null,
    string? entityNamePlural = null,
    string? entityResource = null,
    string? searchAction = null,
    string? createAction = null,
    string? updateAction = null,
    string? deleteAction = null,
    string? importAction = null,
    string? exportAction = null,
    Func<Task>? editFormInitializedFunc = null,
    Func<bool>? hasExtraActionsFunc = null,
    Func<TEntity, bool>? canUpdateEntityFunc = null,
    Func<TEntity, bool>? canDeleteEntityFunc = null)
    : EntityTableContext<TEntity, TId, TRequest>(
        fields,
        idFunc,
        getDefaultsFunc,
        createFunc,
        duplicateFunc,
        getDetailsFunc,
        updateFunc,
        deleteFunc,
        entityName,
        entityNamePlural,
        entityResource,
        searchAction,
        createAction,
        updateAction,
        deleteAction,
        importAction,
        exportAction,
        editFormInitializedFunc,
        hasExtraActionsFunc,
        canUpdateEntityFunc,
        canDeleteEntityFunc)
{
    /// <summary>
    /// A function that loads the specified page from the api with the specified search criteria
    /// and returns a <see cref="PaginationResponse{TEntity}"/>.
    /// </summary>
    public Func<PaginationFilter, Task<PaginationResponse<TEntity>>> SearchFunc { get; } =
        (ValidateFields(fields), searchFunc) switch
        {
            (true, not null) => searchFunc,
            _ => throw new ArgumentNullException(nameof(searchFunc))
        };

    /// <summary>
    /// Enables the advanced search UI which lets users pick specific columns to search on.
    /// </summary>
    public bool EnableAdvancedSearch { get; } = enableAdvancedSearch;

    /// <summary>
    ///     A function that imports data from a file upload and returns the number of records imported.
    /// </summary>
    public Func<FileUploadCommand, Task<int>>? ImportFunc { get; } = importFunc;
    
    /// <summary>
    ///     A function that exports the specified data from the API and returns a file content stream.
    /// </summary>
    public Func<PaginationFilter, Task<FileResponse>>? ExportFunc { get; } = exportFunc;

    private static bool ValidateFields(List<EntityField<TEntity>> value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.Count == 0)
            throw new ArgumentException("Fields must contain at least one column.", nameof(value));
        if (value.Any(f => f is null))
            throw new ArgumentException("Fields cannot contain null entries.", nameof(value));
        return true;
    }
}
