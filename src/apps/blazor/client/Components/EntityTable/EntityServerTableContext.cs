namespace FSH.Starter.Blazor.Client.Components.EntityTable;

/// <summary>
/// Initialization Context for the EntityTable Component.
/// Use this one if you want to use Server Paging, Sorting and Filtering.
/// </summary>
public class EntityServerTableContext<TEntity, TId, TRequest>(
    List<EntityField<TEntity>> fields,
    Func<PaginationFilter, Task<PaginationResponse<TEntity>>> searchFunc,
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
        exportAction,
        editFormInitializedFunc,
        hasExtraActionsFunc,
        canUpdateEntityFunc,
        canDeleteEntityFunc)
{
    /// <summary>
    /// A function that loads the specified page from the api with the specified search criteria
    /// and returns a PaginatedResult of TEntity.
    /// </summary>
    public Func<PaginationFilter, Task<PaginationResponse<TEntity>>> SearchFunc { get; } = searchFunc;

    public bool EnableAdvancedSearch { get; } = enableAdvancedSearch;
}
