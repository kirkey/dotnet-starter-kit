namespace FSH.Starter.Blazor.Client.Pages.Todos;

public partial class Todos
{
    [Inject]
    

    protected EntityServerTableContext<GetTodoResponse, DefaultIdType, TodoViewModel> Context { get; set; } = default!;

    private EntityTable<GetTodoResponse, DefaultIdType, TodoViewModel> _table = default!;

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

public partial class TodoViewModel : UpdateTodoCommand;
