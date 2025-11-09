namespace FSH.Starter.Blazor.Client.Pages.Todos;

/// <summary>
/// Todos page component for managing todo items.
/// </summary>
public partial class Todos
{
    /// <summary>
    /// The entity table context for managing todos with server-side operations.
    /// </summary>
    protected EntityServerTableContext<GetTodoResponse, DefaultIdType, TodoViewModel> Context { get; set; } = null!;

    /// <summary>
    /// Reference to the EntityTable component for todos.
    /// </summary>
    private EntityTable<GetTodoResponse, DefaultIdType, TodoViewModel> _table = null!;

    /// <summary>
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<GetTodoResponse, DefaultIdType, TodoViewModel>(
            entityName: "Todos",
            entityNamePlural: "Todos",
            entityResource: FshResources.Todos,
            fields:
            [
                new EntityField<GetTodoResponse>(response => response.Id, "Id", "Id"),
                new EntityField<GetTodoResponse>(response => response.Name, "Name", "Name"),
                new EntityField<GetTodoResponse>(response => response.Description, "Description", "Description"),
                new EntityField<GetTodoResponse>(response => response.Notes, "Notes", "Notes")
            ],
            enableAdvancedSearch: false,
            idFunc: prod => prod.Id!.Value,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();

                var result = await Client.GetTodoListEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<GetTodoResponse>>();
            },
            createFunc: async todo =>
            {
                await Client.CreateTodoEndpointAsync("1", todo.Adapt<CreateTodoCommand>());
            },
            updateFunc: async (id, todo) =>
            {
                await Client.UpdateTodoEndpointAsync("1", id, todo.Adapt<UpdateTodoCommand>());
            },
            deleteFunc: async id => await Client.DeleteTodoEndpointAsync("1", id));
}

/// <summary>
/// View model for todo operations, extending the update command.
/// </summary>
public partial class TodoViewModel : UpdateTodoCommand;
